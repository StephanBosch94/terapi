import { LanguageType } from "../Types/AppTypes";

const HIDDEN_LANGUAGE_CODES = new Set(["cs"]);

export const getAvailableLanguages = <T extends { languageCode?: string | null }>(all: T[]): T[] => {
    return all.filter(x => !x.languageCode || !HIDDEN_LANGUAGE_CODES.has(x.languageCode));
}

export const getAllExceptSelected = (all: LanguageType[], currentLanguageCode: string | null | undefined) => {
    return getAvailableLanguages(all).filter(x => x.languageCode !== currentLanguageCode);
}