import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import React, { useEffect, useState } from "react";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import H2Styled from "components/styles/typography/H2Styled";
import SubjectSelect from "./SubjectSelect";
import TutorItemSmallList from "components/tutor/TutorItemSmallList";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { CodetableSubjectsResponse } from "core/redux/codetableSlice";
import Box from "@mui/material/Box";
import { AnimatedContainer } from "components/styles/AnimatedContainerStyled";

const BackgroundBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.BACKGROUND,
    padding: "60px 0px"
})) as typeof Box;


const PopularTutors = ({ bgColor, subject, showFilter = true }: { bgColor: string, subject?: CodetableSubjectsResponse[0] | undefined, showFilter?: boolean }) => {
    const [subjectId, setSubjectId] = useState(subject?.subjectId ?? 0);
    const { t } = useTranslation();

    const handleSubjectSelect = (subjectIdx: number) => {
        setSubjectId(subjectIdx);
    }

    useEffect(() => {
        setSubjectId(subject?.subjectId ?? 0);
    }, [subject]);

    return (
        <BackgroundBoxStyled sx={{ backgroundColor: bgColor }}>
            <AnimatedContainer>
                <H2Styled>{t(TRANSLATION_KEYS.POPULAR_TUTORS.SECTION_TITLE)}</H2Styled>
                {showFilter ? (
                    <Box sx={{ marginTop: "16px" }}>
                        <SubjectSelect setActiveId={handleSubjectSelect} defaultId={subject?.subjectId} />
                    </Box>
                ) : <></>}
                <Box sx={{ marginTop: "24px" }}>
                    <TutorItemSmallList subjectId={subjectId} />
                </Box>
            </AnimatedContainer>
        </BackgroundBoxStyled>
    )
}

export default PopularTutors;
