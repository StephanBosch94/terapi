import Confetti from "react-confetti";
import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";

const ConfettiBoxStyled = styled(Box)({
    position: "absolute",
    width: "100%",
    height: "100%",
    zIndex: 100
}) as typeof Box;

const ConfettiBox = ({ gravity = 0.1 }: { gravity?: number }) => {
    return (
        <ConfettiBoxStyled>
            <Confetti recycle={false} gravity={gravity} />
        </ConfettiBoxStyled>
    )
}

export default ConfettiBox;