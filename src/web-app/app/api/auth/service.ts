import { apiClient } from '@/app/lib/api/client';
import { RegisterErrorResponse } from './types';
import {
    DeviceInfoResponse,
    RegisterResult,
    RevokeDeviceRequest,
} from './types';
import {
    AuthResponse,
    LoginRequest,
    RegisterRequest,
    RegisterResponse,
    User,
} from './types';
import { ApiError } from '@/app/lib/api/error';
import { AxiosError } from 'axios';

const API_VERSION = '1.0';
const BASE_PATH = `/api/v${API_VERSION}/auth`;

const authEndpoints = {
    register: `${BASE_PATH}/register`,
    login: `${BASE_PATH}/login`,
    logout: `${BASE_PATH}/logout`,
    refreshToken: `${BASE_PATH}/refresh-token`,
    user: (idOrUsername: string) => `${BASE_PATH}/${idOrUsername}`,
    devices: `${BASE_PATH}/devices`,
    revokeDevice: `${BASE_PATH}/devices/revoke`,
};

const AuthService = {
    async register(payload: RegisterRequest): Promise<RegisterResult> {
        try {
            const response = await apiClient.post<
                RegisterResponse | RegisterErrorResponse
            >(authEndpoints.register, payload);

            return {
                success: true,
                data: response.data as RegisterResponse,
            };
        } catch (error) {
            const axiosError = error as AxiosError;
            return {
                success: false,
                result: (axiosError.response
                    ?.data as RegisterErrorResponse) ?? { errors: {} },
            };
        }
    },

    async login(payload: LoginRequest): Promise<AuthResponse> {
        try {
            const { data } = await apiClient.post<AuthResponse>(
                authEndpoints.login,
                payload,
                { withCredentials: true }
            );

            return data;
        } catch (error) {
            throw ApiError.fromResponse(error);
        }
    },

    async logOut(): Promise<void> {
        try {
            await apiClient.post(authEndpoints.logout, null, {
                withCredentials: true,
            });
        } catch (error) {
            throw ApiError.fromResponse(error);
        }
    },

    async refreshToken(): Promise<AuthResponse> {
        try {
            const { data } = await apiClient.post<AuthResponse>(
                authEndpoints.refreshToken,
                null,
                { withCredentials: true }
            );
            return data;
        } catch (error) {
            throw ApiError.fromResponse(error);
        }
    },

    async getUser(idOrUsername: string): Promise<User> {
        try {
            const { data } = await apiClient.get<User>(
                authEndpoints.user(idOrUsername)
            );
            return data;
        } catch (error) {
            throw ApiError.fromResponse(error);
        }
    },

    async getActiveSessions(): Promise<DeviceInfoResponse[]> {
        try {
            const { data } = await apiClient.get<DeviceInfoResponse[]>(
                authEndpoints.devices,
                { withCredentials: true }
            );
            return data;
        } catch (error) {
            throw ApiError.fromResponse(error);
        }
    },

    async revokeDevice(payload: RevokeDeviceRequest): Promise<void> {
        try {
            await apiClient.post(authEndpoints.revokeDevice, payload, {
                withCredentials: true,
            });
        } catch (error) {
            throw ApiError.fromResponse(error);
        }
    },
};
export default AuthService;
