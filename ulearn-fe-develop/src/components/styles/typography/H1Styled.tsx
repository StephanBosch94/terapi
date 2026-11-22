import { styled, experimental_sx as sx } from "@mui/material/styles";
import Typography from "@mui/material/Typography";
import withTypohraphyVariant from "./helpers/MuiHOC";

const H1Styled = styled(Typography)(() => sx({
  fontWeight: "800 !important",
  fontSize: { xs: "2rem", md: "3rem" }
})) as typeof Typography;

export default withTypohraphyVariant(H1Styled, "h1");