import { styled } from "@mui/material/styles";
import H3Styled from "components/styles/typography/H3Styled";;
import { CloseIconButton } from "components/styles/CloseIconModalStyled";
import Box from "@mui/material/Box";

const HeaderBoxStyled = styled(Box)({
    width: "100%",
    display: "inline-flex",
    alignItems: "flex-start",
    justifyContent: "space-between",
}) as typeof Box;

const ModalsHeader = ({ text, handleClose }: { text: string, handleClose: () => void }) => {

    return (
        <HeaderBoxStyled>
            <H3Styled>{text}</H3Styled>
            <CloseIconButton onClick={handleClose} />
        </HeaderBoxStyled>
    )
}

export default ModalsHeader;