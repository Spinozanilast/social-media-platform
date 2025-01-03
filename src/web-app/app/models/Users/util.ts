export enum FieldId {
    Username = 'username',
    Email = 'email',
    Password = 'password',
    FirstName = 'firstName',
    LastName = 'lastName',
    ConfirmPassword = 'confirmPassword',
}

export type UserApiResponse = {
    isSuccess: boolean;
    errorFields: FieldId[];
    errors: string[];
};
