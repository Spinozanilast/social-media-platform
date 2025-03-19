export type User = {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    phoneNumber?: string;
    roles?: string[];
};

export type UserQueryParams = {
    cache?: 'force-cache' | 'no-cache';
    validateToken?: boolean;
};
