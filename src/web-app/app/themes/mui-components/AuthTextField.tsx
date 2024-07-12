import { alpha, OutlinedInputProps, styled } from "@mui/material";
import TextField, { TextFieldProps } from "@mui/material/TextField";

export const AuthTextField = styled((props: TextFieldProps) => (
    <TextField {...props} />
))(({ theme }) => ({
    "& .MuiFilledInput-root": {
        overflow: "hidden",
        borderRadius: 4,
        backgroundColor:
            theme.palette.mode === "light"
                ? theme.palette.secondary.main
                : theme.palette.secondary.main,
        transition: theme.transitions.create([
            "border-color",
            "background-color",
            "box-shadow",
        ]),
        "&.Mui-focused": {
            backgroundColor: "transparent",
            boxShadow: `${alpha(theme.palette.primary.main, 0.25)} 0 0 0 2px`,
            borderColor: theme.palette.primary.main,
        },
    },
}));
