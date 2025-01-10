import Country from './country';

export default interface Profile {
    about?: string;
    anything?: string;
    birthDate?: Date | string;
    country?: Country;
    interests: string[];
    references: string[];
}

export interface UserData {
    profileInfo: Profile;
    avatar: string;
}
