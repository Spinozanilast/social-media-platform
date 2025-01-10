'use client';

import React, { useEffect, useState } from 'react';
import { Button, Input, Spacer, Link } from '@nextui-org/react';
import { useForm, SubmitHandler } from 'react-hook-form';
import { object, string, TypeOf } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { redirect, useRouter } from 'next/navigation';
import Identity from '@/app/api/services/user';
import { UserApiResponse, FieldId } from '@/app/models/Users/util';
import { FaLock } from 'react-icons/fa';
import '@fontsource/share-tech';
import ShowPasswordButton from '@/app/components/utils/ShowPasswordButton';
import { useTranslations } from 'next-intl';

const registerSchema = object({
    username: string()
        .min(1, 'Username is required')
        .max(64, 'Username must be less than 64 characters'),
    firstName: string()
        .min(1, 'First name is required')
        .max(32, 'First name must be less than 32 characters'),
    lastName: string()
        .min(1, 'Last name is required')
        .max(32, 'Last name must be less than 32 characters'),
    email: string().min(1, 'Email is required').email('Email is invalid'),
    password: string()
        .min(8, 'Password must be more than 8 characters')
        .max(64, 'Password must be less than 64 characters'),
    confirmPassword: string(),
}).refine((data) => data.password === data.confirmPassword, {
    path: ['confirmPassword'],
    message: 'Passwords do not match',
});

type TextFieldData = {
    id: FieldId;
    label: string;
    autoComplete: string;
    type: 'text' | 'password' | string;
    inputWidth?: FieldWidth;
};

enum FieldWidth {
    Full = 1,
    Half = 2,
}

const pageTextFieldsData: TextFieldData[] = [
    {
        id: FieldId.Username,
        label: 'username',
        autoComplete: 'username',
        type: 'text',
        inputWidth: FieldWidth.Full,
    },
    {
        id: FieldId.FirstName,
        label: 'first_name',
        autoComplete: 'given-name',
        type: 'text',
        inputWidth: FieldWidth.Half,
    },
    {
        id: FieldId.LastName,
        label: 'last_name',
        autoComplete: 'family-name',
        type: 'text',
        inputWidth: FieldWidth.Half,
    },
    {
        id: FieldId.Email,
        label: 'email',
        autoComplete: 'email',
        type: 'text',
        inputWidth: FieldWidth.Full,
    },
    {
        id: FieldId.Password,
        label: 'password',
        autoComplete: 'new-password',
        type: 'password',
        inputWidth: FieldWidth.Full,
    },
    {
        id: FieldId.ConfirmPassword,
        label: 'confirm_password',
        autoComplete: '',
        type: 'password',
        inputWidth: FieldWidth.Full,
    },
];

const redirectHandler = async () => {
    const localUser = Identity.getCurrentUser();
    if (localUser) {
        const isUserLoggedIn = await Identity.checkUserIdentity(
            localUser.userName
        );
        if (isUserLoggedIn) {
            redirect(`/${localUser.userName}`);
        } else {
            Identity.removeLocalUser();
        }
    }
};

type RegisterInput = TypeOf<typeof registerSchema>;
const RegisterTranslationSection: string = 'RegisterPage';

export default function RegisterPage() {
    const t = useTranslations(RegisterTranslationSection);
    const {
        register,
        formState: { errors, isSubmitSuccessful },
        reset,
        handleSubmit,
        setError,
    } = useForm<RegisterInput>({
        resolver: zodResolver(registerSchema),
        reValidateMode: 'onChange',
    });

    const [isPasswordVisible, setPasswordVisibility] = useState(false);
    const [isConfirmPasswordVisible, setConfirmPasswordVisibility] =
        useState(false);

    const router = useRouter();

    function togglePasswordVisibility() {
        setPasswordVisibility(!isPasswordVisible);
    }

    function toggleConfirmPasswordVisibility() {
        setConfirmPasswordVisibility(!isConfirmPasswordVisible);
    }

    useEffect(() => {
        redirectHandler();

        if (isSubmitSuccessful) {
            reset();
            router.push('/login');
        }
    }, [isSubmitSuccessful, reset, router]);

    const onSubmit: SubmitHandler<RegisterInput> = async (data) => {
        const response: UserApiResponse = await Identity.registerUser(data);
        console.log(response);
        if (!response.isSuccess && response.errors?.length !== 0) {
            response.errorFields.forEach((errorField: FieldId, idx) => {
                setError(
                    errorField,
                    {
                        type: 'value',
                        message: response.errors[idx],
                    },
                    { shouldFocus: true }
                );
            });

            return;
        }
    };

    return (
        <div className="flex flex-col items-center justify-center py-2">
            <div className="w-full max-w-md p-8 space-y-8 rounded-lg shadow-md">
                <div className="flex flex-col items-center justify-center space-y-2">
                    <div className="p-4 text-white bg-accent-orange rounded-full">
                        <FaLock size={32} />
                    </div>
                    <h2
                        className="text-3xl font-extrabold text-center"
                        style={{ fontFamily: 'Share Tech Mono' }}
                    >
                        {t('register')}
                    </h2>
                </div>
                <form className="mt-8" onSubmit={handleSubmit(onSubmit)}>
                    <div className="grid grid-cols-2 gap-y-2 gap-x-2">
                        {pageTextFieldsData.map((fieldData) => (
                            <div
                                key={fieldData.id}
                                className={`
                                    ${
                                        fieldData.type === 'password'
                                            ? 'col-span-2'
                                            : 'col-span-1'
                                    }
                                    ${
                                        fieldData.id === 'confirmPassword'
                                            ? 'col-span-2'
                                            : 'col-span-1'
                                    }
                                    ${
                                        errors[fieldData.id]
                                            ? 'bg-red-950 rounded-xl'
                                            : ''
                                    }
                                `}
                            >
                                <Input
                                    isRequired
                                    isClearable={
                                        !(
                                            fieldData.id === FieldId.Password ||
                                            fieldData.id ===
                                                FieldId.ConfirmPassword
                                        )
                                    }
                                    fullWidth
                                    variant={
                                        fieldData.id === FieldId.Password ||
                                        fieldData.id === FieldId.ConfirmPassword
                                            ? 'flat'
                                            : 'bordered'
                                    }
                                    label={t(fieldData.label)}
                                    placeholder={t(
                                        `${fieldData.label}_placeholder`
                                    )}
                                    type={
                                        fieldData.id === 'password'
                                            ? isPasswordVisible
                                                ? 'text'
                                                : 'password'
                                            : fieldData.id === 'confirmPassword'
                                            ? isConfirmPasswordVisible
                                                ? 'text'
                                                : 'password'
                                            : fieldData.type
                                    }
                                    className={`${
                                        errors[fieldData.id]
                                            ? 'border-red-500'
                                            : ''
                                    }`}
                                    {...register(fieldData.id)}
                                    endContent={
                                        (fieldData.id === FieldId.Password && (
                                            <ShowPasswordButton
                                                isVisible={isPasswordVisible}
                                                onClick={() => {
                                                    togglePasswordVisibility();
                                                }}
                                            />
                                        )) ||
                                        (fieldData.id ===
                                            FieldId.ConfirmPassword && (
                                            <ShowPasswordButton
                                                isVisible={
                                                    isConfirmPasswordVisible
                                                }
                                                onClick={() => {
                                                    toggleConfirmPasswordVisibility();
                                                }}
                                            />
                                        ))
                                    }
                                />
                                {errors[fieldData.id] && (
                                    <p className="mt-2 p-2 text-sm text-red-600">
                                        {errors[fieldData.id]?.message}
                                    </p>
                                )}
                                <Spacer y={0.5} />
                            </div>
                        ))}
                    </div>
                    <Button
                        type="submit"
                        className="mt-4"
                        fullWidth
                        color="primary"
                    >
                        Sign up
                    </Button>
                </form>
                <div className="flex justify-end mt-2">
                    <Link
                        href="/login"
                        className="text-sm text-blue-600 hover:text-blue-500"
                    >
                        Already have an account? Sign in
                    </Link>
                </div>
            </div>
        </div>
    );
}
