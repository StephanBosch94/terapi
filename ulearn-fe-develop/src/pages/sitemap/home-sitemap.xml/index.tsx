import { ISitemapField, getServerSideSitemapLegacy } from 'next-sitemap'
import { GetServerSideProps } from 'next'
import TranslationService from 'core/services/TranslationCacheService';
import { isFulfilled } from 'core/services/Utils/apiUtil';
import routes from 'routes/routes';
import { getFullUrlWithLocale } from 'core/services/Utils/urlUtil';
import { FALLBACK_LANGUAGE_CODE } from 'core/constants/translationConstants';
import { getAvailableLanguages } from 'core/services/Utils/languageUtil';

export const getServerSideProps: GetServerSideProps = async (ctx) => {
    const languagesPromise = TranslationService.getTranslationLanguages();
    const [languagesRes] = await Promise.allSettled([languagesPromise]);
   
    const languages = isFulfilled(languagesRes) ? getAvailableLanguages(languagesRes.value.data) : undefined;

    if (!languages) {
        throw new Error("Failed to fetch languages");
    }

    const data: ISitemapField[] = ['/']
        .map(x => languages.map(y => ({
            relativePath: routes.home.path,
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