import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { useRouter } from "next/router";
import React, { useEffect, useState } from "react";
import LinearProgressStyled from "components/styles/boxes/LinearProgressStyled";

export interface IExternalProvederParameters {
    userRole: UserRoleEnum,
    scopesKey: string,
    redirectUrlKey: string,
    beApiExternalEndpointUrl: string
}

const ExternalAuthRedirect = ({ parameters }: { parameters: IExternalProvederParameters }) => {
    const router = useRouter();

    useEffect(() => {
        if (!router.isReady) return;
        const timer = setTimeout(() => {
            window.close();
        }, 700);

        return () => {
            clearTimeout(timer);
        }
    }, [router.query]);

    return <LinearProgressStyled />
}

export default ExternalAuthRedirect;