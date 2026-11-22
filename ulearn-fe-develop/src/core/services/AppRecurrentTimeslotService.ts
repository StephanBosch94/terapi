import { paths } from "apiTypes";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";

class AppRepeatingTimeslotService {
    static getByDate(
        request: paths['/api/recurrent-timeslot']['get']['parameters']['query'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/recurrent-timeslot']['post']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: '/api/recurrent-timeslot',
            params: request,
            data: request,
            authorization: true,
            cancelToken,
        });
    }

    static create(
        request: paths['/api/recurrent-timeslot']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/recurrent-timeslot']['post']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: '/api/recurrent-timeslot',
            data: request,
            authorization: true,
            cancelToken,
        });
    }

    static delete(
        request: paths['/api/recurrent-timeslot/{recurrentTimeslotId}']['delete']['parameters']['path'],
        params: paths['/api/recurrent-timeslot/{recurrentTimeslotId}']['delete']['parameters']['query'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/recurrent-timeslot/{recurrentTimeslotId}']['delete']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.delete({
            url: `/api/recurrent-timeslot/${request.recurrentTimeslotId}`,
            params: params,
            authorization: true,
            cancelToken,
        });
    }
}

export default AppRepeatingTimeslotService;