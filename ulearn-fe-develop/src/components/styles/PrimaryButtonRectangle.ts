import Button from "@mui/material/Button";
import { experimental_sx as sx, styled } from "@mui/system";
import { COLORS } from "./constants/ColorsConstatnts";

export const PrimaryButtonRectangle = styled(Button)(() => sx({
  background: COLORS.PRIMARY,
  borderRadius: "4px",
  fontWeight: 500,
  fontSize: "0.875rem",
  lineHeight: "1.25rem",
  color: COLORS.WHITE,
  padding: "12px 24px",
  display: "inline-flex",
  alignItems: "center",
  ':hover': {
    backgroundColor: COLORS.PRIMARY_HOVER,
    cursor: "pointer"
  }
})) as typeof Button;