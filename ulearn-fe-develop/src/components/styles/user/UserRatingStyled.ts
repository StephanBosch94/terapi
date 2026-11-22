import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "../constants/ColorsConstatnts";
import PStyled from "../typography/PStyled";

export const UserRatingStyled = styled(PStyled)(() => sx({
    fontWeight: 600,
    color: COLORS.BLACK
})) as typeof PStyled;