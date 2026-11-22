import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";

export const DesktopBoxStyled = styled(Box)(() => sx({
    display: { xs: "none", md: "flex" },
    width: "100%"
})) as typeof Box;