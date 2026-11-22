import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { useEffect, useState } from "react";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import ButtonUnstyled from '@mui/base/ButtonUnstyled';
import React from "react";
import { useTranslation } from "components/core/TranslationProvider";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import BellIcon from "assets/bell.svg";
import Box from "@mui/material/Box";
import Menu from "components/menu/Menu";
import { selectNotificationsFilter, setNotificationFilter } from "core/redux/notificationSlice";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import Badge from "@mui/material/Badge";
import { selectShortUserInfo} from "core/redux/userSlice";
import Notifications from "./Notifications";

const NavbarButtonStyled = styled(ButtonUnstyled)(() => sx({
    color: COLORS.BLACK,
    fontSize: "0.875rem",
    backgroundColor: "transparent",
    border: "none",
    fontWeight: 500,
    fontFamily: "inherit",
    padding: "8px",
    display: "flex",
    alignItems: "center",
    gap: "8px",
    borderRadius: "4px",
    transition: "none",
    ":hover": {
        backgroundColor: COLORS.PRIMARY_5,
        cursor: "pointer"
    }
}));

const NotificaitonsBoxStyled = styled(Box)(() => sx({
    ":hover": {
        cursor: "pointer"
    }
})) as typeof Box;

const NotificationsMenu = () => {
    const [open, setOpen] = useState(false);
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const notifications = useAppSelector(selectNotificationsFilter);
    const shortUserInfo = useAppSelector(selectShortUserInfo);

    useEffect(() => {
        if (open && !notifications) {
            dispatch(setNotificationFilter({
                appendType: AppendTypeEnum.None,
                request: {
                    skip: 0,
                    take: COMMON_CONSTANTS.DEFAULT_NUMBER_OF_ITEMS_TO_SHOW
                }
            }));
        }
    }, [open]);

    return (
        <Menu
            content={
                <NotificaitonsBoxStyled>
                    <NavbarButtonStyled sx={{ display: "flex", gap: "8px", color: COLORS.BLACK }}>
                        <Badge badgeContent={shortUserInfo?.countOfUnreadNotifications ?? 0} color="error" variant="dot" overlap="circular">
                            <BellIcon />
                        </Badge>
                        {t(TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS_TITLE)}
                    </NavbarButtonStyled>
                </NotificaitonsBoxStyled>
            }
            innerMenu={
               <Notifications />
            }
            onClick={(newValue: boolean) => setOpen(newValue)}
        />
    )
}

export default NotificationsMenu;