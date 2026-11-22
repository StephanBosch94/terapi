import { FALLBACK_LANGUAGE_CODE, TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useAppSelector } from "core/redux/reduxHooks";
import { selectShortUserInfo } from "core/redux/userSlice";
import { useRouter } from "next/router";
import { useEffect, useRef } from "react";
import { getTitleKey, getDescriptionKey } from "routes/routes";
import { DefaultSeo } from 'next-seo';
import { OpenGraph } from "next-seo/lib/types";
import { useTranslation } from "./TranslationProvider";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { getFullUrl, getFullUrlWithLocale, isProd } from "core/services/Utils/urlUtil";

type OpenGraphProps = {
    relativeUrl: string,
    locale: string | undefined,
    title: string | undefined,
    description: string | undefined,
    imagePath?: string
}

export const getOpenGraphConfig = (props: OpenGraphProps): OpenGraph => ({
    url: getFullUrlWithLocale(props.locale, props.relativeUrl),
    title: props.title,
    description: props.description,
    locale: props.locale,
    type: "website",
    images: [{
        url: getFullUrl(props.imagePath ?? '/images/open-graph.png'),
        width: 800,
        height: 420,
        alt: "Yaspeak",
    }]
})

export const getSeoLanguageAlternatives = (locales: string[] | undefined, path: string) => {
    return (locales ?? [])
        .filter(x => x !== 'default')
        .map(x => ({
            href: getFullUrlWithLocale(x, path),
            hrefLang: x
        })).concat([{
            href: getFullUrlWithLocale(FALLBACK_LANGUAGE_CODE, path),
            hrefLang: 'x-default'
        }]);
}

const Seo = () => {
    const router = useRouter();
    const locale = router.locale ?? FALLBACK_LANGUAGE_CODE;
    const locales = router.locales ?? [];
    const userInfo = useAppSelector(selectShortUserInfo);
    const { t, tContains } = useTranslation();
    const titleKey = getTitleKey(router.pathname) ?? TRANSLATION_KEYS.PAGE_TITLE.HOME_PAGE;
    const descriptionKey = getDescriptionKey(router.pathname) ?? TRANSLATION_KEYS.PAGE_DESCRIPTION.HOME_PAGE;
    const defaultTitle = tContains(titleKey) ? t(titleKey) : t(TRANSLATION_KEYS.PAGE_TITLE.HOME_PAGE);
    const description = tContains(descriptionKey) ? t(descriptionKey) : t(TRANSLATION_KEYS.PAGE_DESCRIPTION.HOME_PAGE);

    const temporaryAcc = useRef<string | undefined>();

    useEffect(() => {
        if (!userInfo) return;
        if (!userInfo.countOfUnreadMessages) return;
        const messageTranslation = t(TRANSLATION_KEYS.PAGE_TITLE.YOU_GOT_MESSAGE, { countOfUnreadMessages: userInfo.countOfUnreadMessages }) ?? "";

        const interval = setInterval(() => {
            if (document.title.includes("{{")) return; // Value hasn't initialized yet

            if (!temporaryAcc.current) {
                temporaryAcc.current = document.title;
            }

            document.title = messageTranslation;

            setTimeout(() => {
                document.title = temporaryAcc.current ?? "";
            }, 1000);
        }, 3000);

        return () => {
            clearInterval(interval);
        }
    }, [router.locale, userInfo]);

    return (
        <DefaultSeo
            title={defaultTitle}
            description={description}
            openGraph={getOpenGraphConfig({ relativeUrl: router.pathname, locale, title: defaultTitle, description })}
            themeColor={COLORS.PRIMARY}
            dangerouslySetAllPagesToNoIndex={!isProd()}
            dangerouslySetAllPagesToNoFollow={!isProd()}
            additionalLinkTags={[
                {
                    rel: 'icon',
                    href: '/favicon.ico',
                    sizes: 'any'
                },
                {
                    rel: 'apple-touch-icon',
                    href: '/touch-icon-ipad.jpg',
                    sizes: '76x76'
                },
            ]}
            languageAlternates={getSeoLanguageAlternatives(locales, router.pathname)}
            canonical={getFullUrlWithLocale(locale, router.pathname)}
        />
    )
}

export default Seo;