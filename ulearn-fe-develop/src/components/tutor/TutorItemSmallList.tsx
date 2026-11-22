import { Box, experimental_sx as sx, styled } from "@mui/system";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import React, { useEffect } from "react";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import ErrorLarge from "components/core/error/ErrorLarge";
import TutorItemSmall from "./TutorItemSmall";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useRouter } from 'next/router';
import { useTranslation } from "components/core/TranslationProvider";
import { SecondaryButton } from "components/styles/SecondaryButton";
import { UserSearchQueryType } from "core/services/Types/AppTypes";
import routes from "routes/routes";
import NoResultsFoundWithContext from "./styles/NoResultsFound";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { selectPopularTutorsFilter, selectPopularTutorsFilterStatus, setPopularTutorsFilter } from "core/redux/userRecommendationSlice";

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

const TutorListBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    minHeight: "748px"
})) as typeof Box;

const TutorSmallItemListLoading = () => (
    <TutorListBoxStyled sx={{ width: "100%", alignItems: "center", justifyContent: "center" }}>
        <CircularProgressCustom />
    </TutorListBoxStyled>
)

const TutorSmallItemListError = () => (
    <TutorListBoxStyled sx={{ width: "100%", alignItems: "center", justifyContent: "center" }}>
        <ErrorLarge />
    </TutorListBoxStyled>
)

const TutorItemSmallList = ({ subjectId }: { subjectId: number }) => {
    const COUNT_OF_DEFAULT_SKELETON_ITEMS = 10;
    const dispatch = useAppDispatch();
    const users = useAppSelector(selectPopularTutorsFilter);
    const usersStatus = useAppSelector(selectPopularTutorsFilterStatus);
    const router = useRouter();
    const { t } = useTranslation();

    useEffect(() => {
        dispatch(setPopularTutorsFilter({
            request: {
                filter: {
                    subjectId: subjectId > 0 ? subjectId : null,
                },
                take: COUNT_OF_DEFAULT_SKELETON_ITEMS
            },
            appendType: AppendTypeEnum.None
        }));
    }, [subjectId])

    const handleClick = () => {
        var query: UserSearchQueryType = {};
        if (subjectId) {
            query.subjectId = `${subjectId}`;
        }

        router.push({
            pathname: routes.search.path,
            query: query,
        });
    }

    return (
        <>
            <BoxContainer>
                {usersStatus === LoadingStatusEnum.Error ? <TutorSmallItemListError /> : <></>}
                {usersStatus === LoadingStatusEnum.Loading ? <TutorSmallItemListLoading /> : <></>}
                {usersStatus === LoadingStatusEnum.Loaded && users && !users.size ? <NoResultsFoundWithContext /> : <></>}
                {usersStatus === LoadingStatusEnum.Loaded && users && users.size ? users.data.map((x, idx) => (<TutorItemSmall key={x.userId!} user={x} />)) : <></>}
            </BoxContainer>
            {usersStatus === LoadingStatusEnum.Loaded && users && users.size ? (
                <ButtonBoxStyled>
                    <SecondaryButton onClick={handleClick}>{t(TRANSLATION_KEYS.MATCHING_TUTORS.BUTTON_TEXT)}</SecondaryButton>
                </ButtonBoxStyled>
            ) : <></>}
        </>
    )
}

export default TutorItemSmallList