import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";

const ItemStateButtonBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "row wrap",
    width: "100%",
    alignItems: "center",
    justifyContent: "center",
    gap: "8px"
})) as typeof Box;

export default ItemStateButtonBoxStyled;