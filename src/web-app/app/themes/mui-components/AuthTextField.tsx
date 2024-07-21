import { alpha, styled } from "@mui/material";
import TextField, { TextFieldProps } from "@mui/material/TextField";
import { forwardRef } from "react";

const StyledTextField = styled(TextField)(({ theme }) => ({
    "& .MuiFilledInput-root": {
        overflow: "hidden",
        borderRadius: 4,
        backgroundColor:
            theme.palette.mode === "light"
                ? theme.palette.secondary.main
                : theme.palette.secondary.main,
        border: "1px solid",
        transition: theme.transitions.create([
            "border-color",
            "background-color",
            "box-shadow",
        ]),
        "&:hover": {
            backgroundColor: "transparent",
        },
        "&.Mui-focused": {
            backgroundColor: "transparent",
            boxShadow: `${alpha(theme.palette.primary.main, 0.25)} 0 0 0 2px`,
            borderColor: theme.palette.primary.main,
        },
    },
}));

// eslint-disable-next-line react/display-name
export const AuthTextField = forwardRef<HTMLInputElement, TextFieldProps>(
    (props, ref) => (
        <StyledTextField
            InputProps={{ disableUnderline: true }}
            variant="filled"
            ref={ref}
            {...props}
        />
    )
);
