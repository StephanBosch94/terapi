import React, { createContext, useEffect, useRef, useState } from "react";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { getUserName } from "core/services/Utils/userUtils";
import { useTranslation } from "components/core/TranslationProvider";
import { getProfilePhotoUrl } from "core/services/Utils/imageUtil";
import { useAppDispatch, useAppSelector, contextFactoryCreate, useUpdateEffect } from "core/redux/reduxHooks";
import { createMessageWithAttachment, selectCreateStatus, resetCreateStatus, selectMessagesStatus } from "core/redux/messageSlice";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import CircularProgress from "components/styles/CircularProgressStyled";
import routes from "routes/routes";
import { ModalBoxStyled } from "../styles/boxes/ModalBoxStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import H5Styled from "components/styles/typography/H5Styled";
import ModalsHeader from "./common/ModalsHeader";
import { AppUserCommon } from "core/services/Types/AppTypes";
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import Link from "next/link";
import AuthModalProvider, { useAuthModalContext } from "./AuthModalProvider";
import { selectIsAuthorized } from "core/redux/authSlice";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { ModalStyled } from "components/styles/ModalStyled";
import { selectConversationsStatus } from "core/redux/conversationSlice";
import { enqueueSnackbar } from "notistack";
import { snackbarConstants } from "components/core/Snackbar";
import CircularProgressCustom from "components/styles/CircularProgressStyled";

const AvatarStyled = styled(Avatar)(() => sx({
    width: "48px",
    height: "48px"
})) as typeof Avatar;

const TextBoxStyled = styled(Box)(() => sx({
    width: "100%",
    "textarea": {
        width: "100%",
        border: `1px solid ${COLORS.BLACK_10}`,
        font: "inherit",
        fontSize: "0.9rem",
        p: "8px",
        minHeight: "120px",
        resize: "none",
        borderRadius: "4px"
    }
})) as typeof Box;

type ContactUserModalContextType = {
    setOpen: () => void
} | undefined


const ContactUserModalContext = createContext<ContactUserModalContextType>(undefined);
export const useContactUserModalContext = contextFactoryCreate('ContactTutorPopup', ContactUserModalContext);

const ContactUserModalProvider = ({ userInfo, placeholderKey = TRANSLATION_KEYS.CHAT.WRITE_A_MESSAGE, children }: { userInfo: AppUserCommon, placeholderKey?: string, children: any }) => {
    const [open, setOpen] = useState(false);
    const submitted = useRef(false);
    const { setOpen: setAuthModalOpen } = useAuthModalContext();
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const isAuthorized = useAppSelector(selectIsAuthorized);
    const createMessageStatus = useAppSelector(selectCreateStatus);

    useUpdateEffect(() => {
        if (!submitted.current) return;
        if (createMessageStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.CONTACT_TUTOR_POPUP.MESSAGE_SUCCESSFULLY_SENT), { variant: "success" });
        }
        else if (createMessageStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.CONTACT_TUTOR_POPUP.MESSAGE_SENT_ERROR), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
    }, [createMessageStatus]);

    const handleClose = () => {
        dispatch(resetCreateStatus());
        setOpen(false);
    }

    const handleOpen = () => {
        if (!isAuthorized) {
            setAuthModalOpen(UserRoleEnum.Student, "registration");
        }
        else {
            setOpen(true);
        }
    }

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);
        const fieldValues = Object.fromEntries(formData.entries());
        const text = fieldValues["text"] as string;
        if (!text.length) return;
        submitted.current = true;
        dispatch(createMessageWithAttachment({ opponentId: userInfo.userId!, messageText: text }));
        setOpen(false);
    }

    return (
        <ContactUserModalContext.Provider value={{
            setOpen: handleOpen
        }}
        >
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <ModalBoxStyled>
                    <ModalsHeader text={t(TRANSLATION_KEYS.CONTACT_TUTOR_POPUP.WRITE_A_MESSAGE)} handleClose={handleClose} />
                    <form onSubmit={handleSubmit} style={{ display: "flex", flexFlow: "column noWrap", gap: "32px" }}>
                        <Box sx={{ display: "flex", flexFlow: "row noWrap", gap: "16px", alignItems: "center" }}>
                            <AvatarStyled src={getProfilePhotoUrl(userInfo)} />
                            <H5Styled>{t(TRANSLATION_KEYS.CONTACT_TUTOR_POPUP.CONTACT_TUTOR, { user: getUserName(userInfo) })}</H5Styled>
                        </Box>
                        <TextBoxStyled>
                            <textarea name="text" placeholder={t(placeholderKey)} />
                        </TextBoxStyled>
                        <PrimaryButtonRectangle type="submit" sx={{ width: "100%" }}>
                            {createMessageStatus !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.CONTACT_TUTOR_POPUP.SEND_MESSAGE) : <CircularProgressCustom />}
                        </PrimaryButtonRectangle>
                    </form>
                </ModalBoxStyled>
            </ModalStyled>
        </ContactUserModalContext.Provider>
    )
}

const ContactUserModalProviderWithAuthProvider = ({ userInfo, placeholderKey = TRANSLATION_KEYS.CHAT.WRITE_A_MESSAGE, children }: { userInfo: AppUserCommon, placeholderKey?: string, children: any }) => {

    return (
        <AuthModalProvider>
            <ContactUserModalProvider userInfo={userInfo} placeholderKey={placeholderKey}>
                {children}
            </ContactUserModalProvider>
        </AuthModalProvider>
    )
}

export default ContactUserModalProviderWithAuthProvider;
