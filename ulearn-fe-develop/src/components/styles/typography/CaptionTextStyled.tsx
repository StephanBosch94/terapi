import { styled, experimental_sx as sx } from "@mui/material/styles";
import Typography from "@mui/material/Typography";
import { COLORS } from "../constants/ColorsConstatnts";

const CaptionTextStyled = styled(Typography)(() => sx({
    color: COLORS.BLACK_60,
    fontWeight: "500",
    fontSize: "0.75rem"
})) as typeof Typography;

export default CaptionTextStyled;