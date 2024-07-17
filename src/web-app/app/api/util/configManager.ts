import { ApiConfig } from "../types/apiConfig";

export default class ApiConfigManager {
    static getApiConfig(): ApiConfig {
        const baseUrl = process.env.NEXT_PUBLIC_SERVER_URL;
        console.log("process.env.SERVER_URL is " + baseUrl);
        return {
            baseURL: baseUrl,
            withCredentials: true,
        };
    }
}
