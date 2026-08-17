// hooks/useApiMutation.ts
import { useCallback, useState } from "react";
import { handleApiError } from "@/lib/utils";
import type { ApiError, ApiErrorResponse } from "@/types/api.types";

type MutationFn<TArgs, TResult> = (args: TArgs) => Promise<TResult>;

interface MutationCallbacks<TResult> {
  onSuccess?: (data: TResult) => void;
  onError?: (error: ApiError[]) => void;
  onSettled?: () => void;
}

interface UseApiMutationResult<TArgs, TResult> {
  mutate: (
    args: TArgs,
    callbacks?: MutationCallbacks<TResult>,
  ) => Promise<TResult | undefined>;
  isPending: boolean;
  error: ApiError[] | null;
  reset: () => void;
}

export function useApiMutation<TArgs, TResult>(
  mutationFn: MutationFn<TArgs, TResult>,
): UseApiMutationResult<TArgs, TResult> {
  const [error, setError] = useState<ApiError[] | null>(null);
  const [isPending, setIsPending] = useState(false);

  const mutate = useCallback(
    async (args: TArgs, callbacks?: MutationCallbacks<TResult>) => {
      setIsPending(true);
      setError(null);
      try {
        const data = await mutationFn(args);
        callbacks?.onSuccess?.(data);
        return data;
      } catch (response) {
        const apiErrorResponse = response as ApiErrorResponse;
        setError(handleApiError(apiErrorResponse));
        const apiErrors = handleApiError(apiErrorResponse);
        setError(apiErrors);
        callbacks?.onError?.(apiErrors);
        return undefined;
      } finally {
        setIsPending(false);
        callbacks?.onSettled?.();
      }
    },
    [mutationFn],
  );

  const reset = useCallback(() => setError(null), []);

  return { mutate, isPending, error, reset };
}
