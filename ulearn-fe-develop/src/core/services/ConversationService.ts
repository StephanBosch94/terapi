import { paths } from "apiTypes";
import { AxiosResponse } from "axios";
import { requestToBodyStream } from "next/dist/server/body-streams";
import CoreApiService from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";

class ConversationService {
    static getConversations(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/conversation']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: '/api/conversation',
            authorization: true,
            cancelToken,
        });
    }

    static create(
        requrestBody: paths['/api/conversation']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/conversation']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: '/api/conversation',
            data: requrestBody,
            authorization: true,
            cancelToken,
        });
    }


    static setRead(
        conversationId: number,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/conversation/{conversationId}']['put']['responses']['200']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.put({
            url: `/api/conversation/${conversationId}`,
            authorization: true,
            cancelToken,
        });
    }
}

export default ConversationService;