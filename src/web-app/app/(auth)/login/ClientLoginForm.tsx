'use client';

import React from 'react';
import { Button, Input, Checkbox, Link } from '@heroui/react';

import { useRouter } from 'next/navigation';
import { useState } from 'react';
import { SubmitHandler, useForm } from 'react-hook-form';
import { LockOpen } from 'lucide-react';
import PasswordInput from '@/app/components/special/PasswordInput';
import { LoginRequest, LoginSchema } from '@api/auth/types';
import AuthService from '@api/auth/service';
import { zodResolver } from '@hookform/resolvers/zod';

interface ClientLoginFormProps {
    translations: {
        login: string;
        email: string;
        email_placeholder: string;
        password: string;
        password_placeholder: string;
        remember_me: string;
        forgot_password: string;
        sign_in: string;
        create_account: string;
        error_user_exists_or_not_found: string;
    };
}

export default function ClientLoginForm(props: ClientLoginFormProps) {
    const t = props.translations;

    const router = useRouter();
    const [isSubmitting, setIsSubmitting] = useState(false);

    const {
        register,
        handleSubmit,
        formState: { errors },
        setError,
        watch,
    } = useForm<LoginRequest>({
        resolver: zodResolver(LoginSchema),
        mode: 'onChange',
        defaultValues: {
            rememberMe: true,
        },
    });

    const rememberMe = watch('rememberMe');

    const onSubmit: SubmitHandler<LoginRequest> = async (data) => {
        setIsSubmitting(true);
        try {
            const response = await AuthService.login(data);
            router.push(`/${response.userName}`);
        } catch (error) {
            console.error(error);
            setError('root', {
                message: t.error_user_exists_or_not_found,
            });
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="flex items-center justify-center">
            <div
                className="flex items-center justify-center max-w-sm flex-col gap-4 rounded-large px-8
                    pb-10 pt-6"
            >
                <div className="flex flex-col items-center justify-center">
                    <div className="p-4 text-white bg-accent-orange rounded-full">
                        <LockOpen size={32} />
                    </div>
                    <h1 className="text-3xl my-2 fira-sans">{t.login}</h1>
                </div>
                <form
                    className="flex flex-col gap-4"
                    onSubmit={handleSubmit(onSubmit)}
                >
                    <div className="flex flex-col gap-1">
                        <Input
                            label={t.email}
                            labelPlacement="outside"
                            placeholder={t.email_placeholder}
                            isInvalid={!!errors.email}
                            errorMessage={errors.email?.message}
                            type="email"
                            variant="bordered"
                            className={`${errors.email ? 'border-red-500' : ''}`}
                            {...register('email', {
                                required: 'Email is required',
                            })}
                        />
                    </div>
                    <div className="flex flex-col gap-1">
                        <PasswordInput
                            errors={errors.password?.message}
                            label={t.password}
                            labelPlacement="outside"
                            placeholder={t.password_placeholder}
                            variant="bordered"
                            {...register('password', {
                                required: 'Password is required',
                            })}
                        />
                    </div>
                    {errors.root && (
                        <p className="text-small text-center shadow-sm p-2 text-red-400 shadow-red-500/50 rounded-md">
                            {errors.root.message}
                        </p>
                    )}
                    <div className="flex items-center justify-between px-1 py-2 gap-3">
                        <Checkbox
                            defaultSelected
                            size="sm"
                            isSelected={rememberMe}
                            {...register('rememberMe')}
                        >
                            {t.remember_me}
                        </Checkbox>
                        <Link className="text-default-500" href="#" size="sm">
                            {t.forgot_password}
                        </Link>
                    </div>
                    <Button
                        color="primary"
                        type="submit"
                        isLoading={isSubmitting}
                    >
                        {t.sign_in}
                    </Button>
                </form>
                <p className="text-center text-small">
                    <Link href="/register" size="sm">
                        {t.create_account}
                    </Link>
                </p>
            </div>
        </div>
    );
}
