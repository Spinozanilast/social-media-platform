'use client';
import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { ThemeProvider } from '@mui/material/styles';
import { theme } from '@app/themes/main-dark';
import { AuthTextField } from '@themes/mui-components/AuthTextField';
import '@fontsource/share-tech';
import { useForm, SubmitHandler } from 'react-hook-form';
import { object, string, TypeOf } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect } from 'react';
import { FieldId } from '@models/user/util';
import { UserApiResponse } from '@models/user/util';
import { useRouter } from 'next/navigation';
import UserService from '@/app/api/services/user';

const registerSchema = object({
    username: string()
        .min(1, 'Username is required')
        .max(64, 'Username must be less than 64 characters'),
    firstName: string()
        .min(1, 'Email is required')
        .max(32, 'Password ust be less 32 than characters'),
    lastName: string()
        .min(1, 'Email is required')
        .max(32, 'Password ust be less 32 than characters'),
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
    parentSm?: number;
};

const pageTextFieldsData: TextFieldData[] = [
    {
        id: FieldId.Username,
        label: 'Username',
        autoComplete: 'username',
        type: 'text',
    },
    {
        id: FieldId.FirstName,
        label: 'First Name',
        autoComplete: 'given-name',
        type: 'text',
        parentSm: 6,
    },
    {
        id: FieldId.LastName,
        label: 'Last Name',
        autoComplete: 'family-name',
        type: 'text',
        parentSm: 6,
    },
    {
        id: FieldId.Email,
        label: 'Email Address',
        autoComplete: 'email',
        type: 'text',
    },
    {
        id: FieldId.Password,
        label: 'Password',
        autoComplete: 'new-password',
        type: 'password',
    },
    {
        id: FieldId.ConfirmPassword,
        label: 'Confirm Password',
        autoComplete: '',
        type: 'password',
    },
];

type RegisterInput = TypeOf<typeof registerSchema>;

export default function RegisterPage() {
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

    const router = useRouter();

    useEffect(() => {
        if (isSubmitSuccessful) {
            reset();
            router.push('/login');
        }
    }, [isSubmitSuccessful, reset]);

    const onSubmit: SubmitHandler<RegisterInput> = async (
        data: RegisterInput
    ) => {
        const response: UserApiResponse = await UserService.registerUser(data);
        if (!response.isSuccess && response.errors.length !== 0) {
            console.log(response);
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
        <ThemeProvider theme={theme}>
            <Container component="main" maxWidth="xs">
                <CssBaseline />
                <Box
                    sx={{
                        marginTop: 2,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                    }}
                >
                    <Avatar
                        sx={{
                            m: 1,
                            bgcolor: theme.palette.secondary.main,
                            color: theme.palette.text.primary,
                        }}
                    >
                        <LockOutlinedIcon />
                    </Avatar>
                    <Typography
                        className="text-3xl my-2"
                        fontFamily="Share Tech"
                        color={theme.palette.text.primary}
                    >
                        Register
                    </Typography>
                    <Box
                        className="bg-background-secondary p-8 rounded-xl "
                        component="form"
                        onSubmit={handleSubmit(onSubmit)}
                        sx={{ mt: 3 }}
                    >
                        <Grid container spacing={2}>
                            {pageTextFieldsData.map((fieldData) => (
                                <Grid
                                    item
                                    xs={12}
                                    sm={fieldData.parentSm === 6 ? 6 : 12}
                                    key={fieldData.id}
                                >
                                    <AuthTextField
                                        required
                                        fullWidth
                                        id={fieldData.id}
                                        label={fieldData.label}
                                        type={fieldData.type}
                                        error={!!errors[fieldData.id]}
                                        helperText={
                                            errors[fieldData.id] &&
                                            errors[fieldData.id]!.message
                                        }
                                        {...register(fieldData.id)}
                                    />
                                </Grid>
                            ))}
                        </Grid>
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{ mt: 3, mb: 2 }}
                        >
                            Sign up
                        </Button>
                        <Grid container justifyContent="flex-end">
                            <Grid item>
                                <Link href="/login" variant="body2">
                                    Already have an account? Sign in
                                </Link>
                            </Grid>
                        </Grid>
                    </Box>
                </Box>
            </Container>
        </ThemeProvider>
    );
}
