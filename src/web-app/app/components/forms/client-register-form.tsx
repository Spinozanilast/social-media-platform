'use client';

import {
    RegisterRequest,
    RegisterResult,
    RegisterSchema,
} from '~api/auth/types';
import { useRouter } from 'next/navigation';
import React, { useState } from 'react';
import { SubmitHandler, useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import AuthService from '~api/auth/service';
import { Lock, SeparatorHorizontal } from 'lucide-react';
import { TranslatedFields } from '~/(auth)/register/types';
import { Button, cn, Divider, Input, Link } from '@heroui/react';
import PasswordInput from '~/components/common/password-input';
import GithubAuthButton from '~/components/github-button';

interface ClientRegisterFormProps {
    translations: {
        register: string;
        have_account: string;
        sign_in: string;
        create_account: string;
        password: string;
        password_placeholder: string;
        confirm_password: string;
        confirm_password_placeholder: string;
    };
    formFields: Array<TranslatedFields>;
}

export default function ClientRegisterForm(props: ClientRegisterFormProps) {
    const { translations, formFields } = props;

    const router = useRouter();
    const [isSubmitting, setIsSubmitting] = useState(false);

    const {
        register,
        handleSubmit,
        setError,
        formState: { errors },
    } = useForm<RegisterRequest>({
        resolver: zodResolver(RegisterSchema),
        mode: 'onChange',
    });

    const onSubmit: SubmitHandler<RegisterRequest> = async (data) => {
        setIsSubmitting(true);
        try {
            const response: RegisterResult = await AuthService.register(data);
            if (response.success) {
                router.push('/login');
            } else {
                Object.entries(response.result.errors).forEach(
                    ([errorField, descriptions]) => {
                        const field = errorField as keyof RegisterRequest;
                        const total = (descriptions as string[]).join('\n');
                        setError(field, { message: total });
                    }
                );
            }
        } catch (error) {
            setError('root', {
                message: 'Registration failed. Please try again.',
            });
            console.log(error);
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="flex items-center justify-center p-4">
            <div className="w-full max-w-md space-y-6 rounded-xl p-8 shadow-lg">
                <div className="flex flex-col items-center gap-4">
                    <div className="rounded-full bg-accent-orange p-3 text-white">
                        <Lock className="h-8 w-8" />
                    </div>
                    <h1
                        className="text-center text-3xl font-bold"
                        style={{ fontFamily: 'Share Tech Mono' }}
                    >
                        {translations.register}
                    </h1>
                </div>

                <form
                    onSubmit={handleSubmit(onSubmit)}
                    className="space-y-4 form-container"
                >
                    <GithubAuthButton />
                    <div className="grid grid-cols-2 gap-4">
                        {formFields.map((field) => (
                            <div
                                key={field.data.id}
                                className={cn({
                                    'col-span-2': field.data.width === 'full',
                                    'col-span-2 sm:col-span-1':
                                        field.data.width === 'half',
                                })}
                            >
                                <Input
                                    {...register(field.data.id)}
                                    label={field.label}
                                    placeholder={field.placeholder}
                                    autoComplete={field.data.autoComplete}
                                    type={field.data.type}
                                    isInvalid={!!errors[field.data.id]}
                                    errorMessage={
                                        errors[field.data.id]?.message
                                    }
                                    fullWidth
                                />
                            </div>
                        ))}

                        <div className="col-span-2 space-y-4">
                            <PasswordInput
                                autoComplete="new-password"
                                label={translations.password}
                                placeholder={translations.password_placeholder}
                                errors={errors.password?.message}
                                {...register('password')}
                            />
                            <PasswordInput
                                autoComplete="new-password"
                                label={translations.confirm_password}
                                placeholder={
                                    translations.confirm_password_placeholder
                                }
                                errors={errors.confirmPassword?.message}
                                {...register('confirmPassword')}
                            />
                        </div>
                    </div>

                    {errors.root && (
                        <p className="text-sm text-red-600">
                            {errors.root.message}
                        </p>
                    )}

                    <Button
                        type="submit"
                        color="primary"
                        className="w-full"
                        isLoading={isSubmitting}
                    >
                        {translations.create_account}
                    </Button>
                </form>

                <div className="flex justify-center gap-2">
                    <span className="text-foreground/70">
                        {translations.have_account}
                    </span>
                    <Link
                        href="/login"
                        className="font-semibold hover:underline"
                    >
                        {translations.sign_in}
                    </Link>
                </div>
            </div>
        </div>
    );
}
