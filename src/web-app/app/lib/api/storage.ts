export type StorageType = 'local' | 'session';

const storage = {
    get<T>(key: string, type: StorageType = 'local'): T | null {
        if (typeof window === 'undefined') return null;
        const value = (
            type === 'local' ? localStorage : sessionStorage
        ).getItem(key);
        return value ? JSON.parse(value) : null;
    },

    set(key: string, value: unknown, type: StorageType = 'local'): void {
        if (typeof window === 'undefined') return;
        (type === 'local' ? localStorage : sessionStorage).setItem(
            key,
            JSON.stringify(value)
        );
    },

    remove(key: string, type: StorageType = 'local'): void {
        if (typeof window === 'undefined') return;
        (type === 'local' ? localStorage : sessionStorage).removeItem(key);
    },
};

export default storage;
