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
} from './types';
import { AxiosError } from 'axios';
import UserStorage from '~api/storage/user';

const API_VERSION = '1.0';
const BASE_PATH = `/api/v${API_VERSION}/auth`;

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

    /**
     * Returns currently authenticated user (if there is not the one then returns null)
     * @param {string?} cookie param for using function in server components (they doesnt have direct access to cookies)
     * @returns {User | null}
     */
    async getCurrentUser(cookie?: string): Promise<User | null> {
        try {
            const headers: HeadersInit = {};
            if (cookie) {
                headers['Cookie'] = cookie;
            }

            const response = await fetch(
                apiClient.getUri() +
                    authEndpoints.currentUser({ isUserResponse: true }),
                {
                    headers: headers,
                    method: 'POST',
                    credentials: 'include',
                }
            );

            if (response.ok) {
                return (await response.json()) as User;
            }

            return null;
        } catch (error) {
            return null;
        }
    },

    /**
     * Returns if the user is authorized or not
     * @param {number} cookie param for using function in server components (they doesnt have direct access to cookies)
     * @returns {boolean}
     */
    async getAuthenticated(cookie?: string): Promise<boolean> {
        try {
            const headers: HeadersInit = {};
            if (cookie) {
                headers['Cookie'] = cookie;
            }

            const response = await fetch(
                authEndpoints.currentUser({ isUserResponse: false }),
                {
                    headers: headers,
                    method: 'POST',
                    credentials: 'include',
                }
            );

            return response.ok;
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
