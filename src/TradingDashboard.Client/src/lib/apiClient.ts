import store, { logout, setAccessToken } from "@/store/store";
import axios from "axios";
import { toast } from "sonner";

function handleSessionExpired() {
  store.dispatch(logout());
  toast.error("Your session has expired. Please log in again.");
  // navigateTo("/login");
}

const apiClient = axios.create({
  baseURL: "/api", // Vite proxy forwards this to https://localhost:7186/api
  withCredentials: true, // required so the httpOnly refresh cookie is sent
  headers: {
    "Content-Type": "application/json",
  },
});

// REQUEST interceptor — runs before every request leaves the browser.
// This is where you'll attach the auth token later.
apiClient.interceptors.request.use((config) => {
  const token = store.getState().auth.accessToken;
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

let isRefreshing = false;
type RefreshQueueItem = {
  resolve: (value?: any) => void;
  reject: (reason?: any) => void;
  originalRequest: any;
};

let refreshQueue: RefreshQueueItem[] = [];

apiClient.interceptors.response.use(
  (response) => response.data, // unwrap .data so callers get the payload directly
  async (error) => {
    const originalRequest = error.config;
    const isAuthRoute =
      originalRequest?.url?.includes("/users/login") ||
      originalRequest?.url?.includes("/users/refresh");

    if (
      error.response?.status === 401 &&
      !isAuthRoute &&
      !originalRequest._retry
    ) {
      originalRequest._retry = true;

      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          refreshQueue.push({ resolve, reject, originalRequest });
        });
      }

      isRefreshing = true;
      try {
        const { data } = await axios.post(
          "/users/refresh",
          {},
          { withCredentials: true, baseURL: "/api" },
        );

        store.dispatch(setAccessToken(data.accessToken));

        refreshQueue.forEach(({ resolve, originalRequest: queuedReq }) => {
          queuedReq.headers.Authorization = `Bearer ${data.accessToken}`;
          resolve(apiClient(queuedReq));
        });
        refreshQueue = [];

        originalRequest.headers.Authorization = `Bearer ${data.accessToken}`;
        return apiClient(originalRequest);
      } catch (refreshError) {
        refreshQueue.forEach(({ reject }) => reject(refreshError));
        refreshQueue = [];
        handleSessionExpired();
        return Promise.reject(refreshError);
      } finally {
        isRefreshing = false;
      }
    }

    // Re-throw so individual hooks can still catch and show specific messages
    return Promise.reject(
      error.response?.data ?? { errors: [{ message: "Network error" }] },
    );
  },
);

export default apiClient;
