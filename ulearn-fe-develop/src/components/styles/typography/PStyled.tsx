import { styled, experimental_sx as sx } from "@mui/material/styles";
import Typography from "@mui/material/Typography";
import { COLORS } from "../constants/ColorsConstatnts";

const PStyled = styled(Typography)(() => sx({
    color: COLORS.BLACK_60,
    fontWeight: "400",
    fontSize: "1rem"
})) as typeof Typography;

export default PStyled;