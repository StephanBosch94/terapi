import ExternalAuthRedirect, { IExternalProvederParameters } from "components/auth/ExternalAuthRedirect";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { ENV_CONFIGURATION_KEYS } from "core/constants/envConfigurationConstants";
import React from "react";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import { nameof } from "core/services/Types/AppTypes";
import { paths } from "apiTypes";

const FacebookCompanyRedirectPage = () => {
    const pageParameters: IExternalProvederParameters = {
        beApiExternalEndpointUrl: nameof<paths>('/api/auth/login/facebook'),
        scopesKey: ENV_CONFIGURATION_KEYS.AUTH.FACEBOOK.SCOPES,
        redirectUrlKey: ENV_CONFIGURATION_KEYS.AUTH.FACEBOOK.COMPANY_REDIRECT_URL,
        userRole: UserRoleEnum.Company
    };
    return (
        <>
            <ExternalAuthRedirect parameters={pageParameters} />
        </>
    )
}

export default withAppClientSideProvider(FacebookCompanyRedirectPage);