import { paths } from "apiTypes";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { ENV_CONFIGURATION_KEYS } from "core/constants/envConfigurationConstants";
import { setLogOut } from "core/redux/authSlice";
import { AppDispatch } from "core/redux/store";
import { removeShortUserInfo } from "core/redux/userSlice";
import routes from "routes/routes";
import { getStorage } from "../Core/StorageService";
import { cacheConstants } from "core/constants/cacheConstants";
import { isDispatchFulfilled, isDispatchRejected } from "./apiUtil";

export const getGoogleConfigKeys = (userRole: UserRoleEnum) => {
    switch (userRole) {
        case UserRoleEnum.Tutor:
            return {
                scopes: ENV_CONFIGURATION_KEYS.AUTH.GOOGLE.TUTOR.SCOPES,
                redirectUrl: ENV_CONFIGURATION_KEYS.AUTH.GOOGLE.TUTOR.REDIRECT_URL
            }
        case UserRoleEnum.Company:
            return {
                scopes: ENV_CONFIGURATION_KEYS.AUTH.GOOGLE.COMPANY.SCOPES,
                redirectUrl: ENV_CONFIGURATION_KEYS.AUTH.GOOGLE.COMPANY.REDIRECT_URL
            }
        default:
            return {
                scopes: ENV_CONFIGURATION_KEYS.AUTH.GOOGLE.STUDENT.SCOPES,
                redirectUrl: ENV_CONFIGURATION_KEYS.AUTH.GOOGLE.STUDENT.REDIRECT_URL
            }
    }
}

export const getFacebookConfigKeys = (userRole: UserRoleEnum) => {
    switch (userRole) {
        case UserRoleEnum.Tutor:
            return {
                scopes: ENV_CONFIGURATION_KEYS.AUTH.FACEBOOK.SCOPES,
                redirectUrl: ENV_CONFIGURATION_KEYS.AUTH.FACEBOOK.TUTOR_REDIRECT_URL
            }
        case UserRoleEnum.Company:
            return {
                scopes: ENV_CONFIGURATION_KEYS.AUTH.FACEBOOK.SCOPES,
                redirectUrl: ENV_CONFIGURATION_KEYS.AUTH.FACEBOOK.COMPANY_REDIRECT_URL
            }
        default:
            return {
                scopes: ENV_CONFIGURATION_KEYS.AUTH.FACEBOOK.SCOPES,
                redirectUrl: ENV_CONFIGURATION_KEYS.AUTH.FACEBOOK.STUDENT_REDIRECT_URL
            }
    }
}

export const getSeznamConfigKeys = (userRole: UserRoleEnum) => {
    switch (userRole) {
        case UserRoleEnum.Tutor:
            return {
                scopes: ENV_CONFIGURATION_KEYS.AUTH.SEZNAM.SCOPES,
                redirectUrl: ENV_CONFIGURATION_KEYS.AUTH.SEZNAM.TUTOR_REDIRECT_URL
            }
        case UserRoleEnum.Company:
            return {
                scopes: ENV_CONFIGURATION_KEYS.AUTH.SEZNAM.SCOPES,
                redirectUrl: ENV_CONFIGURATION_KEYS.AUTH.SEZNAM.COMPANY_REDIRECT_URL
            }
        default:
            return {
                scopes: ENV_CONFIGURATION_KEYS.AUTH.SEZNAM.SCOPES,
                redirectUrl: ENV_CONFIGURATION_KEYS.AUTH.SEZNAM.STUDENT_REDIRECT_URL
            }
    }
}


export const getExternalOuthProviderApiLink = (
    envConfigurations: paths['/api/env-configuration']['get']['responses']['200']['content']['application/json'] | undefined,
    apiKey: string,
    clientIdKey: string,
    scopesKey: string,
    scopesSeparator: string,
    redirectUrlKey: string
): string => {
    if (envConfigurations === undefined) {
        return '';
    }
    const url = new URL(envConfigurations[apiKey] as string)
    url.searchParams.append('scope', (envConfigurations[scopesKey] as string[]).join(scopesSeparator));
    url.searchParams.append('client_id', envConfigurations[clientIdKey] as string);
    url.searchParams.append('redirect_uri', envConfigurations[redirectUrlKey] as string);
    url.searchParams.append('response_type', 'code');
    url.searchParams.append('access_type', 'offline');
    
    return url.toString();
}

export const getGoogleApiLink = (
    envConfigurations: paths['/api/env-configuration']['get']['responses']['200']['content']['application/json'] | undefined,
    userRole: UserRoleEnum
): string => {
    const config = getGoogleConfigKeys(userRole);
    return getExternalOuthProviderApiLink(
        envConfigurations,
        ENV_CONFIGURATION_KEYS.AUTH.GOOGLE.API_URL,
        ENV_CONFIGURATION_KEYS.AUTH.GOOGLE.CLIENT_ID,
        config.scopes,
        ' ',
        config.redirectUrl
    );
}

export const getFacebookUrl = (
    envConfigurations: paths['/api/env-configuration']['get']['responses']['200']['content']['application/json'] | undefined,
    userRole: UserRoleEnum
): string => {
    const config = getFacebookConfigKeys(userRole);
    return getExternalOuthProviderApiLink(
        envConfigurations,
        ENV_CONFIGURATION_KEYS.AUTH.FACEBOOK.API_URL,
        ENV_CONFIGURATION_KEYS.AUTH.FACEBOOK.CLIENT_ID,
        config.scopes,
        ',',
        config.redirectUrl
    )
}

export const getSeznamUrl = (
    envConfigurations: paths['/api/env-configuration']['get']['responses']['200']['content']['application/json'] | undefined,
    userRole: UserRoleEnum
): string => {
    const config = getSeznamConfigKeys(userRole);
    return getExternalOuthProviderApiLink(
        envConfigurations,
        ENV_CONFIGURATION_KEYS.AUTH.SEZNAM.API_URL,
        ENV_CONFIGURATION_KEYS.AUTH.SEZNAM.CLIENT_ID,
        config.scopes,
        ',',
        config.redirectUrl
    )
}

export const GetMissingScopes = (given: string[], required: string[]): string[] => {
    return required.filter(x => !required.some(y => x === y))
}

const clearnStorage = async () => {
    const storage = await getStorage();
    storage.removeCommonValue(cacheConstants.studentRegistrationLatestStep);
    storage.removeCommonValue(cacheConstants.tutorRegistrationLatestStep);
}

export const logOut = async (dispatch: AppDispatch) => {
    const logOutResult = await dispatch(setLogOut());
    const removeShortUserInfoResult = dispatch(removeShortUserInfo());
    const isLogOutFailed = isDispatchRejected(logOutResult);
    const isRemoveInfoFailed = isDispatchRejected(removeShortUserInfoResult);
    if (isLogOutFailed){
        console.error("Log Out failed");
    }
    if (isRemoveInfoFailed){
        console.error("removeShortUserInfoResult failed");
    }
    const fail = isLogOutFailed || isRemoveInfoFailed;
    await clearnStorage();
    if (!fail){
        location.replace(routes.home.path);
    }
}