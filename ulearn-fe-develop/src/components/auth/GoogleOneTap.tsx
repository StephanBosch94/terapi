import { snackbarConstants } from 'components/core/Snackbar';
import { useTranslation } from 'components/core/TranslationProvider';
import { useAppDataContext } from 'components/core/providers/AppProvider';
import { LoadingStatusEnum } from 'core/constants/enums/LoadingStatusEnum';
import { UserRoleEnum } from 'core/constants/enums/UserRoleEnum';
import { ENV_CONFIGURATION_KEYS } from 'core/constants/envConfigurationConstants';
import { FALLBACK_LANGUAGE_CODE, TRANSLATION_KEYS } from 'core/constants/translationConstants';
import { selectAuthStatus, selectIsAuthorized, setAuthUsingGoogleOneTapApi } from 'core/redux/authSlice';
import { useAppDispatch, useAppSelector } from 'core/redux/reduxHooks';
import { getTimeZoneName } from 'core/services/Utils/timeUtil';
import { useSnackbar } from 'notistack';
import { useEffect, useState } from 'react';
import GoogleOneTapLogin from 'react-google-one-tap-login';
import routes from 'routes/routes';

type GoogleAuthResponseType = {
    clientId: string,
    credential: string
}

const GoogleOneTap = ({ role, autoRedirectToHomePage }: { role: UserRoleEnum, autoRedirectToHomePage?: boolean }) => {
    const [isClient, setIsClient] = useState(false);
    const { envConfigurations, locale } = useAppDataContext();
    const isUserAuthorized = useAppSelector(selectIsAuthorized);
    const clientId = envConfigurations[ENV_CONFIGURATION_KEYS.AUTH.GOOGLE.CLIENT_ID] as string | undefined;
    const dispatch = useAppDispatch();
    const authStatus = useAppSelector(selectAuthStatus);
    const { enqueueSnackbar } = useSnackbar();
    const { t } = useTranslation();
    
    useEffect(() => {
        setIsClient(true);
    }, []);

    useEffect(() => {
        if (autoRedirectToHomePage && authStatus === LoadingStatusEnum.Loaded) {
            location.replace(routes.home.path);
        }
        if (authStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.AUTHORIZATION.GOOGLE_AUTHENTICATION_FAILED), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
    }, [authStatus, autoRedirectToHomePage]);

    const handleCallback = (e: GoogleAuthResponseType) => {
        dispatch(setAuthUsingGoogleOneTapApi({
            userRoleId: role,
            token: e.credential,
            languageCode: locale ?? FALLBACK_LANGUAGE_CODE,
            timeZone: getTimeZoneName()
        }))
    }

    return isClient && clientId && !isUserAuthorized
        ? <GoogleOneTapLogin
            googleAccountConfigs={{
                client_id: clientId,
                callback: handleCallback
            }} />
        : <></>
}

export default GoogleOneTap;