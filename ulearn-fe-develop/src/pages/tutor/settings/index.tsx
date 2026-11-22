import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";
import React, { useEffect } from "react";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import TutorProfileSettings from "components/registration/TutorProfileSettings";
import Navbar, { NavbarSourceEnum } from "components/navbar/Navbar";
import { useAppSelector } from "core/redux/reduxHooks";
import { selectShortUserInfo } from "core/redux/userSlice";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import routes from "routes/routes";

const TutorViewBoxStyled = styled(Box)(() => sx({
    height: "100%",
    width: "100%",
    alignItems: "center",
    display: "flex",
    flexFlow: "column noWrap",
    overflow: "auto"
})) as typeof Box;

const TutorProfileSettingsPage = () => {
    const userInfo = useAppSelector(selectShortUserInfo);
    
    useEffect(() => {
        if (!userInfo) return;
        if (userInfo.stateId !== UserStateEnum.Confirmed) {
            location.replace(routes.tutorRegistrationForm.path);
        }
    }, [userInfo]);

    return (
        <TutorViewBoxStyled>
            <Box sx={{ width: "100%", display: "flex", flexFlow: "row noWrap", flex: "0 1 auto" }}>
                <Navbar source={NavbarSourceEnum.tutorPage} />
            </Box>
            <TutorProfileSettings pageType="settings" />
        </TutorViewBoxStyled>
    )
}

export default withAppClientSideProvider(TutorProfileSettingsPage);