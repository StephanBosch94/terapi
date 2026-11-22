import { paths } from "apiTypes";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { nameof } from "./Types/AppTypes";

class AppNotificationService {
    static filter(
        request: paths['/api/notification/filter']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/notification/filter']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>('/api/notification/filter'),
            data: request,
            authorization: true,
            cancelToken,
        });
    }

    static setReadAll(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/notification/read']['put']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>('/api/notification/read'),
            authorization: true,
            cancelToken,
        });
    }

    static setRead(
        notificationId: number,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/notification/{notificationId}/read']['put']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: `/api/notification/${notificationId}/read`,
            authorization: true,
            cancelToken,
        });
    }

    static getSettings(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/notification/settings']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>("/api/notification/settings"),
            authorization: true,
            cancelToken,
        });
    }

    static editSettings(
        data: paths['/api/notification/settings']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/notification/settings']['put']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>("/api/notification/settings"),
            data,
            authorization: true,
            cancelToken,
        });
    }
}

export default AppNotificationService;