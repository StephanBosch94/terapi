import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";
import { COLORS } from "../constants/ColorsConstatnts";

export const TextareaBoxStyled = styled(Box)({
    width: "100%",
    "textarea": {
        width: "100%",
        border: `1px solid ${COLORS.BLACK_10}`,
        font: "inherit",
        fontSize: "0.9rem",
        padding: "6px 8px",
        minHeight: "120px",
        resize: "none",
        borderRadius: "4px",
    }
}) as typeof Box;