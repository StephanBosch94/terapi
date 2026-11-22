import { paths } from "apiTypes";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { nameof } from "./Types/AppTypes";

class ReservationService {
    static filter(
        requestBody: paths['/api/reservation/filter']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/reservation/filter']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>('/api/reservation/filter'),
            data: requestBody,
            authorization: true,
            cancelToken,
        });
    }

    static create(
        requestBody: paths['/api/reservation']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/reservation']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: nameof<paths>('/api/reservation'),
            data: requestBody,
            authorization: true,
            cancelToken,
        });
    }

    static edit(
        request: paths['/api/reservation/{reservationId}']['put']['requestBody']['content']['application/json'] & { reservationId: number },
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/reservation/{reservationId}']['put']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: `/api/reservation/${request.reservationId}`,
            data: request,
            authorization: true,
            cancelToken,
        });
    }

    static getCouponInfo(
        request: paths['/api/coupon/code/{code}']['get']['parameters']['path'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/coupon/code/{code}']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: `/api/coupon/code/${request.code}`,
            authorization: true,
            cancelToken,
        });
    }

    static getDefaultCouponInfo(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/coupon/default']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>("/api/coupon/default"),
            authorization: true,
            cancelToken,
        });
    }
}

export default ReservationService;