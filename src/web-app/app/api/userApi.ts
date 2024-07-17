import { LoginRequest, UserApiResponse } from "../models/dto/userDto";
import axios, {
    AxiosInstance,
    AxiosRequestConfig,
    AxiosResponse,
    HttpStatusCode,
    isAxiosError,
} from "axios";
import ApiConfigManager from "./util/configManager";
import { ApiConfig } from "./types/apiConfig";
import { User } from "../models/shared/user";
import apiEndpoints from "../../.history/app/api/endpoints_20240716160112";

export default class UserApi {
    public config: ApiConfig;
    private apiClient: AxiosInstance;

    private readonly userApiEndpoints = {
        register: "user/register",
        login: "user/login",
    };

    constructor(config: ApiConfig) {
        this.config = config;
        this.apiClient = axios.create(config);
    }

    public async registerUser<
        T extends User = User,
        R extends AxiosResponse<T> = AxiosResponse<T>,
        D = { username: string; password: string; [key: string]: any }
    >(data: D, config?: AxiosRequestConfig<D>): Promise<R | void> {
        config = {
            headers: {
                "Content-Type": "application/json",
            },
            url: this.userApiEndpoints.register,
            method: "POST",
            data: data,
            ...config,
        };
        const response = await this.request<T, R, D>(config);
        //this.user.next(response.data);
        return response;
    }

    public request<T = any, R = AxiosResponse<T>, D = any>(
        config: AxiosRequestConfig<D>
    ): Promise<R | void> {
        console.log(config);
        return (
            this.apiClient
                .request<T, R, D>(config)
                .then((response) => response)
                // .catch((error) => this.handleError(error, this.request.bind(this)));
                .catch((error) => console.log(error))
        );
    }
}
