import { styled, experimental_sx as sx } from "@mui/material/styles";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import WaveIcon from "assets/emoji/wave.svg";
import { useContext } from "react";
import { UIContext } from "components/core/UIContextProvider";
import Box from "@mui/material/Box";
import H1Styled from "components/styles/typography/H1Styled";
import PStyled from "components/styles/typography/PStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";

const TitleDektopBoxStyled = styled(Box)<{ scrolled: string }>(({ scrolled }) => sx({
    color: scrolled === "true" ? COLORS.WHITE : COLORS.BLACK,
    display: "inline-flex",
    alignItems: "center",
    gap: "8px",
    "svg": {
        transform: "scale(1.5)",
        margin: "auto 8px"
    }
}));

const SvgBoxItem = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    "svg": {
        transform: "scale(1.5)"
    }
})) as typeof Box;

const PseudoH1Styled = styled(PStyled)(() => sx({
    fontSize: { xs: "1.5rem !important", md: "2rem !important", fontWeight: "800", color: "inherit" }
})) as typeof PStyled;

const MainH1Styled = styled(H1Styled)(() => sx({
    fontSize: { xs: "1.5rem !important", md: "2rem !important" }
})) as typeof H1Styled;

const SearchGreeting = ({ isScrolled, textVariant = "h1" }: { isScrolled: boolean, textVariant?: "h1" | "body1" }) => {
    const { t } = useTranslation();
    const { screenType } = useContext(UIContext);

    return (
        <TitleDektopBoxStyled scrolled={isScrolled.toString()}>
            {screenType === "desktop" ? (
                <>
                    <Box><PseudoH1Styled>{t(TRANSLATION_KEYS.SEARCH.GREETING)}</PseudoH1Styled></Box>
                    <SvgBoxItem><WaveIcon /></SvgBoxItem>
                </>
            ) : <></>}
            <Box>
                {textVariant === "h1" ? <MainH1Styled>{t(TRANSLATION_KEYS.SEARCH.TITLE)}</MainH1Styled> : <></>}
                {textVariant === "body1" ? <PseudoH1Styled>{t(TRANSLATION_KEYS.SEARCH.TITLE)}</PseudoH1Styled> : <></>}
            </Box>
        </TitleDektopBoxStyled>
    )
}

export default SearchGreeting;