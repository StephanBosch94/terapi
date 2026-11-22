import { paths } from "../../apiTypes";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { nameof } from "./Types/AppTypes";

class ReservationReviewService {
    static create(
        request: paths['/api/reservation-review']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/reservation-review']['post']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>('/api/reservation-review'),
            data: request,
            authorization: true,
            cancelToken,
        });
    }

    static edit(
        request: paths['/api/reservation-review']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/reservation-review']['post']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>('/api/reservation-review'),
            data: request,
            authorization: true,
            cancelToken,
        });
    }
}

export default ReservationReviewService;