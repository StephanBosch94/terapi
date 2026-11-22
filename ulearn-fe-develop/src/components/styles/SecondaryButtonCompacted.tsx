import Button from "@mui/material/Button";
import { experimental_sx as sx, styled } from "@mui/system";
import { COLORS } from "./constants/ColorsConstatnts";

export const SecondaryButtonCompacted = styled(Button)(() => sx({
  border: "1px solid",
  borderRadius: "24px",
  fontWeight: 500,
  fontSize: "0.875rem",
  width: "fit-content",
  lineHeight: "1.25rem",
  backgroundColor: COLORS.WHITE,
  textDecoration: "none",
  color: COLORS.PRIMARY,
  padding: "8px 16px",
  ':hover': {
    backgroundColor: COLORS.PRIMARY_5,
    cursor: "pointer"
  }
})) as typeof Button;