import { paths } from "apiTypes";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AxiosResponse } from "axios";
import { nameof } from "./Types/AppTypes";

class UserWalletStatisticsService {
    static getUserWalletStatistics(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user-wallet']['get']['responses']['200']['content']['application/json']>
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/user-wallet'),
            authorization: true,
            cancelToken,
        });
    }

    static depositMoney(
        requestBody: paths['/api/user-wallet/deposit']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user-wallet/deposit']['put']['responses']['200']['content']['application/json']>
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>('/api/user-wallet/deposit'),
            data: requestBody,
            authorization: true,
            cancelToken,
        });
    }
    
    static withdrawMoney(
        requestBody: paths['/api/user-wallet/withdrawal']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user-wallet/withdrawal']['put']['responses']['204']>
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>('/api/user-wallet/withdrawal'),
            data: requestBody,
            authorization: true,
            cancelToken,
        });
    }

    static topUpPayment(
        requestBody: paths['/api/user-wallet/top-up']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user-wallet/top-up']['put']['responses']['204']>
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>('/api/user-wallet/top-up'),
            data: requestBody,
            authorization: true,
            cancelToken,
        });
    }

    static topUpUserPayment(
        requestBody: paths['/api/user-wallet/user/top-up']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user-wallet/user/top-up']['put']['responses']['204']>
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>('/api/user-wallet/user/top-up'),
            data: requestBody,
            authorization: true,
            cancelToken,
        });
    }

    static extractMoney(
        requestBody: paths['/api/user-wallet/user/extract']['put']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/user-wallet/user/extract']['put']['responses']['204']>
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: nameof<paths>('/api/user-wallet/user/extract'),
            data: requestBody,
            authorization: true,
            cancelToken,
        });
    }
}

export default UserWalletStatisticsService;