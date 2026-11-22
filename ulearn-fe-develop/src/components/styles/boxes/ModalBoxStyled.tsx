import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";
import { COLORS } from "../constants/ColorsConstatnts";

export const ModalBoxStyled = styled(Box)(() => sx({
    position: "absolute",
    top: '50%',
    left: '50%',
    width: { xs: "calc(100% - 32px)", md: "560px" },
    borderRadius: "4px",
    transform: 'translate(-50%, -50%)',
    backgroundColor: COLORS.WHITE,
    boxShadow: 24,
    padding: "24px",
    display: "flex",
    flexFlow: "column noWrap",
    gap: "24px"
})) as typeof Box;