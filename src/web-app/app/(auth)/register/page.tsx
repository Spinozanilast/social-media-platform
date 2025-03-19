import { useTranslations } from 'next-intl';
import {
    FormField,
    RegisterFormFields,
    TranslatedFields,
} from '@app/(auth)/register/types';
import ClientRegisterForm from '@app/(auth)/register/ClientRegisterForm';

export default function RegisterPage() {
    const t = useTranslations('RegisterPage');

    const translatedFields: TranslatedFields[] =
        RegisterFormFields.map<TranslatedFields>((field) => {
            const fieldData: FormField = {
                id: field.id,
                autoComplete: field.autoComplete,
                type: field.type,
                width: field.width,
            };

            return {
                data: fieldData,
                label: t(field.translationKey),
                placeholder: t(field.translationKey + '_placeholder'),
            };
        });

    const translations = {
        register: t('register'),
        have_account: t('have_account'),
        sign_in: t('sign_in'),
        create_account: t('create_account'),
        password: t('password'),
        password_placeholder: t('password_placeholder'),
        confirm_password: t('confirm_password'),
        confirm_password_placeholder: t('confirm_password_placeholder'),
    };

    return (
        <ClientRegisterForm
            translations={translations}
            formFields={translatedFields}
        />
    );
}
