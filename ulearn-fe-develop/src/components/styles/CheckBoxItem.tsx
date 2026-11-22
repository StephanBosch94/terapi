import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "./constants/ColorsConstatnts";
import { useState } from "react";
import CheckBox from "./CheckBox";

const CheckBoxItemStyled = styled(Box)(() => sx({
    width: "auto",
    display: "inline-flex",
    gap: "8px",
    alignItems: "center",
    ":hover": {
        cursor: "pointer"
    },
    ":hover .unchecked":{
        border: `2px solid ${COLORS.PRIMARY}`
    },
    ":hover .checked":{
        backgroundColor: COLORS.PRIMARY_HOVER
    }
})) as typeof Box;

const CheckBoxItem = ({ defaultChecked = true, onClick, children }: { defaultChecked: boolean, onClick: () => void, children: any }) => {
    const [ checked, setChecked ] = useState(defaultChecked);
    
    const handleChick = () => {
        setChecked(prev => !prev);
        onClick();
    }

    return (
        <CheckBoxItemStyled onClick={handleChick}>
            <CheckBox checked={checked} />
            {children}
        </CheckBoxItemStyled>
    )
}

export default CheckBoxItem;