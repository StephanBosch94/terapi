
import { styled } from "@mui/material/styles";
import { COLORS } from "./constants/ColorsConstatnts";
import TextField from "@mui/material/TextField";

export const TextFieldStyled = styled(TextField)(({ theme }) => ({
    background: COLORS.WHITE,
    border: `1px solid ${(COLORS.BLACK_10)}`,
    borderRadius: "4px",
    boxSizing: "border-box",
    ":hover": {
        borderColor: COLORS.BLACK_40
    },
    "fieldset": {
        border: "none"
    },
    "input": {
        paddingTop: "10px",
        paddingBottom: "10px",
        fontWeight: 400,
        color: COLORS.BLACK
    },
    "& input::-webkit-outer-spin-button, & input::-webkit-inner-spin-button": {
        display: "none",
    },
    "& input[type=number]": {
        MozAppearance: "textfield",
    },
})) as typeof TextField;