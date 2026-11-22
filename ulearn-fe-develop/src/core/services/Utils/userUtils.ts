import { AppUserReview, AppUserOverviewVM, ClassDurationType } from "../Types/AppTypes"
import { LanguageLevelEnum } from "core/constants/enums/LanguageLevelEnum";
import { ClassDurationEnum } from "core/constants/enums/ClassDurationEnum";
import { AttributeTypeEnum } from "core/constants/enums/AttributeTypeEnum";
import { TFunction } from "components/core/TranslationProvider";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { czkTranslationKey } from "core/constants/currencyContainst";

export const getUserName = (userVm: { firstName?: string | null | undefined, lastName?: string | null | undefined, email?: string | null | undefined } | undefined) => {
    if (userVm?.firstName || userVm?.lastName) {
        return `${userVm?.firstName ?? ""} ${userVm?.lastName ?? ""}`;
    }
    return COMMON_CONSTANTS.DEFAULT_USER_NAME;
}

export const getUserNameOrEmail = (userVm: { firstName?: string | null | undefined, lastName?: string | null | undefined, email?: string | null | undefined } | undefined) => {
    if (userVm?.firstName || userVm?.lastName) {
        return getUserName(userVm);
    }
    if (userVm?.email) {
        return `${userVm?.email ?? ""}`;
    }
    return COMMON_CONSTANTS.DEFAULT_USER_NAME;
}

export const getFirstNameOrCompanyName = (userVm: { firstName?: string | null | undefined, companyName?: string | null | undefined } | undefined) => {
    if (userVm?.firstName) {
        return userVm.firstName;
    }
    if (userVm?.companyName) {
        return userVm?.companyName;;
    }
    return undefined;
}

export const getShortName = (userVm: { firstName?: string | null | undefined, lastName?: string | null | undefined } | undefined) => {
    return `${userVm?.firstName} ${userVm?.lastName?.slice(0, 1)}.`;
}

export const getLanguageAndLevel = (userVm: AppUserOverviewVM, t: TFunction) => {
    return userVm.languages
        ?.map((language, idx) => `${t(language.language?.nameKey ?? "")} (${t(language.level?.nameKey ?? "")})`
        ).join(", ")
}

export const isNativeSubject = (userVm: AppUserOverviewVM) => {
    const subjectKey = userVm.subject?.enumKey;
    return userVm.languages
        ?.filter(x => x.level && x.level?.languageLevelId === LanguageLevelEnum.NATIVE)
        .some(x => x.language?.enumKey === subjectKey);
}

export const getUserAttribute = (userVm: AppUserOverviewVM, attributeTypeId: number) => {
    return userVm.attributes?.find(x => x.attributeType?.attributeTypeId === attributeTypeId);
}

export const getUserRating = (userVm: AppUserOverviewVM) => {
    return userVm?.rating?.toFixed(1) ?? 5.0;
}

export const getPriceWithSale = (hourRate: number, sale: number, duration: ClassDurationEnum): number => {
    const durationCoefficied = duration === ClassDurationEnum.Full ? 1 : 0.5;
    const baseRate = durationCoefficied * hourRate;
    return baseRate - baseRate * sale;
}

export const getDurationInMinutes = (duration: ClassDurationEnum) => duration === ClassDurationEnum.Full ? 50 : 25;

export const getTrialPrice = (hourRate: number | null | undefined) => getPriceWithSale(hourRate ?? 0, 0, ClassDurationEnum.Half);

const getShortenAttributeIfRequired = (val: string | undefined | null, take: number) => {
    if (!take || !val) return "";
    if (val.length > take) return `${val?.slice(0, take)}...`;
    return val;
}

export const getShortenDescription = (userVm: AppUserOverviewVM, maxLength: number) => {
    const description = userVm.attributes?.find(x => x.attributeType?.attributeTypeId === AttributeTypeEnum.Description);
    const shorten = getShortenAttributeIfRequired(description?.value, maxLength);
    return shorten;
}

export const getDescription = (userVm: AppUserOverviewVM) => {
    return userVm.attributes?.find(x => x.attributeType?.attributeTypeId === AttributeTypeEnum.Description)?.value;
}

export const getReviewPairs = (reviews: (AppUserReview[] | null | undefined)): (AppUserReview[][] | undefined | null) => {
    return reviews?.reduce((acc, curr) => acc.length && acc[acc.length - 1].length === 1
        ? [...acc.slice(0, acc.length - 1), [acc[acc.length - 1][0], curr]]
        : [...acc, [curr]]
        , [] as AppUserReview[][])
}

export const getRatingArray = (rating: number | undefined | null): ("filled" | "half_filled" | "empty")[] => {
    const MAX_POSSIBLE_RATING = 5;

    return Array(MAX_POSSIBLE_RATING).fill(1)
        .map((x, idx) => x + idx)
        .map((x, idx) => {
            if (rating && x <= rating) return "filled";
            if (rating && (x - 0.5) <= rating) return "half_filled";
            return "empty";
        })
}

