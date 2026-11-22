import { FALLBACK_LANGUAGE_CODE } from "core/constants/translationConstants";

export const IS_SERVER = typeof window === "undefined";
const PROD_URL = "https://www.yaspeak.com";
export const isProd = () => process.env.NEXT_PUBLIC_DOMAIN_URL === PROD_URL;

export const getCacheDuration = () => isProd() ? 600 : 1;

export const getDomainUrl = () => {
  const domain = process.env.NEXT_PUBLIC_DOMAIN_URL;
  if (domain) {
    return !domain.endsWith('/') ? domain + '/' : domain;
  }
  return "";
}

export const getRelativeUrlWithLoacle = (locale: string | undefined, relativeUrl: string) => {
  return !relativeUrl.startsWith('/')
    ? `/${locale}/${relativeUrl}`
    : `/${locale}${relativeUrl}`
}

export const getFullUrl = (relativeUrl: string) => {
  const domainUrl = getDomainUrl();
  if (relativeUrl === '/') {
    return domainUrl;
  }
  return relativeUrl.startsWith('/')
    ? domainUrl + relativeUrl.slice(1, relativeUrl.length)
    : domainUrl + relativeUrl;
}

export const getFullUrlWithLocale = (locale: string | undefined, relativeUrl: string) => {
  const resolvedLocale = locale ?? FALLBACK_LANGUAGE_CODE;
  const domainUrl = getDomainUrl();
  if (relativeUrl === '/') {
    return domainUrl + resolvedLocale;
  }
  return relativeUrl.startsWith('/')
    ? domainUrl + resolvedLocale + relativeUrl
    : domainUrl + resolvedLocale + '/' + relativeUrl;
}