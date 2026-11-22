import { paths } from "apiTypes";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { nameof } from "./Types/AppTypes";

class AppTransactionService {
    static filter(
        request: paths['/api/transaction/filter']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/transaction/filter']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;
        
        return CoreApiService.post({
            url: nameof<paths>('/api/transaction/filter'),
            data: request,
            authorization: true,
            cancelToken,
        });
    }

    static withdrawFilter(
        request: paths['/api/transaction/withdraw/filter']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/transaction/withdraw/filter']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;
        
        return CoreApiService.post({
            url: nameof<paths>('/api/transaction/withdraw/filter'),
            data: request,
            authorization: true,
            cancelToken,
        });
    }

    static complete(
        params: paths['/api/transaction/{transactionId}/complete']['put']['parameters']['path'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/transaction/{transactionId}/complete']['put']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: `/api/transaction/${params.transactionId}/complete`,
            authorization: true,
            cancelToken,
        });
    }

    static getExportAllPath() {
        const url = nameof<paths>('/api/transaction/export');
        return CoreApiService.createUrl({url, useApiEndpoint: true});
    }

    static getTaxExportPath() {
        const url = nameof<paths>('/api/transaction/export') + '?taxOnly=true';
        return CoreApiService.createUrl({url, useApiEndpoint: true});
    }
}

export default AppTransactionService;