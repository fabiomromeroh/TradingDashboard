import { setTheme } from "../store/store";
import { useAppDispatch, useAppSelector } from "../store/hooks";
import { useEffect } from "react";

export type Theme = "light" | "dark" | "system";

export function useTheme() {
  const dispatch = useAppDispatch();
  const theme = useAppSelector((state) => state.theme.value) as Theme;
  // apply theme to document root so Tailwind's `dark:` styles work
  useEffect(() => {
    const apply = (t: Theme) => {
      let resolved = t;
      if (t === "system") {
        const prefersDark =
          typeof window !== "undefined" &&
          window.matchMedia &&
          window.matchMedia("(prefers-color-scheme: dark)").matches;
        resolved = prefersDark ? "dark" : "light";
      }

      if (resolved === "dark") {
        document.documentElement.classList.add("dark");
      } else {
        document.documentElement.classList.remove("dark");
      }
    };

    apply(theme);

    let mql: MediaQueryList | null = null;
    const handleSystemChange = () => {
      if (theme === "system") {
        apply("system");
      }
    };

    if (
      theme === "system" &&
      typeof window !== "undefined" &&
      window.matchMedia
    ) {
      mql = window.matchMedia("(prefers-color-scheme: dark)");
      const mqlTyped = mql as MediaQueryList & {
        addListener?: (listener: (e: MediaQueryListEvent) => void) => void;
        removeListener?: (listener: (e: MediaQueryListEvent) => void) => void;
      };

      if (mqlTyped.addEventListener)
        mqlTyped.addEventListener(
          "change",
          handleSystemChange as EventListenerOrEventListenerObject,
        );
      else if (mqlTyped.addListener) mqlTyped.addListener(handleSystemChange);
    }

    return () => {
      if (mql) {
        const mqlTyped = mql as MediaQueryList & {
          removeListener?: (listener: (e: MediaQueryListEvent) => void) => void;
        };
        if (mqlTyped.removeEventListener)
          mqlTyped.removeEventListener(
            "change",
            handleSystemChange as EventListenerOrEventListenerObject,
          );
        else if (mqlTyped.removeListener)
          mqlTyped.removeListener(handleSystemChange);
      }
    };
  }, [theme]);

  const handleSetTheme = (newTheme: Theme) => {
    dispatch(setTheme(newTheme));
  };

  return {
    theme,
    setTheme: handleSetTheme,
  };
}
