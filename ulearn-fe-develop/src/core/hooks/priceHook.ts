import { useTranslation } from "components/core/TranslationProvider";
import { useAppDataContext } from "components/core/providers/AppProvider";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { ClassDurationEnum, getDurationInMinutes } from "core/constants/enums/ClassDurationEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { getTrialPrice } from "core/services/Utils/userUtils";

const toOptionalFixed = (num: number | null | undefined, digits: number) => Number.parseFloat((num ?? 0).toFixed(digits));
const toOptionalPriceFixed = (num: number | null | undefined) => toOptionalFixed(num, 2);

export const usePriceLocalization = () => {
    const { locale, currencies, currentCurrency } = useAppDataContext();
    const { t } = useTranslation();

    const localizeTrial = (props: { price: number | null | undefined }) => {
        const converted = convertToUserCurrency(props);
        const trialPrice = getTrialPrice(converted);
        return format({ price: trialPrice });
    }

    const localize = (props: { price: number | null | undefined, translationKey?: string, duration?: ClassDurationEnum, showPlusSign?: boolean, round?: boolean }) => {
        return format({ ...props, price: convertToUserCurrency({ price: props.price }) });
    }

    const convertToUserCurrency = ({ price }: { price: number | null | undefined }) => {
        const internalCurrency = currencies.find(x => x.enumKey === COMMON_CONSTANTS.INTERNAL_CURRENCY_CODE);
        const priceResolved = price ?? 0;
        return priceResolved * (currentCurrency.rate ?? 1) / (internalCurrency?.rate ?? 1);
    }

    const convertToInternalCurrency = ({ price }: { price: number | null | undefined }) => {
        const internalCurrency = currencies.find(x => x.enumKey === COMMON_CONSTANTS.INTERNAL_CURRENCY_CODE);
        const priceResolved = price ?? 0;

        return priceResolved * (internalCurrency?.rate ?? 1) / (currentCurrency.rate ?? 1);
    }

    const formatPrice = ({ price, showPlusSign, round = true }: { price: number | null | undefined, showPlusSign?: boolean, round?: boolean }) => {
        const roundedPrice = Math.round(price ?? 0);
        const rest = !round ? ((price ?? 0) - roundedPrice) : 0;
        const restResolved = Math.abs(toOptionalPriceFixed(rest));
        const sign = showPlusSign && roundedPrice > 0 ? "+" : "";
        const roundedPriceString = roundedPrice.toLocaleString(locale).replace(/[.,]/, ' ');
        const restString = restResolved > 0 ? restResolved.toLocaleString()?.replace(/0/, '') : ""
        return sign + roundedPriceString + restString;
    }

    const format = ({ price, translationKey, duration, showPlusSign, round }: { price: number | null | undefined, translationKey?: string, duration?: ClassDurationEnum, showPlusSign?: boolean, round?: boolean }) => {
        const resolvedDuration = duration ?? ClassDurationEnum.Full;
        const durationInMinutes = getDurationInMinutes(resolvedDuration);

        return t(translationKey ?? TRANSLATION_KEYS.COMMON.PRICE_WITH_CURRENCY, {
            price: formatPrice({ price, showPlusSign, round }),
            currency: currentCurrency?.symbol,
            duration: durationInMinutes
        });
    }

    return { localize, localizeTrial, convertToUserCurrency, convertToInternalCurrency };
}