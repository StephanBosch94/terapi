import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import ButtonUnstyled from '@mui/base/ButtonUnstyled';
import DeleteIcon from "assets/delete.svg";
import Box from "@mui/material/Box";

const DeleteIconBoxStyled = styled(Box)(() => sx({
    position: "absolute",
    bottom: "16px",
    right: "16px"
})) as typeof Box;

const DeleteButtonStyled = styled(ButtonUnstyled)(() => sx({
    borderRadius: "4px",
    border: `1px solid ${COLORS.RED}`,
    backgroundColor: COLORS.WHITE,
    padding: "8px",
    fontFamily: "inherit",
    ":hover": {
        cursor: "pointer",
        border: `1px solid ${COLORS.RED_HOVER}`,

    },
    svg: {
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        color: COLORS.RED
    }
})) as typeof ButtonUnstyled;


export const DeleteIconButton = ({ onClick }: { onClick?: () => void }) => {
    return (
        <DeleteIconBoxStyled onClick={onClick}>
            <DeleteButtonStyled>
                <DeleteIcon />
            </DeleteButtonStyled>
        </DeleteIconBoxStyled>
    )
}


