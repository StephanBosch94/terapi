import { styled } from "@mui/material/styles";
import React from "react";
import ForgotPasswordBox from "components/ForgotPasswordBox";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import Navbar from "components/navbar/Navbar";
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";

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

const ForgotPassword = () => {

    return (
        <>
            <Navbar />
            <BackgroundBoxStyled>
                <StyledContainer>
                    <ForgotPasswordBox />
                </StyledContainer>
            </BackgroundBoxStyled>
        </>
    )
};

export default withAppClientSideProvider(ForgotPassword);