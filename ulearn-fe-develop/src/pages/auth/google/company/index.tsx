import ExternalAuthRedirect from "components/auth/ExternalAuthRedirect";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { ENV_CONFIGURATION_KEYS } from "core/constants/envConfigurationConstants";
import React from "react";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import { nameof } from "core/services/Types/AppTypes";
import { paths } from "apiTypes";

const GoogleCompanyPage = () => (
    <ExternalAuthRedirect parameters={{
        beApiExternalEndpointUrl: nameof<paths>('/api/auth/login/google'),
        scopesKey: ENV_CONFIGURATION_KEYS.AUTH.GOOGLE.COMPANY.SCOPES,
        redirectUrlKey: ENV_CONFIGURATION_KEYS.AUTH.GOOGLE.COMPANY.REDIRECT_URL,
        userRole: UserRoleEnum.Company
    }} />
)

export default withAppClientSideProvider(GoogleCompanyPage);