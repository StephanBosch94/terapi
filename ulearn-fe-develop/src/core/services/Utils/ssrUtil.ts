import { paths } from "apiTypes";
import CodetableService from "../CodetableService"
import TranslationService from "../TranslationCacheService";
import { GetServerSidePropsContext, GetServerSidePropsResult, GetStaticPropsContext, GetStaticPropsResult, PreviewData } from "next";
import { TranslationsType } from "../Types/AppTypes";
import { ViewScreenType } from "components/core/UIContextProvider";
import { isFulfilled } from "./apiUtil";
import { IncomingHttpHeaders } from "http";
import { FALLBACK_LANGUAGE_CODE } from "core/constants/translationConstants";
import { getAvailableLanguages } from "./languageUtil";
import requestIp from 'request-ip';
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { cookieConstants } from "core/constants/cookieConstants";
import CurrencyService from "../CurrencyService";

export type DefaultPagePropsType = {
    screenType: ViewScreenType,
    locale: string | undefined,
    currencyCode: string | undefined,
    currencies: paths['/api/codetable/currency']['get']['responses']['200']['content']['application/json'],
    envConfigurations: paths['/api/env-configuration']['get']['responses']['200']['content']['application/json'],
    subjects: paths['/api/codetable/subject']['get']['responses']['200']['content']['application/json'],
    specializations: paths['/api/codetable/specialization']['get']['responses']['200']['content']['application/json'],
    languages: paths['/api/codetable/translation-language']['get']['responses']['200']['content']['application/json'],
    translations: paths['/api/translation/filter']['post']['responses']['200']['content']['application/json']
}

const getScreenType = (userAgent: string | undefined): ViewScreenType => {
    return /iPhone|iPad|iPod|Android/i.test(userAgent ?? "") ? "mobile" : "desktop";
}

const getLocale = (locale: string | undefined) => {
    return locale !== "default"
        ? (locale ?? FALLBACK_LANGUAGE_CODE)
        : FALLBACK_LANGUAGE_CODE;
}

export const fetchDefaultPageDataFromProps = async (props: GetServerSidePropsContext): Promise<DefaultPagePropsType> => {
    const detectedIp = requestIp.getClientIp(props.req);
    const cookiesCurrency = props.req.cookies[cookieConstants.currencyCode];
    const resolvedCurrency = cookiesCurrency ?? (await CurrencyService.getDefaultCurrencyByIp(detectedIp ?? ""))?.data?.enumKey ?? COMMON_CONSTANTS.DEFAULT_CURRENCY_CODE;

    return fetchDefaultPageData(props.locale, resolvedCurrency, props.req.headers);
}

export const fetchDefaultPageData = async (locale: string | undefined, currencyCode?: string, headers?: IncomingHttpHeaders): Promise<DefaultPagePropsType> => {
    const currentLocale = getLocale(locale);
    const envConfigurationsPromise = CodetableService.getEnvConfiguration();
    const subjectsPromise = CodetableService.getSubjects();
    const specializationsPromise = CodetableService.getSpecializations();
    const currenciesPromise = CodetableService.getCurrencies();
    const currTranslationsPromise = TranslationService.getTranslationsByCode(currentLocale);
    const languagesPromise = TranslationService.getTranslationLanguages();

    const [
        envConfigurationsRes,
        subjectsRes,
        specializationsRes,
        currenciesRes,
        languagesRes,
        currTranslationsRes
    ] = await Promise.allSettled([envConfigurationsPromise, subjectsPromise, specializationsPromise, currenciesPromise, languagesPromise, currTranslationsPromise]);
    const userAgent = headers ? headers["user-agent"] : undefined;
    const envConfigurations = isFulfilled(envConfigurationsRes) ? envConfigurationsRes.value : undefined;
    const subjects = isFulfilled(subjectsRes) ? subjectsRes.value : undefined;
    const specializations = isFulfilled(specializationsRes) ? specializationsRes.value : undefined;
    const currencies = isFulfilled(currenciesRes) ? currenciesRes.value : undefined;
    const languages = isFulfilled(languagesRes) ? languagesRes.value : undefined;
    const currTranslations = isFulfilled(currTranslationsRes) ? currTranslationsRes.value : undefined;

    if (!envConfigurations) {
        throw new Error("Failed to fetch envConfigurations");
    }

    if (!subjects) {
        throw new Error("Failed to fetch subjects");
    }

    if (!specializations) {
        throw new Error("Failed to fetch specializations");
    }

    if (!currencies) {
        throw new Error("Failed to fetch currencies");
    }

    if (!languages) {
        throw new Error("Failed to fetch languages");
    }

    if (!currTranslations) {
        throw new Error("Failed to fetch current locale translations");
    }

    const translations: TranslationsType = {
        [currentLocale]: currTranslations.data
    }

    return {
        screenType: getScreenType(userAgent),
        locale: currentLocale,
        envConfigurations: envConfigurations.data,
        currencies: currencies.data,
        subjects: subjects.data,
        specializations: specializations.data,
        languages: getAvailableLanguages(languages.data),
        currencyCode: currencyCode ?? COMMON_CONSTANTS.DEFAULT_CURRENCY_CODE,
        translations
    }
}

export const getSsrCacheConfig = () => {
    return 'public, s-maxage=3600, stale-while-revalidate=86400';
}

export const getDefaultServerSideProps = async (props: GetServerSidePropsContext): Promise<GetServerSidePropsResult<DefaultPagePropsType>> => {
    try {
        //res.setHeader('Cache-Control', getSsrCacheConfig())

        return {
            props: {
                ...await fetchDefaultPageDataFromProps(props)
            }
        }
    } catch (error) {
        console.error(error);
        return {
            notFound: true
        }
    }
}

export const getDefaultStaticProps = async ({ locale }: GetStaticPropsContext): Promise<GetStaticPropsResult<DefaultPagePropsType>> => {
    try {
        return {
            props: {
                ...await fetchDefaultPageData(locale!)
            },
        }
    } catch (error) {
        console.error(error);
        return {
            notFound: true
        }
    }
}
