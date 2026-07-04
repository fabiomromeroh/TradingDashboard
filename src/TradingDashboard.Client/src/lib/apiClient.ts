import axios from "axios";

const apiClient = axios.create({
  baseURL: "/api", // Vite proxy forwards this to https://localhost:7186/api
  headers: {
    "Content-Type": "application/json",
  },
});

// REQUEST interceptor — runs before every request leaves the browser.
// This is where you'll attach the auth token later.
apiClient.interceptors.request.use((config) => {
  // const token = store.getState().auth.token;  ← you'll add this when auth is ready
  // if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

// RESPONSE interceptor — runs on every response before it reaches your code.
// Handle global errors here instead of in every hook.
apiClient.interceptors.response.use(
  (response) => response.data, // unwrap .data so callers get the payload directly
  (error) => {
    if (error.response?.status === 401) {
      // redirect to login, clear auth state, etc.
    }
    // Re-throw so individual hooks can still catch and show specific messages
    return Promise.reject(
      error.response?.data ?? { errors: [{ message: "Network error" }] },
    );
  },
);

export default apiClient;
