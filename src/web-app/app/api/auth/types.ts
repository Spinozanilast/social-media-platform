import { boolean, object, string, z } from 'zod';

/*
----------------------------------------- Register contracts
*/

export const RegisterSchema = object({
    firstName: string()
        .min(1, 'First name is required')
        .max(32, 'First name must be less than 32 characters'),
    lastName: string()
        .min(1, 'Last name is required')
        .max(32, 'Last name must be less than 32 characters'),
    userName: string()
        .min(1, 'Username is required')
        .max(64, 'Username must be less than 64 characters'),
    email: string().email('Email is invalid'),
    password: string()
        .min(8, 'Password must be more than 8 characters')
        .max(100, 'Password must be less than 100 characters'),
    confirmPassword: string(),
}).refine((data) => data.password === data.confirmPassword, {
    path: ['confirmPassword'],
    message: "Passwords don't match",
});

export type RegisterRequest = z.infer<typeof RegisterSchema>;
export type RegisterSchemaKeys = keyof RegisterRequest;

export interface RegisterResponse {
    userId: string;
    email: string;
}

export type RegisterErrorResponse = {
    errors: Map<RegisterSchemaKeys, string[]>;
};

export type RegisterResult =
    | { success: true; data: RegisterResponse }
    | { success: false; result: RegisterErrorResponse };

/*
----------------------------------------- Login Contracts
*/

export const LoginSchema = object({
    email: string().email('Email is invalid'),
    password: string()
        .min(8, 'Password must be more than 8 characters')
        .max(100, 'Password must be less than 100 characters'),
    rememberMe: boolean().default(true),
});

export type LoginRequest = z.infer<typeof LoginSchema>;

export interface AuthResponse {
    userId: string;
    userName: string;
    email: string;
    roles: string[];
    accessTokenExpiry: string;
}

/*
----------------------------------------- Device Info's
*/

export interface DeviceInfoResponse {
    deviceId: string;
    deviceName: string;
    ipAddress: string;
    createdAt: string;
    expires: string;
}

export interface RevokeDeviceRequest {
    deviceId: string;
}

export interface ProblemDetails {
    title?: string;
    detail?: string;

    [key: string]: unknown;
}

export interface User {
    id: string;
    userName: string;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber?: string;
}
