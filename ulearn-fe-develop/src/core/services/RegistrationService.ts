import { paths } from "apiTypes";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { nameof } from "./Types/AppTypes";

export type RegistrationRequest = paths['/api/registration/register']['post']['requestBody']['content']['application/json'];
export type RegistrationResponse = paths['/api/registration/register']['post']['responses']['200'];
export type ActivateEmailRequest = paths['/api/registration/activate']['put']['requestBody']['content']['application/json'];
export type ActivateEmailResponse = paths['/api/registration/activate']['put']['responses']['204'];
export type ResendCodeResponse = paths['/api/registration/resend-code']['post']['responses']['200']['content']['application/json'];
export type ResendCodeLatestInfoResponse = paths['/api/registration/code-info']['get']['responses']['200']['content']['application/json'];
export type ForgottenPasswordRequest = paths['/api/registration/forgotten-password']['post']['requestBody']['content']['application/json'];
export type ForgottenPasswordResponse = paths['/api/registration/forgotten-password']['post']['responses']['200'];
export type InviteColleaguesRequest = paths['/api/registration/invite']['post']['requestBody']['content']['application/json'];
export type InviteColleaguesResponse = paths['/api/registration/invite']['post']['responses']['200'];

class RegistrationService {
    static register(
        requestData: paths['/api/registration/register']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/registration/register']['post']['responses']['200']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>("/api/registration/register"),
            data: requestData,
            cancelToken,
        });
    }

    static resendCode(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/registration/resend-code']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>("/api/registration/resend-code"),
            cancelToken,
        });
    }

    static linkActivate(
        requestData: { token: string } & paths['/api/registration/link/activate']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/registration/link/activate']['put']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>("/api/registration/link/activate"),
            data: requestData,
            cancelToken,
            headers: {
                Authorization: `Bearer ${requestData.token}`
            }
        });
    }

    static activate(
        requestData: paths['/api/registration/activate']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/registration/activate']['put']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>("/api/registration/activate"),
            data: requestData,
            cancelToken
        });
    }

    static getCodeInfo(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/registration/code-info']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>("/api/registration/code-info"),
            cancelToken
        });
    }

    static forgottenPassword(
        data: ForgottenPasswordRequest,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<ForgottenPasswordResponse>
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>("/api/registration/forgotten-password"),
            data: data,
            cancelToken
        });
    }

    static inviteColleagues(
        data: InviteColleaguesRequest,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<InviteColleaguesResponse>
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>("/api/registration/invite"),
            data: data,
            cancelToken
        });
    }
}

export default RegistrationService;