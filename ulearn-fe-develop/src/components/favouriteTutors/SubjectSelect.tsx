import { styled, experimental_sx as sx } from "@mui/material/styles";
import React, { useState } from "react";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { ScrollBoxStyled } from "components/styles/boxes/ScrollBoxStyled";
import { useTranslation } from "components/core/TranslationProvider";
import { useAppDataContext } from "components/core/providers/AppProvider";
import Box from "@mui/material/Box";
import Alert from "@mui/material/Alert";

const ContainerStyled = styled(Box)(() => sx({
    display: "inline-flex",
    justifyContent: "flex-start",
    width: "100%",
    p: "4px",
    gap: "8px",
    borderRadius: "4px",
    padding: "6px",
    backgroundColor: COLORS.WHITE,
    border: `1px solid ${COLORS.BLACK_10}`,
    overflowX: "auto",
    "::-webkit-scrollbar": {
        display: "none"
    },
    ".active": {
        backgroundColor: COLORS.PRIMARY,
        color: COLORS.WHITE,
        ":hover": {
            backgroundColor: COLORS.PRIMARY
        }
    }
})) as typeof Box;

const ButtonStyled = styled(Box)(() => sx({
    display: "flex",
    py: "4px",
    px: "8px",
    color: COLORS.BLACK,
    borderRadius: "4px",
    minWidth: "max-content",
    fontWeight: 400,
    width: "auto",
    fontSize: { xs: "0.95rem", md: "0.875rem" },
    border: "none",
    backgroundColor: "transparent",
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_10
    }
})) as typeof Box;

const SubjectSelect = ({ setActiveId, defaultId = 0 }: { setActiveId: (idx: number) => void, defaultId?: number | undefined }) => {
    const MAX_NUMBER_OF_SHOWED_SUBJECTS = 10;
    const { t } = useTranslation();
    const [selectedId, setSelectedId] = useState(defaultId ?? 0);
    const { subjects } = useAppDataContext();

    const updateIdx = (idx: number) => {
        setSelectedId(idx);
        setActiveId(idx);
    }

    const handleAllButtonClick = () => { updateIdx(0); }
    const handleItemClick = (itemId: number) => { updateIdx(itemId); }

    return (
        <ScrollBoxStyled>
            {!subjects && <ContainerStyled><Alert severity="error" className="active">{t(TRANSLATION_KEYS.ERRORS.GENERAL_ERROR)}</Alert></ContainerStyled>}
            {subjects &&
                <ContainerStyled>
                    <ButtonStyled onClick={handleAllButtonClick} className={selectedId === 0 ? "active" : "passive"}>{t(TRANSLATION_KEYS.POPULAR_TUTORS.ALL_SUBJECTS_BUTTON)}</ButtonStyled>
                    {subjects.slice(0, MAX_NUMBER_OF_SHOWED_SUBJECTS).map((subject, idx) => (
                        <ButtonStyled onClick={() => handleItemClick(subject.subjectId!)} key={subject.subjectId!} className={selectedId === subject.subjectId! ? "active" : "passive"} >{t(subject.nameKey ?? "")}</ButtonStyled>
                    ))}

                </ContainerStyled>
            }
        </ScrollBoxStyled>
    )
}

export default SubjectSelect;