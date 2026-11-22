import { paths } from "apiTypes";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AxiosResponse } from "axios";
import { nameof } from "./Types/AppTypes";

class AppUserBehaviourService {
    static createTutorProfileVisited(
        params: paths['/api/user-behaviour/tutor-profile/{userId}']['post']['parameters']['path'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user-behaviour/tutor-profile/{userId}']['post']['responses']['204']>
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>('/api/user-behaviour/tutor-profile/{userId}').replace('{userId}', String(params.userId)),
            cancelToken,
            authorization: true
        });
    }

    
}

export default AppUserBehaviourService;