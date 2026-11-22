import { paths } from "apiTypes";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";

class AppTimeslotService {
    static create(
        request: paths['/api/timeslot']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/timeslot']['post']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: '/api/timeslot',
            data: request,
            authorization: true,
            cancelToken,
        });
    }

    static delete(
        request: paths['/api/timeslot/{timeslotId}']['delete']['parameters']['path'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/timeslot/{timeslotId}']['delete']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.delete({
            url: `/api/timeslot/${request.timeslotId}`,
            data: request,
            authorization: true,
            cancelToken,
        });
    }
}

export default AppTimeslotService;