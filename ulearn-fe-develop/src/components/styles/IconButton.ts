import { experimental_sx as sx, styled } from "@mui/system";
import { COLORS } from "./constants/ColorsConstatnts";
import ButtonUnstyled from '@mui/base/ButtonUnstyled';

export const IconButton = styled(ButtonUnstyled)(() => sx({
    borderRadius: "4px",
    border: `1px solid ${COLORS.PRIMARY}`,
    backgroundColor: COLORS.WHITE,
    padding: "8px",
    fontFamily: "inherit",
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_5,
    }
})) as typeof ButtonUnstyled;