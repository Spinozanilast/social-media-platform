export type RegisterRequest = {
    username: string;
    firstName: string;
    lastName: string;
    email: string;
    password: string;
};

export type LoginRequest = {
    email: string;
    password: string;
};

export enum FieldId {
    Username = "username",
    Email = "email",
    Password = "password",
    FirstName = "firstName",
    LastName = "lastName",
    ConfirmPassword = "confirmPassword",
}

export type UserApiResponse = {
    isSuccesfully: boolean;
    errorField: FieldId;
    errors: string[];
};
