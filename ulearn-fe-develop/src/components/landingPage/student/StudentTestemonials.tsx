import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import Testemonials from "../Testimonials";

const TESTEMONIALS = {
    descriptionKey: TRANSLATION_KEYS.STUDENT_TESTEMONIALS.DESCRIPTION,
    personNameKey: TRANSLATION_KEYS.STUDENT_TESTEMONIALS.STUDENT_NAME,
    personDescriptionKey: TRANSLATION_KEYS.STUDENT_TESTEMONIALS.STUDENT_DESCRIPTION,
    imagePath: "/images/testemonials.png",
    imageBlurredPath: "/images/testemonials_blurred.jpg"
}


const StudentTestemonials = ({ bgColor }: { bgColor: string }) => {

    return (
        <Testemonials testemonials={TESTEMONIALS} bgColor={bgColor} />
    )
}

export default StudentTestemonials;