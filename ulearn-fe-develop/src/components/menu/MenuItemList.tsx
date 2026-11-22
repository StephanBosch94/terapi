import { useTranslation } from "components/core/TranslationProvider";
import { useMemo } from "react";
import MenuItem, { SizeType } from "./MenuItem";
import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";

const CheckboxStyled = styled(Box)(() => sx({
    maxHeight: "400px",
    overflow: "auto",
    padding: "8px"
})) as typeof Box;

type CodetableType = { id: number | undefined, nameKey: string | null | undefined };

const MenuItemsList = ({ items, onClick, size }: { items: CodetableType[] | undefined, onClick: (id: number) => void, size?: SizeType }) => {
    const { t } = useTranslation();
    const itemsTranslated = useMemo(() => items?.map(x => ({ ...x, translation: t(x.nameKey ?? "") }))?.sort((x, y) => {
        const xLower = x.translation.toLowerCase();
        const yLower = y.translation.toLowerCase();
        if (xLower < yLower) return -1;
        if (xLower > yLower) return 1;
        return 0;
    }), [items]);

    return (
        <CheckboxStyled>
            {itemsTranslated?.map((x, idx) => <MenuItem key={x.id!} text={x.translation} onClick={() => onClick(x.id!)} size={size} />)}
        </CheckboxStyled>
    )
}

export default MenuItemsList;