import { UserRoleEnum } from 'core/constants/enums/UserRoleEnum';
import { UserStateEnum } from 'core/constants/enums/UserStateEnum';
import { FALLBACK_LANGUAGE_CODE } from 'core/constants/translationConstants';
import { getClaims } from 'core/services/Utils/tokenUtil';
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';
import routes, { isHomePage, isPathAllowed } from 'routes/routes';

const LOCALE_COOKIE_NAME = 'NEXT_LOCALE';
const REMOVED_LOCALES = new Set(['cs']);

const stripRemovedLocale = (pathname: string) => {
  const [, first, ...rest] = pathname.split('/');
  if (!first || !REMOVED_LOCALES.has(first)) return pathname;
  const stripped = `/${rest.join('/')}`;
  return stripped === '/' ? '/' : stripped.replace(/\/$/, '') || '/';
}
const getCookie = (cookie: string | undefined) => {
  if (cookie === "undefined") return undefined;
  return cookie;
}

const getSearchParams = (params: URLSearchParams | undefined) => {
  const paramsString = params?.toString();
  return params && paramsString !== "" ? `?${paramsString}` : "";
}

const getPathWithLocale = (locale: string | undefined, pathname: string, searchParams: string = "") => `/${locale ?? FALLBACK_LANGUAGE_CODE}${pathname}${searchParams}`;

const getNextResponseWithCookie = (locale: string, cookiesLocale: string | undefined) => {
  const response = NextResponse.next();

  if (locale !== cookiesLocale) {
    response.cookies.set(LOCALE_COOKIE_NAME, locale);
  }

  return response;
}

export function middleware(request: NextRequest) {
  const { pathname, locale } = request.nextUrl;
  const searchParams = getSearchParams(request?.nextUrl?.searchParams);
  const rawCookieLocale = getCookie(request.cookies.get(LOCALE_COOKIE_NAME)?.value);
  const cookiesLocale = rawCookieLocale && REMOVED_LOCALES.has(rawCookieLocale) ? undefined : rawCookieLocale;

  // Locale redirect. Czech is no longer a site language.
  if (locale === 'default' || REMOVED_LOCALES.has(locale)) {
    const redirectUrl = getPathWithLocale(cookiesLocale, stripRemovedLocale(pathname), searchParams);
    const response = NextResponse.redirect(new URL(redirectUrl, request.url));
    if (rawCookieLocale && REMOVED_LOCALES.has(rawCookieLocale)) {
      response.cookies.set(LOCALE_COOKIE_NAME, FALLBACK_LANGUAGE_CODE);
    }
    return response;
  }

  // Path validation
  const claims = getClaims(request.cookies.get('BearerCookie')?.value);

  if (!isPathAllowed(pathname, claims?.userRoleId)) {
    const redirectUrl = getPathWithLocale(locale, routes.notFoundPage.path);
    return NextResponse.rewrite(new URL(redirectUrl, request.url));
  }

  // Redirect 
  if (!claims) return getNextResponseWithCookie(locale, cookiesLocale);
  if (!isHomePage(pathname)) return getNextResponseWithCookie(locale, cookiesLocale);

  if (claims.userRoleId) {
    switch (claims.userRoleId) {
      case UserRoleEnum.Student:
        switch (claims.userStateId) {
          case UserStateEnum.EmailVerification:
            return NextResponse.redirect(new URL(routes.confirmEmail.path, request.url));
          case UserStateEnum.RoleSetup:
            return NextResponse.redirect(new URL(routes.setRole.path, request.url));
          case UserStateEnum.PasswordSetup:
            return NextResponse.redirect(new URL(routes.setPassword.path, request.url));
          default:
            return NextResponse.redirect(new URL(routes.studentDashboard.path, request.url));
        }
      case UserRoleEnum.Tutor:
        switch (claims.userStateId) {
          case UserStateEnum.EmailVerification:
            return NextResponse.redirect(new URL(routes.confirmEmail.path, request.url));
          case UserStateEnum.RoleSetup:
            return NextResponse.redirect(new URL(routes.setRole.path, request.url));
          case UserStateEnum.PasswordSetup:
            return NextResponse.redirect(new URL(routes.setPassword.path, request.url));
          case UserStateEnum.Registration:
            return NextResponse.redirect(new URL(routes.tutorRegistrationForm.path, request.url));
          case UserStateEnum.ProfileValidation:
            return NextResponse.redirect(new URL(routes.tutorVerification.path, request.url));
          case UserStateEnum.Refused:
            return NextResponse.redirect(new URL(routes.tutorVerification.path, request.url));
          default:
            return NextResponse.redirect(new URL(routes.tutorDashboard.path, request.url));
        }
      case UserRoleEnum.Admin:
        return NextResponse.redirect(new URL(routes.adminDashboard.path, request.url));
      case UserRoleEnum.Company:
        switch (claims.userStateId) {
          case UserStateEnum.EmailVerification:
            return NextResponse.redirect(new URL(routes.confirmEmail.path, request.url));
          case UserStateEnum.RoleSetup:
            return NextResponse.redirect(new URL(routes.setRole.path, request.url));
          case UserStateEnum.PasswordSetup:
            return NextResponse.redirect(new URL(routes.setPassword.path, request.url));
          default:
            return NextResponse.redirect(new URL(routes.companyDashboard.path, request.url));
        }
    }
  }

  return getNextResponseWithCookie(locale, cookiesLocale);
}

export const config = {
  matcher: [
    /*
     * Match all request paths except for the ones starting with:
     * - api (API routes)
     * - _next/static (static files)
     * - _next/image (image optimization files)
     * - favicon.ico (favicon file)
     */
    '/((?!api|_next/static|_next/image|sitemap|images|legal|favicon.ico|robots.txt|sitemap.xml|sitemap-0.xml|touch-icon-ipad.jpg).*)',
    '/'
  ],
}