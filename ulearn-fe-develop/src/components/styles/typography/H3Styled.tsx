import { styled, experimental_sx as sx } from "@mui/material/styles";
import Typography from "@mui/material/Typography";
import withTypohraphyVariant from "./helpers/MuiHOC";

const H3Styled = styled(Typography)(() => sx({
    fontWeight: "700 !important",
    fontSize: { xs: "1.25rem", md: "1.5rem" }
})) as typeof Typography;

export default withTypohraphyVariant(H3Styled, "h3");