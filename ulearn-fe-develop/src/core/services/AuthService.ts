import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { paths } from "../../apiTypes";

export type ExternalProviderRequestType = paths['/api/auth/login/google']['post']['requestBody']['content']['application/json']

export interface IExternalProviderAuthResponse {
    userId?: number,
    email?: string | null,
    name?: string | null,
    firstName?: string | null,
    lastName?: string | null,
    created?: string,
    birthday?: string | null,
    phone?: string | null,
    stateId?: number,
    roleId?: number,
    hourRate?: number | null,
    tokenType?: string | null,
    token?: string | null
}

class AuthService {
    static googleAuth(
        requestData: paths['/api/auth/login/google']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/auth/login/google']['post']['responses']['200']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: '/api/auth/login/google',
            data: requestData,
            cancelToken,
        });
    }

    static googleOneTapAuth(
        requestData: paths['/api/auth/login/google-one-tap']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/auth/login/google-one-tap']['post']['responses']['200']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: '/api/auth/login/google-one-tap',
            data: requestData,
            cancelToken,
        });
    }

    static externalProviderAuth(
        requestData: ExternalProviderRequestType,
        url: string,
        options: ServiceRequestOptions = {}
    ): Promise<AxiosResponse<IExternalProviderAuthResponse>> {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: url,
            data: requestData,
            cancelToken,
        });
    }

    static login(
        requestData: paths['/api/auth/login']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/auth/login']['post']['responses']['200']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: '/api/auth/login',
            data: requestData,
            cancelToken,
            withCredentials: true
        });
    }

    static async logOut(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/auth/logout']['delete']['responses']['200']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.delete({
            url: '/api/auth/logout',
            cancelToken,
            withCredentials: true
        });
    }
}

export default AuthService;