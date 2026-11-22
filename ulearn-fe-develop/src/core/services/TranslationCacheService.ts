import { AxiosResponse } from "axios";
import CoreApiService from 'core/services/Core/CoreApiService';
import { paths } from 'apiTypes';
import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";

class TranslationService {
    static getFilterTranslations(
        request: paths['/api/translation/filter']['post']['requestBody']['content']['application/json'],
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/translation/filter']['post']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.post({
            url: `/api/translation/filter`,
            data: request,
            authorization: false,
            cancelToken
        });
    }


    static getTranslations(
        translationLanguageId: number,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/translation/language/{translationLanguageId}']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: `/api/translation/language/${translationLanguageId}`,
            authorization: false,
            cancelToken
        });
    }

    static getTranslationsByCode(
        languageCode: string,
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/translation/language/code/{languageCode}']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: `/api/translation/language/code/${languageCode}`,
            authorization: false,
            cancelToken
        });
    }


    static getTranslationLanguages(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<paths['/api/codetable/translation-language']['get']['responses']['200']['content']['application/json']>
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: `/api/codetable/translation-language`,
            authorization: false,
            cancelToken
        });
    }
}

export default TranslationService;