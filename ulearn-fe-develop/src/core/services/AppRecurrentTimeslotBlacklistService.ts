import { paths } from "apiTypes";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";

class AppRecurrentTimeslotBlacklistService {
    static create(
        request: paths['/api/recurrent-timeslot-blacklist']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/recurrent-timeslot-blacklist']['post']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: '/api/recurrent-timeslot-blacklist',
            data: request,
            authorization: true,
            cancelToken,
        });
    }
}

export default AppRecurrentTimeslotBlacklistService;