import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";

const StatisticsColumnsBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: { xs: "column", md: "row" },
    gap: { xs: "24px", md: "40px" }
})) as typeof Box;

export default StatisticsColumnsBoxStyled;