
import { styled } from "@mui/material/styles";
import { COLORS } from "./constants/ColorsConstatnts";
import Box from "@mui/material/Box";
import { experimental_sx as sx } from "@mui/material/styles";


export const PhoneInputBoxStyled = styled(Box)(() => sx({
    width: "100%",
    display: "inline-flex",
    border: `1px solid ${(COLORS.BLACK_10)}`,
    background: COLORS.PRIMARY_10,
    borderRadius: "4px",
    ":hover": {
        borderColor: `${COLORS.BLACK_40} !important`,
        cursor: "pointer"
    },
    ".react-tel-input": {
        font: "inherit",
        lineHeight: "inherit",
        display: "inline-flex",
        border: "none"
    },
    ".flag-dropdown": {
        border: "none",
        background: `${COLORS.BLACK_5} !!important`
    },
    ".selected-flag:hover": {
        cursor: "pointer !important",
        background: `${COLORS.BLACK_5} !important`
    },
    "input": {
        height: "46px !important", // workaround for non-working line-height
        width: "100% !important",
        border: "none !important",
        padding: "10px 14px 10px 48px !important",
        fontWeight: `400 !imporant`,
        color: `${COLORS.BLACK_60} !imporant`,
        font: `inherit !important`,
        lineHeight: `inherit !imporant`,
    },
    ".arrow": {
        borderTop: `4px solid ${COLORS.BLACK} !important`,
    }
})) as typeof Box;