import { paths } from "apiTypes";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AddUserRoomStatisticRequestBody } from "core/redux/userRoomStatisticSlice";


class UserRoomStatisticService {
    static add(
        statistic: AddUserRoomStatisticRequestBody,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user-room-statistic']['post']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: '/api/user-room-statistic',
            authorization: true,
            data: statistic,
            cancelToken,
        });
    }
}


export default UserRoomStatisticService;