import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";

const AddMoreItemsBoxStyled = styled(Box)(() => sx({
    width: "100%",
    display: "flex",
    alignItems: "center",
    justifyContent: "center"
})) as typeof Box

const AddItemsButtonStyled = styled(Button)(() => sx({
    padding: "8px"
})) as typeof Button

const ShowMoreItemsButton = ({ translationKey, handler }: { translationKey: string, handler: () => void }) => {
    const { t } = useTranslation();

    return (
        <AddMoreItemsBoxStyled>
            <AddItemsButtonStyled onClick={handler}>{t(translationKey)}</AddItemsButtonStyled>
        </AddMoreItemsBoxStyled>
    )
}

export default ShowMoreItemsButton;