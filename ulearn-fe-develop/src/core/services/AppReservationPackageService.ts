import { paths } from "apiTypes";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { nameof } from "./Types/AppTypes";

class AppReservationIssueService {
    static delete(
        params: paths['/api/reservation-package/{reservationPackageId}']['delete']['parameters']['path'],
        request: paths['/api/reservation-package/{reservationPackageId}']['delete']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/reservation-package/{reservationPackageId}']['delete']['responses']['204']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.delete({
            url: `/api/reservation-package/${params.reservationPackageId}`,
            data: request,
            authorization: true,
            cancelToken,
        });
    }
}

export default AppReservationIssueService;