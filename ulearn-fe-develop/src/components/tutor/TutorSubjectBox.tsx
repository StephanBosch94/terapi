import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { AppUserOverviewVM } from "core/services/Types/AppTypes";
import { useTranslation } from "components/core/TranslationProvider";
import TeachIcon from "assets/graduation-cap.svg";
import { BadgeTextBoxStyled } from "components/styles/boxes/BadgeTextBoxStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import Body1Styled from "components/styles/typography/Body1Styled";
import { FloatableBoxStyled } from "components/styles/boxes/FloatableBoxStyled";
import { TutorItemRowBoxStyled } from "./styles/TutorItemRowBoxStyled";

const TutorSubjectBox = ({ user }: { user: (AppUserOverviewVM) }) => {
    const { t } = useTranslation();

    return (
        <TutorItemRowBoxStyled>
            <FloatableBoxStyled>
                <TeachIcon />
                <Body1Styled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.TUTOR_ITEM.TEACHES)}</Body1Styled>
            </FloatableBoxStyled>
            <BadgeTextBoxStyled>
                <Body1Styled sx={{ color: COLORS.BLACK }}>{t(user.subject?.nameKey ?? "")}</Body1Styled>
            </BadgeTextBoxStyled>
        </TutorItemRowBoxStyled>
    )
}

export default TutorSubjectBox;