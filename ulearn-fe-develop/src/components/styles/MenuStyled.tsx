import Menu from "@mui/material/Menu";
import { styled, experimental_sx as sx } from "@mui/material/styles";

export const MenuStyled = styled(Menu)(() => sx({
    ".MuiList-padding": { padding: 0 },
    ".MuiPaper-root": {
        boxShadow: "2px 3px 6px rgba(0, 0, 0, 0.15), -1px -1px 6px rgba(0, 0, 0, 0.08)",
        borderRadius: "4px",
        display: "flex",
        alignItems: "left",
        marginTop: "0px",
        padding: "0px"
    },
    ".MuiList-root": {
        width: "100%"
    }
})) as typeof Menu;