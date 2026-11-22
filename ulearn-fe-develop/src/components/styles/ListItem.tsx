import { experimental_sx as sx, styled } from "@mui/system";
import { COLORS } from "./constants/ColorsConstatnts";
import ButtonUnstyled from '@mui/base/ButtonUnstyled';

export const ListItem = styled(ButtonUnstyled)(() => sx({
  backgroundColor: COLORS.WHITE,
  color: COLORS.BLACK,
  borderRadius: "4px",
  padding: "12px 16px",
  display: "flex",
  border: `1px solid ${COLORS.BLACK_10}`,
  alignItems: { xs: "left", md: "center" },
  gap: "16px",
  fontFamily: "inherit",
  ':hover': {
    cursor: "pointer",
  },
})) as typeof ButtonUnstyled;
