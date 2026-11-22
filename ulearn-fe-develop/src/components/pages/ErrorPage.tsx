import Box from "@mui/material/Box";
import { styled } from "@mui/material/styles";
import TranslationsProvider from "components/core/TranslationProvider";
import ErrorLarge from "components/core/error/ErrorLarge";
import { ContainerStyled } from "components/styles/ContainerStyled";
import { useAppDispatch } from "core/redux/reduxHooks";
import { logOut } from "core/services/Utils/authUtil";
import { useRouter } from "next/router";
import { useEffect } from "react";

const ErrorBoxStyled = styled(Box)({
    display: "flex",
    alignItems: "center",
    height: "100vh"
}) as typeof Box;

const ErrorView = () => {
    const dispatch = useAppDispatch();

    const handleLogOut = () => {
        logOut(dispatch);
    }

    return (
        <ContainerStyled>
            <ErrorBoxStyled>
                <ErrorLarge onClick={handleLogOut}/>
            </ErrorBoxStyled>
        </ContainerStyled>
    )
}

const ErrorPage = () => {
    const { locale } = useRouter();

    return (
        <TranslationsProvider
            locale={locale ?? ""}
        >
            <ErrorView />
        </TranslationsProvider>
    )
}

export default ErrorPage;