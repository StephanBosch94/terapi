import Button from "@mui/material/Button";
import { experimental_sx as sx, styled } from "@mui/system";
import { COLORS } from "./constants/ColorsConstatnts";

export const SecondaryButtonRectangle = styled(Button)(() => sx({
  border: "1px solid",
  borderRadius: "4px",
  fontWeight: 500,
  fontSize: "0.875rem",
  lineHeight: "1.25rem",
  backgroundColor: COLORS.WHITE,
  textDecoration: "none",
  color: COLORS.PRIMARY,
  padding: "12px 24px",
  ':hover': {
    backgroundColor: COLORS.PRIMARY_5,
    cursor: "pointer"
  }
})) as typeof Button;