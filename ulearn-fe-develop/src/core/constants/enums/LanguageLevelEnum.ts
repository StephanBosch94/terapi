import { COLORS } from "components/styles/constants/ColorsConstatnts";

export enum LanguageLevelEnum {
    A1 = 1,
    A2 = 2,
    B1 = 3,
    B2 = 4,
    C1 = 5,
    C2 = 6,
    NATIVE = 7
}

export const getLanguageLevelColor = (languageLevelEnum: LanguageLevelEnum) => {
    switch (languageLevelEnum) {
        case LanguageLevelEnum.A1:
            return COLORS.YELLOW_LIGHT
        case LanguageLevelEnum.A2:
            return COLORS.YELLOW_LIGHT
        case LanguageLevelEnum.B1:
            return COLORS.YELLOW_LIGHT
        case LanguageLevelEnum.B2:
            return COLORS.YELLOW_LIGHT
        case LanguageLevelEnum.C1:
            return COLORS.YELLOW_LIGHT
        case LanguageLevelEnum.C2:
            return COLORS.YELLOW_LIGHT
        case LanguageLevelEnum.NATIVE:
            return COLORS.BLUE_LIGHT
        default:
            return "inherit"
    }
}