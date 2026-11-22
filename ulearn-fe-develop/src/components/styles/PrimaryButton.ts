
import { experimental_sx as sx, styled } from "@mui/system";
import { COLORS } from "./constants/ColorsConstatnts";
import Button from "@mui/material/Button";

export const PrimaryButton = styled(Button)(() => sx({
  border: "1px solid",
  background: COLORS.PRIMARY,
  borderRadius: "24px",
  gap: "4px",
  fontWeight: 500,
  fontSize: "0.875rem",
  lineHeight: "1.25rem",
  color: COLORS.WHITE,
  padding: "12px 24px",
  ':hover': {
    backgroundColor: COLORS.PRIMARY_HOVER,
    pointer: "cursor",
    backgroundPosition: "right center",
    backgroundSize: "200% auto",
    animation: "buttonPulse 1.5s infinite"
  },
  a: {
    textDecoration: "none"
  }
})) as typeof Button;