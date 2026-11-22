import { paths } from "../../apiTypes";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { nameof } from "./Types/AppTypes";

class UserReviewService {
    static create(
        request: paths['/api/user-review']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/reservation-review']['post']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>('/api/user-review'),
            data: request,
            authorization: true,
            cancelToken,
        });
    }

    static edit(
        request: paths['/api/user-review']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user-review']['put']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>('/api/user-review'),
            data: request,
            authorization: true,
            cancelToken,
        });
    }
}

export default UserReviewService;