import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";
import { COLORS } from "../constants/ColorsConstatnts";

export const ModalBoxSmallStyled = styled(Box)(() => sx({
    position: "absolute",
    top: '50%',
    left: '50%',
    minWidth: { xs: "calc(100% - 32px)", md: "520px" },
    maxWidth: "640px",
    maxHeight: "100%",
    overflow: "auto",
    borderRadius: "4px",
    transform: 'translate(-50%, -50%)',
    backgroundColor: COLORS.WHITE,
    boxShadow: 24,
    padding: "24px",
    display: "flex",
    flexFlow: "column noWrap",
    gap: "24px",
    "&:focus-visible": {
        outline: "none"
    }
})) as typeof Box;
