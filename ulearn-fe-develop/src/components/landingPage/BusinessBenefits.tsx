import React from "react";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import PStyled from "components/styles/typography/PStyled";
import Image from 'next/image';
import Box from "@mui/material/Box";
import { LOADING_IMAGES } from "core/constants/loadingImagesConstants";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import AppLogo from "assets/app_logo_business_horizontal.svg";
import CheckIcon from "assets/check-circle-fill.svg";
import ArrowIcon from "assets/arrow-right.svg";
import { AnimatedContainer } from "components/styles/AnimatedContainerStyled";
import routes from "routes/routes";
import { useRouter } from "next/router";
import { WhiteButtonStyled } from "components/styles/WhiteButtonStyled";
import H2Styled from "components/styles/typography/H2Styled";


const BackgroundBoxStyled = styled(Box)(() => sx({
    padding: "60px 0px"
})) as typeof Box;

const StyledIcon = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        color: COLORS.WHITE
    }
})) as typeof Box;

const BusinessBenefitsBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: { xs: "column noWrap", md: "row noWrap" },
    alignItems: "center",
    padding: { xs: "24px", md: "56px" },
    borderRadius: "8px",
    height: "100%",
    gap: { xs: "32px", md: "0px" },
    backgroundColor: COLORS.PRIMARY,
})) as typeof Box;


const StyledLogoIcon = styled(Box)(() => sx({
    display: "flex",
    alignItems: "start",
    justifyContent: "center",
})) as typeof Box;

const BenefitBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "8px",
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "center"
})) as typeof Box;

const BenefitsBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "16px",
    flexDirection: "column",
    alignItems: "flex-start"
})) as typeof Box;

const HeaderBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "16px",
    alignItems: "start"
})) as typeof Box;

const ContentBoxStyled = styled(Box)(() => sx({
    height: "100%",
    width: "100%",
    display: "flex",
    flexFlow: "column wrap",
    gap: { xs: "32px", md: "40px" }
})) as typeof Box;


const ImageBoxStyled = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "end",
    width: "100%",
    ".image": {
        width: { md: "80%", xs: "100%" },
        height: "auto"
    }
})) as typeof Box;

const Business = () => {
    const { t } = useTranslation();
    const router = useRouter();

    const handleClick = () => {
        router.push(routes.companyLandingPage.path);
    }

    return (
        <React.Fragment>
            <BackgroundBoxStyled>
                <AnimatedContainer>
                    <BusinessBenefitsBoxStyled>
                        <ContentBoxStyled>
                            <HeaderBoxStyled>
                                <StyledLogoIcon><AppLogo /></StyledLogoIcon>
                                <H2Styled sx={{ color: COLORS.WHITE }}>{t(TRANSLATION_KEYS.COMPANY_BUSINESS_ITEM.DESCRIPTION)}</H2Styled>
                            </HeaderBoxStyled>
                            <BenefitsBoxStyled>
                                <BenefitBoxStyled>
                                    <StyledIcon><CheckIcon /></StyledIcon>
                                    <PStyled sx={{ color: COLORS.WHITE }}>{t(TRANSLATION_KEYS.COMPANY_BUSINESS_ITEM.FIRST_BENEFIT)}</PStyled>
                                </BenefitBoxStyled>
                                <BenefitBoxStyled>
                                    <StyledIcon><CheckIcon /></StyledIcon>
                                    <PStyled sx={{ color: COLORS.WHITE }}>{t(TRANSLATION_KEYS.COMPANY_BUSINESS_ITEM.SECOND_BENEFIT)}</PStyled>
                                </BenefitBoxStyled>
                                <BenefitBoxStyled>
                                    <StyledIcon><CheckIcon /></StyledIcon>
                                    <PStyled sx={{ color: COLORS.WHITE }}>{t(TRANSLATION_KEYS.COMPANY_BUSINESS_ITEM.THIRD_BENEFIT)}</PStyled>
                                </BenefitBoxStyled>
                            </BenefitsBoxStyled>
                            <Box sx={{ display: "inline-block" }}>
                                <WhiteButtonStyled onClick={handleClick}>{t(TRANSLATION_KEYS.COMPANY_BUSINESS_ITEM.LEARN_MORE)} <ArrowIcon /></WhiteButtonStyled>
                            </Box>
                        </ContentBoxStyled>
                        <ImageBoxStyled>
                            <Image className="image" src="/images/business_section.png" alt="Online language lessons for businesses" height={400} width={360} quality={100} priority={true} placeholder="blur" blurDataURL={LOADING_IMAGES.BUSINESS.MAIN} />
                        </ImageBoxStyled>
                    </BusinessBenefitsBoxStyled>
                </AnimatedContainer>
            </BackgroundBoxStyled>
        </React.Fragment >)
}
export default Business;