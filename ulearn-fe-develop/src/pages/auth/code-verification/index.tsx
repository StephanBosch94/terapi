import React, { useEffect } from "react";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import { useRouter } from "next/router";
import LinearProgressStyled from "components/styles/boxes/LinearProgressStyled";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { selectActivateEmailStatus, setActivateEmailByLink } from "core/redux/registrationSlice";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import routes from "routes/routes";
import { useTranslation } from "components/core/TranslationProvider";
import Box from "@mui/material/Box";
import H3StyledComponent from "components/styles/typography/H3Styled";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { selectShortUserInfo } from "core/redux/userSlice";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";

type PathParams = {
    code: string;
    registrationToken: string;
}

const ErrorView = () => {
    const { t } = useTranslation();

    return (
        <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", height: "100%" }}>
            <H3StyledComponent>{t(TRANSLATION_KEYS.AUTHORIZATION.ERRORS.INVALID_VERIFICATION_CODE)}</H3StyledComponent>
        </Box>
    )
}

const CodeVerification = () => {
    const router = useRouter();
    const { code, registrationToken } = router.query as PathParams;
    const dispath = useAppDispatch();
    const activateEmailStatus = useAppSelector(selectActivateEmailStatus);
    const shortUserInfo = useAppSelector(selectShortUserInfo);
    const isAlreadyVerified = shortUserInfo?.stateId !== undefined && shortUserInfo.stateId !== UserStateEnum.PasswordSetup;

    useEffect(() => {
        if (isAlreadyVerified){
            location.replace(routes.home.path);
        }
    }, [isAlreadyVerified]);
    
    useEffect(() => {
        if (!router.isReady) return;
        if (isAlreadyVerified) return;
        dispath(setActivateEmailByLink({
            registrationCode: code,
            token: registrationToken
        }));
    }, [shortUserInfo, router]);

    useEffect(() => {
        if (activateEmailStatus === LoadingStatusEnum.Loaded) {
            location.replace(routes.home.path);
        }
    }, [activateEmailStatus]);

    return (
        <>
            {activateEmailStatus === LoadingStatusEnum.Loading ? <LinearProgressStyled /> : <></>}
            {activateEmailStatus === LoadingStatusEnum.Error ? <ErrorView /> : <></>}
        </>
    )
}

export default withAppClientSideProvider(CodeVerification);