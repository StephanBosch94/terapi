import Box from "@mui/material/Box";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { styled, experimental_sx as sx } from "@mui/material/styles";

export const TabsBoxStyled = styled(Box)(() => sx({
    border: `1px solid ${COLORS.BLACK_10}`,
    display: "flex",
    flexDirection: "row",
    gap: "8px",
    borderRadius: "4px",
    padding: "6px",
    backgroundColor: COLORS.WHITE,
    width: "100%",
    overflowX: "auto",
    "::-webkit-scrollbar": {
        display: "none"
    }
})) as typeof Box;