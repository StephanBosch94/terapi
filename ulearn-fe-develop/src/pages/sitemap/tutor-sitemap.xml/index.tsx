import { ISitemapField, getServerSideSitemapLegacy } from 'next-sitemap'
import { GetServerSideProps } from 'next'
import UserService from 'core/services/UserService'
import TranslationService from 'core/services/TranslationCacheService';
import { isFulfilled } from 'core/services/Utils/apiUtil';
import routes from 'routes/routes';
import { getFullUrlWithLocale } from 'core/services/Utils/urlUtil';
import { FALLBACK_LANGUAGE_CODE } from 'core/constants/translationConstants';
import { getAvailableLanguages } from 'core/services/Utils/languageUtil';

export const getServerSideProps: GetServerSideProps = async (ctx) => {
    const tutorsPromise = UserService.getTutorSitemapInfo();
    const languagesPromise = TranslationService.getTranslationLanguages();
    const [tutorsRes, languagesRes] = await Promise.allSettled([tutorsPromise, languagesPromise]);
    const tutors = isFulfilled(tutorsRes) ? tutorsRes.value.data : undefined;
    const languages = isFulfilled(languagesRes) ? getAvailableLanguages(languagesRes.value.data) : undefined;

    if (!tutors) {
        throw new Error("Failed to fetch tutors");
    }

    if (!languages) {
        throw new Error("Failed to fetch languages");
    }

    const data: ISitemapField[] = tutors
        .map(x => languages.map(y => ({
            relativePath: routes.tutorIdPage.path(x.userId ?? 0),
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