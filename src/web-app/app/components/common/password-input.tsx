'use client';
import { Input, InputProps } from '@heroui/react'; // Import InputProps from the library
import { Eye, EyeClosed } from 'lucide-react';
import { useState, forwardRef } from 'react';

type PasswordInputProps = InputProps & {
    errors?: string;
};

const PasswordInput = forwardRef<HTMLInputElement, PasswordInputProps>(
    ({ errors, ...props }, ref) => {
        const [isPasswordVisible, setPasswordVisibility] = useState(false);
        const errorsSeparated = errors?.split('\n');
        return (
            <Input
                {...props}
                ref={ref}
                type={isPasswordVisible ? 'text' : 'password'}
                isInvalid={!!errors}
                errorMessage={() =>
                    errorsSeparated && (
                        <ul>
                            {errorsSeparated.map((error, i) => (
                                <li key={i}>{error}</li>
                            ))}
                        </ul>
                    )
                }
                endContent={
                    <button
                        type="button"
                        onClick={() =>
                            setPasswordVisibility(!isPasswordVisible)
                        }
                        className="focus:outline-none"
                    >
                        {isPasswordVisible ? (
                            <EyeClosed className="h-5 w-5 text-gray-400" />
                        ) : (
                            <Eye className="h-5 w-5 text-gray-400" />
                        )}
                    </button>
                }
            />
        );
    }
);

PasswordInput.displayName = 'PasswordInput';

export default PasswordInput;
