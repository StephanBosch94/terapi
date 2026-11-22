import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import CtaSection from "../CtaSection";

const CTA_SECTION = {
    headingKey: TRANSLATION_KEYS.STUDENT_CTA_ITEM.HEADING,
    descriptionKey: TRANSLATION_KEYS.STUDENT_CTA_ITEM.DESCRIPTION,
    buttonTextKey: TRANSLATION_KEYS.STUDENT_CTA_ITEM.START_LEARING,
}

const StudentCtaSection = ({bgColor}: {bgColor: string}) => {

    return (
        <CtaSection cta={CTA_SECTION} bgColor={bgColor} source="studentPage" />
    )
}

export default StudentCtaSection;