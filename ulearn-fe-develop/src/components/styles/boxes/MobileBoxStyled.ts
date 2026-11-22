import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";

export const MobileBoxStyled = styled(Box)(() => sx({
    display: { xs: "flex", md: "none" },
    width: "100%",
    flexFlow: "column nowrap",
})) as typeof Box;