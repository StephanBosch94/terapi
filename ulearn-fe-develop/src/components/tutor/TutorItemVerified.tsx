import { useTranslation } from "components/core/TranslationProvider";
import { styled } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import VerifiedIcon from "assets/verified.svg";
import Body1Styled from "components/styles/typography/Body1Styled";
import Box from "@mui/material/Box";
import Tooltip from "components/styles/Tooltip";

const VerificationBoxStyled = styled(Box)({
    display: "flex",
    flexDirection: "row",
    alignItems: "center",
    gap: "8px",
    ":hover": {
        cursor: "pointer"
    },
    "svg": {
        color: COLORS.PRIMARY,
        display: "flex",
        alignItems: "center",
    }
}) as typeof Box;

const TutorItemVerified = () => {
    const { t } = useTranslation();
    return (
        <Tooltip text={t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.VERIFIED_TOOLTIP)}>
            <VerificationBoxStyled>
                <VerifiedIcon />
                <Body1Styled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.TUTOR_ITEM.VERIFIED)}</Body1Styled>
            </VerificationBoxStyled>
        </Tooltip>
    )
}

export default TutorItemVerified;