import Axios from 'axios';
import { setupCache } from 'axios-cache-interceptor';
import { config } from '../../../middleware';

const instance = Axios.create({
    baseURL: process.env.NEXT_PUBLIC_GATEWAY_URL!,
    headers: {
        'Content-Type': 'application/json',
        Accept: 'application/json',
    },
    timeout: 10000,
});

const apiClient = setupCache(instance);

export default apiClient;
