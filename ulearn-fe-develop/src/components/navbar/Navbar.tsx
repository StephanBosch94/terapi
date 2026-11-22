import routes, { getDefaultPage, getProfilePath, staticRoutes } from "routes/routes";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import TeachIcon from "assets/teach.svg";
import HomeIcon from "assets/home.svg";
import AppLogo from "assets/logo.svg";
import AppLogoBusiness from "assets/app_logo_business.svg";
import { useTranslation } from "components/core/TranslationProvider";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { selectAuthStatus, selectIsAuthorized, selectUserRoleId } from "core/redux/authSlice";
import MenuIcon from "assets/menu.svg";
import KeyboardArrowLeftIcon from "assets/arrow-left.svg";
import KeyboardArrowRightIcon from "assets/arrow-right.svg";
import SwipeableDrawer from '@mui/material/SwipeableDrawer';
import LanguageIcon from "assets/language.svg";
import LogOutIcon from "assets/log_out.svg";
import ButtonUnstyled from '@mui/base/ButtonUnstyled';
import { useRouter } from "next/router";
import UserInfo from "./UserInfo";
import ChatBadge from "./ChatBadge";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { PrimaryButton } from "components/styles/PrimaryButton";
import { getAllExceptSelected } from "core/services/Utils/languageUtil";
import { logOut } from "core/services/Utils/authUtil";
import HrefLink from "components/core/HrefLink";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { SecondaryButtonRectangle } from "components/styles/SecondaryButtonRectangle";
import { UIContext } from "components/core/UIContextProvider";
import { TertiaryButton } from "components/styles/TertiaryButton";
import Notifications from "components/navbar/NotificationsMenu";
import TermsIcon from "assets/terms.svg";
import PrivacyIcon from "assets/privacy.svg";
import AuthModalProvider, { useAuthModalContext } from "components/modals/AuthModalProvider";
import { useAppDataContext } from "components/core/providers/AppProvider";
import { selectShortUserInfo } from "core/redux/userSlice";
import Box from "@mui/material/Box";
import { AvatarStyled } from "components/styles/user/AvatarStyled";
import React, { useContext } from "react";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import ProfileIcon from "assets/profile.svg";
import MatchingTutorsIcon from "assets/matching_tutors.svg";
import { StudentDashboardTabPathParsedType } from "pages/student/dashboard";
import MailIcon from "assets/chat.svg";
import BellIcon from "assets/bell.svg";
import SearchIcon from "assets/search.svg";
import MoneyIcon from "assets/money.svg";
import { getProfilePhotoThumbnailUrl } from "core/services/Utils/imageUtil";
import Body1Styled from "components/styles/typography/Body1Styled";
import { SecondaryButton } from "components/styles/SecondaryButton";
import Badge from "@mui/material/Badge";
import YaspeakBusinessIcon from "assets/yaspeak_business.svg";
import { UserSearchQueryType } from "core/services/Types/AppTypes";
import { getFullUrlWithLocale } from "core/services/Utils/urlUtil";
import Link from "next/link";
import { useWindowScroll } from "core/hooks/scrollHooks";
import { ContainerStyled } from "components/styles/ContainerStyled";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import LanguageMenu from "./LanguageMenu";
import CurrencyMenu from "./CurrencyMenu";
import { formatCurrency } from "core/services/Utils/walletUtil";

export enum NavbarDisplay { all = 0, smOnly = 1, regularOnly = 2 }
export enum NavbarSourceEnum { tutorPage = 1, studentPage = 2, videoPage = 3, other = 4, companyPage = 5 }
export enum NavbarAuthDisplay { all = 1, nonAuthOnly = 2 }

enum MenuState {
  closed = 1,
  mainMenu = 2,
  languageMenu = 3,
  currencyMenu = 4
}

export interface ILink {
  titleKey: string,
  display: NavbarDisplay,
  auth: NavbarAuthDisplay,
  url: string,
  icon: () => JSX.Element,
  title: string | undefined,
  addSeparatorAfter: boolean
}

const LeftItemBoxStyled = styled(Box)(() => sx({
  padding: "8px",
  display: "flex",
  justifyContent: "center",
  alignContent: "center",
})) as typeof Box;

const LeftItemMobileBoxStyled = styled(Box)(() => sx({
  padding: "4px 8px",
  display: "flex",
  justifyContent: "center",
  alignContent: "center",
})) as typeof Box;

const IconBoxStyled = styled(Box)(() => sx({
  svg: {
    color: COLORS.BLACK,
  },
  display: "flex",
  alignItems: "center",
  fill: "currentColor"
})) as typeof Box;

const UserProfileBoxStyled = styled(Box)(() => sx({
  display: "flex",
  alignItems: "center",
  gap: "8px",
  padding: "8px",
  borderRadius: "4px",
  ":hover": {
    backgroundColor: COLORS.PRIMARY_5
  }
})) as typeof Box;

const DesktopIconBoxStyled = styled(Box)(() => sx({
  display: "flex",
  alignContent: "center",
  justifyContent: "center",
  ":hover": {
    cursor: "pointer"
  },
  "a": {
    display: "flex",
    alignItems: "center",
  },
  "svg": {
    display: "flex"
  }
})) as typeof Box;

const MenuIconBoxStyled = styled(Box)(() => sx({
  display: "flex",
  alignItems: "center",
  paddingRight: "8px",
  svg: {
    color: COLORS.PRIMARY,
    transform: "scale(1.5)"
  }
})) as typeof Box;

const BottomLogoBoxStyled = styled(Box)(() => sx({
  display: "flex",
  justifyContent: "left",
  paddingLeft: "16px",
  width: "100%",
  textAlign: "center"
})) as typeof Box;

const MobileLinksBoxStyled = styled(Box)(() => sx({
  width: "100%",
  padding: "4px",
  display: "flex",
  flexWrap: "wrap",
  gap: "8px"
})) as typeof Box;

const NavbarMobileBoxStyled = styled(Box)(() => sx({
  display: "flex",
  justifyContent: "space-between",
  alignItems: "center",
  width: "100%"
})) as typeof Box;

const NavbarMobileRightBoxStyled = styled(Box)(() => sx({
  display: "flex",
  gap: "12px",
  height: "100%"
})) as typeof Box;

const ButtonsBoxStyled = styled(Box)(() => sx({
  display: "flex",
  flexWrap: "wrap",
  width: "100%",
  textDecoration: "none",
  gap: "16px"
})) as typeof Box;

const NavbarRegularButtonBoxStyled = styled(Box)(() => sx({
  display: "flex",
  gap: "8px",
  textDecoration: "none",
  alignItems: "center"
})) as typeof Box;

const SeparatorBoxStyled = styled(Box)(() => sx({
  backgroundColor: COLORS.BLACK_5,
  width: "100%",
  height: "1px"
})) as typeof Box;

const NavbarButtonStyled = styled(ButtonUnstyled)(() => sx({
  color: COLORS.BLACK,
  fontSize: "0.875rem",
  backgroundColor: "transparent",
  display: "flex",
  border: "none",
  fontWeight: 500,
  fontFamily: "inherit",
  padding: "8px",
  alignItems: "center",
  gap: "8px",
  borderRadius: "4px",
  transition: "none",
  ":hover": {
    backgroundColor: COLORS.PRIMARY_5,
    cursor: "pointer"
  }
}));

const MobileMenuBoxStyled = styled(Box)(() => sx({
  display: "flex",
  height: "100%",
  flexWrap: "wrap",
  alignContent: "space-between",
  padding: "4px 8px 4px 8px",
  a: {
    textDecoration: "none"
  }
})) as typeof Box;

const ListItemLeftBoxStyled = styled(Box)(() => sx({
  width: "100%",
  display: "flex",
  alignItems: "center",
  justifyContent: "flex-start",
  padding: "4px 0px",
  gap: "8px",
  ":hover": {
    cursor: "pointer"
  }
})) as typeof Box;

const MenuLanguageItem = styled(Box)(() => sx({
  padding: "4px 0px"
})) as typeof Box;

const ListLanguageItemWithoutIconItemLeftBoxStyled = styled(Box)(() => sx({
  padding: "0px 40px",
  textDecoration: "none",
})) as typeof Box;

const ListLanguageItemLeftBoxStyled = styled(Box)(() => sx({
  width: "100%",
  display: "flex",
  alignItems: "center",
  justifyContent: "space-between",
  ":hover": {
    cursor: "pointer"
  }
})) as typeof Box;

const DesktopMenuBoxStyled = styled(Box)(() => sx({
  flexGrow: 1,
  display: { xs: "none", md: "flex" },
  alignItems: "center",
  justifyContent: "flex-end",
  gap: "4px",
  a: {
    textDecoration: "none"
  }
})) as typeof Box;

const NavbarMenuBoxStyled = styled(Box)(() => sx({
  display: "flex",
  alignItems: "center",
  justifyContent: "space-between",
  width: "100%"
})) as typeof Box;

const MiddleMenuBoxStyled = styled(Box)(() => sx({
  display: "flex",
  flexFlow: "row nowrap",
  alignItems: "center",
  gap: "16px"
})) as typeof Box;

const RightMenuBoxStyled = styled(Box)(() => sx({
  display: "flex",
  flexFlow: "row nowrap",
  gap: "4px",
  alignItems: "center"
})) as typeof Box;

const LogoBoxStyled = styled(Box)(() => sx({
  minWidth: "248px",
  display: "flex",
  justifyContent: "start"
})) as typeof Box;

const PrimaryButtonStyled = styled(PrimaryButton)(() => sx({
  padding: "9px 24px !important",
})) as typeof PrimaryButton;

const SecondaryButtonStyled = styled(SecondaryButton)(() => sx({
  padding: "9px 24px !important",
})) as typeof SecondaryButton;

const SwipeableDrawerStyled = styled(SwipeableDrawer)(() => sx({
  ".MuiDrawer-paper": {
    width: "70vw"
  }
})) as typeof SwipeableDrawer;

const AppBarStyled = styled(AppBar)<{ position: PositionType, scrolled: string }>(({ position, scrolled }) => sx({
  position: position,
  boxShadow: (scrolled === "true" && position === "sticky")
    ? "0px 4px 5px 0px rgba(0,0,0,0.05)"
    : "none",
  zIndex: position === "sticky" ? 1000 : 1,
  top: 0,
  backgroundColor: COLORS.WHITE,
  borderBottom: `1px solid ${COLORS.BLACK_10}`,
}));

const NavIconStyled = styled(Box)(() => sx({
  paddingTop: "4px"
})) as typeof Box;

const getUserRole = (source: NavbarSourceEnum) => {
  switch (source) {
    case NavbarSourceEnum.studentPage:
      return UserRoleEnum.Student;
    case NavbarSourceEnum.tutorPage:
      return UserRoleEnum.Tutor;
    case NavbarSourceEnum.companyPage:
      return UserRoleEnum.Company;
    default:
      return UserRoleEnum.Student;
  }
}

export const NavbarRegularLogInButton = ({ source }: { source: NavbarSourceEnum }) => {
  const { t } = useTranslation();
  const { setOpen } = useAuthModalContext();

  const handleLogInModalClick = () => {
    setOpen(getUserRole(source), "login");
  }

  return (
    <Box>
      <TertiaryButton sx={{ padding: "8px", ":hover": { backgroundColor: "transparent", textDecoration: "underline" } }} onClick={handleLogInModalClick}>
        {t(TRANSLATION_KEYS.AUTHORIZATION.LOGIN.TITLE)}
      </TertiaryButton>
    </Box>
  )
}

export const NavbarRegularSignUpButton = ({ source, position }: { source: NavbarSourceEnum, position: PositionType }) => {
  const { t } = useTranslation();
  const { setOpen } = useAuthModalContext();
  const { isScrolled } = useWindowScroll();

  const handleSignUpModalClick = () => {
    setOpen(getUserRole(source), "registration");
  }

  return (
    <Box>
      {isScrolled && position === "sticky" ? (
        <PrimaryButtonStyled onClick={handleSignUpModalClick}>{t(TRANSLATION_KEYS.AUTHORIZATION.SIGN_UP.TITLE)}</PrimaryButtonStyled>
      ) : (
        <SecondaryButtonStyled onClick={handleSignUpModalClick}>{t(TRANSLATION_KEYS.AUTHORIZATION.SIGN_UP.TITLE)}</SecondaryButtonStyled>
      )}
    </Box>
  )
}

export const NavbarMobileLogInButton = ({ source }: { source: NavbarSourceEnum }) => {
  const { t } = useTranslation();
  const { setOpen } = useAuthModalContext();

  const handleLogInModalClick = () => {
    setOpen(getUserRole(source), "login");
  }

  return (
    <SecondaryButtonRectangle onClick={handleLogInModalClick} sx={{ width: "100%" }}>{t(TRANSLATION_KEYS.AUTHORIZATION.LOGIN.TITLE)}</SecondaryButtonRectangle>
  )
}

export const NavbarMobileSignUpButton = ({ source }: { source: NavbarSourceEnum }) => {
  const { t } = useTranslation();
  const { setOpen } = useAuthModalContext();

  const handleSignUpModalClick = () => {
    setOpen(getUserRole(source), "registration");
  }

  return (
    <PrimaryButtonRectangle onClick={handleSignUpModalClick} sx={{ width: "100%" }}>{t(TRANSLATION_KEYS.AUTHORIZATION.SIGN_UP.TITLE)}</PrimaryButtonRectangle>
  )
}

export const NavbarMobileMenuLogOutItemLink = () => {
  const { t } = useTranslation();
  const dispatch = useAppDispatch();

  const handleLogOut = () => {
    logOut(dispatch);
  }

  return (
    <ListItemLeftBoxStyled onClick={handleLogOut}>
      <LeftItemBoxStyled><IconBoxStyled><LogOutIcon /></IconBoxStyled></LeftItemBoxStyled>
      <MenuLanguageItem>
        <Body1Styled sx={{ color: COLORS.BLACK, fontWeight: 500 }}>{t(TRANSLATION_KEYS.PROFILE_ICON.LOGOUT)}</Body1Styled>
      </MenuLanguageItem>

    </ListItemLeftBoxStyled>
  )
}

const YaspeakIcon = () => {
  const userRoleId = useAppSelector(selectUserRoleId);
  const router = useRouter();
  return (
    <DesktopIconBoxStyled>
      <HrefLink href={getDefaultPage(userRoleId)} aria-label="Yaspeak">
        {router.pathname === routes.companyLandingPage.path ?
          <AppLogoBusiness /> : <AppLogo />
        }
      </HrefLink>
    </DesktopIconBoxStyled>
  )
}

const HomeButton = () => {
  const { t } = useTranslation();
  const userRoleId = useAppSelector(selectUserRoleId);

  return (
    <HrefLink href={getDefaultPage(userRoleId)}>
      <NavbarButtonStyled>
        <NavIconStyled>
          <HomeIcon />
        </NavIconStyled>
        {t(TRANSLATION_KEYS.NAVBAR.HOME)}
      </NavbarButtonStyled>
    </HrefLink>
  )
}

const FindATutorButton = ({ showIcon = false }: { showIcon?: boolean }) => {
  const { t } = useTranslation();
  const shortUserInfo = useAppSelector(selectShortUserInfo);
  const searchQuery: UserSearchQueryType | undefined = shortUserInfo ? {
    subjectId: shortUserInfo.subjectId ? `${shortUserInfo.subjectId}` : undefined
  } : undefined;

  return (
    <HrefLink href={{
      pathname: routes.search.path,
      query: searchQuery
    }}>
      <NavbarButtonStyled>
        {showIcon ? (
          <NavIconStyled>
            <SearchIcon />
          </NavIconStyled>
        ) : <></>}
        {t(TRANSLATION_KEYS.NAVBAR.FIND_A_TUTOR)}
      </NavbarButtonStyled>
    </HrefLink>
  )
}

const BecomeATutorButton = () => {
  const { t } = useTranslation();

  return (
    <HrefLink href={routes.tutorLandingPage.path}>
      <NavbarButtonStyled>
        {t(TRANSLATION_KEYS.NAVBAR.BECOME_A_TUTOR)}
      </NavbarButtonStyled>
    </HrefLink>
  )
}

const YaspeakBusinessButton = () => {
  const { t } = useTranslation();

  return (
    <HrefLink href={routes.companyLandingPage.path}>
      <NavbarButtonStyled>
        {t(TRANSLATION_KEYS.NAVBAR.YASPEAK_BUSINESS)}
      </NavbarButtonStyled>
    </HrefLink>
  )
}

const LanguageMenuButtonUnauthorized = () => {
  const { screenType } = useContext(UIContext);

  return (
    <>
      {screenType === "desktop" ? <LanguageMenu /> : <></>}
    </>
  )
}

const LanguageMenuButtonAuthorized = () => {
  const { screenType } = useContext(UIContext);

  return (
    <>
      {screenType === "desktop" ? <LanguageMenu /> : <></>}
    </>
  )
}

const NavbarDesktopAuthorized = ({ source }: { source: NavbarSourceEnum | undefined }) => {
  const userRoleId = useAppSelector(selectUserRoleId);
  const { screenType } = useContext(UIContext);

  return (
    <NavbarMenuBoxStyled>
      <LogoBoxStyled>
        <YaspeakIcon />
      </LogoBoxStyled>
      <MiddleMenuBoxStyled>
        <HomeButton />
        {userRoleId === UserRoleEnum.Student ? <FindATutorButton showIcon={true} /> : <></>}
        <ChatBadge />
        <Notifications />
      </MiddleMenuBoxStyled>
      <RightMenuBoxStyled>
        {screenType === "desktop" ? <LanguageMenu /> : <></>}
        {screenType === "desktop" ? <CurrencyMenu /> : <></>}
        <UserInfo />
      </RightMenuBoxStyled>
    </NavbarMenuBoxStyled>
  );
}

const NavbarDesktopUnauthorized = ({ source, position }: { source: NavbarSourceEnum, position: PositionType }) => {
  const { screenType } = useContext(UIContext);

  return (
    <NavbarMenuBoxStyled>
      <LogoBoxStyled>
        <YaspeakIcon />
      </LogoBoxStyled>
      <MiddleMenuBoxStyled>
        <FindATutorButton />
        <YaspeakBusinessButton />
        <BecomeATutorButton />
      </MiddleMenuBoxStyled>
      <RightMenuBoxStyled>
        {screenType === "desktop" ? <LanguageMenu /> : <></>}
        {screenType === "desktop" ? <CurrencyMenu /> : <></>}
        <NavbarRegularButtonBoxStyled>
          <NavbarRegularLogInButton source={source} />
          <NavbarRegularSignUpButton source={source} position={position} />
        </NavbarRegularButtonBoxStyled>
      </RightMenuBoxStyled>
    </NavbarMenuBoxStyled>
  )
}

const NavbarDesktopLoading = ({ source }: { source: NavbarSourceEnum }) => (
  <>
    <YaspeakIcon />
    <DesktopMenuBoxStyled>
      <FindATutorButton />
      <YaspeakBusinessButton />
      <BecomeATutorButton />
      <NavbarRegularButtonBoxStyled>
        <NavbarRegularLogInButton source={source} />
        <PrimaryButton><CircularProgressCustom color={COLORS.WHITE} /></PrimaryButton>
      </NavbarRegularButtonBoxStyled>
    </DesktopMenuBoxStyled>
  </>
)

const NavbarDesktop = ({ source, position }: { source: NavbarSourceEnum, position: PositionType }) => {
  const isUserAuthorized = useAppSelector(selectIsAuthorized);
  const loadingStatus = useAppSelector(selectAuthStatus);

  if (loadingStatus === LoadingStatusEnum.Loading) {
    return <NavbarDesktopLoading source={source} />;
  }

  return isUserAuthorized
    ? <NavbarDesktopAuthorized source={source} />
    : <NavbarDesktopUnauthorized source={source} position={position} />
}

const NavbarMobileMenuItemLink = ({ path, icon, translationKey }: { path: string, icon: JSX.Element, translationKey: string }) => {
  const { t } = useTranslation();

  return (
    <HrefLink href={path} width="100%">
      <ListItemLeftBoxStyled>
        <LeftItemBoxStyled><IconBoxStyled>{icon}</IconBoxStyled></LeftItemBoxStyled>
        <Body1Styled sx={{ color: COLORS.BLACK, fontWeight: 500 }}>{t(translationKey)}</Body1Styled>
      </ListItemLeftBoxStyled>
    </HrefLink>
  )
}

const NavbarMobileMenuItemClickable = ({ icon, translation, onClick }: { icon: JSX.Element, translation: string, onClick: () => void }) => {
  return (
    <ListItemLeftBoxStyled onClick={onClick}>
      <LeftItemBoxStyled><IconBoxStyled>{icon}</IconBoxStyled></LeftItemBoxStyled>
      <Body1Styled sx={{ color: COLORS.BLACK, fontWeight: 500 }}>{translation}</Body1Styled>
    </ListItemLeftBoxStyled>
  )
}

const NavbarMobileMenuItemWithBadgeClickable = ({ onClick, icon, translation, countOfEvents }: { onClick: () => void, translation: string, icon: JSX.Element, countOfEvents: number }) => {

  return (
    <ListItemLeftBoxStyled onClick={onClick}>
      <LeftItemBoxStyled>
        <Badge badgeContent={countOfEvents ?? 0} color="error" variant="dot" overlap="circular">
          {icon}
        </Badge>
      </LeftItemBoxStyled>
      <Body1Styled sx={{ color: COLORS.BLACK, fontWeight: 500 }}>{translation}</Body1Styled>
    </ListItemLeftBoxStyled>
  )
}

const NavbarMobileLanguageMenuItemClickableWithoutIcon = ({ translation, languageCode }: { translation: string, languageCode: string }) => {
  const router = useRouter();

  return (
    <ListItemLeftBoxStyled>
      <ListLanguageItemWithoutIconItemLeftBoxStyled>
        <Link href={getFullUrlWithLocale(languageCode, router.asPath)} rel="alternate" locale={false}>
          <Body1Styled sx={{ color: COLORS.BLACK, fontWeight: 500 }}>{translation}</Body1Styled>
        </Link>
      </ListLanguageItemWithoutIconItemLeftBoxStyled>
    </ListItemLeftBoxStyled>
  )
}

const NavbarMobileLanguageMenuItemClickable = ({ iconBefore, iconAfter, translation, onClick }: { iconBefore: JSX.Element, iconAfter: JSX.Element, translation: string, onClick: () => void }) => {
  return (
    <ListLanguageItemLeftBoxStyled onClick={onClick}>
      <ListItemLeftBoxStyled>
        <LeftItemBoxStyled><IconBoxStyled>{iconBefore}</IconBoxStyled></LeftItemBoxStyled>
        <Body1Styled sx={{ color: COLORS.BLACK, fontWeight: 500 }}>{translation}</Body1Styled>
      </ListItemLeftBoxStyled>
      <LeftItemBoxStyled><IconBoxStyled>{iconAfter}</IconBoxStyled></LeftItemBoxStyled>
    </ListLanguageItemLeftBoxStyled>
  )
}

const NavbarMobileMenuItemBackClickable = ({ onMenuStateChange }: { onMenuStateChange: (menuState: MenuState) => void }) => {
  const { t } = useTranslation();

  return (
    <ListItemLeftBoxStyled onClick={() => onMenuStateChange(MenuState.mainMenu)}>
      <LeftItemMobileBoxStyled><IconBoxStyled><KeyboardArrowLeftIcon /></IconBoxStyled></LeftItemMobileBoxStyled>
      <MenuLanguageItem sx={{ padding: "0px !important" }}><Body1Styled sx={{ color: COLORS.BLACK, fontWeight: 500 }}>{t(TRANSLATION_KEYS.NAVBAR.BACK)}</Body1Styled></MenuLanguageItem>
    </ListItemLeftBoxStyled>
  )
}

const NavbarMobileBottomLogoLink = ({ onMenuStateChange }: { onMenuStateChange: (menuState: MenuState) => void }) => {
  const userRoleId = useAppSelector(selectUserRoleId);

  return (
    <BottomLogoBoxStyled onClick={() => onMenuStateChange(MenuState.closed)}>
      <HrefLink href={getDefaultPage(userRoleId)}><AppLogo /></HrefLink>
    </BottomLogoBoxStyled>
  )
}

const NavbarMobileMainMenuAuthorized = ({ onMenuStateChange }: { onMenuStateChange: (menuState: MenuState) => void }) => {
  const { currentLanguage, currentCurrency } = useAppDataContext();
  const { t } = useTranslation();
  const userRoleId = useAppSelector(selectUserRoleId);
  const router = useRouter();
  const shortUserInfo = useAppSelector(selectShortUserInfo);
  const profileLink = getProfilePath(userRoleId);

  const handleMatchingTutorsClick = () => {
    const query: StudentDashboardTabPathParsedType = { tab: "matching_tutors" };
    router.push({
      pathname: routes.studentDashboard.path,
      query
    });
  }

  const handleMessagesClick = () => {
    router.push(routes.messagesPage.path)
  }

  const handleNotificationsClick = () => {
    router.push(routes.notifications.path)
  };

  const handleSearchClick = () => {
    router.push(routes.search.path)
  }

  return (
    <>
      <MobileLinksBoxStyled>
        <NavbarMobileMenuItemLink path={routes.home.path} icon={<HomeIcon />} translationKey={TRANSLATION_KEYS.NAVBAR.HOME} />
        <NavbarMobileMenuItemLink path={profileLink ?? ""} icon={<ProfileIcon />} translationKey={TRANSLATION_KEYS.PROFILE_ICON.PROFILE} />
        <NavbarMobileMenuItemWithBadgeClickable onClick={handleMessagesClick} icon={<MailIcon />} translation={t(TRANSLATION_KEYS.PROFILE_ICON.MESSAGES)} countOfEvents={shortUserInfo?.countOfUnreadMessages ?? 0} />
        <NavbarMobileMenuItemWithBadgeClickable onClick={handleNotificationsClick} icon={<BellIcon />} translation={t(TRANSLATION_KEYS.PROFILE_ICON.NOTIFICATIONS)} countOfEvents={shortUserInfo?.countOfUnreadNotifications ?? 0} />
        {userRoleId === UserRoleEnum.Student ?
          (<>
            <SeparatorBoxStyled />
            <NavbarMobileMenuItemClickable onClick={handleSearchClick} icon={<SearchIcon />} translation={t(TRANSLATION_KEYS.NAVBAR.FIND_A_TUTOR)} />
            <NavbarMobileMenuItemClickable onClick={handleMatchingTutorsClick} icon={<MatchingTutorsIcon />} translation={t(TRANSLATION_KEYS.PROFILE_ICON.MATCHING_TUTORS)} />
          </>
          ) : <></>
        }
        <SeparatorBoxStyled />
        <NavbarMobileLanguageMenuItemClickable onClick={() => onMenuStateChange(MenuState.languageMenu)} iconBefore={<LanguageIcon />} iconAfter={<KeyboardArrowRightIcon />} translation={t(currentLanguage?.language ?? "")} />
        <NavbarMobileLanguageMenuItemClickable onClick={() => onMenuStateChange(MenuState.currencyMenu)} iconBefore={<MoneyIcon />} iconAfter={<KeyboardArrowRightIcon />} translation={currentCurrency.enumKey?.toUpperCase() ?? ""} />
        <SeparatorBoxStyled />
        <NavbarMobileMenuItemLink path={staticRoutes.termsOfUse} icon={<TermsIcon />} translationKey={TRANSLATION_KEYS.FOOTER.TERMS_OF_USE} />
        <NavbarMobileMenuItemLink path={staticRoutes.gdpr} icon={<PrivacyIcon />} translationKey={TRANSLATION_KEYS.FOOTER.PRIVACY_POLICY} />
        <SeparatorBoxStyled />
        <NavbarMobileMenuLogOutItemLink />
      </MobileLinksBoxStyled>
      <NavbarMobileBottomLogoLink onMenuStateChange={onMenuStateChange} />
    </>
  )
}

const NavbarMobileMainMenuNotAuthorized = ({ source, onMenuStateChange }: { source: NavbarSourceEnum, onMenuStateChange: (menuState: MenuState) => void }) => {
  const { currentLanguage, currentCurrency } = useAppDataContext();
  const { t } = useTranslation();

  return (
    <React.Fragment>
      <MobileLinksBoxStyled>
        <NavbarMobileMenuItemLink path={routes.home.path} icon={<HomeIcon />} translationKey={TRANSLATION_KEYS.NAVBAR.HOME} />
        <NavbarMobileMenuItemLink path={routes.search.path} icon={<SearchIcon />} translationKey={TRANSLATION_KEYS.NAVBAR.FIND_A_TUTOR} />
        <NavbarMobileMenuItemLink path={routes.tutorLandingPage.path} icon={<TeachIcon />} translationKey={TRANSLATION_KEYS.NAVBAR.BECOME_A_TUTOR} />
        <NavbarMobileMenuItemLink path={routes.companyLandingPage.path} icon={<YaspeakBusinessIcon />} translationKey={TRANSLATION_KEYS.NAVBAR.YASPEAK_BUSINESS} />
        <NavbarMobileLanguageMenuItemClickable onClick={() => onMenuStateChange(MenuState.languageMenu)} iconBefore={<LanguageIcon />} iconAfter={<KeyboardArrowRightIcon />} translation={t(currentLanguage?.language ?? "")} />
        <NavbarMobileLanguageMenuItemClickable onClick={() => onMenuStateChange(MenuState.currencyMenu)} iconBefore={<MoneyIcon />} iconAfter={<KeyboardArrowRightIcon />} translation={currentCurrency.enumKey?.toUpperCase() ?? ""} />
        <ButtonsBoxStyled>
          <NavbarMobileLogInButton source={source} />
          <NavbarMobileSignUpButton source={source} />
        </ButtonsBoxStyled>
      </MobileLinksBoxStyled>
      <NavbarMobileBottomLogoLink onMenuStateChange={onMenuStateChange} />
    </React.Fragment>
  )
}

const NavbarMobileMainMenu = ({ source, onMenuStateChange }: { source: NavbarSourceEnum, onMenuStateChange: (menuState: MenuState) => void }) => {
  const isUserAuthorized = useAppSelector(selectIsAuthorized);

  return (
    <MobileMenuBoxStyled>
      {isUserAuthorized
        ? <NavbarMobileMainMenuAuthorized onMenuStateChange={onMenuStateChange} />
        : <NavbarMobileMainMenuNotAuthorized onMenuStateChange={onMenuStateChange} source={source} />
      }
    </MobileMenuBoxStyled>
  )
}

const NavbarMobileLanguageMenu = ({ onMenuStateChange }: { onMenuStateChange: (menuState: MenuState) => void }) => {
  const { languages, currentLanguage } = useAppDataContext();

  return (
    <MobileMenuBoxStyled>
      <MobileLinksBoxStyled>
        <NavbarMobileMenuItemBackClickable onMenuStateChange={onMenuStateChange} />
        {getAllExceptSelected(languages, currentLanguage?.languageCode).map((language, idx) =>
          <NavbarMobileLanguageMenuItemClickableWithoutIcon
            languageCode={language?.languageCode ?? ""}
            key={idx}
            translation={language?.language ?? ""} />
        )}
      </MobileLinksBoxStyled>
      <NavbarMobileBottomLogoLink onMenuStateChange={onMenuStateChange} />
    </MobileMenuBoxStyled>
  )
}


const NavbarMobileCurrencyMenu = ({ onMenuStateChange }: { onMenuStateChange: (menuState: MenuState) => void }) => {
  const { currencies, currencyCode, changeCurrency } = useAppDataContext();

  const handleCurrencyClick = (newCurrencyCode: string) => {
    changeCurrency(newCurrencyCode);
    onMenuStateChange(MenuState.closed);
  }

  return (
    <MobileMenuBoxStyled>
      <MobileLinksBoxStyled>
        <NavbarMobileMenuItemBackClickable onMenuStateChange={onMenuStateChange} />
        {currencies.filter(x => x.enumKey !== currencyCode).map((x, idx) =>
          <ListItemLeftBoxStyled key={x.currencyId!} onClick={() => handleCurrencyClick(x.enumKey ?? "")}>
            <ListLanguageItemWithoutIconItemLeftBoxStyled>
              <Body1Styled sx={{ color: COLORS.BLACK, fontWeight: 500 }}>{formatCurrency(x)}</Body1Styled>
            </ListLanguageItemWithoutIconItemLeftBoxStyled>
          </ListItemLeftBoxStyled>
        )}
      </MobileLinksBoxStyled>
      <NavbarMobileBottomLogoLink onMenuStateChange={onMenuStateChange} />
    </MobileMenuBoxStyled>
  )
}

const NavbarMobileUserProfileLoaded = ({ onClick }: { onClick: () => void }) => {
  const shortUserInfo = useAppSelector(selectShortUserInfo);

  return (
    <UserProfileBoxStyled onClick={onClick}>
      <Badge
        badgeContent={(shortUserInfo?.countOfUnreadMessages ?? 0) || (shortUserInfo?.countOfUnreadNotifications ?? 0)}
        color="error"
        variant="dot"
        overlap="circular"
      >
        <AvatarStyled src={getProfilePhotoThumbnailUrl(shortUserInfo)} />
      </Badge>
    </UserProfileBoxStyled>
  )
}

const NavbarMobileMenuButton = ({ onClick }: { onClick: () => void }) => {
  const shortUserInfo = useAppSelector(selectShortUserInfo);
  const loadingStatus = useAppSelector(selectAuthStatus);

  if (loadingStatus === LoadingStatusEnum.Loading) {
    return (
      <UserProfileBoxStyled><CircularProgressCustom /></UserProfileBoxStyled>
    );
  }

  return shortUserInfo
    ? <NavbarMobileUserProfileLoaded onClick={onClick} />
    : <MenuIconBoxStyled onClick={onClick}><MenuIcon /></MenuIconBoxStyled>;
}

const NavbarMobileMenuViews = ({ source, state, setState }: { source: NavbarSourceEnum, state: MenuState, setState: (newState: MenuState) => void }) => (
  <SwipeableDrawerStyled
    open={state !== MenuState.closed}
    onClose={() => setState(MenuState.closed)}
    onOpen={() => setState(MenuState.mainMenu)}
  >
    {state === MenuState.mainMenu ? <NavbarMobileMainMenu source={source} onMenuStateChange={setState} /> : <></>}
    {state === MenuState.languageMenu ? <NavbarMobileLanguageMenu onMenuStateChange={setState} /> : <></>}
    {state === MenuState.currencyMenu ? <NavbarMobileCurrencyMenu onMenuStateChange={setState} /> : <></>}
  </SwipeableDrawerStyled>
)

const NavbarMobile = ({ source }: { source: NavbarSourceEnum }) => {
  const [menuState, setMenuState] = React.useState(MenuState.closed);

  const onMenuStateChange = (menuState: MenuState) => setMenuState(menuState);
  const handleMenuClick = () => setMenuState(MenuState.mainMenu);

  return (
    <NavbarMobileBoxStyled>
      <Box sx={{ display: "flex", height: "100%", justifyContent: "center" }} >
        <YaspeakIcon />
      </Box>
      <NavbarMobileRightBoxStyled>
        <NavbarMobileMenuButton onClick={handleMenuClick} />
        <NavbarMobileMenuViews source={source} state={menuState} setState={onMenuStateChange} />
      </NavbarMobileRightBoxStyled>
    </NavbarMobileBoxStyled>
  );
}

type PositionType = "static" | "sticky"

const Navbar = ({ position = "sticky", source = NavbarSourceEnum.other }: { position?: PositionType, source?: NavbarSourceEnum }) => {
  const { screenType } = React.useContext(UIContext);
  const { isScrolled } = useWindowScroll();

  return (
    <AuthModalProvider>
      <AppBarStyled position={position} scrolled={isScrolled.toString()} >
        <ContainerStyled>
          <Toolbar disableGutters variant='dense' sx={{ height: 56 }}>
            {screenType === "mobile" ? <NavbarMobile source={source} /> : <></>}
            {screenType === "desktop" ? <NavbarDesktop source={source} position={position} /> : <></>}
          </Toolbar>
        </ContainerStyled>
      </AppBarStyled>
    </AuthModalProvider>
  );
};

export default Navbar;