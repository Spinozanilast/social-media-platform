import axios, {
    AxiosError,
    AxiosInstance,
    AxiosRequestConfig,
    AxiosResponse,
    isAxiosError,
} from "axios";
import { ApiConfig } from "./types/apiConfig";
import { RegisterRequest, UserApiResponse } from "@models/dto/userDto";

export default class UserApi {
    public config: ApiConfig;
    private apiClient: AxiosInstance;

    private readonly userApiEndpoints = {
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
        config = {
            validateStatus: (status) => status < 500,
            url: this.userApiEndpoints.register,
            method: "POST",
            data: data,
            ...config,
        };

        let response: AxiosResponse;
        try {
            response = await this.request<T, R, D>(config);
            if (response.status >= 400) {
                return response.data as UserApiResponse;
            }
        } catch (error) {
            console.error(error);
        }
        return { isSuccesfully: false } as UserApiResponse;
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
}
