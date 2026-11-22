import Button from "@mui/material/Button";
import { experimental_sx as sx, styled } from "@mui/system";
import { COLORS } from "./constants/ColorsConstatnts";

export const OutlineButtonStyled = styled(Button)(() => sx({
  border: `1px solid ${COLORS.WHITE}`,
  borderRadius: "24px",
  fontWeight: 500,
  fontSize: "0.875rem",
  lineHeight: "1.25rem",
  textDecoration: "none",
  color: COLORS.WHITE,
  padding: "12px 24px",
  ':hover': {
    cursor: "pointer",
    backgroundPosition: "right center",
    backgroundSize: "200% auto",
    animation: "buttonPulse 1.5s infinite"
  }
})) as typeof Button;