import { experimental_sx as sx, styled } from "@mui/system";
import { COLORS } from "./constants/ColorsConstatnts";
import Box from "@mui/material/Box";

export const SocialButtonStyled = styled(Box)(() => sx({
    background: COLORS.WHITE,
    display: "inline-flex",
    alignItems: "center",
    justifyContent: "center",
    minWidth: "max-content",
    gap: "8px",
    border: `1px solid ${COLORS.BLACK_10}`,
    borderRadius: "4px",
    padding: "10px",
    fontWeight: 500,
    width: "100%",
    textDecoration: "none",
    ":hover": {
        backgroundColor: COLORS.PRIMARY_5,
        cursor: "pointer"
    }
})) as typeof Box;