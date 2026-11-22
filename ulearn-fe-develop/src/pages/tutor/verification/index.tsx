import { useTranslation } from "components/core/TranslationProvider";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import Body1Styled from "components/styles/typography/Body1Styled";
import H2Styled from "components/styles/typography/H2Styled";
import TimeIcon from "assets/emoji/time.svg";
import NavbarWithoutLinks from "components/navbar/NavbarWithoutLinks";
import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import { selectUserStateId } from "core/redux/authSlice";
import { useAppSelector } from "core/redux/reduxHooks";
import { useRouter } from "next/router";
import { useEffect } from "react";
import routes from "routes/routes";
import { selectShortUserInfo } from "core/redux/userSlice";

const IconBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    height: "96px",
    justifyContent: "center",
    svg: {
        transform: "scale(6)"
    }
})) as typeof Box;

const CenterBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flex: "1 1 auto",
    alignItems: "center",
    justifyContent: "center",
    flexFlow: "column noWrap",
    gap: "16px",
    textAlign: "center"
})) as typeof Box;

const VerificationPage = () => {
    const { t } = useTranslation();
    const shortUserInfo = useAppSelector(selectShortUserInfo);
    
    useEffect(() => {
        if (shortUserInfo?.stateId && shortUserInfo.stateId !== UserStateEnum.ProfileValidation) {
            location.replace(routes.home.path);
        }
    }, [shortUserInfo]);

    return (
        <Box sx={{ display: "flex", flexFlow: "column noWrap", overflow: "auto", height: "100%" }}>
            <Box sx={{ width: "100%", display: "flex", flexFlow: "row noWrap", flex: "0 1 auto" }}>
                <NavbarWithoutLinks />
            </Box>
            <CenterBoxStyled>
                <H2Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.FINAL_STEP)}</H2Styled>
                <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.FINAL_STEP_DESCRIPTION)}</Body1Styled>
                <IconBoxStyled><TimeIcon /></IconBoxStyled>
            </CenterBoxStyled>
        </Box>
    )
}

export default withAppClientSideProvider(VerificationPage);