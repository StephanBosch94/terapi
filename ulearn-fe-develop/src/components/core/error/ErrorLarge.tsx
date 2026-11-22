import { useTranslation } from "components/core/TranslationProvider";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import ButtonUnstyled from '@mui/base/ButtonUnstyled';
import React from "react";
import { DEFAULT_TRANSLATIONS, TRANSLATION_KEYS } from "core/constants/translationConstants";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import ErrorIcon from "assets/warning-circle.svg";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";

const ErrorBox = styled(Box)(() => sx({
    display: "flex",
    flexWrap: "wrap",
    alignItems: "center",
    gap: "32px",
    justifyContent: "center",
    width: "100%",
    py: "32px",
    borderRadius: "8px",
    "> div": {
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
    }
})) as typeof Box;

const ErrorButtonStyled = styled(ButtonUnstyled)(() => sx({
    display: "flex",
    py: "8px",
    px: "16px",
    color: COLORS.WHITE,
    borderRadius: "4px",
    fontWeight: 500,
    font: "inherit",
    fontSize: { xs: "0.95rem", md: "0.875rem" },
    fontFamily: "inherit",
    border: "none",
    backgroundColor: COLORS.PRIMARY,
    ":hover": {
        cursor: "pointer",
        opacity: "0.9"
    }
})) as typeof ButtonUnstyled;

const ErrorLarge = ({ color, onClick }: { color?: string, onClick?: () => void }) => {
    const { t, tContains } = useTranslation();
    const descriptionKey = TRANSLATION_KEYS.ERRORS.GENERAL_ERROR;
    const buttonKey = TRANSLATION_KEYS.COMMON.RELOAD_BUTTON;

    const refreshPage = () => {
        window.location.reload();
    }

    return (
        <ErrorBox color={color}>
            <Box width="100%" sx={{ svg: { transform: "scale(4)", color: COLORS.RED } }}><ErrorIcon /></Box>
            <Box width="100%" textAlign="center">
                <Typography>{
                    tContains(descriptionKey)
                        ? <div dangerouslySetInnerHTML={{ __html: t((t(descriptionKey))) }} />
                        : <div dangerouslySetInnerHTML={{ __html: t((t((DEFAULT_TRANSLATIONS.ERRORS.GENERAL_ERROR)))) }} />
                }</Typography>
            </Box>
            <Box width="100%">
                <ErrorButtonStyled onClick={onClick ?? refreshPage}>
                    {tContains(buttonKey) ? t(buttonKey) : DEFAULT_TRANSLATIONS.COMMON.RELOAD_BUTTON}
                </ErrorButtonStyled>
            </Box>
        </ErrorBox>
    )
}

export default ErrorLarge;