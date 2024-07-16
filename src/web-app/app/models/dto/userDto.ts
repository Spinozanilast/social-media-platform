export type RegisterRequest = {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
};

export type LoginRequest = {
    email: string;
    password: string;
};

export type AuthorizeResponse = {
    isSuccesfully: boolean;
    errors: string[];
};
