import ChatCircleText from "assets/chat-circle-text.svg";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import Badge from "components/styles/Badge";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import Body1Styled from "components/styles/typography/Body1Styled";
import { getLanguageLevelColor } from "core/constants/enums/LanguageLevelEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { AppUserOverviewVM } from "core/services/Types/AppTypes";
import { useTranslation } from "components/core/TranslationProvider";
import { FloatableBoxStyled } from "components/styles/boxes/FloatableBoxStyled";
import Box from "@mui/material/Box";
import { TutorItemRowBoxStyled } from "./styles/TutorItemRowBoxStyled";

const LanguageBoxStyled = styled(Box)({
    display: "inline-flex",
    alignItems: "center",
    gap: "4px",
}) as typeof Box;

const TutorLanguagesList = ({ user }: { user: (AppUserOverviewVM) }) => {
    const { t } = useTranslation();

    return (
        <>
            {user.languages?.map((lang, idx) =>
                <LanguageBoxStyled key={lang.language?.languageId!}>
                    <Body1Styled sx={{ color: COLORS.BLACK }}>{t(lang?.language?.nameKey ?? "")}</Body1Styled>
                    <Badge bgColor={getLanguageLevelColor(lang?.level?.languageLevelId ?? 0)}>
                        <Body1Styled sx={{ color: COLORS.BLACK }}>{t(lang?.level?.nameKey ?? "")}</Body1Styled>
                    </Badge>
                </LanguageBoxStyled>
            )}
        </>
    )
}

const TutorLanguagesBox = ({ user }: { user: (AppUserOverviewVM) }) => {
    const { t } = useTranslation();

    return (
        <TutorItemRowBoxStyled>
            <FloatableBoxStyled>
                <ChatCircleText />
                <Body1Styled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.TUTOR_ITEM.SPEAKS)}</Body1Styled>
            </FloatableBoxStyled>
            <TutorLanguagesList user={user} />
        </TutorItemRowBoxStyled>
    )
}

export default TutorLanguagesBox;