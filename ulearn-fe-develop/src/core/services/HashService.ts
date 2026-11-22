import { paths } from "../../apiTypes";
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AxiosResponse } from "axios";
import CoreApiService from "./Core/CoreApiService";
import { getStorage } from "../services/Core/StorageService";
import Logger from "./Utils/Logger";

class HashService {
    static getHashes(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/codetable/master-cache']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: '/api/codetable/master-cache',
            cancelToken,
        });
    }


    static async setHashToStorage(key: string, hash: string): Promise<void> {
        try {
            await (await getStorage()).setCacheHash(key, hash);
        } catch (error) {
            Logger.error(error);
        }
    }

    static async getHashFromStorage(key: string): Promise<string | undefined> {
        return await (await getStorage()).getCacheHash(key);
    }
}

export default HashService;