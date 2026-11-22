import React, { createContext, useEffect, useRef, useState } from "react";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { getUserName } from "core/services/Utils/userUtils";
import { useTranslation } from "components/core/TranslationProvider";
import { getProfilePhotoUrl } from "core/services/Utils/imageUtil";
import { useAppDispatch, useAppSelector, contextFactoryCreate } from "core/redux/reduxHooks";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { ModalBoxStyled } from "../styles/boxes/ModalBoxStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import H5Styled from "components/styles/typography/H5Styled";
import ModalsHeader from "./common/ModalsHeader";
import Box from "@mui/material/Box";
import Modal from "@mui/material/Modal";
import Avatar from "@mui/material/Avatar";
import StarsRating from "components/profile/reviews/StarsRating";
import { useSnackbar } from "notistack";
import { PrimaryButton } from "components/styles/PrimaryButton";
import { snackbarConstants } from "components/core/Snackbar";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { GetMyTutorsResponseBody } from "core/redux/userSlice";
import { createUserReview, editUserReview, selectCreateUserReviewStatus, selectEditUserReviewStatus } from "core/redux/userReviewSlice";
import { getLoadingStatus } from "core/services/Utils/apiUtil";

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

const PrimaryButtonStyled = styled(PrimaryButton)(() => sx({
    borderRadius: "4px"
})) as typeof PrimaryButton;

type UserReviewContextType = {
    setOpen: (selectedRating?: number | undefined) => void,
    selectedRating: number | undefined
} | undefined

const UserReviewContext = createContext<UserReviewContextType>(undefined);
export const useUserReviewContext = contextFactoryCreate('UserReview', UserReviewContext);

type ModalType = "create_review" | "edit_review"
type ModelStaticData = {
    type: ModalType,
    titleKey: string,
    buttonKey: string
}
const modalStaticData: ModelStaticData[] = [
    { type: "create_review", titleKey: TRANSLATION_KEYS.DASHBOARD.REVIEW_MODAL.TITLE, buttonKey: TRANSLATION_KEYS.DASHBOARD.REVIEW_MODAL.SEND_REVIEW_BUTTON },
    { type: "edit_review", titleKey: TRANSLATION_KEYS.DASHBOARD.REVIEW_MODAL.EDIT_TITLE, buttonKey: TRANSLATION_KEYS.DASHBOARD.REVIEW_MODAL.SAVE_BUTTON },
]

const ReservationReviewModalProvider = ({ user, onSubmit, children }: { user: GetMyTutorsResponseBody["data"][0], onSubmit?: () => void, children: any }) => {
    const [open, setOpen] = useState(false);
    const [rating, setRating] = useState(user.userReview?.rate ?? 0);
    const [text, setText] = useState(user.userReview?.comment ?? "");
    const snackbarPending = useRef(false);
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const createStatus = useAppSelector(selectCreateUserReviewStatus);
    const updateStatus = useAppSelector(selectEditUserReviewStatus);
    const mode: ModalType = user.userReview ? "edit_review" : "create_review";
    const staticData = modalStaticData.find(x => x.type === mode)!;
    const status = getLoadingStatus([createStatus, updateStatus]);

    const { enqueueSnackbar } = useSnackbar();

    useEffect(() => {
        setRating(user.userReview?.rate ?? 0);
        setText(user.userReview?.comment ?? "");
    }, [user]);

    useEffect(() => {
        if (!snackbarPending.current) return;
        if (createStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.DASHBOARD.REVIEW_MODAL.REVIEW_SENT), { variant: "success" });
            snackbarPending.current = false;
        }
        else if (createStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.DASHBOARD.REVIEW_MODAL.FAILED_TO_SENT_REVIEW), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            snackbarPending.current = false;
        };
    }, [createStatus]);

    useEffect(() => {
        if (!snackbarPending.current) return;
        if (updateStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.DASHBOARD.REVIEW_MODAL.REVIEW_WAS_SUCCESSUFULLY_UPDATED), { variant: "success" });
            snackbarPending.current = false;
        }
        else if (updateStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.DASHBOARD.REVIEW_MODAL.FAILED_TO_UPDATE_REVIEW), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            snackbarPending.current = false;
        };
    }, [updateStatus]);

    const handleClose = () => {
        setOpen(false);
    }

    const handleOpen = (selectedRating?: number | undefined) => {
        if (selectedRating) {
            setRating(selectedRating);
        }
        setOpen(true);
    }

    const handleSetRating = (newRating: number) => {
        setRating(newRating);
    }

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const value = e.target.value;
        setText(value as string);
    }

    const handleSubmit = async () => {
        snackbarPending.current = true;
        if (!user.userReview) {
            await dispatch(createUserReview({
                userId: user.userId,
                textValue: text,
                value: rating
            }));
        }
        else {
            await dispatch(editUserReview({
                userId: user.userId,
                textValue: text,
                value: rating
            }));
        }

        if (onSubmit) {
            onSubmit();
        }

        handleClose();
    }

    return (
        <UserReviewContext.Provider value={{
            setOpen: handleOpen,
            selectedRating: rating
        }}
        >
            {children}
            <Modal open={open} onClose={handleClose}>
                <ModalBoxStyled >
                    <ModalsHeader text={t(staticData.titleKey)} handleClose={handleClose} />
                    <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "32px"}}>
                        <Box sx={{ display: "flex", flexFlow: "row noWrap", gap: "16px", alignItems: "flex-start" }}>
                            <AvatarStyled src={getProfilePhotoUrl(user)} />
                            <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "4px" }}>
                                <H5Styled>{getUserName(user)}</H5Styled>
                                <StarsRating rating={rating} onClick={handleSetRating} />
                            </Box>
                        </Box>
                        <TextBoxStyled>
                            <textarea name="text" placeholder={t(TRANSLATION_KEYS.DASHBOARD.REVIEW_MODAL.REVIEW_PLACEHOLDER)} value={text} onChange={handleChange} />
                        </TextBoxStyled>
                        <PrimaryButtonStyled onClick={handleSubmit}>
                            {status !== LoadingStatusEnum.Loading ? t(staticData.buttonKey) : <CircularProgressCustom color={COLORS.WHITE} />}
                        </PrimaryButtonStyled>
                    </Box>
                </ModalBoxStyled>
            </Modal>
        </UserReviewContext.Provider>
    )
}

export default ReservationReviewModalProvider;