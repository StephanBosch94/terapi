import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { selectShortUserInfo } from "core/redux/userSlice";
import { useState } from "react";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { getProfilePhotoThumbnailUrl } from "core/services/Utils/imageUtil";
import ProfileIcon from "assets/profile.svg";
import LogOutIcon from "assets/log_out.svg";
import MatchingTutorsIcon from "assets/matching_tutors.svg";
import React from "react";
import { useTranslation } from "components/core/TranslationProvider";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import routes, { getProfilePath } from "routes/routes";
import { useRouter } from "next/router";
import { AvatarStyled } from "components/styles/user/AvatarStyled";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { logOut } from "core/services/Utils/authUtil";
import { MenuStyled } from "components/styles/MenuStyled";
import { selectUserRoleId } from "core/redux/authSlice";
import MenuItem from "components/menu/MenuItem";
import { MenuBoxStyled } from "components/styles/boxes/MenuBoxStyled";
import HrefLink from "components/core/HrefLink";
import Box from "@mui/material/Box";
import Divider from "@mui/material/Divider";
import PStyled from "components/styles/typography/PStyled";
import { StudentDashboardTabPathParsedType } from "pages/student/dashboard";
import { getFirstNameOrCompanyName } from "core/services/Utils/userUtils";

const MainBoxStyled = styled(Box)(() => sx({
    userSelect: "false"
})) as typeof Box;

const UserOuterBoxStyled = styled(Box)(() => sx({
    userSelect: "false",
    display: 'flex',
    alignItems: 'center',
    textAlign: 'center'
})) as typeof Box;


const UserProfile = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    gap: "8px",
    padding: "8px",
    borderRadius: "4px",
    ":hover": {
        backgroundColor: COLORS.PRIMARY_5
    }
})) as typeof Box;

const FirstNameBoxStyled = styled(PStyled)(() => sx({
    color: COLORS.BLACK,
    fontWeight: 500,
    fontSize: "0.875rem"
})) as typeof PStyled;

const UserBoxStyled = styled(Box)(() => sx({
    ":hover": {
        cursor: "pointer"
    }
})) as typeof Box;


const UserInfo = () => {
    const router = useRouter();
    const dispatch = useAppDispatch();
    const { t } = useTranslation();
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const userRoleId = useAppSelector(selectUserRoleId);
    const shortUserInfo = useAppSelector(selectShortUserInfo);
    const profileLink = getProfilePath(userRoleId);
    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleLogOut = () => {
        logOut(dispatch);
    }

    const handleMatchingTutorsClick = () => {
        const query: StudentDashboardTabPathParsedType = { tab: "matching_tutors" };
        router.push({
            pathname: routes.studentDashboard.path,
            query
        });
    }

    return (
        <MainBoxStyled>
            <UserOuterBoxStyled>
                <UserBoxStyled
                    onClick={handleClick}
                    aria-controls={open ? 'account-menu' : undefined}
                    aria-haspopup="true"
                    aria-expanded={open ? 'true' : undefined}
                >
                    <UserProfile>
                        <AvatarStyled sx={{ width: 32, height: 32, }} src={getProfilePhotoThumbnailUrl(shortUserInfo)} />
                        <FirstNameBoxStyled>{getFirstNameOrCompanyName(shortUserInfo) ?? t(TRANSLATION_KEYS.USER_PROFILE.DEFAULT_USER_NAME)}</FirstNameBoxStyled>
                    </UserProfile>
                </UserBoxStyled>
            </UserOuterBoxStyled>
            <MenuStyled
                anchorEl={anchorEl}
                open={open}
                onClose={handleClose}
                onClick={handleClose}
                transformOrigin={{ horizontal: 'right', vertical: 'top' }}
                anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
            >
                <MenuBoxStyled gap="4px">
                    <HrefLink href={profileLink ?? ""}><MenuItem icon={<ProfileIcon />} text={t(TRANSLATION_KEYS.PROFILE_ICON.PROFILE)} /></HrefLink>
                    <Divider sx={{ color: COLORS.BLACK_5 }} />
                    {userRoleId === UserRoleEnum.Student ? (
                        <MenuItem onClick={handleMatchingTutorsClick} icon={<MatchingTutorsIcon />} text={t(TRANSLATION_KEYS.PROFILE_ICON.MATCHING_TUTORS)} />
                    ) : <></>}
                    {userRoleId === UserRoleEnum.Student ? (
                        <Divider sx={{ color: COLORS.BLACK_5 }} />
                    ) : <></>}
                    <MenuItem onClick={handleLogOut} icon={<LogOutIcon />} text={t(TRANSLATION_KEYS.PROFILE_ICON.LOGOUT)} />
                </MenuBoxStyled>
            </MenuStyled>
        </MainBoxStyled>
    )
}

export default UserInfo;