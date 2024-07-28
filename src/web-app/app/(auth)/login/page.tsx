"use client";
import * as React from "react";
import Avatar from "@mui/material/Avatar";
import Button from "@mui/material/Button";
import CssBaseline from "@mui/material/CssBaseline";
import FormControlLabel from "@mui/material/FormControlLabel";
import Checkbox from "@mui/material/Checkbox";
import Link from "@mui/material/Link";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import Container from "@mui/material/Container";
import { ThemeProvider, Typography } from "@mui/material";
import { AuthTextField } from "@themes/mui-components/AuthTextField";
import { theme } from "@themes/main-dark";
import "@fontsource/share-tech-mono";
import UserApi from "@/app/api/userApi";
import { getUserApi } from "@/app/api/apiManagement";
import { LoginRequest, LoginResponse } from "@/app/models/user/login";
import { ErrorOption, SubmitHandler, useForm } from "react-hook-form";
import { UserApiResponse } from "@/app/models/user/util";
import isEmailValid from "@/app/helpers/email-validation";
import { useEffect, useState } from "react";

export default function SignInPage() {
    const [isSuccess, setLoginSuccessfulity] = useState(true);
    const {
        register,
        handleSubmit,
        formState: { errors },
        setError,
    } = useForm<LoginRequest>();

    const onSubmit: SubmitHandler<LoginRequest> = async (
        data: LoginRequest
    ) => {
        const api: UserApi = getUserApi();
        const response: UserApiResponse | LoginResponse = await api.loginUser(
            data as LoginRequest
        );
        if (!(response as UserApiResponse).isSuccesfully) {
            const formError: ErrorOption = {
                type: "server",
                message: "Email or Password Incorrect",
            };

            setError("email", formError);
            setError("password", formError);
        }
    };

    return (
        <ThemeProvider theme={theme}>
            <Container component="main" maxWidth="xs">
                <CssBaseline />
                <Box
                    sx={{
                        marginTop: 8,
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center",
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
                        Log in
                    </Typography>
                    <Box
                        className="bg-background-secondary p-8 rounded-xl "
                        component="form"
                        onSubmit={handleSubmit(onSubmit)}
                        sx={{ mt: 1 }}
                    >
                        <Grid container gap={1}>
                            <AuthTextField
                                required
                                fullWidth
                                id="email"
                                label="Email Address"
                                autoComplete="email"
                                error={!!errors["email"]}
                                helperText={
                                    errors["email"] && errors["email"]!.message
                                }
                                {...register("email", {
                                    validate: (value) => {
                                        const isValid = isEmailValid(value);
                                        if (!isValid) {
                                            setError("email", {
                                                type: "validate",
                                                message: "Email is not valid",
                                            });
                                        }
                                        return isValid;
                                    },
                                })}
                            />
                            <AuthTextField
                                required
                                fullWidth
                                label="Password"
                                type="password"
                                id="password"
                                autoComplete="current-password"
                                error={!!errors["password"]}
                                helperText={
                                    errors["password"] &&
                                    errors["password"]!.message
                                }
                                {...register("password")}
                            />
                        </Grid>
                        <FormControlLabel
                            control={
                                <Checkbox value="remember" color="primary" />
                            }
                            label="Remember me"
                        />
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{ mt: 3, mb: 2 }}
                        >
                            Log in
                        </Button>
                        <Grid container>
                            <Grid item xs>
                                <Link href="#" variant="body2">
                                    Forgot password?
                                </Link>
                            </Grid>
                            <Grid item>
                                <Link href="/register" variant="body2">
                                    {"Don't have an account? Sign Up"}
                                </Link>
                            </Grid>
                        </Grid>
                    </Box>
                </Box>
            </Container>
        </ThemeProvider>
    );
}
