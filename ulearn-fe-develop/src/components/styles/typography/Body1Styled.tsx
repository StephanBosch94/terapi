import { styled, experimental_sx as sx } from "@mui/material/styles";
import Typography from "@mui/material/Typography";
import { COLORS } from "../constants/ColorsConstatnts";

const Body1Styled = styled(Typography)(() => sx({
    color: COLORS.BLACK_60,
    fontWeight: "400",
    fontSize: "0.875rem"
})) as typeof Typography;

export default Body1Styled;