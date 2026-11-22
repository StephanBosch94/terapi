import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import React, { useContext } from "react";
import { useTranslation } from "components/core/TranslationProvider";
import routes from "routes/routes";
import Body1Styled from "components/styles/typography/Body1Styled";
import Box from "@mui/material/Box";
import Link from "next/link";
import { WhiteButtonStyled } from "components/styles/WhiteButtonStyled";
import { UIContext } from "components/core/UIContextProvider";
import { WhiteButtonCompactedStyled } from "components/styles/WhiteButtonCompactedStyled";
import PartyIcon from "assets/emoji/party_popper.svg";
import { useAppSelector } from "core/redux/reduxHooks";
import { selectShortUserInfo } from "core/redux/userSlice";

const InnerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "row wrap",
    gap: "12px",
    flex: "1 1 auto"
})) as typeof Box;

const TitleStyled = styled(Body1Styled)(() => sx({
    color: COLORS.WHITE
})) as typeof Body1Styled;

const IconBoxStyled = styled(Box)(() => sx({
    width: "auto",
    svg: { transform: "scale(1.25)" },
    display: { xs: "none", md: "flex" }
})) as typeof Box;

const BoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: { xs: "start", md: "center" },
    justifyContent: "space-between",
    padding: "12px 12px",
    justifyItems: "center",
    flexFlow: { xs: "column noWrap", md: "row noWrap" },
    width: "100%",
    gap: "16px",
    backgroundColor: COLORS.PRIMARY,
    borderRadius: "4px"
})) as typeof Box;

const BookLesson = ({ titleKey, buttonKey, subjectId, showIcon = true }: { titleKey: string, buttonKey: string, subjectId: number | null | undefined, showIcon?: boolean }) => {
    const { t } = useTranslation();
    const { screenType } = useContext(UIContext);
    const param = subjectId ? `?subjectId=${subjectId}` : '';

    return (
        <BoxStyled>
            <InnerBoxStyled>
                {showIcon ? <IconBoxStyled><PartyIcon /></IconBoxStyled> : <></>}
                <TitleStyled>{t(titleKey)}</TitleStyled>
            </InnerBoxStyled>
            <Link href={routes.search.path + param} style={{ textDecoration: "none" }}>
                {screenType === "mobile" ? <WhiteButtonCompactedStyled>{t(buttonKey)}</WhiteButtonCompactedStyled> : <></>}
                {screenType === "desktop" ? <WhiteButtonStyled>{t(buttonKey)}</WhiteButtonStyled> : <></>}
            </Link>
        </BoxStyled>
    )
}

const BookLessonOptional = () => {
    const shortUserInfo = useAppSelector(selectShortUserInfo);
    if (!shortUserInfo) return null;

    return (
        <>
            {!shortUserInfo?.hasBookedAnyLesson ? (
                <BookLesson
                    subjectId={shortUserInfo?.subjectId}
                    titleKey={TRANSLATION_KEYS.STUDENT_DASHBOARD.BOOK_FIRST_LESSON_CTA.HEADING}
                    buttonKey={TRANSLATION_KEYS.STUDENT_DASHBOARD.BOOK_FIRST_LESSON_CTA.BUTTON}
                />) : <></>}
            {shortUserInfo?.hasBookedAnyLesson && !shortUserInfo?.hasFutureBookings ? (
                <BookLesson
                    subjectId={shortUserInfo?.subjectId}
                    titleKey={TRANSLATION_KEYS.STUDENT_DASHBOARD.BOOK_LESSON_CTA.HEADING}
                    buttonKey={TRANSLATION_KEYS.STUDENT_DASHBOARD.BOOK_LESSON_CTA.BUTTON}
                    showIcon={false}
                />) : <></>}
        </>
    );
}

export default BookLessonOptional;