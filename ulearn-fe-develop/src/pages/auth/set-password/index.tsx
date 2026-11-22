import React, { useEffect } from "react";
import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import NavbarWithoutLinks from "components/navbar/NavbarWithoutLinks";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import SetPasswordBox from "components/tutor/SetPasswordBox";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { selectShortUserInfo, setChangePassword } from "core/redux/userSlice";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import routes from "routes/routes";
import { ContainerStyled } from "components/styles/ContainerStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import LargeModalBoxStyled from "components/styles/boxes/LargeModalBoxStyled";

const ContainerCustomStyled = styled(ContainerStyled)(() => sx({
    display: "flex",
    flex: "1 1 auto",
    width: "100%",
    height: "100%",
    minHeight: 0,
    flexFlow: "row nowrap",
    flexGrow: 1,
    gap: "12px"
})) as typeof ContainerStyled;

const BackgroundBoxStyled = styled(Box)(() => ({
    backgroundColor: COLORS.PRIMARY_10,
    display: "flex",
    flexFlow: "row nowrap",
    width: "100%",
    height: "100%",
    flex: "1 1 auto",
})) as typeof Box;


const SetPassword = () => {
    const dispatch = useAppDispatch();
    const shortUserInfo = useAppSelector(selectShortUserInfo);

    useEffect(() => {
        if (shortUserInfo?.stateId && shortUserInfo.stateId !== UserStateEnum.PasswordSetup) {
            location.replace(routes.home.path);
        }
    }, [shortUserInfo]);

    const onClick = async (newPassword: string) => {
        await dispatch(setChangePassword({ newPassword }))
        location.replace(routes.home.path);
    }

    return (
        <Box sx={{ height: "100%", display: "flex", flexFlow: "column noWrap" }}>
            <NavbarWithoutLinks />
            <BackgroundBoxStyled>
                <ContainerCustomStyled>
                    <LargeModalBoxStyled>
                        <SetPasswordBox
                            type="set_new_password"
                            onClick={onClick}
                        />
                    </LargeModalBoxStyled>
                </ContainerCustomStyled>
            </BackgroundBoxStyled>
        </Box>
    )
};

export default withAppClientSideProvider(SetPassword);