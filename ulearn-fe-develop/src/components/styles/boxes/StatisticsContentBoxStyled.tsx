

import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "../constants/ColorsConstatnts";

const StatisticsColumnsBoxStyled = styled(Box)(() => sx({
    border: `1px solid ${COLORS.BLACK_10}`,
    padding: "16px",
    display: "flex",
    flexDirection: "column",
    gap: "12px",
    borderRadius: "4px",
    height: "100%"
})) as typeof Box;

export default StatisticsColumnsBoxStyled;