import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { paths } from "apiTypes";

class UserFileService {
    static uploadProfilePhoto(
        file: File,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user-file/profile-photo']['post']['responses']['200']
        >
    > {
        const { cancelToken } = options;
        const formData = new FormData();
        formData.append(file.name, file);

        return CoreApiService.post({
            url: '/api/user-file/profile-photo',
            headers: {
                'Content-Type': 'multipart/form-data'
            },
            authorization: true,
            data: formData,
            cancelToken,
        });
    }

    static uploadCetrificates(
        files: File[],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user-file/certificate']['post']['responses']['200']
        >
    > {
        const { cancelToken } = options;
        const formData = new FormData();
        files.forEach(file => {
            formData.append(file.name, file);
        })

        return CoreApiService.post({
            url: '/api/user-file/certificate',
            headers: {
                'Content-Type': 'multipart/form-data'
            },
            authorization: true,
            data: formData,
            cancelToken,
        });
    }


    static deleteUserFile(
        params: paths['/api/user-file/filepath/{filePath}']['delete']['parameters']['path'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/user-file/filepath/{filePath}']['delete']['responses']['200']
        >
    > {
        const { cancelToken } = options;
        const { filepath } = params;
        return CoreApiService.delete({
            url: `/api/user-file/filepath/${encodeURIComponent(filepath)}`,
            authorization: true,
            cancelToken,
        });
    }
}

export default UserFileService;