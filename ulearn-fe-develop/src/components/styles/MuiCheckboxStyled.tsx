import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "./constants/ColorsConstatnts";
import Checkbox from "@mui/material/Checkbox";

export const MuiCheckboxStyled = styled(Checkbox)(() => sx({
    color: COLORS.PRIMARY,
    padding: "0px 0px 0px 8px",
    fontSize: "1rem"
})) as typeof Checkbox;