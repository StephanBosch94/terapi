
import { experimental_sx as sx, styled } from "@mui/system";
import { COLORS } from "./constants/ColorsConstatnts";
import Button from "@mui/material/Button";

export const WhiteButtonStyled = styled(Button)(() => sx({
  background: COLORS.WHITE,
  borderRadius: "24px",
  gap: "4px",
  fontWeight: 500,
  fontSize: "0.875rem",
  lineHeight: "1.25rem",
  color: COLORS.PRIMARY,
  padding: "12px 24px",
  ':hover': {
    backgroundColor: COLORS.WHITE,
    pointer: "cursor",
    backgroundPosition: "right center",
    backgroundSize: "200% auto",
    animation: "buttonPulse 1.5s infinite"
  },
  a: {
    textDecoration: "none"
  }
})) as typeof Button;