import axios from 'axios';
import AuthService from '@api/auth/service';

const apiClient = axios.create({
    baseURL: process.env.NEXT_PUBLIC_SERVER_URL!,
    headers: {
        'Content-Type': 'application/json',
        Accept: 'application/json',
    },
    timeout: 10000,
});

export default apiClient;
