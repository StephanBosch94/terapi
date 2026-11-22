import Badge from "@mui/material/Badge"
import { useAppSelector } from "core/redux/reduxHooks";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import React from "react";
import ChatIcon from "assets/chat.svg";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import router from "next/router";
import ButtonUnstyled from '@mui/base/ButtonUnstyled';
import routes from "routes/routes";
import { selectShortUserInfo } from "core/redux/userSlice";
import Box from "@mui/material/Box";

const StyledIcon = styled(Box)(() => sx({
    color: COLORS.BLACK,
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        transform: "scale(1.25)"
    }
})) as typeof Box;

const NavbarButtonStyled = styled(ButtonUnstyled)(() => sx({
    color: COLORS.BLACK,
    fontSize: "1rem",
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

const ChatBadgeMobile = () => {
    const shortUserInfo = useAppSelector(selectShortUserInfo);
    const handleMessagesClick = () => {
        router.push(routes.messagesPage.path);
    }

    return shortUserInfo ? (
        <React.Fragment>
            <NavbarButtonStyled onClick={handleMessagesClick}>
                <Badge badgeContent={shortUserInfo.countOfUnreadMessages ?? 0} color="error" variant="dot">
                    <StyledIcon><ChatIcon /></StyledIcon>
                </Badge>
            </NavbarButtonStyled>
        </React.Fragment>
    ) : <></>
}

export default ChatBadgeMobile;