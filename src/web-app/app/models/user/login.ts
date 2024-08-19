import { ErrorOption } from "react-hook-form";

export type LoginRequest = {
    email: string;
    password: string;
};

export type LoginResponse = {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
};

export type IsErrorResult = {
    isError: boolean;
};

export type LoginErrorResult = ErrorOption & IsErrorResult;
