import { ServiceRequestOptions } from "./Types/ServiceRequestOptions";
import { AxiosResponse } from "axios";
import { paths } from "../../apiTypes";
import CoreApiService from "./Core/CoreApiService";
import { nameof } from "./Types/AppTypes";

class CodetableService {
    static getEnvConfiguration(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/env-configuration']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/env-configuration'),
            authorization: false,
            cancelToken
        });
    }

    static getCountries(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/codetable/country']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/codetable/country'),
            authorization: false,
            cancelToken
        });
    }

    static getLanguages(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/codetable/language']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/codetable/language'),
            authorization: false,
            cancelToken
        });
    }

    static getLanguageLevels(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/codetable/language-level']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/codetable/language-level'),
            authorization: false,
            cancelToken
        });
    }

    static getSubjects(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/codetable/subject']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/codetable/subject'),
            authorization: false,
            cancelToken
        });
    }


    static getSubjectLevels(
        options: ServiceRequestOptions = {},
    ): Promise<
        AxiosResponse<
            paths['/api/codetable/subject-level']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/codetable/subject-level'),
            authorization: false,
            cancelToken
        });
    }

    static getSpecializations(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/codetable/specialization']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/codetable/specialization'),
            authorization: false,
            cancelToken
        });
    }

    static getTeachingExperiences(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/codetable/teaching-experience']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/codetable/teaching-experience'),
            authorization: false,
            cancelToken
        });
    }

    static getWeeklyGoals(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/codetable/weekly-goal']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/codetable/weekly-goal'),
            authorization: false,
            cancelToken
        });
    }

    static getInterests(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/codetable/interest']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/codetable/interest'),
            authorization: false,
            cancelToken
        });
    }

    static getReservationIssueTypes(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/codetable/reservation-issue-types']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/codetable/reservation-issue-types'),
            authorization: false,
            cancelToken
        });
    }

    static getCurrencies(
        options: ServiceRequestOptions = {}
    ): Promise<
        AxiosResponse<
            paths['/api/codetable/currency']['get']['responses']['200']['content']['application/json']
        >
    > {
        const { cancelToken } = options;

        return CoreApiService.get({
            url: nameof<paths>('/api/codetable/currency'),
            authorization: false,
            cancelToken
        });
    }
}

export default CodetableService;