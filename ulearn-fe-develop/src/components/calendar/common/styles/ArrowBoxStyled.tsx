import Box from "@mui/material/Box";
import { styled } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";

export const ArrowBoxStyled = styled(Box)({
    display: "flex",
    borderRadius: "50%",
    border: `1px solid ${COLORS.BLACK_10}`,
    padding: "4px",
    color: COLORS.PRIMARY,
    svg: {
        fill: COLORS.PRIMARY
    },
    ":hover": {
        cursor: "pointer",
        border: `1px solid ${COLORS.PRIMARY_40}`,
        backgroundColor: COLORS.PRIMARY_5
    }
}) as typeof Box;

