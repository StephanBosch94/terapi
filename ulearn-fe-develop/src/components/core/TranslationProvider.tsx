import { setLanguage } from "core/moment";
import { contextFactoryCreate } from "core/redux/reduxHooks";
import { TranslationsType } from "core/services/Types/AppTypes";
import { createContext, useEffect } from "react";

type ParamType = { [key: string]: string | number | undefined | null }
export type TFunction = (key: string, params?: ParamType) => string;
type TranslationsContextType = {
    t: TFunction,
    tContains: (key: string) => boolean,
    currLangCode: string
}

const TranslationContext = createContext<TranslationsContextType | undefined>(undefined);
export const useTranslation = contextFactoryCreate('Translation', TranslationContext);

export const translationConfig = {
    fallbackCode: 'en',
    suffixes: {
        countParamName: 'count',
        zero: '_zero',
        one: '_one',
        few: '_few',
        many: `_many`
    }
}

const getKeyPlaceholder = (key: string) => `{{${key}}}`;

const insertParams = (translation: string, params: ParamType) => {
    let result = String(translation);
    for (const [key, value] of Object.entries(params)) {
        result = result.replaceAll(getKeyPlaceholder(key), `${value}`);
    }
    return result;
}

const getKeyBasedOnParams = (key: string, params?: ParamType) => {
    if (!params) return key;
    const countParam = Object.entries(params).find(x => x[0] === translationConfig.suffixes.countParamName);
    if (!countParam) return key;
    const count = countParam[1] as number | undefined;
    if (count === undefined) return key;
    if (count === 0) return `${key}${translationConfig.suffixes.zero}`;
    if (count === 1) return `${key}${translationConfig.suffixes.one}`;
    if (count > 1 && count < 5) return `${key}${translationConfig.suffixes.few}`
    if (count >= 5) return `${key}${translationConfig.suffixes.many}`;
    return key;
}

const TranslationsProvider = ({ children, locale, translations }: { children: any, locale: string | undefined, translations?: TranslationsType | undefined }) => {
    const lang = locale ?? translationConfig.fallbackCode;
    setLanguage(lang); // cannot be inside of useEffect

    const t = (key: string, params?: ParamType): string => {
        const suffixKey = getKeyBasedOnParams(key, params)
        if (!translations) return key;
        if (!translations[lang]) return key;
        const translation = translations[lang][suffixKey]
            ?? translations[lang][key]
            ?? key;
        if (!params) return translation;

        return insertParams(translation, params);
    }

    const tContains = (key: string): boolean => {
        const translation = t(key);
        return translation !== key;
    }

    return (
        <TranslationContext.Provider value={{
            t,
            tContains,
            currLangCode: lang
        }}>
            {children}
        </TranslationContext.Provider>
    )
}

export default TranslationsProvider;