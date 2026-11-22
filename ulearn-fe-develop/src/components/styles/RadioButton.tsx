import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";
import { COLORS } from "./constants/ColorsConstatnts";
import { nameof } from "core/services/Types/AppTypes";

type SizeType = "regular" | "large";
type RadioBoxPropsType = {
    nonActiveColor: string,
    size: SizeType
}

const RadioBoxStyled = styled(Box, {
    shouldForwardProp: (prop) => prop !== nameof<RadioBoxPropsType>('nonActiveColor') && prop !== nameof<RadioBoxPropsType>('size')
})<RadioBoxPropsType>(({ nonActiveColor, size }) => ({
    display: "inline-flex",
    alignItems: "flex-start",
    /* Hide the default radio button */
    input: {
        margin: 0,
        position: "absolute",
        opacity: 0,
        cursor: "pointer"
    },
    ".outerCheck": {
        padding: "2px",
        width: size === "regular" ? "16px" : "18px",
        height: size === "regular" ? "16px" : "18px",
        borderRadius: "50%",
        border: `2px solid ${nonActiveColor}`,
        backgroundColor: COLORS.WHITE
    },
    ".check": {
        display: "none",
        height: "100%",
        width: "100%",
        borderRadius: "50%"
    },
    "input:checked ~ .outerCheck": {
        border: `1px solid ${COLORS.PRIMARY} !important`
    },
    "input:checked ~ * .check": {
        display: "block",
        backgroundColor: COLORS.PRIMARY
    }
}));

const RadioButton = ({ checked, nonActiveColor = COLORS.BLACK_10, size = "regular" }: { checked: boolean, nonActiveColor?: string, size?: SizeType }) => (
    <RadioBoxStyled nonActiveColor={nonActiveColor} size={size} >
        <input type="radio" checked={checked} readOnly />
        <span className="outerCheck">
            <span className="check" />
        </span>
    </RadioBoxStyled>
)

export default RadioButton;
