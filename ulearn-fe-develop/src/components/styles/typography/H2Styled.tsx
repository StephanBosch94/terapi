import { styled, experimental_sx as sx } from "@mui/material/styles";
import Typography from "@mui/material/Typography";
import withTypohraphyVariant from "./helpers/MuiHOC";

const H2Styled = styled(Typography)(() => sx({
    fontWeight: "800 !important",
    fontSize: { xs: "1.5rem", md: "2rem" }
})) as typeof Typography;


export default withTypohraphyVariant(H2Styled, "h2");