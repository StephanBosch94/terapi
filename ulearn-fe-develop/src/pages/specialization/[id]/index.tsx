import { styled, experimental_sx as sx } from "@mui/material/styles";
import React, { useContext } from "react";
import Footer from "components/landingPage/Footer";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import Navbar, { NavbarSourceEnum } from "components/navbar/Navbar";
import { useTranslation } from "components/core/TranslationProvider";
import Box from "@mui/material/Box";
import Copyright from "components/landingPage/Copyright";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { DefaultPagePropsType, fetchDefaultPageDataFromProps } from "core/services/Utils/ssrUtil";
import { GetServerSideProps } from "next";
import { withAppProvider } from "components/core/providers/AppProvider";
import { NextSeo } from "next-seo";
import { useRouter } from "next/router";
import { getOpenGraphConfig, getSeoLanguageAlternatives } from "components/core/Seo";
import { getFullUrlWithLocale } from "core/services/Utils/urlUtil";
import { SpecializationType, SubjectType, UserSearchQueryParsed } from "core/services/Types/AppTypes";
import { parseSearchQuery } from "routes/routesQuery";
import Search from "components/search/Search";
import UserService from "core/services/UserService";
import { removeUndefinedProperties } from "core/services/Utils/objectUtils";
import { FilterTutorsResponseBody } from "core/redux/userSlice";
import { UIContext } from "components/core/UIContextProvider";
import GoogleOneTap from "components/auth/GoogleOneTap";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import StudentQuestionAndAnswers from "components/landingPage/student/StudentQuestionAndAnswers";
import StudentCtaSection from "components/landingPage/student/StudentCtaSection";
import SpecializationHero from "components/landingPage/SpecializationHero";
import SpecializationsBrowser from "components/landingPage/SpecializationsBrowser";

const BackgroundBoxStyled = styled(Box)(() => sx({
    padding: "60px 0px",
    backgroundColor: COLORS.BACKGROUND
})) as typeof Box;

type PropsType = {
    query: UserSearchQueryParsed,
    subject: SubjectType,
    specialization: SpecializationType,
    tutors: FilterTutorsResponseBody
};
type PageDataType = DefaultPagePropsType & PropsType;

export const getServerSideProps: GetServerSideProps<PageDataType> = async (props) => {
    try {
        // temporarily disable cache due to mobile device screen detection
        // props.res.setHeader('Cache-Control', getSsrCacheConfig())

        const specializationKey = props.params?.id as string;
        const query = parseSearchQuery(props.query);
        const staticData = await fetchDefaultPageDataFromProps(props);
        const specialization = staticData?.specializations?.find(x => x.enumKey === specializationKey);

        if (!specialization) {
            throw new Error("Invalid specialization key");
        }

        const subject = staticData?.subjects?.find(x => x.subjectId === specialization.subjectId);
        if (!subject) {
            throw new Error("Invalid subject id");
        }

        const tutors = await UserService.filterTutors({
            filter: {
                specializationIds: query.specializationIds ?? [specialization.specializationId!],
                toTrialPrice: query.toTrialPrice,
                subjectId: query.toTrialPrice ?? specialization.subjectId
            }
        });

        const resolvedQuery = removeUndefinedProperties({
            ...query,
            subjectId: subject.subjectId,
            specializationIds: [...new Set([...(query.specializationIds ?? []), specialization.specializationId!])]
        });

        return {
            props: {
                ...staticData,
                subject,
                specialization,
                tutors: tutors.data,
                query: resolvedQuery
            }
        }
    } catch (error) {
        console.error(error);
        return {
            notFound: true
        }
    }
};

const Seo = ({ specialization }: { specialization: SpecializationType }) => {
    const { t } = useTranslation();
    const router = useRouter();
    const title = t(specialization.nameKey ?? "");
    const description = t(specialization.heroDescriptionKey ?? "");
    const relativeUrl = router.pathname.replace('[id]', specialization.enumKey ?? "");

    return (
        <NextSeo
            title={title}
            description={description}
            openGraph={getOpenGraphConfig({ relativeUrl, locale: router.locale, title, description, imagePath: '/images/open-graph-subject.png' })}
            languageAlternates={getSeoLanguageAlternatives(router.locales, relativeUrl)}
            canonical={getFullUrlWithLocale(router.locale, relativeUrl)}
        />
    )
}

const TutorSearch = ({ specialization, query, tutors, bgColor }: PropsType & { bgColor: string }) => {
    const router = useRouter();
    const relativePath = router.pathname.replace('[id]', specialization.enumKey ?? "");

    return (
        <BackgroundBoxStyled sx={{ backgroundColor: bgColor }}>
            <Search
                relativePath={relativePath}
                query={query}
                initData={tutors}
                disabledSpecializationId={specialization.specializationId}
                source="specialization_page"
            />
        </BackgroundBoxStyled>
    )
}

const SubjectLandingPage = (props: PropsType) => {
    const { screenType } = useContext(UIContext);
    const isDesktop = screenType === "desktop";

    return (
        <>
            <Seo specialization={props.specialization} />
            <GoogleOneTap role={UserRoleEnum.Student} autoRedirectToHomePage={false} />
            <Box>
                <Navbar source={NavbarSourceEnum.studentPage} position={isDesktop ? "static" : "sticky"} />
                <SpecializationHero specialization={props.specialization} bgColor={COLORS.BACKGROUND} />
                <TutorSearch {...props} bgColor={COLORS.WHITE} />
                {/* <StudentTestemonials bgColor={COLORS.WHITE} /> */}
                <StudentCtaSection bgColor={COLORS.BACKGROUND} />
                <StudentQuestionAndAnswers bgColor={COLORS.WHITE} />
                <SpecializationsBrowser
                    subjectId={props.specialization.subjectId!}
                    titleKey={TRANSLATION_KEYS.SUBJECTS.OTHER_SPECIALIZATIONS_TITLE} 
                    excludeSpecializationId={props.specialization.specializationId}
                    bgColor={COLORS.BACKGROUND}
                />
                <Footer bgColor={COLORS.WHITE} />
                <Copyright />
            </Box>
        </>
    )
}

export default withAppProvider<PageDataType>(SubjectLandingPage) 