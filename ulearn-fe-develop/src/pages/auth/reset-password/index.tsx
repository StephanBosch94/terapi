import React from "react";
import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import NavbarWithoutLinks from "components/navbar/NavbarWithoutLinks";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import SetPasswordBox from "components/tutor/SetPasswordBox";
import { selectResetPasswordStatus, setResetPassword } from "core/redux/userSlice";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { useRouter } from "next/router";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { ContainerStyled } from "components/styles/ContainerStyled";
import routes from "routes/routes";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import LargeModalBoxStyled from "components/styles/boxes/LargeModalBoxStyled";

type PathParams = {
    registrationToken: string;
}

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

const ResetPassword = () => {
    const dispatch = useAppDispatch();
    const router = useRouter();
    const { registrationToken } = router.query as PathParams;

    const onClick = async (newPassword: string) => {
        await dispatch(setResetPassword({ newPassword, token: registrationToken }))
        location.replace(routes.home.path);
    }

    return (
        <Box sx={{ height: "100%", display: "flex", flexFlow: "column noWrap" }}>
            <Box sx={{ display: "inline-flex", flex: "0 1 auto" }}>
                <NavbarWithoutLinks />
            </Box>

            <BackgroundBoxStyled>
                <ContainerCustomStyled>
                    <LargeModalBoxStyled>
                        <SetPasswordBox
                            type="recover_password"
                            onClick={onClick}
                        />
                    </LargeModalBoxStyled>
                </ContainerCustomStyled>
            </BackgroundBoxStyled>
        </Box>
    )
}

export default withAppClientSideProvider(ResetPassword);