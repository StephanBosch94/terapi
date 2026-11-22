import Box from "@mui/material/Box";
import { styled } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";

export const DotsBoxStyled = styled(Box)({
    ".dot": {
        borderRadius: "50%"
    },
    ".dot.freeSlot": {
        backgroundColor: COLORS.GREEN,
    },
    ".dot.reservation": {
        backgroundColor: COLORS.RED
    }
}) as typeof Box;