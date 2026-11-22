import { styled, experimental_sx as sx } from "@mui/material/styles";
import React, { useContext } from "react";
import Footer from "components/landingPage/Footer";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import Navbar, { NavbarSourceEnum } from "components/navbar/Navbar";
import { useTranslation } from "components/core/TranslationProvider";
import Box from "@mui/material/Box";
import Copyright from "components/landingPage/Copyright";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import Hero from "components/landingPage/SubjectHero";
import SubjectsBrowser from "components/landingPage/SubjectsBrowser";
import { DefaultPagePropsType, fetchDefaultPageDataFromProps, getSsrCacheConfig } from "core/services/Utils/ssrUtil";
import { GetServerSideProps } from "next";
import { withAppProvider } from "components/core/providers/AppProvider";
import { NextSeo } from "next-seo";
import { useRouter } from "next/router";
import { getOpenGraphConfig, getSeoLanguageAlternatives } from "components/core/Seo";
import { getFullUrlWithLocale } from "core/services/Utils/urlUtil";
import { SubjectType, UserSearchQueryParsed } from "core/services/Types/AppTypes";
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
import SpecializationsBrowser from "components/landingPage/SpecializationsBrowser";


const BackgroundBoxStyled = styled(Box)(() => sx({
    padding: "60px 0px",
    backgroundColor: COLORS.BACKGROUND
})) as typeof Box;


type PropsType = {
    query: UserSearchQueryParsed,
    subject: SubjectType,
    tutors: FilterTutorsResponseBody
};
type PageDataType = DefaultPagePropsType & PropsType;

export const getServerSideProps: GetServerSideProps<PageDataType> = async (props) => {
    try {
        // temporarily disable cache due to mobile device screen detection
        // props.res.setHeader('Cache-Control', getSsrCacheConfig())
        
        const subjectKey = props.params?.id as string;
        const query = parseSearchQuery(props.query);
        const staticData = await fetchDefaultPageDataFromProps(props);
        const subject = staticData?.subjects?.find(x => x.enumKey === subjectKey);

        if (!subject) {
            throw new Error("Invalid subject key");
        }

        const tutors = await UserService.filterTutors({
            filter: {
                specializationIds: query.specializationIds,
                toTrialPrice: query.toTrialPrice,
                subjectId: subject.subjectId
            }
        });

        return {
            props: {
                ...staticData,
                subject: subject,
                tutors: tutors.data,
                query: removeUndefinedProperties({ ...query, subjectId: subject.subjectId })
            }
        }
    } catch (error) {
        console.error(error);
        return {
            notFound: true
        }
    }
};

const Seo = ({ subject }: { subject: SubjectType }) => {
    const { t } = useTranslation();
    const router = useRouter();
    const title = t(subject.pageTitleKey ?? "");
    const description = t(subject.heroTitleDescriptionKey ?? "");
    const relativeUrl = router.pathname.replace('[id]', subject.enumKey ?? "");

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

const TutorSearch = ({ subject, query, tutors }: PropsType) => {
    const router = useRouter();
    const relativePath = router.pathname.replace('[id]', subject.enumKey ?? "");

    return (
        <BackgroundBoxStyled>
            <Search relativePath={relativePath} query={query} initData={tutors} source="subject_page" />
        </BackgroundBoxStyled>
    )
}

const SubjectLandingPage = (props: PropsType) => {
    const { screenType } = useContext(UIContext);
    const isDesktop = screenType === "desktop";

    return (
        <>
            <Seo subject={props.subject} />
            <GoogleOneTap role={UserRoleEnum.Student} autoRedirectToHomePage={false} />
            <Box>
                <Navbar source={NavbarSourceEnum.studentPage} position={isDesktop ? "static" : "sticky"} />
                <Hero subject={props.subject} />
                <TutorSearch {...props} />
                <SpecializationsBrowser subjectId={props.subject.subjectId!} titleKey={TRANSLATION_KEYS.SUBJECTS.SPECIALIZATIONS_TITLE} bgColor={COLORS.WHITE} />
                {/* <StudentTestemonials bgColor={COLORS.WHITE} /> */}
                <StudentQuestionAndAnswers bgColor={COLORS.BACKGROUND} />
                <StudentCtaSection bgColor={COLORS.WHITE} />
                <SubjectsBrowser titleKey={TRANSLATION_KEYS.SUBJECTS.OTHER_SUBJECTS_TITLE} bgColor={COLORS.BACKGROUND} excludeSubjectKey={props.subject.enumKey ?? ""} />
                <Footer bgColor={COLORS.WHITE} />
                <Copyright />
            </Box>
        </>
    )
}

export default withAppProvider<PageDataType>(SubjectLandingPage) 