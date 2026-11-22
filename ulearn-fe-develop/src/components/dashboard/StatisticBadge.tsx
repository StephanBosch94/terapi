import Box from "@mui/material/Box";
import { styled } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { nameof } from "core/services/Types/AppTypes";

type BadgeBoxType = {
    score: number
}
const BadgeBoxStyled = styled(Box, {
    shouldForwardProp: (prop) => prop !== nameof<BadgeBoxType>('score')
})<BadgeBoxType>(({ score }) => ({
    display: "flex",
    backgroundColor: getBgColor(score),
    padding: "2px 6px",
    alignItems: "center",
    justifyContent: "center",
    borderRadius: "30px"
}));

const BadgeTypohgraphyStyled = styled(CaptionTextStyled)<BadgeBoxType>(({ score }) => ({
    color: getColor(score),
}));

const getStatisticScore = (previousValue: number | undefined | null, currentValue: number | undefined | null) => {
    if (!previousValue) {
        return (currentValue ?? 0) * 100;
    }
    return (((currentValue ?? 0) - (previousValue ?? 0))) * 100 / previousValue;
}

const getBgColor = (score: number) => {
    const rounded = Math.round(score);
    if (rounded === 0) {
        return COLORS.PRIMARY_10;
    }
    if (rounded > 0) {
        return COLORS.GREEN_LIGHT;
    }
    return COLORS.RED_10;
}
const getColor = (score: number) => {
    const rounded = Math.round(score);
    if (rounded === 0) {
        return COLORS.PRIMARY;
    }
    if (rounded > 0) {
        return COLORS.GREEN;
    }
    return COLORS.RED;
}

const getStatisticSign = (score: number) => {
    const rounded = Math.round(score);
    if (rounded === 0) {
        return "";
    }
    if (rounded > 0) {
        return "+";
    }
    return "";
}

const getFormattedScore = (score: number, thousandTranslation: string) => {
    if (score > 1000 || score < -1000) {
        return `${Math.round(score / 1000)} ${thousandTranslation}`;
    }
    return `${score}`;
}

const getStatisticText = (score: number, thousandTranslation: string) => {
    const sign = getStatisticSign(score);
    const rounded = Math.round(score);
    const value = getFormattedScore(rounded, thousandTranslation);
    return `${sign} ${value} %`;
}

const StatisticBadge = ({ previousValue, currentValue }: { previousValue: number | undefined | null, currentValue: number | undefined | null }) => {
    const score = getStatisticScore(previousValue, currentValue);
    const { t } = useTranslation();
    return previousValue ? (
        <BadgeBoxStyled score={score}>
            <BadgeTypohgraphyStyled score={score}>
                {getStatisticText(score, t(TRANSLATION_KEYS.DASHBOARD.STATISTIC_THOUSANDS))}
            </BadgeTypohgraphyStyled>
        </BadgeBoxStyled>
    ) : <></>
}

export default StatisticBadge;