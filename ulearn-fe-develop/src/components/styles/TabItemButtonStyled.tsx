import { COLORS } from "./constants/ColorsConstatnts";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { PrimaryButtonRectangle } from "./PrimaryButtonRectangle";
import Button from "@mui/material/Button";

export const TabItemButtonStyled = styled(PrimaryButtonRectangle)(() => sx({
    padding: "4px 8px",
    fontWeight: 400,
    color: COLORS.BLACK,
    backgroundColor: COLORS.WHITE,
    minWidth: "max-content",
    border: `1px solid transparent`,
    ":hover": {
        backgroundColor: COLORS.PRIMARY_10,
        color: COLORS.BLACK,
        transition: "all 0.3s ease-in"
    }
})) as typeof Button;