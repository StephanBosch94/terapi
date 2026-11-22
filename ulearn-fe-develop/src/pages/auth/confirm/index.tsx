import { styled } from "@mui/material/styles";
import React, { useEffect } from "react";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import ConfirmEmailBox from "components/ConfirmEmailBox";
import NavbarWithoutLinks from "components/navbar/NavbarWithoutLinks";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";
import { useAppSelector } from "core/redux/reduxHooks";
import { selectShortUserInfo } from "core/redux/userSlice";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import routes from "routes/routes";
import LargeModalBoxStyled from "components/styles/boxes/LargeModalBoxStyled";

const StyledContainer = styled(Container)(() => ({
    display: "flex",
    alignItems: "center",
    justifyContent: "center"
})) as typeof Container;

const BackgroundBoxStyled = styled(Box)(() => ({
    backgroundColor: COLORS.PRIMARY_10,
    display: "flex",
    flexFlow: "row nowrap",
    width: "100%",
    height: "100%",
    flex: "1 1 auto",
})) as typeof Box;

const ConfirmEmail = () => {
    const shortUserInfo = useAppSelector(selectShortUserInfo);

    useEffect(() => {
        if (shortUserInfo?.stateId && shortUserInfo.stateId !== UserStateEnum.EmailVerification) {
            location.replace(routes.home.path);
        }
    }, [shortUserInfo]);

    const handleSuccess = () => {
        location.replace(routes.setPassword.path);
    }

    return (
        <>
            <NavbarWithoutLinks />
            <BackgroundBoxStyled>
                <StyledContainer>
                    <LargeModalBoxStyled>
                        <ConfirmEmailBox onSuccess={handleSuccess} />
                    </LargeModalBoxStyled>
                </StyledContainer>
            </BackgroundBoxStyled>
        </>
    );
}

export default withAppClientSideProvider(ConfirmEmail);