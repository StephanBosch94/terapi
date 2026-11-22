import Badge from "@mui/material/Badge"
import { useAppSelector } from "core/redux/reduxHooks";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import React from "react";
import ChatIcon from "assets/chat.svg";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import ButtonUnstyled from '@mui/base/ButtonUnstyled';
import routes from "routes/routes";
import { selectShortUserInfo } from "core/redux/userSlice";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import HrefLink from "components/core/HrefLink";

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

const ChatBadge = () => {
    const shortUserInfo = useAppSelector(selectShortUserInfo);
    const { t } = useTranslation();

    return (
        <HrefLink href={routes.messagesPage.path}>
            <NavbarButtonStyled>
                <Badge badgeContent={shortUserInfo?.countOfUnreadMessages ?? 0} color="error" variant="dot" overlap="circular">
                    <ChatIcon />
                </Badge>
                {t(TRANSLATION_KEYS.NAVBAR.MESSAGES)}
            </NavbarButtonStyled>
        </HrefLink>
    )
}

export default ChatBadge;