import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import React from "react";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import Body1Styled from "components/styles/typography/Body1Styled";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import { ContainerStyled } from "components/styles/ContainerStyled";

const BackgroundBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.PRIMARY,
    padding: "20px 0px"
})) as typeof Box;

const Copyright = () => {
    const { t } = useTranslation();
    return (
        <BackgroundBoxStyled>
            <ContainerStyled>
                <Grid container>
                    <Grid item sx={{ width: "100%", display: "flex", justifyContent: "center" }}>
                        <Body1Styled sx={{ color: COLORS.WHITE }}>{t(TRANSLATION_KEYS.COPYRIGHT.TITLE)}</Body1Styled>
                    </Grid>
                </Grid>
            </ContainerStyled>
        </BackgroundBoxStyled>
    )
}
export default Copyright;