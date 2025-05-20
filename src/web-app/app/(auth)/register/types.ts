import { RegisterSchemaKeys } from '~api/auth/types';

export type FormField = {
    id: RegisterSchemaKeys;
    autoComplete: string;
    type?: 'text' | 'password';
    width?: 'full' | 'half';
};
export type TranslationKey = { translationKey: string };
export type FormFieldsForTranslation = FormField & TranslationKey;

export const RegisterFormFields: FormFieldsForTranslation[] = [
    {
        id: 'userName',
        translationKey: 'username',
        autoComplete: 'username',
        type: 'text',
        width: 'full',
    },
    {
        id: 'firstName',
        translationKey: 'first_name',
        autoComplete: 'given-name',
        type: 'text',
        width: 'half',
    },
    {
        id: 'lastName',
        translationKey: 'last_name',
        autoComplete: 'family-name',
        type: 'text',
        width: 'half',
    },
    {
        id: 'email',
        translationKey: 'email',
        autoComplete: 'email',
        type: 'text',
        width: 'full',
    },
];

export type TranslatedFields = {
    data: FormField;
    label: string;
    placeholder: string;
};
