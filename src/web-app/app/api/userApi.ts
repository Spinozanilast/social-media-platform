import axios, {
    AxiosError,
    AxiosInstance,
    AxiosRequestConfig,
    AxiosResponse,
} from "axios";
import { ApiConfig } from "./types/apiConfig";
import { RegisterRequest } from "@models/user/register";
import { UserApiResponse } from "@models/user/util";
import { LoginRequest, LoginResponse } from "@models/user/login";

export default class UserApi {
    public config: ApiConfig;
    private apiClient: AxiosInstance;

    private readonly Endpoints = {
        register: "user/register",
        login: "user/login",
    };

    constructor(config: ApiConfig) {
        this.config = config;
        this.apiClient = axios.create({
            ...config,
            headers: {
                "Content-Type": "application/json",
            },
        });
    }

    public async registerUser<
        T extends UserApiResponse = UserApiResponse,
        R extends AxiosResponse<T> = AxiosResponse<T>,
        D extends RegisterRequest = RegisterRequest
    >(data: D, config?: AxiosRequestConfig<D>): Promise<UserApiResponse> {
        config = this.prepareConfig<D>(
            config,
            data,
            this.Endpoints.register,
            "POST"
        );

        let response: AxiosResponse;
        try {
            response = await this.request<T, R, D>(config);
            if (response.status >= 400) {
                return response.data as UserApiResponse;
            }
        } catch (error) {
            console.error(error);
        }
        return { isSuccesfully: true } as UserApiResponse;
    }

    public async loginUser<
        T extends LoginResponse = LoginResponse,
        R extends AxiosResponse<T> = AxiosResponse<T>,
        D extends LoginRequest = LoginRequest
    >(
        data: D,
        config?: AxiosRequestConfig<D>
    ): Promise<LoginResponse | UserApiResponse> {
        config = this.prepareConfig<D>(
            config,
            data,
            this.Endpoints.login,
            "POST"
        );

        try {
            const response = await this.request<T, R, D>(config);
            return response.data;
        } catch (error) {
            console.error(error);
            return { isSuccesfully: false } as UserApiResponse;
        }
    }

    public request<T = any, R = AxiosResponse<T>, D = any>(
        config: AxiosRequestConfig<D>
    ): Promise<R> {
        return this.apiClient
            .request<T, R, D>(config)
            .then((response) => response)
            .catch((error: Error | AxiosError) => {
                return Promise.reject(error.name);
            });
    }

    private prepareConfig<D>(
        config: AxiosRequestConfig<D> | undefined,
        data: D,
        url: string,
        method: string
    ) {
        config = {
            validateStatus: (status) => status < 500,
            url: url,
            method: method,
            data: data,
            ...config,
        };
        return config;
    }
}
