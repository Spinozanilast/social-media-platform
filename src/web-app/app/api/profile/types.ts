/*
----------------------------------------- Profile
*/

export type SaveProfileRequest = {
    about: string;
    anything: string;
    birthDate: Date;
    country: Country;
};

export type Country = {
    id: number;
    isoCode: string;
    name: string;
};

export type Profile = {
    userId: string;
    about: string;
    birthDate?: string;
    country?: Country;
    anything?: string;
    interests?: string[];
    references?: string[];
};
