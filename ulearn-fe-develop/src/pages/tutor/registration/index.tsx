import { styled, experimental_sx as sx } from "@mui/material/styles";
import React, { useEffect } from "react";
import Box from "@mui/material/Box";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import NavbarWithoutLinks from "components/navbar/NavbarWithoutLinks";
import routes from 'routes/routes';
import { useAppSelector } from "core/redux/reduxHooks";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import { selectUserInfo } from "core/redux/userSlice";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import TutorProfileSettings from "components/registration/TutorProfileSettings";

const TutorViewBoxStyled = styled(Box)(() => sx({
    height: "100%",
    width: "100%",
    alignItems: "center",
    display: "flex",
    flexFlow: "column noWrap",
    overflow: "auto"
})) as typeof Box;

const TutorRegistration = () => {
    const userInfo = useAppSelector(selectUserInfo);

    useEffect(() => {
        if (userInfo?.stateId === UserStateEnum.Confirmed) {
            location.replace(routes.tutorDashboard.path);
        }
        else if (userInfo?.stateId === UserStateEnum.ProfileValidation) {
            location.replace(routes.tutorVerification.path);
        }
    }, [userInfo]);

    return (
        <TutorViewBoxStyled>
            <Box sx={{ width: "100%", display: "flex", flexFlow: "row noWrap", flex: "0 1 auto" }}>
                <NavbarWithoutLinks />
            </Box>
            <TutorProfileSettings pageType="registration" />
        </TutorViewBoxStyled>
    )
}

export default withAppClientSideProvider(TutorRegistration);