import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";
import { COLORS } from "../constants/ColorsConstatnts";
import { nameof } from "core/services/Types/AppTypes";

type MenuItemPropsType = {
    disabled?: boolean
}

export const MenuItemBoxStyled = styled(Box, {
    shouldForwardProp: (prop) => prop !== nameof<MenuItemPropsType>('disabled')
})<MenuItemPropsType>(({ disabled }) => ({
    display: "inline-flex",
    padding: "6px 12px",
    alignItems: "center",
    gap: "8px",
    width: "100%",
    ":hover": {
        cursor: !disabled ? "pointer" : "auto",
        backgroundColor: !disabled ? COLORS.PRIMARY_5 : "transparent"
    }
}));