import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "./constants/ColorsConstatnts";
import CloseIcon from "assets/close.svg";

const CloseIconModalStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    color: COLORS.BLACK_60,
    "svg": {
        transform: "scale(1.25)"
    },
    ":hover": {
        cursor: "pointer",
        color: COLORS.BLACK
    }
})) as typeof Box;

export const CloseIconButton = ({ onClick }: { onClick: () => void }) => {

    return (
        <CloseIconModalStyled onClick={onClick}><CloseIcon /></CloseIconModalStyled>
    )
}
