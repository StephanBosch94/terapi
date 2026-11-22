import Navbar, { NavbarSourceEnum } from "../components/navbar/Navbar";
import React, { useEffect, useState } from "react";
import Process from "components/landingPage/Process";
import Benefits from "components/landingPage/Benefits";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import Footer from "components/landingPage/Footer";
import SubjectsBrowser from "components/landingPage/SubjectsBrowser";
import Copyright from "components/landingPage/Copyright";
import PopularTutors from "components/favouriteTutors/PopularTutors";
import StudentHero from "components/landingPage/student/StudentHero";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import Box from "@mui/material/Box";
import { DefaultPagePropsType, getDefaultServerSideProps } from "core/services/Utils/ssrUtil";
import { withAppProvider } from "components/core/providers/AppProvider";
import { GetServerSideProps } from "next";
import Business from "components/landingPage/BusinessBenefits";
import GoogleOneTap from "components/auth/GoogleOneTap";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import StudentQuestionAndAnswers from "components/landingPage/student/StudentQuestionAndAnswers";
import StudentCtaSection from "components/landingPage/student/StudentCtaSection";

export const getServerSideProps: GetServerSideProps<DefaultPagePropsType> = async (props) => getDefaultServerSideProps(props);

const BENEFITS = [
    {
        nameKey: TRANSLATION_KEYS.USER_BENEFITS.FORTH_BENEFIT,
        descriptionKey: TRANSLATION_KEYS.USER_BENEFITS.FORTH_BENEFIT_DESCRIPTION,
        imgPath: "/images/benefits/personalized_lessons.png",
        altText: TRANSLATION_KEYS.USER_BENEFITS.FORTH_BENEFIT
    },
    {
        nameKey: TRANSLATION_KEYS.USER_BENEFITS.THIRD_BENEFIT,
        descriptionKey: TRANSLATION_KEYS.USER_BENEFITS.THIRD_BENEFIT_DESCRIPTION,
        imgPath: "/images/benefits/professional_tutors.png",
        altText: TRANSLATION_KEYS.USER_BENEFITS.THIRD_BENEFIT
    },
    {
        nameKey: TRANSLATION_KEYS.USER_BENEFITS.FIRST_BENEFIT,
        descriptionKey: TRANSLATION_KEYS.USER_BENEFITS.FIRST_BENEFIT_DESCRIPTION,
        imgPath: "/images/benefits/matching_tutors.png",
        altText: TRANSLATION_KEYS.USER_BENEFITS.FIRST_BENEFIT
    },
    {
        nameKey: TRANSLATION_KEYS.USER_BENEFITS.SECOND_BENEFIT,
        descriptionKey: TRANSLATION_KEYS.USER_BENEFITS.SECOND_BENEFIT_DESCRIPTION,
        imgPath: "/images/benefits/faster_progress.png",
        altText: TRANSLATION_KEYS.USER_BENEFITS.SECOND_BENEFIT
    }
]

const BENEFITS_HEADING = {
    titleKey: TRANSLATION_KEYS.USER_BENEFITS.SECTION_TITLE,
    descriptionKey: TRANSLATION_KEYS.USER_BENEFITS.SECTION_DESCRIPTION
}

const PROCESSES = [
    {
        nameKey: TRANSLATION_KEYS.STUDENT_PROCESS_ITEM.PROCESS_ONE_HEADING,
        descriptionKey: TRANSLATION_KEYS.STUDENT_PROCESS_ITEM.PROCESS_ONE_DESCRIPTION
    },
    {
        nameKey: TRANSLATION_KEYS.STUDENT_PROCESS_ITEM.PROCESS_TWO_HEADING,
        descriptionKey: TRANSLATION_KEYS.STUDENT_PROCESS_ITEM.PROCESS_TWO_DESCRIPTION
    },
    {
        nameKey: TRANSLATION_KEYS.STUDENT_PROCESS_ITEM.PROCESS_THREE_HEADING,
        descriptionKey: TRANSLATION_KEYS.STUDENT_PROCESS_ITEM.PROCESS_THREE_DESCRIPTION
    },
    {
        nameKey: TRANSLATION_KEYS.STUDENT_PROCESS_ITEM.PROCESS_FOUR_HEADING,
        descriptionKey: TRANSLATION_KEYS.STUDENT_PROCESS_ITEM.PROCESS_FOUR_DESCRIPTION
    }
]

const PROCESSES_HEADING =
{
    titleKey: TRANSLATION_KEYS.STUDENT_PROCESS_ITEM.SECTION_TITLE,
    descriptionKey: TRANSLATION_KEYS.STUDENT_PROCESS_ITEM.SECTION_DESCRIPTION
}


const Home = () => {
    return (
        <>
            <GoogleOneTap role={UserRoleEnum.Student} autoRedirectToHomePage={false} />
            <Box>
                <Navbar source={NavbarSourceEnum.studentPage} />
                <StudentHero />
                <SubjectsBrowser bgColor={COLORS.BACKGROUND} />
                <PopularTutors bgColor={COLORS.WHITE} />
                <Benefits benefits={BENEFITS} bgColor={COLORS.BACKGROUND} heading={BENEFITS_HEADING} />
                <Process processes={PROCESSES} headings={PROCESSES_HEADING} bgColor={COLORS.WHITE} />
                {/* <StudentTestemonials bgColor={COLORS.BACKGROUND} /> */}
                <Business />
                <StudentQuestionAndAnswers bgColor={COLORS.WHITE} />
                <StudentCtaSection bgColor={COLORS.BACKGROUND} />
                <Footer bgColor={COLORS.WHITE} />
                <Copyright />
            </Box>
        </>
    );
};

export default withAppProvider(Home);
