"use client";
import Typography from "@mui/material/Typography";
import Link from "next/link";
import { theme } from "@themes/main-dark";

export default function Copyright(props: any) {
    return (
        <Typography
            fontFamily="Share Tech Mono"
            variant="body2"
            color={theme.palette.text.secondary}
            align="center"
            {...props}
        >
            {"Copyright © "}
            <Link color="inherit" href="https://github.com/spinozanilast">
                Platform
            </Link>{" "}
            {new Date().getFullYear()}
            {"."}
        </Typography>
    );
}
