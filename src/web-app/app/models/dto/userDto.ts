export type RegisterRequest = {
    username: string;
    firstName: string;
    lastName: string;
    email: string;
    password: string;
};

export type LoginRequest = {
    email: string;
    password: string;
};

export type UserApiResponse = {
    isSuccesfully: boolean;
    errors: string[];
};
