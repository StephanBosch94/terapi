import ExternalAuthRedirect, { IExternalProvederParameters } from "components/auth/ExternalAuthRedirect";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { ENV_CONFIGURATION_KEYS } from "core/constants/envConfigurationConstants";
import React from "react";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import { nameof } from "core/services/Types/AppTypes";
import { paths } from "apiTypes";

const SeznamStudentRedirectPage = () => {
    const pageParameters: IExternalProvederParameters = {
        beApiExternalEndpointUrl: nameof<paths>('/api/auth/login/seznam'),
        scopesKey: ENV_CONFIGURATION_KEYS.AUTH.SEZNAM.SCOPES,
        redirectUrlKey: ENV_CONFIGURATION_KEYS.AUTH.SEZNAM.STUDENT_REDIRECT_URL,
        userRole: UserRoleEnum.Student
    };
    return (
        <>
            <ExternalAuthRedirect parameters={pageParameters} />
        </>
    )
}

export default withAppClientSideProvider(SeznamStudentRedirectPage);