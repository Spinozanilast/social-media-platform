import axios, { AxiosInstance } from 'axios';
import { ApiError } from './error';

const createApiClient = (baseURL: string): AxiosInstance => {
    const client = axios.create({
        baseURL,
        headers: {
            'Content-Type': 'application/json',
            Accept: 'application/json',
        },
        timeout: 10000,
    });

    return client;
};

export const apiClient = createApiClient(process.env.NEXT_PUBLIC_SERVER_URL!);
