import { useTranslation } from "components/core/TranslationProvider";
import { PrimaryButton } from "components/styles/PrimaryButton";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { InputNames } from "./TutorProfileSettings";
import Box from "@mui/material/Box"
import { ContainerStyled } from "components/styles/ContainerStyled";

const SaveButton = () => {
    const { t } = useTranslation();

    return (
        <Box sx={{ borderTop: `1px solid ${COLORS.BLACK_10}`, width: "100%", paddingTop: { xs: "16px", md: "24px" } }}>
            <ContainerStyled sx={{ display: "flex", justifyContent: "flex-end", width: "100%", padding: "0 16px" }}>
                <PrimaryButton type="submit" name={InputNames.saveButton}>{t(TRANSLATION_KEYS.COMMON.SAVE)}</PrimaryButton>
            </ContainerStyled>
        </Box>
    )
}

export default SaveButton;