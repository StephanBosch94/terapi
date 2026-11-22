import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import H2Styled from "components/styles/typography/H2Styled";
import PStyled from "components/styles/typography/PStyled";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import KeyboardArrowUpIcon from 'assets/caret-up.svg';
import KeyboardArrowDownIcon from 'assets/caret-down.svg';
import React from "react";
import { QuestionAndAnswersType as QuestionAndAnswers } from "core/services/Types/AppTypes";
import H5Styled from "components/styles/typography/H5Styled";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";
import { AnimatedContainer } from "components/styles/AnimatedContainerStyled";

const BackgroundBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.BACKGROUND,
    padding: "60px 0px"
})) as typeof Box;

const ItemListStyled = styled(Box)(() => sx({
    display: "flex",
    width: "100%",
    flexDirection: "column",
    color: COLORS.BLACK,
    backgroundColor: COLORS.WHITE,
    borderRadius: "4px",
    border: `1px solid ${COLORS.BLACK_10}`,
    padding: { xs: "20px 16px 20px 16px", md: "20px 16px 20px 23px" },
    gap: "16px",
    transition: "border-color .3s",
    ":hover": {
        padding: { xs: "20px 16px 20px 13px", md: "20px 16px 20px 16px" },
        cursor: "pointer",
        borderTop: `1px solid ${COLORS.BLACK_10}`,
        borderBottom: `1px solid ${COLORS.BLACK_10}`,
        borderRight: `1px solid ${COLORS.BLACK_10}`,
        borderLeft: `8px solid ${COLORS.PRIMARY}`,
        backgroundColor: COLORS.PRIMARY_5
    }

})) as typeof Box;

const ItemFirstRowStyled = styled(Box)(() => sx({
    display: "flex",
    width: "100%",
    flexFlow: "row noWrap",
    alignItems: "center",
    justifyContent: "space-between",
    gap: "8px",
    transition: "border-color 0.2s ease 0s, box-shadow 0.2s ease 0s",
    boxShadow: "none"
})) as typeof Box;

const IconStyled = styled(Box)(() => sx({
    svg: {
        transform: "scale(1)"
    }
})) as typeof Box;

const QuestionItem = ({ name, description }: { name: string, description: string }) => {
    const { t, tContains } = useTranslation();
    const [open, setOpen] = React.useState(false);

    return tContains(name) && tContains(description) && t(name) !== "" && t(description) !== "" ? (
        <Grid item columns={12} style={{ padding: "0px", marginBottom: "16px" }}>
            <ItemListStyled
                sx={{
                    borderLeft: { md: `1px solid ${COLORS.BLACK_10}`, xs: open ? `8px solid ${COLORS.PRIMARY}` : `1px solid ${COLORS.BLACK_10}` },
                    padding: { md: "20px 16px 20px 23px", xs: open ? "20px 16px 20px 13px" : "20px 16px 20px 20px" },
                }}
                onClick={() => { setOpen(!open) }}>
                <ItemFirstRowStyled >
                    <H5Styled>{t(name)}</H5Styled>
                    {open ? <IconStyled><KeyboardArrowUpIcon /></IconStyled> : <IconStyled><KeyboardArrowDownIcon /></IconStyled>}
                </ItemFirstRowStyled>
                <PStyled sx={{ display: open ? "block" : "none", paddingRight: "32px", color: COLORS.BLACK }}>{t(description)}</PStyled>
            </ItemListStyled>
        </Grid>
    ) : <></>
}
const QuestionAndAnswers = ({ questionsAndAnswers, bgColor }: { questionsAndAnswers: QuestionAndAnswers[], bgColor: string }) => {
    const { t } = useTranslation();
    return (
        <BackgroundBoxStyled sx={{ backgroundColor: bgColor }}>
            <AnimatedContainer>
                <H2Styled sx={{ marginBottom: "24px" }}>{t(TRANSLATION_KEYS.STUDENT_QUESTION_AND_ANSWERS.SECTION_TITLE)}</H2Styled>
                <Grid container columns={12} spacing={4} sx={{ display: "flex", flexDirection: "column", width: "100%", margin: "0px" }}>
                    {questionsAndAnswers.map((key, index) => <React.Fragment key={index}><QuestionItem name={key.nameKey} description={key.descriptionKey} /></React.Fragment>)}
                </Grid>
            </AnimatedContainer>
        </BackgroundBoxStyled>
    )
}
export default QuestionAndAnswers;