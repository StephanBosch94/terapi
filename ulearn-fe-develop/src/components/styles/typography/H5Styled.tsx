import { styled, experimental_sx as sx } from "@mui/material/styles";
import Typography from "@mui/material/Typography";
import withTypohraphyVariant from "./helpers/MuiHOC";

const H5Styled = styled(Typography)(() => sx({
    fontWeight: "600 !important",
    fontSize: "1rem",
    fontFamily: "inherit"
})) as typeof Typography;

export default withTypohraphyVariant(H5Styled, "h5");