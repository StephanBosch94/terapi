import { paths } from "apiTypes";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { nameof } from "./Types/AppTypes";

class AppReservationIssueService {
    static create(
        request: paths['/api/reservation-issue']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/reservation-issue']['post']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>('/api/reservation-issue'),
            data: request,
            authorization: true,
            cancelToken,
        });
    }

    static filter(
        request: paths['/api/reservation-issue/filter']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/reservation-issue/filter']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>('/api/reservation-issue/filter'),
            data: request,
            authorization: true,
            cancelToken,
        });
    }

    static approve(
        request: paths['/api/reservation-issue/approve']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/reservation-issue/approve']['put']['responses']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>('/api/reservation-issue/approve'),
            data: request,
            authorization: true,
            cancelToken,
        });
    }

    static decline(
        request: paths['/api/reservation-issue/decline']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/reservation-issue/decline']['put']['responses']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>('/api/reservation-issue/decline'),
            data: request,
            authorization: true,
            cancelToken,
        });
    }
}

export default AppReservationIssueService;