'use client';

import { ProblemDetails } from './types';

export class ApiError extends Error {
    public readonly status: number;
    public readonly details?: unknown;
    public readonly code?: string;

    constructor(
        message: string,
        status: number,
        details?: unknown,
        code?: string
    ) {
        super(message);
        this.name = 'ApiError';
        this.status = status;
        this.details = details;
        this.code = code;
    }

    static fromResponse(error: unknown): ApiError {
        if (typeof error !== 'object' || error === null) {
            return new ApiError('Unknown error occurred', 500);
        }

        const axiosError = error as {
            isAxiosError?: boolean;
            response?: {
                status: number;
                data: ProblemDetails;
            };
            message?: string;
        };

        if (!axiosError.isAxiosError) {
            return new ApiError('Network error occurred', 0);
        }

        const response = axiosError.response?.data;

        return new ApiError(
            response?.title || 'Request failed',
            axiosError.response?.status || 500,
            response?.detail
        );
    }
}

export const isApiError = (error: unknown): error is ApiError => {
    return error instanceof ApiError;
};
