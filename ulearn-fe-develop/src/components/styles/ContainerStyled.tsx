import Container from "@mui/material/Container";
import { experimental_sx as sx, styled } from "@mui/system";

export const ContainerStyled = styled(Container)(() => sx({
    maxWidth: "1400px !important"
})) as typeof Container;