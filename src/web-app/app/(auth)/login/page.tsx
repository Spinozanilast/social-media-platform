import { getTranslations } from 'next-intl/server';
import ClientLoginForm from '@app/(auth)/login/ClientLoginForm';

export default async function LoginPage() {
    const t = await getTranslations('LoginPage');

    return (
        <ClientLoginForm
            translations={{
                login: t('login'),
                email: t('email'),
                email_placeholder: t('email_placeholder'),
                password: t('password'),
                password_placeholder: t('password_placeholder'),
                remember_me: t('remember_me'),
                forgot_password: t('forgot_password'),
                sign_in: t('sign_in'),
                create_account: t('create_account'),
                error_user_exists_or_not_found: t('error_user_exists_or_not_found'),
            }}
        />
    );
}
