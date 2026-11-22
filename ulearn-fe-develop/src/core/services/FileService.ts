import { getBeApiBaseUrl } from "./Core/CoreApiService";


class FileService {
    static getFileUrl = (filePath: string) => `${getBeApiBaseUrl()}api/file/filepath/${filePath}`
}


export default FileService;