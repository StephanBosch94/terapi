import { styled, experimental_sx as sx } from "@mui/material/styles";
import Typography from "@mui/material/Typography";
import withTypohraphyVariant from "./helpers/MuiHOC";

const H4Styled = styled(Typography)(() => sx({
    fontWeight: "700 !important",
    fontSize: { xs: "1rem", md: "1.25rem" },
})) as typeof Typography;

export default withTypohraphyVariant(H4Styled, "h4");