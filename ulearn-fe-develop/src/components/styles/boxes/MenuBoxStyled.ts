import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";

export const MenuBoxStyled = styled(Box)({
    display: "flex",
    flexFlow: "column noWrap",
    width: "100%",
    minWidth: "200px",
    padding: "4px 0"
}) as typeof Box;