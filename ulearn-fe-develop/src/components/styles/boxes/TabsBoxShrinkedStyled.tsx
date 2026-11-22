import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { TabsBoxStyled } from "./TabsBoxStyled";

export const TabsBoxShrinkedStyled = styled(TabsBoxStyled)(() => sx({
    width: { xs: "auto", md: "fit-content" },
    display: "flex",
    margin: "0 auto",
    overflow: "auto"
})) as typeof Box;