import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AxiosResponse } from "axios";
import { paths } from "../../apiTypes";
import CoreApiService from "./Core/CoreApiService";

class CurrencyService {
    static getDefaultCurrencyByIp(
        ip: string,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/currency/ip/{ip}']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: `/api/currency/ip/${ip}`,
            authorization: false,
            cancelToken
        });
    }

    
}

export default CurrencyService;