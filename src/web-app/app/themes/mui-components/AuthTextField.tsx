import { alpha, styled } from "@mui/material";
import TextField, { TextFieldProps } from "@mui/material/TextField";
export const AuthTextField = styled((props: TextFieldProps) => (
    <TextField {...props} />
))(({ theme }) => ({
    "& .MuiFilledInput-root": {
        overflow: "hidden",
        borderRadius: 4,
        border: "1px solid",
        borderColor:
            theme.palette.mode === "light"
                ? //TODO: Color themes applying
                  theme.palette.text.primary
                : theme.palette.text.secondary,
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
