import { Box, experimental_sx as sx, styled } from "@mui/system";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import React, { useEffect } from "react";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import ErrorLarge from "components/core/error/ErrorLarge";
import TutorItemSmall from "./TutorItemSmall";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import { FilterSimilarTutorsResponseBody, selectSimilarTutorsFilter, selectSimilarTutorsFilterStatus, setFilterSimilarUsers } from "core/redux/userRecommendationSlice";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";
import H2Styled from "components/styles/typography/H2Styled";
import { ContainerStyled } from "components/styles/ContainerStyled";

const BoxContainer = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "flex-start",
    width: "100%",
    gap: "8px",
    flexFlow: { xs: "row noWrap", md: "row wrap" },
    overflowX: "auto",
    "::-webkit-scrollbar": {
        display: "none"
    }
})) as typeof Box;

const BackgroundBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.BACKGROUND,
    padding: "60px 0px"
})) as typeof Box;

const SimilarTutors = ({ tutors }: { tutors: FilterSimilarTutorsResponseBody }) => {
    const { t } = useTranslation();
    return (
        <Box sx={{ display: "flex", gap: "24px", flexFlow: "column noWrap" }}>
            <H2Styled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.SIMILAR_TUTORS)}</H2Styled>
            <BoxContainer>
                {tutors.data.map((x, idx) => (<TutorItemSmall key={x.userId!} user={x} />))}
            </BoxContainer>
        </Box>
    )
}

const SimilarTutorsLoading = () => (
    <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", minHeight: "680px", width: "100%" }}>
        <CircularProgressCustom />
    </Box>
)

const SimilarTutorsError = () => (
    <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", minHeight: "680px", width: "100%" }}>
        <ErrorLarge />
    </Box>
)

const SimilarTutorsOptional = ({ userId, amount = 5 }: { userId: number, amount?: number }) => {
    const dispatch = useAppDispatch();
    const similarTutors = useAppSelector(selectSimilarTutorsFilter);
    const similarTutorsStatus = useAppSelector(selectSimilarTutorsFilterStatus);
    const show = similarTutorsStatus === LoadingStatusEnum.Loading || (similarTutors && similarTutors.size && similarTutors.size > 0);

    useEffect(() => {
        dispatch(setFilterSimilarUsers({
            appendType: AppendTypeEnum.None,
            request: {
                userId: userId,
                take: amount
            }
        }));
    }, [])

    return (
        <BackgroundBoxStyled sx={{ display: show ? "flex" : "none" }}>
            <ContainerStyled>
                {similarTutorsStatus === LoadingStatusEnum.Error ? <SimilarTutorsError /> : <></>}
                {similarTutorsStatus === LoadingStatusEnum.Loading ? <SimilarTutorsLoading /> : <></>}
                {similarTutorsStatus === LoadingStatusEnum.Loaded && similarTutors && similarTutors.size ? <SimilarTutors tutors={similarTutors} /> : <></>}
            </ContainerStyled>
        </BackgroundBoxStyled>
    )
}

export default SimilarTutorsOptional;