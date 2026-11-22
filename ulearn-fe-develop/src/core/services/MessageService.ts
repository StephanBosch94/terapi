import { paths } from "apiTypes";
import { AxiosResponse } from "axios";
import CoreApiService, { getBeApiBaseUrl } from "./Core/CoreApiService";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AppMessageWithConversationInpuVM } from "./Types/AppTypes";

class MessageService {
    static create(
        input: AppMessageWithConversationInpuVM,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/message']['post']['responses']['201']
        >
    > {
        const { cancelToken } = options;
        const formData = new FormData();
        formData.append("messageText", input.messageText ?? "");
        formData.append("opponentId", `${input.opponentId}`);
        input.files?.forEach((file) => {
            formData.append(file.name, file);
        })

        return CoreApiService.post({
            url: '/api/message',
            headers: {
                'Content-Type': 'multipart/form-data'
            },
            authorization: true,
            data: formData,
            cancelToken,
        });
    }

    static filter(
        conversationId: number,
        requestBody: paths['/api/message/conversation/{conversationId}/filter']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/message/conversation/{conversationId}/filter']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: `/api/message/conversation/${conversationId}/filter`,
            data: requestBody,
            authorization: true,
            cancelToken,
        });
    }
}

export default MessageService;