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

export const setLocaleCookie = (locale: string) => {
  document.cookie = `NEXT_LOCALE=${locale}; max-age=31536000; path=/; SameSite=Lax`
}

const LanguageMenu = () => {
  const [open, setOpen] = useState(false);
  const { languages, currentLanguage } = useAppDataContext();
  const [languageCode, setLanguageCode] = useState(currentLanguage?.languageCode);
  const selectedLanguage = languages.find(x => x.languageCode === languageCode);
  const allExceptCurrent = useMemo(() => getAllExceptSelected(languages, languageCode), [languages, languageCode]);

  const dispatch = useAppDispatch();
  const router = useRouter();

  useEffect(() => {
    setLanguageCode(currentLanguage.languageCode)
  }, [currentLanguage]);

  const isUserAuthorized = useAppSelector(selectIsAuthorized);

  const handleMenuItemClick = (
    newLanguageCode: string
  ) => {
    if (languageCode === newLanguageCode) return;

    setLanguageCode(newLanguageCode);
    setLocaleCookie(newLanguageCode);

    if (isUserAuthorized) {
      dispatch(setChangeLanguage({ languageCode: newLanguageCode }));
    }

    location.replace(getFullUrlWithLocale(newLanguageCode, router.asPath));
  };

  return (
    <Box>
      <Menu
        content={
          <LangugageDropdownBoxStyled>
            <IconStyledBox><LanguageIcon /></IconStyledBox>
            <ListItemTextStyled primary={selectedLanguage?.language ?? ""} />
            {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
          </LangugageDropdownBoxStyled>
        }
        innerMenu={
          <MenuBoxStyled gap="4px" >
            {allExceptCurrent.map((language, index) => (
              <MenuItem
                key={index}
                onClick={() => handleMenuItemClick(language.languageCode!)}
                text={language.language ?? ""}
              />
            ))}
          </MenuBoxStyled >
        }
        sameWidths={true}
        autoClose={true}
        onClick={(newValue: boolean) => setOpen(newValue)}
      />
    </Box>
  )
}

export default LanguageMenu;
