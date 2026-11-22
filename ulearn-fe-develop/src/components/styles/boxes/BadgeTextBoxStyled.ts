import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";

export const BadgeTextBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "4px",
})) as typeof Box;