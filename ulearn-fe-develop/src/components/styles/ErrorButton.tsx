import { experimental_sx as sx, styled } from "@mui/system";
import { COLORS } from "./constants/ColorsConstatnts";
import Button from "@mui/material/Button";

export const ErrorButton = styled(Button)(() => sx({
  background: COLORS.RED,
  borderRadius: "4px",
  fontWeight: 400,
  fontSize: "0.825rem",
  lineHeight: "1.25rem",
  color: COLORS.WHITE,
  padding: "9px 16px",
  ':hover': {
    backgroundColor: COLORS.RED_HOVER
  }
})) as typeof Button;