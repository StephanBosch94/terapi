import { Box, experimental_sx as sx, styled } from "@mui/system";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import React, { useEffect } from "react";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import ErrorLarge from "components/core/error/ErrorLarge";
import TutorItemSmall from "./TutorItemSmall";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useRouter } from 'next/router';
import { useTranslation } from "components/core/TranslationProvider";
import { SecondaryButton } from "components/styles/SecondaryButton";
import { UserSearchQueryType } from "core/services/Types/AppTypes";
import routes from "routes/routes";
import NoResultsFoundWithContext from "./styles/NoResultsFound";
import { selectMatchingTutors, selectMatchingTutorsStatus, setMatchingTutors } from "core/redux/userRecommendationSlice";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import CircularProgressCustom from "components/styles/CircularProgressStyled";

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

const ButtonBoxStyled = styled(Box)(() => sx({
    width: "100%",
    display: "flex",
    justifyContent: "center",
    marginTop: "24px"
})) as typeof Box;


const MatchingTutorsEmptyBoxStyled = styled(Box)(() => sx({
    padding: "16px",
    display: "flex",
    flexDirection: "row",
    width:"100%",
    border: `1px solid ${COLORS.BLACK_10}`,
    height: "100%",
    borderRadius: "4px"
    
})) as typeof Box;

const MatchingTutors = () => {
    const dispatch = useAppDispatch();
    const matchingTutors = useAppSelector(selectMatchingTutors);
    const matchingTutorsStatus = useAppSelector(selectMatchingTutorsStatus);
    const router = useRouter();
    const { t } = useTranslation();

    useEffect(() => {
        dispatch(setMatchingTutors({
            take: COMMON_CONSTANTS.MAX_NUMBER_OF_MATCHING_TUTORS
        }));
    }, [])

    const handleShowMoreClick = () => {
        var query: UserSearchQueryType = {};

        router.push({
            pathname: routes.search.path,
            query: query
        });
    }
    const MathcingTutorsLoading = () => (
        <Box sx={{ display:"flex", alignItems: "center", justifyContent: "center", minHeight: "680px", width:"100%"}}>
            <CircularProgressCustom />
        </Box>
    )
    
    return (
        <>
            <BoxContainer>
                {matchingTutorsStatus === LoadingStatusEnum.Error ? <ErrorLarge /> : <></>}
                {matchingTutorsStatus === LoadingStatusEnum.Loading ? <MathcingTutorsLoading/> : <></>}
                {matchingTutorsStatus === LoadingStatusEnum.Loaded && matchingTutors && !matchingTutors.size ? <MatchingTutorsEmptyBoxStyled><NoResultsFoundWithContext /></MatchingTutorsEmptyBoxStyled> : <></>}
                {matchingTutorsStatus === LoadingStatusEnum.Loaded && matchingTutors && matchingTutors.size ? matchingTutors.data.map((x, idx) => (<TutorItemSmall key={x.userId!} user={x} />)) : <></>}
            </BoxContainer>
            {matchingTutorsStatus === LoadingStatusEnum.Loaded && matchingTutors && matchingTutors.size ? (
                <ButtonBoxStyled>
                    <SecondaryButton onClick={handleShowMoreClick}>{t(TRANSLATION_KEYS.MATCHING_TUTORS.BUTTON_TEXT)}</SecondaryButton>
                </ButtonBoxStyled>
            ) : <></>}
        </>
    )
}

export default MatchingTutors;