import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import React from "react";
import H1Styled from "components/styles/typography/H1Styled";
import PStyled from "components/styles/typography/PStyled";
import { PrimaryButton } from "components/styles/PrimaryButton";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import AuthModalProvider, { useAuthModalContext } from "components/modals/AuthModalProvider";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { CodetableSubjectsResponse } from "core/redux/codetableSlice";
import Image from 'next/image';
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";
import { LOADING_IMAGES } from "core/constants/loadingImagesConstants";
import ArrowIcon from "assets/arrow-right.svg";
import { ContainerStyled } from "components/styles/ContainerStyled";
import InjectIcon from "components/core/icons/InjectIcon";

const BackgroundBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    padding: { xs: "32px 0px", md: "80px 0px" },
})) as typeof Box;

const LeftHeroSectionStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    justifyContent: "center",
    height: "100%",
    flex: "1 1 55%",
    gap: { xs: "16px", md: "24px" },
    paddingRight: { xs: "0px", md: "40px" }
})) as typeof Box;

const ContainerCustomStyled = styled(ContainerStyled)(() => sx({
    animation: "fade-up 0.5s",
    display: "flex",
    gap: "48px",
    alignItems: { xs: "flex-start", md: "center" },
    flexDirection: { xs: "column", md: "row" },
})) as typeof Container;

const ImageBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "flex-end",
    position: "relative",
    flex: "1 1 45%",
    img: {
        width: "100%",
        height: "auto"
    }
})) as typeof Box;

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

const getHeroImagePath = (subjectKey: string) => {
    return `/images/subjects/${subjectKey}.png`;
}

const subjectKeys = {
    english: "english",
    czech: "czech",
    french: "french",
    spanish: "spanish",
    german: "german",
    italian: "italian",
}

const getBlurDataURL = (subjectKey: string) => {
    switch (subjectKey) {
        case subjectKeys.english:
            return LOADING_IMAGES.SUBJECT_HERO.ENGLISH;
        case subjectKeys.czech:
            return LOADING_IMAGES.SUBJECT_HERO.CZECH;
        case subjectKeys.french:
            return LOADING_IMAGES.SUBJECT_HERO.FRENCH;
        case subjectKeys.spanish:
            return LOADING_IMAGES.SUBJECT_HERO.SPANISH;
        case subjectKeys.german:
            return LOADING_IMAGES.SUBJECT_HERO.GERMAN;
        case subjectKeys.italian:
            return LOADING_IMAGES.SUBJECT_HERO.ITALIAN;
        default:
            return LOADING_IMAGES.PRIMARY.RECTANGLE;
    }
}

const SubjectHero = ({ subject }: { subject: CodetableSubjectsResponse[0] }) => {
    const { t } = useTranslation();
    const { setOpen } = useAuthModalContext();

    return (
        <BackgroundBoxStyled>
            <ContainerCustomStyled>
                <LeftHeroSectionStyled>
                    <TitleBoxStyled>
                        <H1HeroStyled>
                            <div dangerouslySetInnerHTML={{ __html: t((t(subject.heroTitleKey ?? ""))) }} />
                        </H1HeroStyled>
                        <DesktopBoxStyled>
                            <InjectIcon category="emoji" iconName={subject.iconName ?? ""} size={48} />
                        </DesktopBoxStyled>
                    </TitleBoxStyled>
                    <PStyled>{t(subject.heroTitleDescriptionKey ?? "")}</PStyled>
                    <Box sx={{ display: "block", paddingTop: { xs: "0px", md: "24px" } }}>
                        <PrimaryButton onClick={() => setOpen(UserRoleEnum.Student, "registration")}>{t(TRANSLATION_KEYS.SUBJECT_HERO.TEXT_BUTTON)} <ArrowIcon /></PrimaryButton>
                    </Box>
                </LeftHeroSectionStyled>
                <ImageBoxStyled>
                    <Image src={getHeroImagePath(subject.enumKey ?? "")} placeholder="blur" blurDataURL={getBlurDataURL(subject.enumKey ?? "")} priority={true} quality={75} alt="Become fluent in your favorite language." width={424} height={408} />
                </ImageBoxStyled>
            </ContainerCustomStyled>
        </BackgroundBoxStyled>
    )
}

const SubjectHeroWithProviders = ({ subject }: { subject: CodetableSubjectsResponse[0] }) => {

    return (
        <AuthModalProvider>
            <SubjectHero subject={subject} />
        </AuthModalProvider>
    )
}

export default SubjectHeroWithProviders;