// import './App.css'
import { useEffect, useRef } from "react";
import Router from "./Router";
import { Toaster } from "sonner";
import axios from "axios";
import {
  logout,
  setAccessToken,
  setAuthCheckComplete,
  setUser,
} from "@/store/store";
import { useAppDispatch, useAppSelector } from "@/store/hooks";

function App() {
  const authCheckComplete = useAppSelector(
    (state) => state.auth.authCheckComplete,
  );
  const effectRan = useRef(false);
  const dispatch = useAppDispatch();
  useEffect(() => {
    if (effectRan.current) return; // skip the StrictMode double-invoke

    axios
      .post("/api/users/refresh", {}, { withCredentials: true })
      .then(({ data }) => {
        dispatch(setAccessToken(data.accessToken));
        dispatch(setUser(data.user));
      })
      .catch(() => {
        dispatch(logout());
      }) // no valid session, fine
      .finally(() => dispatch(setAuthCheckComplete(true)));

    return () => {
      effectRan.current = true;
    };
  }, []);

  if (!authCheckComplete) {
    return <div>Loading...</div>; // nothing else mounts until this resolves
  }

  return (
    <>
      <Router />

      <Toaster richColors position="top-right" />
    </>
  );
}

export default App;
