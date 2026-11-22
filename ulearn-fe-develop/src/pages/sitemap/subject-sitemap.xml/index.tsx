import { ISitemapField, getServerSideSitemapLegacy } from 'next-sitemap'
import { GetServerSideProps } from 'next'
import TranslationService from 'core/services/TranslationCacheService';
import { isFulfilled } from 'core/services/Utils/apiUtil';
import routes from 'routes/routes';
import { getFullUrlWithLocale } from 'core/services/Utils/urlUtil';
import { FALLBACK_LANGUAGE_CODE } from 'core/constants/translationConstants';
import { getAvailableLanguages } from 'core/services/Utils/languageUtil';
import CodetableService from 'core/services/CodetableService';

export const getServerSideProps: GetServerSideProps = async (ctx) => {
    const subjectsPromise = CodetableService.getSubjects();
    const languagesPromise = TranslationService.getTranslationLanguages();
    const [subjectsRes, languagesRes] = await Promise.allSettled([subjectsPromise, languagesPromise]);
    const subjects = isFulfilled(subjectsRes) ? subjectsRes.value.data : undefined;
    const languages = isFulfilled(languagesRes) ? getAvailableLanguages(languagesRes.value.data) : undefined;

    if (!subjects) {
        throw new Error("Failed to fetch subjects");
    }

    if (!languages) {
        throw new Error("Failed to fetch languages");
    }

    const data: ISitemapField[] = subjects
        .map(x => languages.map(y => ({
            relativePath: routes.subjectLandingPage.path(x.enumKey ?? ""),
            locale: y.languageCode
        })))
        .flat()
        .map(x => ({
            loc: getFullUrlWithLocale(x.locale ?? FALLBACK_LANGUAGE_CODE, x.relativePath),
            lastmod: new Date().toISOString(),
            changefreq: "daily",
            alternateRefs: languages.map(y => ({
                href: getFullUrlWithLocale(y.languageCode ?? FALLBACK_LANGUAGE_CODE, x.relativePath),
                hreflang: y.languageCode ?? FALLBACK_LANGUAGE_CODE,
                hrefIsAbsolute: true
            }))
        }))


    return getServerSideSitemapLegacy(ctx, data)
}

// Default export to prevent next.js errors
export default function Sitemap() { }