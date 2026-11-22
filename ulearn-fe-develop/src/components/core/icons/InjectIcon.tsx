import { CSSProperties } from "react";
import InjectSVG from "./InjectSVG";
import Skeleton from "@mui/material/Skeleton";
import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";
import { COLORS } from "components/styles/constants/ColorsConstatnts";

const fallbackSizePx = 16;
const FallbackComponent = styled(Box)(() => ({
    backgroundColor: COLORS.BLACK_10,
    width: `${fallbackSizePx}px`,
    height: `${fallbackSizePx}px`
})) as typeof Box;

export type IconCategoryType = "interests" | "flags" | "emoji" | "specializations" | "subjectLevels";

type PropsType = {
    category: IconCategoryType,
    iconName: string;
    FallbackComponent?: React.ElementType;
    size?: number;
    LoadingComponent?: React.ElementType;
    style?: CSSProperties;
    className?: string;
    injectedStyles?: string;
};

const getSrc = (props: { category: IconCategoryType, iconName: string }) => {
    return `/images/icons/${props.category}/${props.iconName}.svg`;
}

const getSizeStyles = (props: PropsType): CSSProperties => (props.size ? {
    display: "flex",
    alignItems: "center",
    width: `${props.size}px`,
    height: `${props.size}px`
} : {})

const InjectIcon = (props: PropsType) => (
    <InjectSVG
        httpRequestWithCredentials
        src={getSrc(props)}
        beforeInjection={(svg) => {
            svg.setAttribute('style', `${props.injectedStyles ?? ''}`);
        }}
        loading={
            props.LoadingComponent ??
            (() => (
                <Skeleton
                    variant="rectangular"
                    width={props.size ?? fallbackSizePx}
                    height={props.size ?? fallbackSizePx}
                    sx={{ borderRadius: "4px" }}
                />
            ))
        }
        fallback={props.FallbackComponent ?? FallbackComponent}
        style={{
            ...getSizeStyles(props),
            ...props.style
        }}
    />
)

export default InjectIcon;