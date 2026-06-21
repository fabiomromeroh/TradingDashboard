import { useCallback, useEffect, useState } from "react";
import { getUsers } from "../api/usersApi";
import type { UserQuery } from "../types/user.type";

interface UseUsersResult {
    users: UserQuery[];
    error: string | null;
    isLoading: boolean;
    refetch: () => void;
};

export function useUsers(): UseUsersResult {

const [users, setUsers] = useState<UserQuery[]>([]);
const [error, setError] = useState<string | null>(null);
const [isLoading, setIsLoading] = useState(false);

    const _getUsers = useCallback(async () => {
        setIsLoading(true);
        await getUsers().then((data) => {
            setUsers(data);
            console.log('Fetched users:', data);
        }).catch((error) => {
            setError(error.message || 'Failed to fetch users');
            console.error('Error fetching users:', error);
        }).finally(() => {
            setIsLoading(false);
        });
    }, []);

    useEffect(() => {
        _getUsers();
    }, [_getUsers]);

    return { users, error, isLoading, refetch: _getUsers };
}