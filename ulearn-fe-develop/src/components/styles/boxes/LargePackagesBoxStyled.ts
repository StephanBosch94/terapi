import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";

const PackagesBoxStyled = styled(Box)({
    display: "inline-flex",
    alignItems: "flex-end",
    flexFlow: "row noWrap",
    padding: "16px 0px",
    width: "100%",
    gap: "24px"
}) as typeof Box;

export default PackagesBoxStyled;