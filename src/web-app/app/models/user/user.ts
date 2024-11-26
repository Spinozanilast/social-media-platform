export interface User {
    id: string;
    firstName: string;
    lastName: string;
    username: string;
    email: string;
    phoneNumber: string;
    roles?: string[];
}

export interface Profile {
    about: string;
    anything?: string;
    birthDate?: Date;
    country?: string;
    interests: string[];
    refs: string[];
}

export interface UserData {
    profileInfo: Profile;
    avatar: string;
}
