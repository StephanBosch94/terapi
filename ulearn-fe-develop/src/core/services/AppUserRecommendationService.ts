import { paths } from "apiTypes";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AxiosResponse } from "axios";
import { nameof } from "./Types/AppTypes";

class UserStatisticsService {
    static getMatchingTutors(
        params: paths['/api/user-recommendation/matching-tutors']['get']['parameters']['query'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user-recommendation/matching-tutors']['get']['responses']['200']['content']['application/json']>
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/user-recommendation/matching-tutors'),
            params: params,
            cancelToken,
            authorization: true
        });
    }

    static filterPopularTutors(
        requestBody: paths['/api/user-recommendation/popular-tutors/filter']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user-recommendation/popular-tutors/filter']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>('/api/user-recommendation/popular-tutors/filter'),
            data: requestBody,
            cancelToken,
        });
    }

    static filterSimilarUsers(
        request: { userId: number } & paths['/api/user-recommendation/tutor/{userId}/similar-tutors/filter']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user-recommendation/tutor/{userId}/similar-tutors/filter']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;
        const { userId, ...other } = request;
        return CoreApiService.post({
            url: `/api/user-recommendation/tutor/${userId}/similar-tutors/filter`,
            data: other,
            cancelToken,
        });
    }
}

export default UserStatisticsService;