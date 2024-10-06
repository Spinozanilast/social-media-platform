export interface User {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    phoneNumber: string;
    profilePicture: string;
    roles?: string[];
}
