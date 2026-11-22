import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";

const StatisticsColumnBoxStyled = styled(Box)(() => sx({
    width: "100%",
    display: "flex",
    gap: "8px",
    flexDirection: "column"
})) as typeof Box;

export default StatisticsColumnBoxStyled;