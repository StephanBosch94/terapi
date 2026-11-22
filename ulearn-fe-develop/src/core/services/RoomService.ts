import { paths } from "apiTypes";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";

class RoomService {
    static get(
        key: string,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/room/{key}']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: `/api/room/${key}`,
            authorization: true,
            cancelToken,
        });
    }
}

export default RoomService;