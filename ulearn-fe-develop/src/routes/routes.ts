import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { GetServerSidePropsContext, GetServerSidePropsResult } from "next";

export type StringPageQueryParams = { id: string }
export type ServerSideProps = GetServerSidePropsContext<StringPageQueryParams>;

type BasicRouteType = {
    type: "static" | "dynamic",
    roles: UserRoleEnum[] | undefined,
    titleKey: string | undefined,
    descriptionKey: string | undefined,
    showChat: boolean
}

type StaticRouteType = BasicRouteType & {
    type: "static",
    path: string
}

type DynamicRouteType = BasicRouteType & {
    type: "dynamic",
    matchingPath: string,
    path: (...args: any[]) => string,
    getPathParams: (pathName: string) => any
}

type RoutesType = {
    home: StaticRouteType;
    tutorDashboard: StaticRouteType;
    studentDashboard: StaticRouteType;
    companyDashboard: StaticRouteType;
    adminDashboard: StaticRouteType;
    googleAuthTutor: StaticRouteType;
    facebookAuthTutor: StaticRouteType;
    seznamAuthTutor: StaticRouteType;
    googleAuthCompany: StaticRouteType;
    facebookAuthCompany: StaticRouteType;
    seznamAuthCompany: StaticRouteType;
    googleAuthStudent: StaticRouteType;
    facebookAuthStudent: StaticRouteType;
    seznamAuthStudent: StaticRouteType;
    forgotPassword: StaticRouteType;
    setPassword: StaticRouteType;
    resetPassword: StaticRouteType;
    confirmEmail: StaticRouteType;
    setRole: StaticRouteType;
    emailCodeVerification: StaticRouteType;
    privacyPolicy: StaticRouteType;
    termsOfUse: StaticRouteType;
    tutorLandingPage: StaticRouteType;
    companyLandingPage: StaticRouteType;
    tutorRegistrationForm: StaticRouteType;
    companyRegistrationForm: StaticRouteType;
    tutorVerification: StaticRouteType;
    search: StaticRouteType;
    tutorSettingsPage: StaticRouteType;
    studentSettingsPage: StaticRouteType;
    companySettingsPage: StaticRouteType;
    messagesPage: StaticRouteType;
    notFoundPage: StaticRouteType;
    reservationPaymentSuccess: StaticRouteType;
    moneyRefillSuccess: StaticRouteType;
    notifications: StaticRouteType;
    subjectLandingPage: Omit<DynamicRouteType, 'path'> & { path: (subjectEnumKey: string) => string };
    specializationLandingPage: Omit<DynamicRouteType, 'path'> & { path: (specializationEnumKey: string) => string };
    tutorIdPage: Omit<DynamicRouteType, 'path'> & { path: (tutorId: number) => string };
    meetingPage: Omit<DynamicRouteType, 'path'> & { path: (meetingKey: string) => string };
}

export const staticRoutes = {
    termsOfUse: "/legal/Terms.pdf",
    tutorTermsOfUse: "/legal/TermsTutor.pdf",
    gdpr: "/legal/GDPR.pdf",
    cookies: "/legal/Cookies.pdf",
    facebook: "https://www.facebook.com/people/Yaspeak/100092916536364/",
    instagram: "https://www.instagram.com/yaspeak_cz/"
}

export const routes: RoutesType = {
    // Static Paths
    home: {
        type: "static",
        path: "/",
        roles: undefined,
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.HOME_PAGE,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.HOME_PAGE,
        showChat: true
    },
    tutorDashboard: {
        type: "static",
        path: "/tutor/dashboard",
        roles: [UserRoleEnum.Tutor],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.DASHBOARD,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.DASHBOARD,
        showChat: true
    },
    studentDashboard: {
        type: "static",
        path: "/student/dashboard",
        roles: [UserRoleEnum.Student],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.DASHBOARD,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.DASHBOARD,
        showChat: true
    },
    adminDashboard: {
        type: "static",
        path: "/admin/dashboard",
        roles: [UserRoleEnum.Admin],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.DASHBOARD,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.DASHBOARD,
        showChat: true
    },
    companyDashboard: {
        type: "static",
        path: "/company/dashboard",
        roles: [UserRoleEnum.Company],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.DASHBOARD,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.DASHBOARD,
        showChat: true
    },
    forgotPassword: {
        type: "static",
        path: "/auth/forgotten-password",
        roles: undefined,
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.FORGOT_PASSWORD,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.FORGOT_PASSWORD,
        showChat: true
    },
    setPassword: {
        type: "static",
        path: "/auth/set-password",
        roles: undefined,
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.SET_PASSWORD,
        descriptionKey: undefined,
        showChat: true
    },
    resetPassword: {
        type: "static",
        path: "/auth/reset-password",
        roles: undefined,
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.SET_PASSWORD,
        descriptionKey: undefined,
        showChat: true
    },
    googleAuthCompany: {
        type: "static",
        path: "/auth/google/company",
        roles: undefined,
        titleKey: undefined,
        descriptionKey: undefined,
        showChat: false
    },
    facebookAuthCompany: {
        type: "static",
        path: "/auth/facebook/company",
        roles: undefined,
        titleKey: undefined,
        descriptionKey: undefined,
        showChat: false
    },
    seznamAuthCompany: {
        type: "static",
        path: "/auth/seznam/company",
        roles: undefined,
        titleKey: undefined,
        descriptionKey: undefined,
        showChat: false
    },
    googleAuthTutor: {
        type: "static",
        path: "/auth/google/tutor",
        roles: undefined,
        titleKey: undefined,
        descriptionKey: undefined,
        showChat: false
    },
    facebookAuthTutor: {
        type: "static",
        path: "/auth/facebook/tutor",
        roles: undefined,
        titleKey: undefined,
        descriptionKey: undefined,
        showChat: false
    },
    seznamAuthTutor: {
        type: "static",
        path: "/auth/seznam/tutor",
        roles: undefined,
        titleKey: undefined,
        descriptionKey: undefined,
        showChat: false
    },
    googleAuthStudent: {
        type: "static",
        path: "/auth/google/student",
        roles: undefined,
        titleKey: undefined,
        descriptionKey: undefined,
        showChat: false
    },
    facebookAuthStudent: {
        type: "static",
        path: "/auth/facebook/student",
        roles: undefined,
        titleKey: undefined,
        descriptionKey: undefined,
        showChat: false
    },
    seznamAuthStudent: {
        type: "static",
        path: "/auth/seznam/student",
        roles: undefined,
        titleKey: undefined,
        descriptionKey: undefined,
        showChat: false
    },
    confirmEmail: {
        type: "static",
        path: "/auth/confirm",
        roles: [UserRoleEnum.Admin, UserRoleEnum.Tutor, UserRoleEnum.Student, UserRoleEnum.Company],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.CONFIRM_YOUR_EMAIL,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.CONFIRM_YOUR_EMAIL,
        showChat: true
    },
    setRole: {
        type: "static",
        path: "/auth/set-role",
        roles: [UserRoleEnum.Admin, UserRoleEnum.Tutor, UserRoleEnum.Student, UserRoleEnum.Company],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.SET_ROLE,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.SET_ROLE,
        showChat: true
    },
    emailCodeVerification: {
        type: "static",
        path: "/auth/code-verification",
        roles: undefined,
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.EMAIL_CODE_VERIFICATION,
        descriptionKey: undefined,
        showChat: true
    },
    privacyPolicy: {
        type: "static",
        path: "/privacy",
        roles: undefined,
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.PRIVACY_POLICY,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.PRIVACY_POLICY,
        showChat: true
    },
    termsOfUse: {
        type: "static",
        path: "/terms-of-use",
        roles: undefined,
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.TERMS_OF_USE,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.TERMS_OF_USE,
        showChat: true
    },
    tutorLandingPage: {
        type: "static",
        path: "/tutor",
        roles: undefined,
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.BECOME_A_TUTOR,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.BECOME_A_TUTOR,
        showChat: true
    },
    tutorRegistrationForm: {
        type: "static",
        path: "/tutor/registration",
        roles: [UserRoleEnum.Tutor],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.REGISTRATION,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.REGISTRATION,
        showChat: false
    },
    companyRegistrationForm: {
        type: "static",
        path: "/company/registration",
        roles: [UserRoleEnum.Company],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.REGISTRATION,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.REGISTRATION,
        showChat: false
    },
    companyLandingPage: {
        type: "static",
        path: "/company",
        roles: undefined,
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.YASPEAK_BUSINESS,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.YASPEAK_BUSINESS,
        showChat: true
    },
    tutorVerification: {
        type: "static",
        path: "/tutor/verification",
        roles: [UserRoleEnum.Tutor],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.REGISTRATION,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.REGISTRATION,
        showChat: true
    },
    search: {
        type: "static",
        path: "/tutors/search",
        roles: undefined,
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.FIND_A_TUTOR,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.FIND_A_TUTOR,
        showChat: true
    },
    tutorSettingsPage: {
        type: "static",
        path: "/tutor/settings",
        roles: [UserRoleEnum.Tutor],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.SETTINGS,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.SETTINGS,
        showChat: false
    },
    studentSettingsPage: {
        type: "static",
        path: "/student/settings",
        roles: [UserRoleEnum.Student],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.SETTINGS,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.SETTINGS,
        showChat: false
    },
    companySettingsPage: {
        type: "static",
        path: "/company/settings",
        roles: [UserRoleEnum.Company],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.SETTINGS,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.SETTINGS,
        showChat: false
    },
    messagesPage: {
        type: "static",
        path: "/messages",
        roles: [UserRoleEnum.Admin, UserRoleEnum.Tutor, UserRoleEnum.Student],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.MESSAGES,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.MESSAGES,
        showChat: false
    },
    notFoundPage: {
        type: "static",
        path: "/404",
        roles: undefined,
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.PAGE_NOT_FOUND,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.PAGE_NOT_FOUND,
        showChat: false
    },
    reservationPaymentSuccess: {
        type: "static",
        path: "/payment/reservation/success",
        roles: [UserRoleEnum.Admin, UserRoleEnum.Tutor, UserRoleEnum.Student],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.PAYMENT_SUCCESS,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.PAYMENT_SUCCESS,
        showChat: false
    },
    moneyRefillSuccess: {
        type: "static",
        path: "/payment/money-refill/success",
        roles: [UserRoleEnum.Admin, UserRoleEnum.Tutor, UserRoleEnum.Student],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.PAYMENT_SUCCESS,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.PAYMENT_SUCCESS,
        showChat: false
    },
    notifications: {
        type: "static",
        path: "/notifications",
        roles: [UserRoleEnum.Admin, UserRoleEnum.Tutor, UserRoleEnum.Student],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.NOTIFICATIONS,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.NOTIFICATIONS,
        showChat: false
    },
    // Dynamic Paths
    tutorIdPage: {
        type: "dynamic",
        matchingPath: "/tutor/[...id]",
        path: (tutorId: number) => `/tutor/${tutorId}`,
        roles: undefined,
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.TUTOR_PUBLIC_PAGE,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.TUTOR_PUBLIC_PAGE,
        getPathParams: (pathName: string) => {
            const params = pathName.replace('/tutor/', '').split('/');
            return {
                subjectId: parseInt(params[0])
            }
        },
        showChat: false
    },
    subjectLandingPage: {
        type: "dynamic",
        matchingPath: "/subject/[id]",
        path: (subjectEnumKey: string) => `/subject/${subjectEnumKey}`,
        roles: undefined,
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.SUBJECT_PAGE,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.SUBJECT_PAGE,
        getPathParams: (pathName: string) => {
            const params = pathName.replace('/subject/', '').split('/');
            return {
                subject: params[0].charAt(0).toUpperCase() + params[0].slice(1, params[0].length)
            }
        },
        showChat: true
    },
    specializationLandingPage: {
        type: "dynamic",
        matchingPath: "/specialization/[id]",
        path: (specializationEnumKey: string) => `/specialization/${specializationEnumKey}`,
        roles: undefined,
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.SPECIALIZATION_PAGE,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.SPECIALIZATION_PAGE,
        getPathParams: (pathName: string) => {
            const params = pathName.replace('/specialization/', '').split('/');
            return {
                subject: params[0].charAt(0).toUpperCase() + params[0].slice(1, params[0].length)
            }
        },
        showChat: true
    },
    meetingPage: {
        type: "dynamic",
        matchingPath: "/meeting/[id]",
        path: (meetingKey: string) => `/meeting/${meetingKey}`,
        roles: [UserRoleEnum.Tutor, UserRoleEnum.Student, UserRoleEnum.Admin],
        titleKey: TRANSLATION_KEYS.PAGE_TITLE.MEETING_PAGE,
        descriptionKey: TRANSLATION_KEYS.PAGE_DESCRIPTION.MEETING_PAGE,
        getPathParams: (pathName: string) => {
            const params = pathName.replace('/meeting/', '').split('/');
            return {
                meetingId: params[0]
            }
        },
        showChat: false
    }
}

const isStaticPath = (path: BasicRouteType): path is StaticRouteType => {
    return path.type === "static";
}

const isDynamicPath = (path: BasicRouteType): path is DynamicRouteType => {
    return path.type === "dynamic";
}

export const getFromPathString = (path: string): BasicRouteType | undefined => {
    const staticPaths = Object.values(routes).filter(isStaticPath);
    const staticPathMacth = staticPaths.filter(x => x.path === path);
    if (staticPathMacth.length > 0) {
        return staticPathMacth[0];
    }

    const dynamicPaths = Object.values(routes).filter(isDynamicPath);
    const dynamicPathMatch = dynamicPaths.filter(x => path === x.matchingPath);
    if (dynamicPathMatch.length > 0) {
        return dynamicPathMatch[0];
    }

    return undefined;
}

export const isHomePage = (path: string): boolean => {
    return path === "/";
}

export const isPathAllowed = (pathString: string, userRole: UserRoleEnum | undefined): boolean => {
    try {
        const path = getFromPathString(pathString);
        if (!path || !path.roles) {
            return true;
        }
        return path?.roles?.some(x => x === userRole);
    } catch (err) {
        return false;
    }
}

export const isDynamicPage = (pathString: string): boolean => {
    try {
        const path = getFromPathString(pathString);
        if (!path) return false;
        return isDynamicPath(path);
    } catch (err) {
        return false;
    }
}

export const getTitleKey = (pathString: string): string => {
    try {
        const path = getFromPathString(pathString);
        return path?.titleKey ?? TRANSLATION_KEYS.PAGE_TITLE.HOME_PAGE;
    } catch (err) {
        return TRANSLATION_KEYS.PAGE_TITLE.HOME_PAGE;
    }
}

export const getDescriptionKey = (pathString: string): string => {
    try {
        const path = getFromPathString(pathString);
        return path?.descriptionKey ?? TRANSLATION_KEYS.PAGE_DESCRIPTION.HOME_PAGE;
    } catch (err) {
        return TRANSLATION_KEYS.PAGE_DESCRIPTION.HOME_PAGE;
    }
}

export const getParams = (pathString: string, windowPathName: string | undefined): any => {
    try {
        const path = getFromPathString(pathString);
        if (!path) return {};
        if (path?.type === "dynamic" && windowPathName) {
            const dynamicPath = path as DynamicRouteType;
            const paramsObj = dynamicPath.getPathParams(windowPathName);
            return paramsObj;
        }
        return {};
    } catch (err) {
        return {};
    }
}

export const parseStringQueryParams = <T>({ params, }: ServerSideProps, mapFunc: (id: string) => T): GetServerSidePropsResult<T> => {
    try {
        const parameter = params?.id;
        if (!parameter) {
            return { notFound: true }
        }
        return {
            props: mapFunc(parameter)
        }
    } catch (error) {
        return {
            notFound: true
        }
    }
}

export const getDefaultPage = (userRoleId: UserRoleEnum | undefined) => {

    switch (userRoleId) {
        case UserRoleEnum.Admin:
            return routes.adminDashboard.path;
        case UserRoleEnum.Student:
            return routes.studentDashboard.path;
        case UserRoleEnum.Tutor:
            return routes.tutorDashboard.path;
        case UserRoleEnum.Company:
            return routes.companyDashboard.path;
        default:
            return routes.home.path;
    }
}


export const getProfilePath = (userRoleId: UserRoleEnum | undefined) => {
    switch (userRoleId) {
        case UserRoleEnum.Admin:
            return undefined;
        case UserRoleEnum.Student:
            return routes.studentSettingsPage.path;
        case UserRoleEnum.Tutor:
            return routes.tutorSettingsPage.path;
        case UserRoleEnum.Company:
            return routes.companySettingsPage.path;
        default:
            return routes.home.path;
    }
}

export default routes;