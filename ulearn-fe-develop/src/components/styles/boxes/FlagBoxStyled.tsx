import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";
import { COLORS } from "../constants/ColorsConstatnts";

export const FlagBoxStyled = styled(Box)({
    display: "flex",
    backgroundColor: COLORS.BLACK_10, // gray placeholder when icon is beening fetched from API
    color: COLORS.BLACK_10, // make text insivisble
    borderRadius: "4px",
    "img": {
        borderRadius: "4px",
        border: `1px solid ${COLORS.BLACK_10}`,
        width: "20px",
        height: "15px",
        objectFit: "unset",
        objectPosition: "unset"
    }
}) as typeof Box;