import Modal from "@mui/material/Modal";
import { styled, experimental_sx as sx } from "@mui/material/styles";

export const ModalStyled = styled(Modal)(() => sx({
    maxHeight: "100%",
    "> *:focus-visible": {
        outline: "none",
    },
})) as typeof Modal;
