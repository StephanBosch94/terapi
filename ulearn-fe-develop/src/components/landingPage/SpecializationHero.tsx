import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import React from "react";
import H1Styled from "components/styles/typography/H1Styled";
import PStyled from "components/styles/typography/PStyled";
import { useTranslation } from "components/core/TranslationProvider";
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";
import { ContainerStyled } from "components/styles/ContainerStyled";
import InjectIcon from "components/core/icons/InjectIcon";
import { SpecializationType } from "core/services/Types/AppTypes";

const BackgroundBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    padding: { xs: "32px 0px", md: "120px 0px" },
})) as typeof Box;

const ContainerCustomStyled = styled(ContainerStyled)(() => sx({
    animation: "fade-up 0.5s",
    display: "flex",
    gap: "32px",
    alignItems: "flex-start",
    flexFlow: "column noWrap",
})) as typeof Container;

const TitleBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "row wrap",
    alignItems: "center",
    gap: "8px",
    "svg": {
        transform: "scale(2.75)"
    }
})) as typeof Box;

const DesktopBoxStyled = styled(Box)(() => sx({
    display: { xs: "none", md: "flex" }
})) as typeof Box;

const H1HeroStyled = styled(H1Styled)(() => sx({
    width: "auto",
    ".mark": {
        color: COLORS.PRIMARY
    }
})) as typeof H1Styled;

type PropsType = { specialization: SpecializationType, bgColor: string };

const SpecializationHero = ({ specialization, bgColor }: PropsType) => {
    const { t } = useTranslation();
    
    return (
        <BackgroundBoxStyled sx={{ backgroundColor: bgColor}}>
            <ContainerCustomStyled>
                <TitleBoxStyled>
                    <H1HeroStyled>
                        <div dangerouslySetInnerHTML={{ __html: t((t(specialization.nameKey ?? ""))) }} />
                    </H1HeroStyled>
                    <DesktopBoxStyled>
                        <InjectIcon category="specializations" iconName={specialization.iconName ?? ""} size={48} />
                    </DesktopBoxStyled>
                </TitleBoxStyled>
                <PStyled>{t(specialization.heroDescriptionKey ?? "")}</PStyled>
            </ContainerCustomStyled>
        </BackgroundBoxStyled>
    )
}

export default SpecializationHero;