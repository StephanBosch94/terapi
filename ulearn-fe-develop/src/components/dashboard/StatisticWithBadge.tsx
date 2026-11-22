import Box from "@mui/material/Box";
import { styled } from "@mui/material/styles";
import Body1Styled from "components/styles/typography/Body1Styled";
import H3Styled from "components/styles/typography/H3Styled";
import StatisticBadge from "./StatisticBadge";
import StatisticsColumnBoxStyled from "components/styles/boxes/StatisticsColumnBoxStyled";

const StatisticsInlineBoxStyled = styled(Box)({
    display: "inline-flex",
    alignItems: "center",
    gap: "4px"
}) as typeof Box;

const StatisticWithBadge = ({ titleText, valueText, previousValue, currentValue }: { titleText: string, valueText: any, previousValue: number | undefined | null, currentValue: number | undefined | null }) => (
    <StatisticsColumnBoxStyled>
        <Body1Styled>{titleText}</Body1Styled>
        <StatisticsInlineBoxStyled>
            <H3Styled>{valueText}</H3Styled>
            <StatisticBadge previousValue={previousValue} currentValue={currentValue} />
        </StatisticsInlineBoxStyled>
    </StatisticsColumnBoxStyled>
)

export default StatisticWithBadge;