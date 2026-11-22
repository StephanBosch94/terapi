import React from "react";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import PStyled from "components/styles/typography/PStyled";
import { CtaType } from "core/services/Types/AppTypes";
import H3Styled from "components/styles/typography/H3Styled";;
import { TertiaryButton } from "components/styles/TertiaryButton";
import AuthModalProvider, { useAuthModalContext } from "components/modals/AuthModalProvider";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import Box from "@mui/material/Box";
import ArrowIcon from "assets/arrow-right.svg";
import { AnimatedContainer } from "components/styles/AnimatedContainerStyled";


const BackgroundBoxStyled = styled(Box)(() => sx({
    padding: { xs: "60px 0px", md: "120px 0px" }
})) as typeof Box;

const CtaBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
    padding: { xs: "24px", md: "56px" },
    borderRadius: "8px",
    width: "100%",
    gap: "32px",
    backgroundColor: COLORS.PRIMARY,
})) as typeof Box;

const TextBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    gap: "16px",
    alignItems: "center",
})) as typeof Box;


const TertiaryButtonStyled = styled(TertiaryButton)(() => sx({
    borderRadius: "24px",
    gap: "4px"
})) as typeof TertiaryButton;

type PageSourceType = "studentPage" | "tutorPage" | "companyPage";

const CtaSection = ({ cta, bgColor, source }: { cta: CtaType, bgColor: string, source: PageSourceType }) => {
    const { t } = useTranslation();
    const { setOpen } = useAuthModalContext();

    return (
        <React.Fragment>
            <BackgroundBoxStyled sx={{ backgroundColor: bgColor }}>
                <AnimatedContainer>
                    <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
                        <CtaBoxStyled>
                            <TextBoxStyled>
                                <H3Styled sx={{ color: COLORS.WHITE, textAlign: "center" }} >{t(cta.headingKey)}</H3Styled>
                                <PStyled sx={{ color: COLORS.WHITE, textAlign: "center" }}>{t(cta.descriptionKey)}</PStyled>
                            </TextBoxStyled>
                            <Box>
                                {source === "studentPage" ? <TertiaryButtonStyled onClick={() => setOpen(UserRoleEnum.Student, "registration")}>{t(cta.buttonTextKey)} <ArrowIcon/></TertiaryButtonStyled> : <></>}
                                {source === "tutorPage" ? <TertiaryButtonStyled onClick={() => setOpen(UserRoleEnum.Tutor, "registration")}>{t(cta.buttonTextKey)} <ArrowIcon/></TertiaryButtonStyled> : <></>}
                                {source === "companyPage" ? <TertiaryButtonStyled onClick={() => setOpen(UserRoleEnum.Company, "registration")}>{t(cta.buttonTextKey)} <ArrowIcon/></TertiaryButtonStyled> : <></>}
                            </Box>
                        </CtaBoxStyled>
                    </Box>
                </AnimatedContainer>
            </BackgroundBoxStyled>
        </React.Fragment>
    )
}

const CtaSectionWithProviders = (props: { cta: CtaType, bgColor: string, source: PageSourceType }) => {

    return (
        <AuthModalProvider>
            <CtaSection {...props} />
        </AuthModalProvider>
    )
}

export default CtaSectionWithProviders;