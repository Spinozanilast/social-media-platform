import { ApiConfig } from "../types/apiConfig";

export default class ApiConfigManager {
    static getUserApiConfig(): ApiConfig {
        const baseUrl = process.env.NEXT_PUBLIC_SERVER_URL;
        return {
            baseURL: baseUrl,
        };
    }
}
