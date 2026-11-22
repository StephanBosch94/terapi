import ButtonUnstyled from "@mui/base/ButtonUnstyled";
import styled from "@mui/system/styled";
import { nameof } from "core/services/Types/AppTypes";
import { COLORS } from "../constants/ColorsConstatnts";

type ListItemSpecializaitonType = {
    active?: boolean
}
const ItemStateButtonStyled = styled(ButtonUnstyled, {
    shouldForwardProp: (prop) => prop !== nameof<ListItemSpecializaitonType>('active')
})<ListItemSpecializaitonType>(({ active }) => ({
    width: "auto",
    backgroundColor: active ? COLORS.PRIMARY : COLORS.WHITE,
    color: active ? COLORS.WHITE : COLORS.BLACK,
    borderRadius: "24px",
    padding: "12px 24px",
    display: "flex",
    border: `1px solid ${active ? COLORS.PRIMARY : COLORS.BLACK_10}`,
    alignItems: "center",
    gap: "12px",
    fontFamily: "inherit",
    ':hover': {
        cursor: "pointer",
        border: `1px solid ${COLORS.PRIMARY}`
    },
}));

export default ItemStateButtonStyled;