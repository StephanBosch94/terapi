import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import React from "react";
import H1Styled from "components/styles/typography/H1Styled";
import PStyled from "components/styles/typography/PStyled";
import Link from "next/link";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import AuthModalProvider, { useAuthModalContext } from "components/modals/AuthModalProvider";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import Image from "next/image";
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";
import { LOADING_IMAGES } from "core/constants/loadingImagesConstants";
import ArrowIcon from "assets/arrow-right.svg";
import Body1Styled from "components/styles/typography/Body1Styled";
import CheckIcon from "assets/check-circle.svg";
import { WhiteButtonStyled } from "components/styles/WhiteButtonStyled";
import { OutlineButtonStyled } from "components/styles/OutlineButtonStyled";
import { ContainerStyled } from "components/styles/ContainerStyled";

const BackgroundBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.PRIMARY,
    padding: { xs: "32px 0px", md: "80px 0px" }
})) as typeof Box;

const LeftHeroSectionStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    justifyContent: "center",
    height: "100%",
    flex: "1 1 auto",
    width: { xs: "100%", md: "auto" },
    gap: { xs: "16px", md: "24px" },
    paddingRight: { xs: "0px", md: "40px" }
})) as typeof Box;

const PStyledText = styled(PStyled)(() => sx({
    color: COLORS.WHITE
})) as typeof PStyled;

const Body1Text = styled(Body1Styled)(() => sx({
    color: COLORS.WHITE
})) as typeof Body1Styled;

const DescriptionBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "row",
    gap: "4px",
    "b": {
        fontWeight: 600,
        color: COLORS.BLACK
    }
})) as typeof Box;

const ContainerCustomStyled = styled(ContainerStyled)(() => sx({
    animation: "fade-up 0.5s",
    display: "flex",
    gap: "40px",
    alignItems: { xs: "flex-start", lg: "center" },
    flexDirection: { xs: "column", lg: "row" }
})) as typeof Container;

const ImageBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "flex-end",
    position: "relative",
    flex: "1 1 auto",
    width: { xs: "100%", lg: "auto" },
    img: {
        minWidth: { xs: "auto", lg: "552px" },
        maxWidth: { xs: "auto", lg: "552px" },
        width: "100%",
        height: "auto"
    }
})) as typeof Box;

const IconStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        color: COLORS.WHITE
    }
})) as typeof Box;

const FeatureBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "start",
    gap: "4px",
    flexFlow: "row noWrap"
})) as typeof Box;

const CompanyHero = () => {
    const { t } = useTranslation();
    const { setOpen } = useAuthModalContext();

    return (
        <React.Fragment>
            <BackgroundBoxStyled>
                <ContainerCustomStyled>
                    <LeftHeroSectionStyled>
                        <H1Styled sx={{ color: COLORS.WHITE }}>{t(TRANSLATION_KEYS.COMPANY_LANDING_PAGE.TITLE)}</H1Styled>
                        <DescriptionBoxStyled>
                            <PStyledText>{t(TRANSLATION_KEYS.COMPANY_LANDING_PAGE.DESCRIPTION)}</PStyledText>
                        </DescriptionBoxStyled>
                        <Box sx={{ display: "flex", gap: "16px", marginTop: "16px" }}>
                            <WhiteButtonStyled onClick={() => setOpen(UserRoleEnum.Company, "registration")}>{t(TRANSLATION_KEYS.COMPANY_LANDING_PAGE.BUTTON_TEXT)}<ArrowIcon /></WhiteButtonStyled>
                            <Link href={t(TRANSLATION_KEYS.COMPANY_LANDING_PAGE.CONTACT_SALES_HUBSPOT)}><OutlineButtonStyled>{t(TRANSLATION_KEYS.COMPANY_LANDING_PAGE.SECONDARY_BUTTON_TEXT)}</OutlineButtonStyled></Link>
                        </Box>
                        <FeatureBoxStyled>
                            <IconStyled><CheckIcon /></IconStyled>
                            <Body1Text>{t(TRANSLATION_KEYS.COMPANY_LANDING_PAGE.FEATURE_TEXT)}</Body1Text>
                        </FeatureBoxStyled>
                    </LeftHeroSectionStyled>
                    <ImageBoxStyled>
                        <Image src="/images/business_hero.png" placeholder="blur" blurDataURL={LOADING_IMAGES.COMPANY_HERO.MAIN} width={552} height={531} quality={75} priority={true} alt={t(TRANSLATION_KEYS.COMPANY_LANDING_PAGE.TITLE)} />
                    </ImageBoxStyled>
                </ContainerCustomStyled>
            </BackgroundBoxStyled>
        </React.Fragment>
    )
}

const CompanyHeroWithProviders = () => {
    return (
        <AuthModalProvider>
            <CompanyHero />
        </AuthModalProvider>
    )
}

export default CompanyHeroWithProviders;