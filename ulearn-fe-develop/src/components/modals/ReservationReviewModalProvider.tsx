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
import Avatar from "@mui/material/Avatar";
import { ReservationFilterResponse } from "core/redux/reservationSlice";
import StarsRating from "components/profile/reviews/StarsRating";
import { useSnackbar } from "notistack";
import { PrimaryButton } from "components/styles/PrimaryButton";
import { snackbarConstants } from "components/core/Snackbar";
import { createReservationReview, editReservationReview, selectCreateReservationReviewStatus, selectEditReservationReviewStatus } from "core/redux/reservationReviewSlice";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import { getFormattedDate } from "core/services/Utils/reservationUtil";
import { ModalStyled } from "components/styles/ModalStyled";
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

type ReservationReviewContextType = {
    setOpen: (selectedRating?: number | undefined) => void,
    selectedRating: number | undefined
} | undefined

type ModalType = "create_review" | "edit_review"
type ModelStaticData = {
    type: ModalType,
    titleKey: string,
    buttonKey: string
}
const modalStaticData: ModelStaticData[] = [
    { type: "create_review", titleKey: TRANSLATION_KEYS.PROFILE.RESERVATIONS.REVIEW.TITLE, buttonKey: TRANSLATION_KEYS.PROFILE.RESERVATIONS.REVIEW.SEND_REVIEW_BUTTON },
    { type: "edit_review", titleKey: TRANSLATION_KEYS.PROFILE.RESERVATIONS.REVIEW.EDIT_TITLE, buttonKey: TRANSLATION_KEYS.PROFILE.RESERVATIONS.REVIEW.SAVE_BUTTON },
]

const ReservationReviewContext = createContext<ReservationReviewContextType>(undefined);
export const useReservationReviewContext = contextFactoryCreate('ReservationReview', ReservationReviewContext);

const ReservationReviewModalProvider = ({ reservation, onSubmit, children }: { reservation: ReservationFilterResponse["data"][0], onSubmit?: () => void, children: any }) => {
    const [open, setOpen] = useState(false);
    const [rating, setRating] = useState(reservation.reservationReview?.value ?? 0);
    const [text, setText] = useState("");
    const snackbarPending = useRef(false);
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const createStatus = useAppSelector(selectCreateReservationReviewStatus);
    const updateStatus = useAppSelector(selectEditReservationReviewStatus);
    const status = getLoadingStatus([createStatus, updateStatus]);

    const { enqueueSnackbar } = useSnackbar();
    const mode: ModalType = reservation.reservationReview ? "edit_review" : "create_review";
    const saticData = modalStaticData.find(x => x.type === mode)!;

    useEffect(() => {
        if (!snackbarPending.current) return;
        if (createStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.REVIEW.REVIEW_SENT), { variant: "success" });
            snackbarPending.current = false;
        }
        else if (createStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.REVIEW.FAILED_TO_SENT_REVIEW), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            snackbarPending.current = false;
        };
    }, [createStatus]);

    useEffect(() => {
        if (!snackbarPending.current) return;
        if (updateStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.REVIEW.REVIEW_WAS_SUCCESSUFULLY_UPDATED), { variant: "success" });
            snackbarPending.current = false;
        }
        else if (updateStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.REVIEW.FAILED_TO_UPDATE_REVIEW), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            snackbarPending.current = false;
        };
    }, [updateStatus]);

        const handleClose = () => {
            setRating(reservation.reservationReview?.value ?? 0);
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

        if (reservation.reservationReview) {
            await dispatch(editReservationReview({
                reservationReviewId: reservation.reservationReview.reservationReviewId!,
                reservationId: reservation.reservationId!,
                textValue: text,
                value: rating
            }));
        }
        else {
            await dispatch(createReservationReview({
                reservationId: reservation.reservationId!,
                textValue: text,
                value: rating
            }));
        }

        if (onSubmit) {
            onSubmit();
        }

        setOpen(false);
    }

    return (
        <ReservationReviewContext.Provider value={{
            setOpen: handleOpen,
            selectedRating: rating
        }}
        >
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <ModalBoxStyled >
                    <ModalsHeader text={t(saticData.titleKey)} handleClose={handleClose} />
                    <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "32px" }}>
                        <Box sx={{ display: "flex", flexFlow: "row noWrap", gap: "16px", alignItems: "flex-start" }}>
                            <AvatarStyled src={getProfilePhotoUrl(reservation.opponent)} />
                            <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "4px" }}>
                                <H5Styled>{getUserName(reservation.opponent)}</H5Styled>
                                <CaptionTextStyled>{getFormattedDate(reservation.startTime, reservation.endTime)}</CaptionTextStyled>
                                <StarsRating rating={rating} onClick={handleSetRating} />
                            </Box>
                        </Box>
                        <TextBoxStyled>
                            <textarea name="text" placeholder={t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.REVIEW.PLACEHOLDER)} value={text} onChange={handleChange} />
                        </TextBoxStyled>
                        <PrimaryButtonStyled onClick={handleSubmit}>
                            {status !== LoadingStatusEnum.Loading ? t(saticData.buttonKey) : <CircularProgressCustom color={COLORS.WHITE} />}
                        </PrimaryButtonStyled>

                    </Box>
                </ModalBoxStyled>
            </ModalStyled>
        </ReservationReviewContext.Provider>
    )
}

export default ReservationReviewModalProvider;