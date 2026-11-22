import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";

export const ScrollBoxStyled = styled(Box)(() => sx({
    userSelect: "none",
    overflowX: "auto",
    "::-webkit-scrollbar": {
        display: "none"
    }
})) as typeof Box;