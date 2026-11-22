import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import React from "react";
import Navbar, { NavbarSourceEnum } from "components/navbar/Navbar";
import Benefits from "components/landingPage/Benefits";
import QuestionAndAnswers from "components/landingPage/QuestionAndAnswers";
import CtaSection from "components/landingPage/CtaSection";
import Footer from "components/landingPage/Footer";
import Copyright from "components/landingPage/Copyright";
import Testemonials from "components/landingPage/Testimonials";
import Process from "components/landingPage/Process";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { getDefaultStaticProps } from "core/services/Utils/ssrUtil";
import { withAppProvider } from "components/core/providers/AppProvider";
import { GetStaticPropsContext } from "next";
import Box from "@mui/material/Box";
import CompanyHero from "components/landingPage/CompanyHero";
import GoogleOneTap from "components/auth/GoogleOneTap";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";

export const getStaticProps = async (props: GetStaticPropsContext) => getDefaultStaticProps(props);

const BENEFITS =
    [
        {
            nameKey: TRANSLATION_KEYS.COMPANY_BENEFITS.FIRST_BENEFIT,
            descriptionKey: TRANSLATION_KEYS.COMPANY_BENEFITS.FIRST_BENEFIT_DESCRIPTION,
            imgPath: "/images/benefits/improved_communication.png",
            altText: TRANSLATION_KEYS.COMPANY_BENEFITS.FIRST_BENEFIT
        },
        {
            nameKey: TRANSLATION_KEYS.COMPANY_BENEFITS.SECOND_BENEFIT,
            descriptionKey: TRANSLATION_KEYS.COMPANY_BENEFITS.SECOND_BENEFIT_DESCRIPTION,
            imgPath: "/images/benefits/global_reach.png",
            altText: TRANSLATION_KEYS.COMPANY_BENEFITS.SECOND_BENEFIT
        },
        {
            nameKey: TRANSLATION_KEYS.COMPANY_BENEFITS.THIRD_BENEFIT,
            descriptionKey: TRANSLATION_KEYS.COMPANY_BENEFITS.THIRD_BENEFIT_DESCRIPTION,
            imgPath: "/images/benefits/professional_development.png",
            altText: TRANSLATION_KEYS.COMPANY_BENEFITS.THIRD_BENEFIT
        },
        {
            nameKey: TRANSLATION_KEYS.COMPANY_BENEFITS.FORTH_BENEFIT,
            descriptionKey: TRANSLATION_KEYS.COMPANY_BENEFITS.FORTH_BENEFIT_DESCRIPTION,
            imgPath: "/images/benefits/boost_atmosphere.png",
            altText: TRANSLATION_KEYS.COMPANY_BENEFITS.FORTH_BENEFIT
        }
    ]

const BENEFITS_HEADING = {
    titleKey: TRANSLATION_KEYS.COMPANY_BENEFITS.SECTION_TITLE,
    descriptionKey: TRANSLATION_KEYS.COMPANY_BENEFITS.SECTION_DESCRIPTION
}

const QUESTIONS_AND_ANSWERS = [
    {
        nameKey: TRANSLATION_KEYS.COMPANY_QUESTION_AND_ANSWERS.FIRST_QUESTION,
        descriptionKey: TRANSLATION_KEYS.COMPANY_QUESTION_AND_ANSWERS.FIRST_ANSWER
    },
    {
        nameKey: TRANSLATION_KEYS.COMPANY_QUESTION_AND_ANSWERS.SECOND_QUESTION,
        descriptionKey: TRANSLATION_KEYS.COMPANY_QUESTION_AND_ANSWERS.SECOND_ANSWER
    },
    {
        nameKey: TRANSLATION_KEYS.COMPANY_QUESTION_AND_ANSWERS.THIRD_QUESTION,
        descriptionKey: TRANSLATION_KEYS.COMPANY_QUESTION_AND_ANSWERS.THIRD_ANSWER
    },
    {
        nameKey: TRANSLATION_KEYS.COMPANY_QUESTION_AND_ANSWERS.FORTH_QUESTION,
        descriptionKey: TRANSLATION_KEYS.COMPANY_QUESTION_AND_ANSWERS.FORTH_ANSWER
    },
    {
        nameKey: TRANSLATION_KEYS.COMPANY_QUESTION_AND_ANSWERS.FIFTH_QUESTION,
        descriptionKey: TRANSLATION_KEYS.COMPANY_QUESTION_AND_ANSWERS.FIFTH_ANSWER
    }

]

const CTA_SECTION = {
    headingKey: TRANSLATION_KEYS.COMPANY_CTA_ITEM.SECTION_TITLE,
    descriptionKey: TRANSLATION_KEYS.COMPANY_CTA_ITEM.DESCRIPTION,
    buttonTextKey: TRANSLATION_KEYS.COMPANY_CTA_ITEM.BUTTON_TEXT,
}

const PROCESSES = [
    {
        nameKey: TRANSLATION_KEYS.COMPANY_PROCESS_ITEM.PROCESS_ONE_HEADING,
        descriptionKey: TRANSLATION_KEYS.COMPANY_PROCESS_ITEM.PROCESS_ONE_DESCRIPTION
    },
    {
        nameKey: TRANSLATION_KEYS.COMPANY_PROCESS_ITEM.PROCESS_FIVE_HEADING,
        descriptionKey: TRANSLATION_KEYS.COMPANY_PROCESS_ITEM.PROCESS_FIVE_DESCRIPTION
    },
    {
        nameKey: TRANSLATION_KEYS.COMPANY_PROCESS_ITEM.PROCESS_TWO_HEADING,
        descriptionKey: TRANSLATION_KEYS.COMPANY_PROCESS_ITEM.PROCESS_TWO_DESCRIPTION
    },
    {
        nameKey: TRANSLATION_KEYS.COMPANY_PROCESS_ITEM.PROCESS_THREE_HEADING,
        descriptionKey: TRANSLATION_KEYS.COMPANY_PROCESS_ITEM.PROCESS_THREE_DESCRIPTION
    },
    {
        nameKey: TRANSLATION_KEYS.COMPANY_PROCESS_ITEM.PROCESS_FOUR_HEADING,
        descriptionKey: TRANSLATION_KEYS.COMPANY_PROCESS_ITEM.PROCESS_FOUR_DESCRIPTION
    }
]

const PROCESSES_HEADING =
{
    titleKey: TRANSLATION_KEYS.COMPANY_PROCESS_ITEM.SECTION_TITLE,
    descriptionKey: TRANSLATION_KEYS.COMPANY_PROCESS_ITEM.SECTION_DESCRIPTION
}

const CompanyLandingPage = () => {

    return (
        <>
            <GoogleOneTap role={UserRoleEnum.Company} autoRedirectToHomePage={false} />
            <Box>
                <Navbar source={NavbarSourceEnum.companyPage} />
                <CompanyHero />
                <Benefits benefits={BENEFITS} bgColor={COLORS.BACKGROUND} heading={BENEFITS_HEADING} />
                <Process processes={PROCESSES} headings={PROCESSES_HEADING} bgColor={COLORS.WHITE} />
                <CtaSection cta={CTA_SECTION} bgColor={COLORS.BACKGROUND} source="companyPage" />
                <QuestionAndAnswers questionsAndAnswers={QUESTIONS_AND_ANSWERS} bgColor={COLORS.WHITE} />
                <Footer bgColor={COLORS.BACKGROUND} />
                <Copyright />
            </Box>
        </>
    );
};

export default withAppProvider(CompanyLandingPage);