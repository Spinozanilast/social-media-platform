import axios from 'axios';
import ApiConfigManager from '../util/configManager';
import Profile from '@models/Profiles/profile';
import Country from '@models/Profiles/country';

const USER_API_KEYWORD = 'profiles';
const GATEWAY_URL: string = ApiConfigManager.getApiConfig().baseURL!;
const PROFILE_API_BASE_URL = GATEWAY_URL + `/${USER_API_KEYWORD}`;

const axiosInstance = axios.create({
    baseURL: PROFILE_API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
    withCredentials: true,
});

enum ProfileApiEndpoints {
    GetProfileData = '',
    GetProfileImage = 'image',
    UpdateProfile = 'update',
    UploadProfileImage = 'image/upload',
}

const getProfileImage = async (userId: string): Promise<Blob> => {
    const response = await fetch(
        createUserProfileUrl(userId, ProfileApiEndpoints.GetProfileImage)
    );
    if (!response.ok) {
        throw new Error('Failed to fetch profile image');
    }
    const imageBlob: Blob = await response.blob();
    return imageBlob;
};

const getProfileData = async (userId: string): Promise<Profile | null> => {
    const response = await fetch(createUserProfileUrl(userId), {
        next: { revalidate: 0 },
    });

    if (response.status === 404) {
        return null;
    }
    return response.json();
};

const updateProfile = async (
    profile: Profile,
    userId: string
): Promise<void> => {
    await axiosInstance.put(
        createUserProfileUrl(userId, ProfileApiEndpoints.UpdateProfile),
        profile
    );
};

const uploadProfileImage = async (
    profileImage: Blob,
    userId: string
): Promise<void> => {
    const formData = new FormData();
    formData.append('profileImage', profileImage);
    formData.append('userId', userId);
    await axiosInstance.post(
        createUserProfileUrl(userId, ProfileApiEndpoints.UploadProfileImage),
        formData,
        {
            headers: { 'Content-Type': 'multipart/form-data' },
        }
    );
};

const getCountries = async (): Promise<Country[]> => {
    const response = await fetch(`${PROFILE_API_BASE_URL}/countries`, {
        cache: 'force-cache',
    });
    console.log(response);
    return response.json();
};

const createUserProfileUrl = (
    userId: string,
    endpoint?: ProfileApiEndpoints
) => {
    if (!endpoint) {
        return `${PROFILE_API_BASE_URL}/${userId}`;
    }

    return `${PROFILE_API_BASE_URL}/${userId}/${endpoint}`;
};

const ProfileService = {
    getProfileImage,
    getProfileData,
    updateProfile,
    uploadProfileImage,
    getCountries,
};

export default ProfileService;
