export type LoginRequest = {
    email: string;
    password: string;
};

export type LoginResponse = {
    id: string;
    firstName: string;
    lastName: string;
    username?: string;
    token: string;
};
