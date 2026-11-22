import axios from 'axios';
import { RequestMethodEnum } from 'core/constants/enums/RequestMethodEnum';
import { AxiosRequestConfig, AxiosResponse, CancelToken } from 'axios';
import { getConfig } from "../Utils/configUtil";
import * as qs from 'query-string';

type CoreApiServiceOptions = {
    url: string;
    data?: any;
    params?: any;
    cancelToken?: CancelToken | null;
    headers?: {
        [key: string]: string;
    };
    authorization?: boolean;
    useApiEndpoint?: boolean;
    responseType?: AxiosRequestConfig['responseType'];
    withCredentials?: boolean;
};


export const getBeApiBaseUrl = () => {
    const envBaseUrl = process.env.NEXT_PUBLIC_BE_API_URL;
    const prodBaseUrl = process.env.NEXT_PUBLIC_PROD_PUBLIC_BE_API_URL;

    return prodBaseUrl || envBaseUrl || getConfig().BE_API_URL;
}

const getWebsocketApiBaseUrl = () => {
    const envBaseUrl = process.env.NEXT_PUBLIC_WEBSOCKET_API_ENDPOINT;
    const prodBaseUrl = process.env.NEXT_PUBLIC_PROD_PUBLIC_WEBSOCKET_API_ENDPOINT;

    return prodBaseUrl || envBaseUrl || getConfig().WEBSOCKET_API_ENDPOINT;
}

const createUrl = ({ url, useApiEndpoint }: CoreApiServiceOptions): string => {
    return useApiEndpoint ? `${getBeApiBaseUrl()}${url.startsWith('/') ? url.slice(1) : url}` : url;
};

const createWebsocketUrl = ({ url }: CoreApiServiceOptions): string => {
    return `${getWebsocketApiBaseUrl()}${url.startsWith('/') ? url.slice(1) : url}`;
};

export const getWebApiHubUrl = async (url: string, params?: any) => {
    const baseUrl = createWebsocketUrl({ url });
    return params
        ? `${baseUrl}?${qs.stringify({ ...params })}`
        : baseUrl;
}

const createApiRequest = async (
    options: CoreApiServiceOptions,
    method: RequestMethodEnum,
): Promise<AxiosResponse> => {
    const {
        url,
        data = {},
        params = {},
        cancelToken,
        headers = {},
        useApiEndpoint = true,
        responseType,
        withCredentials
    } = options;
    const requestHeaders = { ...headers };


    return axios.request({
        method,
        url: createUrl({ url, useApiEndpoint }),
        headers: requestHeaders,
        cancelToken: cancelToken ?? undefined,
        data,
        params,
        responseType,
        withCredentials: true
    });
};

const CoreApiService = {
    get: <T = any>(options: CoreApiServiceOptions): Promise<AxiosResponse<T>> =>
        createApiRequest(options, RequestMethodEnum.Get),

    post: <T = any>(options: CoreApiServiceOptions): Promise<AxiosResponse<T>> =>
        createApiRequest(options, RequestMethodEnum.Post),

    put: <T = any>(options: CoreApiServiceOptions): Promise<AxiosResponse<T>> =>
        createApiRequest(options, RequestMethodEnum.Put),

    patch: <T = any>(options: CoreApiServiceOptions): Promise<AxiosResponse<T>> =>
        createApiRequest(options, RequestMethodEnum.Patch),

    delete: <T = any>(options: CoreApiServiceOptions): Promise<AxiosResponse<T>> =>
        createApiRequest(options, RequestMethodEnum.Delete),

    createUrl,
};

export default CoreApiService;
