import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";

export const FloatableBoxStyled = styled(Box)({
    display: "flex",
    flexFlow: "row noWrap",
    flex: "0 1 auto",
    gap: "8px",
    alignItems: "center",
    justifyContent: "center",
}) as typeof Box;