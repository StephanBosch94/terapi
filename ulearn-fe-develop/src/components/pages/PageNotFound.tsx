import React from "react";
import Navbar, { NavbarSourceEnum } from "components/navbar/Navbar";
import PageNotFoundBox from "components/pages/PageNotFoundBox";
import { styled } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import { NextSeo } from 'next-seo';
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";

const StyledContainer = styled(Container)(() => ({
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  height: "calc(100vh - 56px)",
})) as typeof Container;

const BackgroundBoxStyled = styled(Box)(() => ({
  backgroundColor: COLORS.WHITE
})) as typeof Box;

const PageNotFound = () => {
  const { t } = useTranslation();

  <NextSeo title={t(TRANSLATION_KEYS.PAGE_TITLE.PAGE_NOT_FOUND)} />

  return (
    <>
      <Navbar source={NavbarSourceEnum.studentPage} />
      <BackgroundBoxStyled>
        <StyledContainer>
          <PageNotFoundBox />
        </StyledContainer>
      </BackgroundBoxStyled>
    </>
  );
};


export default PageNotFound;
