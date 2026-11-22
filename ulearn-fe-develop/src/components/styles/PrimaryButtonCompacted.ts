import Button from "@mui/material/Button";
import { experimental_sx as sx, styled } from "@mui/system";
import { COLORS } from "./constants/ColorsConstatnts";

export const PrimaryButtonCompacted = styled(Button)(() => sx({
  background: COLORS.PRIMARY,
  borderRadius: "24px",
  fontWeight: 500,
  width: "fit-content",
  fontSize: "0.875rem",
  lineHeight: "1.25rem",
  color: COLORS.WHITE,
  padding: "8px 16px",
  ':hover': {
    backgroundColor: COLORS.PRIMARY_HOVER,
    pointer: "cursor"
  }
})) as typeof Button;