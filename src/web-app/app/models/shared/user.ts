import { IdentityUser } from "./identity-user";
import { ProfilePicture } from "./profile-picture";

export interface User extends IdentityUser {
    firstName: string;
    lastName: string;
    profilePicture: ProfilePicture;
}
