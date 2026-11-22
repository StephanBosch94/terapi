import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import React from "react";
import Navbar, { NavbarSourceEnum } from "components/navbar/Navbar";
import Benefits from "components/landingPage/Benefits";
import QuestionAndAnswers from "components/landingPage/QuestionAndAnswers";
import CtaSection from "components/landingPage/CtaSection";
import Hero from "components/landingPage/TutorHero";
import Footer from "components/landingPage/Footer";
import Copyright from "components/landingPage/Copyright";
import Process from "components/landingPage/Process";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { useTranslation } from "components/core/TranslationProvider";
import { DefaultPagePropsType, getDefaultServerSideProps } from "core/services/Utils/ssrUtil";
import { withAppProvider } from "components/core/providers/AppProvider";
import { GetServerSideProps } from "next";
import Box from "@mui/material/Box";
import SubjectsBrowser from "components/landingPage/SubjectsBrowser";
import GoogleOneTap from "components/auth/GoogleOneTap";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";

export const getServerSideProps: GetServerSideProps<DefaultPagePropsType> = async (props) => getDefaultServerSideProps(props);

const BENEFITS =
    [
        {
            nameKey: TRANSLATION_KEYS.TUTOR_BENEFITS.SECOND_BENEFIT,
            descriptionKey: TRANSLATION_KEYS.TUTOR_BENEFITS.SECOND_BENEFIT_DESCRIPTION,
            imgPath: "/images/benefits/teach_everywhere.png",
            altText: TRANSLATION_KEYS.TUTOR_BENEFITS.SECOND_BENEFIT
        },
        {
            nameKey: TRANSLATION_KEYS.TUTOR_BENEFITS.THIRD_BENEFIT,
            descriptionKey: TRANSLATION_KEYS.TUTOR_BENEFITS.THIRD_BENEFIT_DESCRIPTION,
            imgPath: "/images/benefits/grow_professionally.png",
            altText: TRANSLATION_KEYS.TUTOR_BENEFITS.THIRD_BENEFIT
        },
        {
            nameKey: TRANSLATION_KEYS.TUTOR_BENEFITS.FORTH_BENEFIT,
            descriptionKey: TRANSLATION_KEYS.TUTOR_BENEFITS.FIRST_BENEFIT_DESCRIPTION,
            imgPath: "/images/benefits/connect_students.png",
            altText: TRANSLATION_KEYS.TUTOR_BENEFITS.FIRST_BENEFIT
        },
        {
            nameKey: TRANSLATION_KEYS.TUTOR_BENEFITS.FIRST_BENEFIT,
            descriptionKey: TRANSLATION_KEYS.TUTOR_BENEFITS.FIRST_BENEFIT_DESCRIPTION,
            imgPath: "/images/benefits/own_rate.png",
            altText: TRANSLATION_KEYS.TUTOR_BENEFITS.FIRST_BENEFIT
        }
    ]

const BENEFITS_HEADING = {
    titleKey: TRANSLATION_KEYS.TUTOR_BENEFITS.SECTION_TITLE,
    descriptionKey: TRANSLATION_KEYS.TUTOR_BENEFITS.SECTION_DESCRIPTION
}

const QUESTIONS_AND_ANSWERS = [
    {
        nameKey: TRANSLATION_KEYS.TUTOR_QUESTION_AND_ANSWERS.FIRST_QUESTION,
        descriptionKey: TRANSLATION_KEYS.TUTOR_QUESTION_AND_ANSWERS.FIRST_ANSWER
    },
    {
        nameKey: TRANSLATION_KEYS.TUTOR_QUESTION_AND_ANSWERS.SECOND_QUESTION,
        descriptionKey: TRANSLATION_KEYS.TUTOR_QUESTION_AND_ANSWERS.SECOND_ANSWER
    },
    {
        nameKey: TRANSLATION_KEYS.TUTOR_QUESTION_AND_ANSWERS.THIRD_QUESTION,
        descriptionKey: TRANSLATION_KEYS.TUTOR_QUESTION_AND_ANSWERS.THIRD_ANSWER
    },
    {
        nameKey: TRANSLATION_KEYS.TUTOR_QUESTION_AND_ANSWERS.FORTH_QUESTION,
        descriptionKey: TRANSLATION_KEYS.TUTOR_QUESTION_AND_ANSWERS.FORTH_ANSWER
    },
    {
        nameKey: TRANSLATION_KEYS.TUTOR_QUESTION_AND_ANSWERS.FIFTH_QUESTION,
        descriptionKey: TRANSLATION_KEYS.TUTOR_QUESTION_AND_ANSWERS.FIFTH_ANSWER
    },
    {
        nameKey: TRANSLATION_KEYS.TUTOR_QUESTION_AND_ANSWERS.SIXTH_QUESTION,
        descriptionKey: TRANSLATION_KEYS.TUTOR_QUESTION_AND_ANSWERS.SIXTH_ANSWER
    },
    {
        nameKey: TRANSLATION_KEYS.TUTOR_QUESTION_AND_ANSWERS.SEVENTH_QUESTION,
        descriptionKey: TRANSLATION_KEYS.TUTOR_QUESTION_AND_ANSWERS.SEVENTH_ANSWER
    }
]

const CTA_SECTION = {
    headingKey: TRANSLATION_KEYS.TUTOR_CTA_ITEM.SECTION_TITLE,
    descriptionKey: TRANSLATION_KEYS.TUTOR_CTA_ITEM.DESCRIPTION,
    buttonTextKey: TRANSLATION_KEYS.TUTOR_CTA_ITEM.START_TEACHING,
}

const TESTEMONIALS = {
    descriptionKey: TRANSLATION_KEYS.TUTOR_TESTEMONIALS.DESCRIPTION,
    personNameKey: TRANSLATION_KEYS.TUTOR_TESTEMONIALS.TUTOR_NAME,
    personDescriptionKey: TRANSLATION_KEYS.TUTOR_TESTEMONIALS.TUTOR_DESCRIPTION,
    imagePath: "/images/tutor_dashboard_testemonials.png",
    imageBlurredPath: "/images/tutor_dashboard_testemonials_blurred.jpg"
}

const PROCESSES = [
    {
        nameKey: TRANSLATION_KEYS.TUTOR_PROCESS_ITEM.PROCESS_ONE_HEADING,
        descriptionKey: TRANSLATION_KEYS.TUTOR_PROCESS_ITEM.PROCESS_ONE_DESCRIPTION
    },
    {
        nameKey: TRANSLATION_KEYS.TUTOR_PROCESS_ITEM.PROCESS_TWO_HEADING,
        descriptionKey: TRANSLATION_KEYS.TUTOR_PROCESS_ITEM.PROCESS_TWO_DESCRIPTION
    },
    {
        nameKey: TRANSLATION_KEYS.TUTOR_PROCESS_ITEM.PROCESS_THREE_HEADING,
        descriptionKey: TRANSLATION_KEYS.TUTOR_PROCESS_ITEM.PROCESS_THREE_DESCRIPTION
    },
    {
        nameKey: TRANSLATION_KEYS.TUTOR_PROCESS_ITEM.PROCESS_FOUR_HEADING,
        descriptionKey: TRANSLATION_KEYS.TUTOR_PROCESS_ITEM.PROCESS_FOUR_DESCRIPTION
    }
]

const PROCESSES_HEADING =
{
    titleKey: TRANSLATION_KEYS.TUTOR_PROCESS_ITEM.SECTION_TITLE,
    descriptionKey: TRANSLATION_KEYS.TUTOR_PROCESS_ITEM.SECTION_DESCRIPTION
}

const TutorLandingPage = () => {
    const { t } = useTranslation();

    return (
        <>
            <GoogleOneTap role={UserRoleEnum.Tutor} autoRedirectToHomePage={false} />
            <Box>
                <Navbar source={NavbarSourceEnum.tutorPage} />
                <Hero />
                <SubjectsBrowser bgColor={COLORS.BACKGROUND} titleKey={TRANSLATION_KEYS.TUTOR_SUBJECTS.SECTION_TITLE} linkType="registration" />
                <Benefits benefits={BENEFITS} bgColor={COLORS.WHITE} heading={BENEFITS_HEADING} />
                <Process processes={PROCESSES} headings={PROCESSES_HEADING} bgColor={COLORS.BACKGROUND} />
                {/* <Testemonials testemonials={TESTEMONIALS} bgColor={COLORS.BACKGROUND} /> */}
                <CtaSection cta={CTA_SECTION} bgColor={COLORS.WHITE} source="tutorPage" />
                <QuestionAndAnswers questionsAndAnswers={QUESTIONS_AND_ANSWERS} bgColor={COLORS.BACKGROUND} />
                <Footer bgColor={COLORS.WHITE} />
                <Copyright />
            </Box>
        </>
    );
};

export default withAppProvider(TutorLandingPage);