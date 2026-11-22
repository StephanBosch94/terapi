import Box from "@mui/material/Box";
import { PopoverOrigin } from "@mui/material/Popover";
import { SxProps } from "@mui/material/styles";
import { MenuStyled } from "components/styles/MenuStyled";
import { useState, useCallback } from "react";

export type MenuPropsType = {
    content: JSX.Element,
    innerMenu: JSX.Element,
    sx?: SxProps,
    autoClose?: boolean,
    transformOrigin?: PopoverOrigin | undefined,
    anchorOrigin?: PopoverOrigin | undefined,
    onClick?: (newValue: boolean) => void,
    sameWidths?: boolean,
    fullHeight?: boolean
};
const Menu = ({
    content,
    innerMenu,
    sx,
    autoClose = false,
    transformOrigin = { horizontal: 'right', vertical: 'top' },
    anchorOrigin = { horizontal: 'right', vertical: 'bottom' },
    onClick = () => { },
    sameWidths = false,
    fullHeight = true
}: MenuPropsType) => {
    const [parentWidth, setParentWidth] = useState(0);
    const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
    const parentRefCallback = useCallback((node: HTMLElement | null) => {
        if (node !== null) {
            setParentWidth(node.clientWidth);
        }
    }, []);

    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
        onClick(true);
    };

    const handleClose = () => {
        setAnchorEl(null);
        onClick(false);
    };

    const handleMenuClick = () => {
        if (!autoClose) return;
        handleClose();
    }

    return (
        <>
            <Box
                ref={parentRefCallback}
                onClick={handleClick}
                sx={sx}
            >
                {content}
            </Box>
            <MenuStyled
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClick={handleMenuClick}
                onClose={handleClose}
                transformOrigin={transformOrigin}
                anchorOrigin={anchorOrigin}
                marginThreshold={18}
                sx={{
                    marginTop: "4px"
                }}

            >
                <Box sx={{
                    width: sameWidths ? parentWidth : "auto",
                    maxHeight: fullHeight ? "auto" : "500px"
                }}>
                    {innerMenu}
                </Box>
            </MenuStyled>
        </>
    )
}

export default Menu;