import { paths } from "apiTypes";
import { TFunction } from "components/core/TranslationProvider";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { ClassDurationEnum } from "core/constants/enums/ClassDurationEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";

const getSaleMultiplyCoefficient = (numberOfLessons: number) => {
    if (numberOfLessons === 1) {
        return 1 - COMMON_CONSTANTS.PACKAGES.ONE_LESSON_SALE;
    }
    if (numberOfLessons === 5) {
        return 1 - COMMON_CONSTANTS.PACKAGES.FIVE_LESSONS_SALE;
    }
    if (numberOfLessons === 10) {
        return 1 - COMMON_CONSTANTS.PACKAGES.TEN_LESSONS_SALE;
    }
    return 1;
}


const getDiscountMultiplyCoefficient = (discountPercentage: number | undefined) => {
    return 1 - ((discountPercentage ?? 0) / 100);
}

const getDurationMultiplyCoefficient = (duration: ClassDurationEnum) => {
    return duration === ClassDurationEnum.Full ? 1 : 0.5;
}

export const toOptionalFixed = (num: number | undefined, digits: number) => Number.parseFloat((num ?? 0).toFixed(digits));
export const toOptionalPriceFixed = (num: number | undefined) => toOptionalFixed(num, 2);

const getFormattedPrice = (price: number | undefined, locale: string | undefined, showPlusSign: boolean) => {
    const priceNumber = toOptionalPriceFixed(price);
    const sign = showPlusSign && priceNumber > 0 ? "+" : "";
    return `${sign}${priceNumber.toLocaleString(locale)}`;
}

export const getPriceTranslation = (t: TFunction, price: number | undefined, currencyKey: string | null | undefined, locale: string | undefined, showPlusSign: boolean = false) => {
    const formattedPrice = getFormattedPrice(price, locale, showPlusSign);
    const currency = t(currencyKey ?? "");
    return t(TRANSLATION_KEYS.COMMON.PRICE_WITH_CURRENCY, { price: formattedPrice, currency })
}

export const getPrice = ({ hourRate, numberOfLessons, duration }: { hourRate: number | null | undefined, numberOfLessons: number, duration: ClassDurationEnum }) => {
    const durationCoefficient = getDurationMultiplyCoefficient(duration);
    const saleCoefficient = getSaleMultiplyCoefficient(numberOfLessons);
    return toOptionalPriceFixed(durationCoefficient * saleCoefficient * numberOfLessons * (hourRate ?? 0));
}

export const getTotalPrice = ({ hourRate, numberOfLessons, duration, couponDiscountPercentage }: { hourRate: number | null | undefined, numberOfLessons: number, duration: ClassDurationEnum, couponDiscountPercentage: number | undefined }) => {
    const durationCoefficient = getDurationMultiplyCoefficient(duration);
    const saleCoefficient = getSaleMultiplyCoefficient(numberOfLessons);
    const discountCoefficient = getDiscountMultiplyCoefficient(couponDiscountPercentage);
    const restLessonPrices = durationCoefficient * saleCoefficient * (numberOfLessons - 1)  * (hourRate ?? 0);
    const firstLessonPrice = durationCoefficient * saleCoefficient * 1 * (hourRate ?? 0) * discountCoefficient;
    return toOptionalPriceFixed(firstLessonPrice + restLessonPrices);
}

export const getDiscountAmount = ({ hourRate, numberOfLessons, duration, couponDiscountPercentage }: { hourRate: number | null | undefined, numberOfLessons: number, duration: ClassDurationEnum, couponDiscountPercentage: number | undefined }) => {
    const durationCoefficient = getDurationMultiplyCoefficient(duration);
    const saleCoefficient = getSaleMultiplyCoefficient(numberOfLessons);
    const discountCoefficient = getDiscountMultiplyCoefficient(couponDiscountPercentage);
    const firstLessonPrice = durationCoefficient * saleCoefficient * 1 * (hourRate ?? 0);
    const firstLessonPriceWithDiscount = firstLessonPrice * discountCoefficient;
    return toOptionalPriceFixed(firstLessonPrice - firstLessonPriceWithDiscount);
}

export const getInternalCreditsAmountToUse = (price: number, availableCredits: number | undefined, useInternalCredits: boolean) => {
    if (!availableCredits) return 0;
    if (!useInternalCredits) return 0;
    return availableCredits > price ? price : availableCredits;
}

export const getAmountToPay = (price: number, creditsToUse: number) => {
    return price - creditsToUse;
}

export const formatCurrency = (currency?: paths['/api/codetable/currency']['get']['responses']['200']['content']['application/json'][0]) => {
    return `${(currency?.enumKey ?? "").toUpperCase()} ${currency?.symbol ?? ""}`
}