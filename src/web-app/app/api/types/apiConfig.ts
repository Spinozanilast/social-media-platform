import { CreateAxiosDefaults } from "axios";
import { ApiStore } from "./apiStore";

export interface ApiConfig extends CreateAxiosDefaults {
    baseURL?: string;
    headers?: Record<string, string>;
    store?: ApiStore;
    withCredentials?: boolean;
}
