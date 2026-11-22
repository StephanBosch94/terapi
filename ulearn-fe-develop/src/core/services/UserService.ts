import { paths } from "../../apiTypes";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { getStorage } from "../services/Core/StorageService";
import { CACHE_KEYS } from "core/constants/localStorageContants";
import { toUserRoleEnum, UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import jwt_decode from "jwt-decode";
import { IJwtToken, nameof, TokenVM } from "./Types/AppTypes";
import { ChangeLanguageRequestParams } from "core/redux/userSlice";

export type ChangePasswordRequest = paths['/api/user/password']['put']['requestBody']['content']['application/json'];
export type ChangePasswordResponse = paths['/api/user/password']['put']['responses']['200'];
export type ResetPasswordRequest = paths['/api/user/password']['put']['requestBody']['content']['application/json'];
export type ResetPasswordResponse = paths['/api/user/reset-password']['put']['responses']['200'];


class UserService {
    static getPublicTutorInfoById(
        userId: number,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user/{userId}']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: `/api/user/${userId}`,
            authorization: false,
            cancelToken,
        });
    }

    static getUserInfo(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;
        return CoreApiService.get({
            url: nameof<paths>('/api/user'),
            authorization: true,
            cancelToken,
        });
    }

    static getShortUserInfo(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user/short-info']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/user/short-info'),
            authorization: true,
            cancelToken,
            withCredentials: true
        });
    }

    static updateUserInfo(
        requestBody: paths['/api/user']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user']['put']['responses']['200']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>('/api/user'),
            data: requestBody,
            authorization: true,
            cancelToken,
        });
    }

    static editEmployeeAccountInfo(
        requestBody: paths['/api/user/employee']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user/employee']['put']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>('/api/user/employee'),
            data: requestBody,
            authorization: true,
            cancelToken,
        });
    }

    static getStudents(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user/students']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/user/students'),
            authorization: true,
            cancelToken,
        });
    }

    static getTutors(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user/tutors']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/user/tutors'),
            authorization: true,
            cancelToken,
        });
    }

    static filterTutors(
        requestBody: paths['/api/user/tutor/filter']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user/tutor/filter']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>('/api/user/tutor/filter'),
            data: requestBody,
            cancelToken,
        });
    }

    static filterStudents(
        requestBody: paths['/api/user/student/filter']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user/student/filter']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>(`/api/user/student/filter`),
            authorization: true,
            data: requestBody,
            cancelToken,
        });
    }

    static filterEmployees(
        requestBody: paths['/api/user/employee/filter']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user/employee/filter']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>('/api/user/employee/filter'),
            authorization: true,
            data: requestBody,
            cancelToken,
        });
    }

    static approve(
        userId: paths['/api/user/{userId}/approve']['put']['parameters']["path"]["userId"],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user/{userId}/approve']['put']['responses']['200']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: `/api/user/${userId}/approve`,
            authorization: true,
            cancelToken,
        });
    }

    static refuse(
        userId: paths['/api/user/{userId}/refuse']['put']['parameters']["path"]["userId"],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
        paths['/api/user/{userId}/refuse']['put']['responses']['200']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: `/api/user/${userId}/refuse`,
            authorization: true,
            cancelToken,
        });
    }

    static changeLanguage(
        params: ChangeLanguageRequestParams,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user/change-language/{languageCode}']['put']['responses']['200']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: `/api/user/change-language/${params.languageCode}`,
            authorization: true,
            cancelToken,
        });
    }

    static changePassword(
        params: ChangePasswordRequest,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<ChangePasswordResponse>
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>("/api/user/password"),
            data: params,
            authorization: true,
            cancelToken,
        });
    }

    static resetPassword(
        params: ResetPasswordRequest,
        token: string,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<ResetPasswordResponse>
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>("/api/user/reset-password"),
            data: params,
            headers: {
                'authorization': `Bearer ${token}`
            },
            authorization: true,
            cancelToken,
        });
    }

    static deleteEmployeeAccount(
        params: paths['/api/user/employee']['delete']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user/employee']['delete']['responses']['204']>
    > {
        const { cancelToken } = options;

        return CoreApiService.delete({
            url: nameof<paths>("/api/user/employee"),
            data: params,
            authorization: true,
            cancelToken,
        });
    }

    static adminEditAccount(
        params: paths['/api/user/admin']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user/admin']['put']['responses']['204']>
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>("/api/user/admin"),
            data: params,
            authorization: true,
            cancelToken,
        });
    }

    static async getBearerToken(): Promise<string | undefined> {
        return await (await getStorage()).getCommonValue<string>(CACHE_KEYS.COMMON.BEARER_TOKEN);
    }

    static async isAuthorized(): Promise<boolean> {
        return (await (await getStorage()).getCommonValue<string>(CACHE_KEYS.COMMON.BEARER_TOKEN)) !== undefined;
    }

    static async getUserTokenInfo() {
        const token = await this.getBearerToken();
        if (!token) { return; }
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace('-', '+').replace('_', '/');
        return JSON.parse(window.atob(base64)) as TokenVM;
    }

    static async getUserRole(): Promise<UserRoleEnum | undefined> {
        const token = await this.getBearerToken();
        if (token === undefined) {
            return undefined;
        }
        var decoded = jwt_decode(token) as IJwtToken;
        return toUserRoleEnum(decoded.ROLES_CLAIM);
    }

    static getTutorSitemapInfo(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user/tutor/sitemap-info']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/user/tutor/sitemap-info'),
            cancelToken,
        });
    }

    static getTutorAdminInfo(
        params: paths['/api/user/admin/{userId}']['get']['parameters']['path'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user/admin/{userId}']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;
        return CoreApiService.get({
            url: `/api/user/admin/${params.userId}`,
            cancelToken,
        });
    }
}

export default UserService;