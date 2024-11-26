import { ErrorOption } from 'react-hook-form';

export type LoginRequest = {
    email: string;
    password: string;
    rememberMe: boolean;
};

export type LoginResponse = {
    id: string;
    firstName: string;
    lastName: string;
    username: string;
    roles?: string[];
};

export type IsErrorResult = {
    isError: boolean;
};

export type LoginErrorResult = ErrorOption & IsErrorResult;
