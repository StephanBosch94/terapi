import Box from "@mui/material/Box";
import { styled } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import H4Styled from "components/styles/typography/H4Styled";
import { useAppSelector } from "core/redux/reduxHooks";
import { selectShortUserInfo } from "core/redux/userSlice";
import WaveIcon from "assets/emoji/wave.svg";
import { getFirstNameOrCompanyName } from "core/services/Utils/userUtils";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";

const WelcomeBackBoxStyled = styled(Box)({
    display: "flex",
    alignItems: "center",
    gap: "8px",
}) as typeof Box;

const EmojiIconStyled = styled(Box)({
    display: "flex",
    alignItems: "center",
    svg: {
        transform: "scale(1.25)"
    }
}) as typeof Box;

const WelcomeBackUser = () => {
    const { t } = useTranslation();
    const shortUserInfo = useAppSelector(selectShortUserInfo);

    return (
        <WelcomeBackBoxStyled>
            <Box sx={{ display: "flex", flexDirection: "row", gap: "4px" }}>
                <H4Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.WELCOME_BACK, { userName: getFirstNameOrCompanyName(shortUserInfo) ?? COMMON_CONSTANTS.DEFAULT_USER_NAME })}</H4Styled>
            </Box>
            <EmojiIconStyled><WaveIcon /></EmojiIconStyled>
        </WelcomeBackBoxStyled>
    )
}

export default WelcomeBackUser;