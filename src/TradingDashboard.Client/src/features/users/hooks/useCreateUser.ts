import { useCallback,  useState } from "react";
import { createUser } from "../api/usersApi";
import type { CreateUserRequest } from "../types/user.type";


export function useCreateUser(onSuccess?: () => void) {

    const [error, setError] = useState<string | null>(null);
    const [isCreating, setIsCreating] = useState(false);

    
    const create = useCallback(async (user: CreateUserRequest): Promise<boolean> => {
        setIsCreating(true);
        setError(null);
        try {
            await createUser(user);
            onSuccess?.();
            return true;
        } catch (err: unknown) {
            const message = err instanceof Error ? err.message : 'Failed to create user';
            setError(message);
            return false;
        } finally {
            setIsCreating(false);
        }
    }, [onSuccess]);


    return { error, isCreating, execute : create };

}