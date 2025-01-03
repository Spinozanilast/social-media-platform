import { ApiConfig } from '../types/apiConfig';

export default class ApiConfigManager {
    static getApiConfig(): ApiConfig {
        const baseUrl = process.env.NEXT_PUBLIC_SERVER_URL;
        return {
            baseURL: baseUrl,
        };
    }
}
