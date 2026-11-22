import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "./constants/ColorsConstatnts";
import CheckIcon from "assets/check.svg";

const CheckBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    alignItems: "center",
    justifyContent: "center",
    /* Hide the default radio button */
    input: {
        margin: 0,
        position: "absolute",
        opacity: 0,
        cursor: "pointer"
    },
    "div": {
        display: "inline-flex",
        alignItems: "center",
        justifyContent: "center",
        padding: "2px",
        width: "16px",
        height: "16px",
        borderRadius: "2px",
        color: COLORS.WHITE,
    },
    ".checked": {
        backgroundColor: COLORS.PRIMARY
    },
    ".unchecked": {
        border: `2px solid ${COLORS.BLACK_10}`
    },
    "svg": {
        scale: "transform(0.75)"
    }
})) as typeof Box;

const CheckBox = ({ checked }: { checked: boolean }) => {

    return (
        <CheckBoxStyled>
            <input type="checkbox" checked={checked} readOnly/>
            <div className={checked ? "checked" : "unchecked"}>
                {checked ? <CheckIcon /> : <></>}
            </div>
        </CheckBoxStyled>
    )
}

export default CheckBox;