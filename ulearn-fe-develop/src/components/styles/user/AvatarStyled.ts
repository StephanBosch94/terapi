import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import Avatar from "@mui/material/Avatar";

export const AvatarStyled = styled(Avatar)(() => sx({
    width: 36,
    height: 36,
    border: `2px solid ${COLORS.PRIMARY}`
})) as typeof Avatar;