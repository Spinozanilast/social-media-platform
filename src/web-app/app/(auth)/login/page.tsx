'use client';

import React from 'react';
import { Button, Input, Checkbox, Link } from '@nextui-org/react';
import { FaLockOpen } from 'react-icons/fa';

import {
    LoginRequest,
    LoginResponse,
    LoginErrorResult,
} from '@/app/models/Users/login';
import { UserApiResponse } from '@/app/models/Users/util';
import { useRouter } from 'next/navigation';
import { useState } from 'react';
import Identity from '@/app/api/services/user';
import { ErrorOption, SubmitHandler, useForm } from 'react-hook-form';
import { User } from '@/app/models/Users/user';
import isEmailValid from '@/app/helpers/email-validation';
import { GoEyeClosed } from 'react-icons/go';
import { GoEye } from 'react-icons/go';

const handleLogin: SubmitHandler<LoginRequest> = async (
    data: LoginRequest
): Promise<LoginErrorResult | LoginResponse> => {
    console.log(data);
    const response: UserApiResponse | LoginResponse = await Identity.loginUser({
        email: data.email,
        password: data.password,
        rememberMe: true,
    } as LoginRequest);
    if ((response as UserApiResponse).isSuccess !== undefined) {
        const formError: LoginErrorResult = {
            isError: true,
            type: 'server',
            message: 'Email or Password Incorrect',
        };

        return formError;
    }

    return response as LoginResponse;
};

export default function LoginPage() {
    const [rememberUserState, setRememberUserState] = useState(false);
    const [isPasswordVisible, setPasswordVisibility] = useState(false);
    const router = useRouter();
    const {
        register,
        handleSubmit,
        formState: { errors },
        setError,
    } = useForm<LoginRequest>();

    const handleCheckboxChange = (
        event: React.ChangeEvent<HTMLInputElement>
    ) => {
        setRememberUserState(event.target.checked);
    };

    const togglePasswordVisibility = () =>
        setPasswordVisibility(!isPasswordVisible);

    const onSubmit: SubmitHandler<LoginRequest> = async (
        data: LoginRequest
    ) => {
        const result = await handleLogin(data);
        if ((result as LoginErrorResult).isError) {
            const errorOption = result as ErrorOption;
            setError('password', errorOption);
            setError('email', errorOption);
            return;
        }

        const response = result as LoginResponse;

        if (rememberUserState) {
            Identity.saveUserLocally(response as User);
        }
        router.push(`/${response.userName}`);
    };

    return (
        <div className="flex items-center justify-center">
            <div className="flex items-center justify-center max-w-sm flex-col gap-4 rounded-large px-8 pb-10 pt-6">
                <div className="flex flex-col items-center justify-center">
                    <FaLockOpen width={48} />
                    <h1
                        className="text-3xl my-2"
                        style={{ fontFamily: 'Share Tech Mono' }}
                    >
                        Log in
                    </h1>
                </div>
                <form
                    className="flex flex-col gap-4"
                    onSubmit={handleSubmit(onSubmit)}
                >
                    <div className="flex flex-col gap-1">
                        <Input
                            label="Email"
                            labelPlacement="outside"
                            placeholder="Enter your email"
                            isInvalid={!!errors.email}
                            errorMessage={errors.email?.message}
                            type="email"
                            variant="bordered"
                            className={`${
                                errors.email ? 'border-red-500' : ''
                            }`}
                            {...register('email', {
                                validate: (value) => {
                                    const isValid = isEmailValid(value);
                                    if (!isValid) {
                                        return 'Email is not valid';
                                    }
                                    return isValid;
                                },
                                required: 'Email is required',
                            })}
                        />
                    </div>
                    <div className="flex flex-col gap-1">
                        <Input
                            endContent={
                                <button
                                    type="button"
                                    onClick={togglePasswordVisibility}
                                >
                                    {isPasswordVisible ? (
                                        <GoEyeClosed className="pointer-events-none text-2xl text-default-400" />
                                    ) : (
                                        <GoEye className="pointer-events-none text-2xl text-default-400" />
                                    )}
                                </button>
                            }
                            isInvalid={!!errors.password}
                            errorMessage={errors.password?.message}
                            label="Password"
                            labelPlacement="outside"
                            placeholder="Enter your password"
                            type={isPasswordVisible ? 'text' : 'password'}
                            variant="bordered"
                            className={`${
                                errors.password ? 'border-red-500' : ''
                            }`}
                            {...register('password', {
                                required: 'Password is required',
                            })}
                        />
                    </div>
                    <div className="flex items-center justify-between px-1 py-2 gap-3">
                        <Checkbox
                            defaultSelected
                            name="remember"
                            size="sm"
                            onChange={handleCheckboxChange}
                        >
                            Remember me
                        </Checkbox>
                        <Link className="text-default-500" href="#" size="sm">
                            Forgot password?
                        </Link>
                    </div>
                    <Button color="primary" type="submit">
                        Log In
                    </Button>
                </form>
                <p className="text-center text-small">
                    <Link href="/register" size="sm">
                        Create an account
                    </Link>
                </p>
            </div>
        </div>
    );
}
