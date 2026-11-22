import { styled } from "@mui/material/styles";
import { COLORS } from '../constants/ColorsConstatnts';
import Tooltip, { TooltipProps, tooltipClasses } from "@mui/material/Tooltip";

export const LightTooltipStyled = styled(({ className, ...props }: TooltipProps) => (
    <Tooltip {...props} classes={{ popper: className }} />
))(({ theme }) => ({
    [`& .${tooltipClasses.tooltip}`]: {
        backgroundColor: `${COLORS.WHITE} !important`,
        boxShadow: "2px 3px 6px rgba(0, 0, 0, 0.15), -1px -1px 6px rgba(0, 0, 0, 0.08)",
        borderRadius: "4px",
        padding: "12px",
        color: COLORS.BLACK
    },
}));