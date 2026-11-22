import { styled, experimental_sx as sx } from "@mui/material/styles";
import { PrimaryButton } from "components/styles/PrimaryButton";
import PStyled from "components/styles/typography/PStyled";
import React from "react";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import routes from "routes/routes";
import Link from "next/link";
import { PrimaryButtonRectangle } from "../styles/PrimaryButtonRectangle";
import H2Styled from "../styles/typography/H2Styled";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";

const StyledBox = styled(Box)(() => sx({
    maxWidth: "460px",
    padding: "40px",
    display: "flex",
    flexDirection: "column",
    borderRadius: "4px",
    border: `1px solid ${COLORS.BLACK_10}`,
    alignItems: "center",
    textAlign: "center",
    backgroundColor: COLORS.WHITE
})) as typeof Box;

const StyledText = styled(Typography)(() => sx({
    fontWeight: 700,
    fontFamily: "inherit",
    fontSize: "6rem",
    color: COLORS.PRIMARY
})) as typeof Typography;

const StyledPrimaryButton = styled(PrimaryButtonRectangle)(() => sx({
    width: "100%",
})) as typeof PrimaryButton;

const HorizontalDivider = styled(Box)(() => sx({
    height: "1px",
    width: "100%",
    backgroundColor: COLORS.BLACK_10,
    margin: "40px 0px"
})) as typeof Box;

const PageNotFoundBox = () => {
    const { t } = useTranslation();

    return (
        <StyledBox>
            <H2Styled>{t(TRANSLATION_KEYS.NO_FOUND_PAGE.TITLE)}</H2Styled>
            <StyledText>{t(TRANSLATION_KEYS.NO_FOUND_PAGE.HEADING)}</StyledText>
            <PStyled>{t(TRANSLATION_KEYS.NO_FOUND_PAGE.DESCRIPTION)}</PStyled>
            <HorizontalDivider />
            <Link href={routes.home.path}><StyledPrimaryButton>{t(TRANSLATION_KEYS.NO_FOUND_PAGE.BUTTON_TEXT)}</StyledPrimaryButton></Link>
        </StyledBox>
    );

}; export default PageNotFoundBox;

