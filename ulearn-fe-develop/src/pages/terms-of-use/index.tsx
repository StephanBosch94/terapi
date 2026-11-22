import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";
import React from "react";
import Navbar, { NavbarSourceEnum } from "components/navbar/Navbar";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import H3Styled from "components/styles/typography/H3Styled";;
import PStyled from "components/styles/typography/PStyled";
import Footer from "components/landingPage/Footer";
import { useTranslation } from "components/core/TranslationProvider";
import { getDefaultStaticProps } from "core/services/Utils/ssrUtil";
import { withAppProvider } from "components/core/providers/AppProvider";
import { GetStaticProps } from "next";
import Container from "@mui/material/Container";

const BackgroundBoxStyled = styled(Box)(() => ({
    backgroundColor: COLORS.WHITE
})) as typeof Box;

const StyledContainer = styled(Container)(() => ({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    flexDirection: "column"
})) as typeof Container;

const ContentBoxStyled = styled(Box)(() => ({
    padding: "32px 0px",
    display: "flex",
    flexDirection: "column",
    maxWidth: "840px",
    gap: "16px"
})) as typeof Box;

export const getStaticProps: GetStaticProps = async (props) => getDefaultStaticProps(props);

const TermsOfUse = () => {
    const { t } = useTranslation();

    return (
        <>
            <Navbar source={NavbarSourceEnum.studentPage} />
            <BackgroundBoxStyled>
                <StyledContainer>
                    <ContentBoxStyled>
                        <H3Styled>Základní informace</H3Styled>
                        <PStyled sx={{ lineHeight: "32px", color: COLORS.BLACK }}>TODO</PStyled>
                    </ContentBoxStyled>
                    <ContentBoxStyled>
                        <H3Styled>Základní informace</H3Styled>
                        <PStyled sx={{ lineHeight: "32px", color: COLORS.BLACK }}>TODO</PStyled>
                    </ContentBoxStyled>
                </StyledContainer>
            </BackgroundBoxStyled>
            <Footer bgColor={COLORS.BACKGROUND} />
        </>

    );
};

export default withAppProvider(TermsOfUse);
