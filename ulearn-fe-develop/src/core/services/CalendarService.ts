import { paths } from "apiTypes";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { nameof } from "./Types/AppTypes";

class CalendarService {
    static getPrivateEvents(
        queryParams: paths['/api/calendar']['get']['parameters']['query'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/calendar']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/calendar'),
            params: queryParams,
            authorization: true,
            cancelToken,
        });
    }

    static getFreeSlots(
        queryParams: paths['/api/calendar/free-slot']['get']['parameters']['query'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/calendar/free-slot']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/calendar/free-slot'),
            params: queryParams,
            authorization: true,
            cancelToken,
        });
    }

    static getPublicEvents(
        request: paths['/api/calendar/user/{userId}']['get']['parameters']['query'] & { userId: number },
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/calendar/user/{userId}']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: `/api/calendar/user/${request.userId}`,
            params: request,
            authorization: false,
            cancelToken,
        });
    }

    static getPrivateCalendarStatistics(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/calendar/statistics']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/calendar/statistics'),
            authorization: true,
            cancelToken,
        });
    }
}

export default CalendarService;