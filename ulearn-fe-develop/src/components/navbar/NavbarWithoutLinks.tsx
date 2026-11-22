import { styled } from "@mui/material/styles";
import AppLogo from "assets/logo.svg";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { ModalBoxStyled } from "components/styles/boxes/ModalBoxStyled";
import { useTranslation } from "components/core/TranslationProvider";
import ModalsHeader from "components/modals/common/ModalsHeader";
import PStyled from "components/styles/typography/PStyled";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useAppDispatch } from "core/redux/reduxHooks";
import { logOut } from "core/services/Utils/authUtil";
import { TextButtonStyled } from "components/styles/TextButton";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import { useContext, useState } from "react";
import { ModalStyled } from "components/styles/ModalStyled";
import { ContainerStyled } from "components/styles/ContainerStyled";
import LanguageMenu from "./LanguageMenu";
import { UIContext } from "components/core/UIContextProvider";
import CurrencyMenu from "./CurrencyMenu";

const LogoBoxStyled = styled(Box)({
    ":hover": {
        cursor: "pointer"
    }
}) as typeof Box;

const ButtonsBoxStyled = styled(Box)({
    display: "inline-flex",
    width: "100%",
    justifyContent: "flex-end",
    alignItems: "center",
    gap: "8px"
}) as typeof Box;

const PrimaryButtonRectangleWarningStyled = styled(PrimaryButtonRectangle)({
    backgroundColor: COLORS.RED,
    ":hover": { backgroundColor: COLORS.RED_HOVER }
}) as typeof Button;

const ConfirmationModal = ({ open, onClose }: { open: boolean, onClose: () => void }) => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();

    const handleReset = () => {
        logOut(dispatch);
    }

    return (
        <ModalStyled open={open} onClose={onClose}>
            <ModalBoxStyled>
                <ModalsHeader text={t(TRANSLATION_KEYS.REGISTRATION.EXIT_MODAL.TITLE)} handleClose={onClose} />
                <PStyled>{t(TRANSLATION_KEYS.REGISTRATION.EXIT_MODAL.DESCRIPTION)}</PStyled>
                <ButtonsBoxStyled>
                    <TextButtonStyled onClick={onClose} sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.COMMON.CANCEL)}</TextButtonStyled>
                    <PrimaryButtonRectangleWarningStyled onClick={handleReset} sx={{ backgroundColor: COLORS.RED }}>{t(TRANSLATION_KEYS.REGISTRATION.EXIT_MODAL.BUTTON_TEXT)}</PrimaryButtonRectangleWarningStyled>
                </ButtonsBoxStyled>
            </ModalBoxStyled>
        </ModalStyled>
    )
}

const NavbarWithoutLinks = () => {
    const [open, setOpen] = useState(false);
    const { screenType } = useContext(UIContext);

    const handleClick = () => {
        setOpen(true);
    }

    const handleClose = () => {
        setOpen(false);
    }

    return (
        <>
            <AppBar position="static" color={"transparent"} sx={{ boxShadow: "none", borderBottom: `1px solid ${COLORS.BLACK_10}` }}>
                <ContainerStyled>
                    <Box sx={{ display: "inline-flex", justifyContent: "space-between", alignItems: "center", width: "100%" }}>
                        <Toolbar disableGutters variant='dense' sx={{ height: 56 }}>
                            <LogoBoxStyled onClick={handleClick} aria-label="Yaspeak"><AppLogo /></LogoBoxStyled>
                        </Toolbar>
                        <Box sx={{ display: "inline-flex", alignItems: "center", gap: "4px" }}>
                            {screenType === "desktop" ? <CurrencyMenu /> : <></>}
                            <LanguageMenu />
                        </Box>
                    </Box>
                </ContainerStyled>
            </AppBar>
            <ConfirmationModal open={open} onClose={handleClose} />
        </>
    )
}

export default NavbarWithoutLinks;