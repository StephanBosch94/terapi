import { styled, experimental_sx as sx } from "@mui/material/styles";
import { MenuItemBoxStyled } from "components/styles/boxes/MenuItemBoxStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";

const MenuItemTypographyStyled = styled(CaptionTextStyled)({
    fontWeight: 500,
    color: COLORS.BLACK,
    fontSize: "0.875rem",
}) as typeof CaptionTextStyled;

export type SizeType = "large" | "normal";
type ItemBoxInputPropType = {
    size: SizeType
}

const ItemBoxStyled = styled(MenuItemBoxStyled)<ItemBoxInputPropType>(({ size }) => ({
    padding: size === "normal" ? "4px 12px" : "8px 12px"
}));

const MenuItem = ({ text, icon = null, onClick, size = "normal" }: { text: string, icon?: JSX.Element | null, onClick?: () => void, size?: SizeType }) => (
    <ItemBoxStyled onClick={onClick} size={size}>
        {icon}
        <MenuItemTypographyStyled>{text}</MenuItemTypographyStyled>
    </ItemBoxStyled >
)

export default MenuItem;