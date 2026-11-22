import { styled, experimental_sx as sx } from "@mui/material/styles";
import KeyboardArrowDownIcon from 'assets/arrow-down.svg';
import KeyboardArrowUpIcon from "assets/arrow-up.svg";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { useRouter } from 'next/router';
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import LanguageIcon from "assets/language.svg"
import { getAllExceptSelected } from 'core/services/Utils/languageUtil';
import { MenuBoxStyled } from 'components/styles/boxes/MenuBoxStyled';
import MenuItem from "components/menu/MenuItem";
import { useAppDataContext } from 'components/core/providers/AppProvider';
import { setChangeLanguage } from 'core/redux/userSlice';
import { selectIsAuthorized } from 'core/redux/authSlice';
import Box from "@mui/material/Box";
import ListItemText from "@mui/material/ListItemText";
import { useEffect, useMemo, useState } from "react";
import Menu from "components/menu/Menu";
import { getFullUrlWithLocale } from "core/services/Utils/urlUtil";
import Cookies from "js-cookie";
import { cookieConstants } from "core/constants/cookieConstants";
import { paths } from "apiTypes";
import { formatCurrency } from "core/services/Utils/walletUtil";

const ListItemTextStyled = styled(ListItemText)(() => sx({
    span: {
        fontSize: "0.875rem",
        fontWeight: 500,
    },
    margin: 0,
    padding: 0,
    color: COLORS.BLACK,
}));

const IconStyledBox = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
}))

const LangugageDropdownBoxStyled = styled(Box)(() => sx({
    padding: "6px 8px",
    backgroundColor: COLORS.WHITE,
    color: COLORS.BLACK,
    fontSize: 14,
    fontWeight: 500,
    cursor: "pointer",
    display: "flex",
    gap: "8px",
    alignItems: "center",
    justifyContent: "center",
    borderRadius: 1,
    transition: "none",
    "&:hover": {
        backgroundColor: COLORS.PRIMARY_5,
        cursor: "pointer",
    }
})) as typeof Box;

export const setLocaleCookie = (newCurrencyCode: string) => {
    Cookies.set(cookieConstants.currencyCode, newCurrencyCode);
}

const CurrencyMenu = () => {
    const [open, setOpen] = useState(false);
    const { currencies, currentCurrency, currencyCode, changeCurrency } = useAppDataContext();
    const allExceptCurrent = currencies.filter(x => x.enumKey !== currencyCode);

    const handleMenuItemClick = (newCurrencyCode: string) => {
        if (currencyCode === newCurrencyCode) return;

        changeCurrency(newCurrencyCode);
        setLocaleCookie(newCurrencyCode);
    };


    return (
        <Box>
            <Menu
                content={
                    <LangugageDropdownBoxStyled>
                        <ListItemTextStyled primary={(currentCurrency?.enumKey ?? "").toUpperCase()} />
                        {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
                    </LangugageDropdownBoxStyled>
                }
                innerMenu={
                    <MenuBoxStyled gap="4px" sx={{ minWidth: "auto" }}>
                        {allExceptCurrent.map((x, index) => (
                            <MenuItem
                                key={index}
                                onClick={() => handleMenuItemClick(x.enumKey!)}
                                text={formatCurrency(x)}
                            />
                        ))}
                    </MenuBoxStyled >
                }
                autoClose={true}
                onClick={(newValue: boolean) => setOpen(newValue)}
            />
        </Box>
    )
}

export default CurrencyMenu;
