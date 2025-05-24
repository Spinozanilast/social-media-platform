import apiClient from '~/lib/api/client';
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
} from '~api/auth/types';
import { AxiosError } from 'axios';
import UserStorage from '~api/storage/user';

const API_VERSION = '1';
const IDENTITY_SERVICE_BASE_PATH = process.env.NEXT_PUBLIC_IDENTITY_SERVICE_URL;
// const BASE_PATH = `${IDENTITY_SERVICE_BASE_PATH}/api/v${API_VERSION}/auth`;
const BASE_PATH = `${IDENTITY_SERVICE_BASE_PATH}`;

type CurrentUserQuery = { isUserResponse: boolean };

const authEndpoints = {
    register: `${BASE_PATH}/register`,
    login: `${BASE_PATH}/login`,
    logout: `${BASE_PATH}/logout`,
    refreshToken: `${BASE_PATH}/refresh-token`,
    user: (idOrUsername: string) => `${BASE_PATH}/${idOrUsername}`,
    devices: `${BASE_PATH}/devices`,
    revokeDevice: `${BASE_PATH}/devices/revoke`,
    currentUser: (query: CurrentUserQuery) =>
        `${BASE_PATH}/me?isUserResponse=${query.isUserResponse}`,
};

const oauthEndpoints = {
    githubSignIn: `${IDENTITY_SERVICE_BASE_PATH}/github/login`,
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
        const { data } = await apiClient.post<AuthResponse>(
            authEndpoints.login,
            payload,
            { withCredentials: true }
        );

        UserStorage.saveUserId(data.userId);
        return data;
    },

    async githubSignIn(): Promise<void> {
        window.location.href = oauthEndpoints.githubSignIn;
    },

    async logOut(): Promise<void> {
        await apiClient.post(authEndpoints.logout, null, {
            withCredentials: true,
        });
    },

    async refreshToken(): Promise<AuthResponse> {
        const { data } = await apiClient.post<AuthResponse>(
            authEndpoints.refreshToken,
            null,
            { withCredentials: true }
        );
        return data;
    },

    async getUser(idOrUsername: string): Promise<User | null> {
        try {
            const response = await apiClient.get<User>(
                authEndpoints.user(idOrUsername)
            );

            if (response && response.status === 200) {
                return response.data as User;
            }

            return null;
        } catch (error) {
            return null;
        }
    },

    async getCurrentUser(): Promise<User | null> {
        try {
            const response = await apiClient.post<User>(
                authEndpoints.currentUser({ isUserResponse: true }),
                null,
                {
                    withCredentials: true,
                }
            );
            return response.data;
        } catch (error) {
            return null;
        }
    },

    async getAuthenticated(cookie?: string): Promise<boolean> {
        try {
            const response = await apiClient.post(
                authEndpoints.currentUser({ isUserResponse: false }),
                null,
                {
                    withCredentials: true,
                }
            );
            return response.status === 200;
        } catch (error) {
            return false;
        }
    },

    async getActiveSessions(): Promise<DeviceInfoResponse[]> {
        const { data } = await apiClient.get<DeviceInfoResponse[]>(
            authEndpoints.devices,
            { withCredentials: true }
        );
        return data;
    },

    async revokeDevice(payload: RevokeDeviceRequest): Promise<void> {
        await apiClient.post(authEndpoints.revokeDevice, payload, {
            withCredentials: true,
        });
    },
};

export default AuthService;
