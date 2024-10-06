import axios, { AxiosError, AxiosResponse } from 'axios';
import ApiConfigManager from '../util/configManager';
import { UserApiResponse } from '@models/user/util';
import { RegisterRequest } from '@models/user/register';
import { LoginRequest, LoginResponse } from '@models/user/login';
import { User } from '@models/user/user';

const USER_API_KEYWORD = 'user';
const USER_API_URL: string = ApiConfigManager.getUserApiConfig().baseURL!;
const USER_API_BASE_URL = USER_API_URL + `/${USER_API_KEYWORD}`;

const axiosInstance = axios.create({
    baseURL: USER_API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
    withCredentials: true,
});

const userApiEndpoints = {
    login: '/login',
    register: '/register',
    logout: '/logout',
};

const registerUser = (values: RegisterRequest) => {
    return axiosInstance
        .post<UserApiResponse>(userApiEndpoints.register, values)
        .then(() => {
            return { isSuccess: true } as UserApiResponse;
        })
        .catch((error: AxiosError) => {
            return error.response?.data as UserApiResponse;
        });
};

const loginUser = (values: LoginRequest) => {
    return axiosInstance
        .post<LoginResponse | UserApiResponse>(userApiEndpoints.login, values)
        .then((response) => {
            const userData: LoginResponse = response.data as LoginResponse;
            saveUserLocally(userData as User);
            return userData;
        })
        .catch(() => {
            return { isSuccess: false } as UserApiResponse;
        });
};

const logOut = () => {
    removeLocalUser();
    return axiosInstance
        .post<UserApiResponse>(userApiEndpoints.logout)
        .then(() => {
            return { isSuccess: true } as UserApiResponse;
        })
        .catch(() => {
            return { isSuccess: false } as UserApiResponse;
        });
};

const getCurrentUser = (): User | undefined => {
    if (localStorage.getItem(USER_API_KEYWORD)) {
        return JSON.parse(
            localStorage.getItem(USER_API_KEYWORD) as string
        ) as User;
    }
};

const saveUserLocally = (user: User): void => {
    if (localStorage.getItem(USER_API_KEYWORD)) {
        localStorage.removeItem(USER_API_KEYWORD);
    }
    localStorage.setItem(USER_API_KEYWORD, JSON.stringify(user));
};

const removeLocalUser = (): void => {
    if (localStorage.getItem(USER_API_KEYWORD)) {
        localStorage.removeItem(USER_API_KEYWORD);
    }
};

const UserService = {
    registerUser,
    loginUser,
    logOut,
    getCurrentUser,
    saveUserLocally,
    removeLocalUser,
};

export default UserService;
