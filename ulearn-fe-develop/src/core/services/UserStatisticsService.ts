import { paths } from "apiTypes";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AxiosResponse } from "axios";
import { nameof } from "./Types/AppTypes";

class UserStatisticsService {
    static getPriceStatistics(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user-statistics/price']['get']['responses']['200']['content']['application/json']>
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: '/api/user-statistics/price',
            cancelToken,
        });
    }

    static getTutorPublicStatistics(
        userId: number,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user-statistics/tutor/{userId}']['get']['responses']['200']['content']['application/json']>
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: `/api/user-statistics/tutor/${userId}`,
            cancelToken,
        });
    }

    static getTutorPrivateStatistics(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user-statistics/tutor']['get']['responses']['200']['content']['application/json']>
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/user-statistics/tutor'),
            cancelToken,
        });
    }

    static getStudentPrivateStatistics(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user-statistics/student']['get']['responses']['200']['content']['application/json']>
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/user-statistics/student'),
            cancelToken,
        });
    }

    static getCompanyPrivateStatistics(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user-statistics/company']['get']['responses']['200']['content']['application/json']>
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/user-statistics/company'),
            cancelToken,
        });
    }
}

export default UserStatisticsService;