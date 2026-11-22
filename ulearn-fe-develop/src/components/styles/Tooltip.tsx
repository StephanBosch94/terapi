import { styled } from "@mui/material/styles";
import { COLORS } from "./constants/ColorsConstatnts";
import { LightTooltipStyled } from "./boxes/LightTooltipStyled";
import { ReactNode } from "react";
import Body1Styled from "./typography/Body1Styled";

const TooltipTypographyStyled = styled(Body1Styled)({
    color: COLORS.BLACK
}) as typeof Body1Styled;

const Tooltip = ({ children, text }: { children: any, text: NonNullable<ReactNode> }) => {

    return (
        <LightTooltipStyled
            placement="top"
            title={<TooltipTypographyStyled>{text}</TooltipTypographyStyled>}
        >
            {children}
        </LightTooltipStyled>
    )
}

export default Tooltip;