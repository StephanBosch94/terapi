import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";

export const DashboardBoxStyled = styled(Box)(() => sx({
    width: "100%",
    borderRadius: "4px",
    display: "flex",
    gap: { xs: "8px", md: "16px" },
    flexDirection: "column",
})) as typeof Box;