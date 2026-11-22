import { paths } from "apiTypes";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AxiosResponse } from "axios";

class RoomActivityService {
    static add(
        roomKey: string,
        input: paths['/api/room-activity/room/{roomKey}']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<AxiosResponse<paths['/api/room-activity/room/{roomKey}']['post']['responses']>> {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: `/api/room-activity/room/${roomKey}`,
            data: input,
            authorization: true,
            cancelToken
        });
    }
}

export default RoomActivityService;