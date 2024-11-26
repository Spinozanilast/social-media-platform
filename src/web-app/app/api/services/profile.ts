import axios from 'axios';
import ApiConfigManager from '../util/configManager';
import { Profile } from '@models/user/user';

const USER_API_KEYWORD = 'profile';
const GATEWAY_URL: string = ApiConfigManager.getUserApiConfig().baseURL!;
const USER_API_BASE_URL = GATEWAY_URL + `/${USER_API_KEYWORD}`;

const axiosInstance = axios.create({
    baseURL: USER_API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
    withCredentials: true,
});

enum ProfileApiEndpoints {
    GetProfileData = '',
    GetProfileImage = '/image',
}

const getProfileImage = async (userId: string): Promise<Blob> => {
    const response = await fetch(
        createUserProfileUrl(userId, ProfileApiEndpoints.GetProfileImage),
        {
            cache: 'force-cache',
        }
    );
    const user: Blob = await response.blob();
    return user;
};

const getProfileData = async (userId: string): Promise<Profile> => {
    const response = await fetch(createUserProfileUrl(userId), {
        cache: 'force-cache',
    });
    return response.json();
};

const createUserProfileUrl = (
    userId: string,
    endpoint?: ProfileApiEndpoints
) => {
    if (!endpoint) {
        return `${USER_API_BASE_URL}/${userId}`;
    }

    return `${USER_API_BASE_URL}/${userId}/${endpoint}`;
};

const ProfileService = {
    getProfileImage,
    getProfileData,
};

export default ProfileService;
