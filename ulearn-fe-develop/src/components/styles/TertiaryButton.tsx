import Button from "@mui/material/Button";
import { experimental_sx as sx, styled } from "@mui/system";
import { COLORS } from "./constants/ColorsConstatnts";

export const TertiaryButton = styled(Button)(() => sx({
  backgroundColor: COLORS.WHITE,
  color: COLORS.PRIMARY,
  borderRadius: "4px",
  padding: "12px 24px",
  lineHeight: "1.25rem",
  ':hover': {
    backgroundColor: COLORS.PRIMARY_5,
    cursor: "pointer",

  },
})) as typeof Button;
