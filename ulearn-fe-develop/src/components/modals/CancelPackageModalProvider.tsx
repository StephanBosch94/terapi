import { ReservationFilterResponse } from "core/redux/reservationSlice";
import { createContext, useEffect, useRef, useState } from "react";
import { getUserName } from "core/services/Utils/userUtils";
import { useTranslation } from "components/core/TranslationProvider";
import { getProfilePhotoUrl } from "core/services/Utils/imageUtil";
import { COLORS } from "../styles/constants/ColorsConstatnts";
import { useAppDispatch, contextFactoryCreate, useAppSelector, useUpdateEffect } from "core/redux/reduxHooks";
import { ModalBoxStyled } from "../styles/boxes/ModalBoxStyled";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import ModalsHeader from "./common/ModalsHeader";
import Body1Styled from "components/styles/typography/Body1Styled";
import H5Styled from "components/styles/typography/H5Styled";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { TextareaBoxStyled } from "components/styles/boxes/TextareaBoxStyled";
import { useSnackbar } from "notistack";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import CircularProgress from "components/styles/CircularProgressStyled";
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import { ModalStyled } from "components/styles/ModalStyled";
import { deleteReservationPackage, selectDeleteReservationPackageStatus } from "core/redux/reservationPackageSlice";
import { snackbarConstants } from "components/core/Snackbar";
import { usePriceLocalization } from "core/hooks/priceHook";

const AvatarStyled = styled(Avatar)({
    width: "48px",
    height: "48px"
}) as typeof Avatar;

const ModalUserBoxStyled = styled(Box)({
    display: "flex",
    flexDirection: "row",
    alignItems: "center",
    gap: "12px"
}) as typeof Box;

const PrimaryButtonRectangleWarningStyled = styled(PrimaryButtonRectangle)({
    width: "100%",
    backgroundColor: COLORS.RED,
    padding: "12px 24px",
    ":hover": { backgroundColor: COLORS.RED_HOVER }
}) as typeof PrimaryButtonRectangle;

type CancelLessonModalContextType = {
    setOpen: () => void
} | undefined

const CancelLessonModalContext = createContext<CancelLessonModalContextType>(undefined);
export const useCancelPackageModalContext = contextFactoryCreate('CancelLessonModal', CancelLessonModalContext);

const CancelPackageModalProvider = ({ reservation, onSubmit, children }: { reservation: ReservationFilterResponse["data"][0], onSubmit?: () => Promise<void> | void, children: any }) => {
    const [open, setOpen] = useState(false);
    const { enqueueSnackbar } = useSnackbar();
    const { localize } = usePriceLocalization();
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const deleteStatus = useAppSelector(selectDeleteReservationPackageStatus);
    const submitted = useRef(false);
    const formRef = useRef<HTMLFormElement>(null);

    useUpdateEffect(() => {
        if (!submitted.current) return;
        if (deleteStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.CANCEL_PACKAGE_MODAL.CANCEL_SUCCESS), { variant: "success" });
        }
        else if (deleteStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.CANCEL_PACKAGE_MODAL.FAILED_TO_CANCEL), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
    }, [deleteStatus]);

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    });

    const handleClose = () => {
        setOpen(false);
    }

    const handleOpen = () => {
        setOpen(true);
    }

    const handleSubmitAsync = async (text: string) => {
        await dispatch(deleteReservationPackage({
            reservationPackageId: reservation.reservationPackageId ?? 0,
            reasonText: text
        }));

        if (onSubmit) {
            await onSubmit();
        }
        handleClose();
    }

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        submitted.current = true;
        const formData = new FormData(event.currentTarget);
        const fieldValues = Object.fromEntries(formData.entries());
        const text = fieldValues["text"] as string;
        handleSubmitAsync(text);
    }

    const handleKeydown = (e: KeyboardEvent) => {
        if (e.repeat) {
            return;
        }
        if (!open) return;
        if (e.key === 'Enter' && formRef.current) {
            formRef.current.submit();
        }
    }

    return (
        <CancelLessonModalContext.Provider value={{
            setOpen: handleOpen
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <form onSubmit={handleSubmit} ref={formRef}>
                    <ModalBoxStyled>
                        <ModalsHeader text={t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.CANCEL_PACKAGE_MODAL.TITLE)} handleClose={handleClose} />
                        <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "12px", b: { color: COLORS.BLACK } }}>
                            <Body1Styled
                                dangerouslySetInnerHTML={{ __html: t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.CANCEL_PACKAGE_MODAL.DESCRIPTION, { count: reservation.remainingInPackage ?? 0 }) }}
                            />
                            <Body1Styled>{t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.CANCEL_PACKAGE_MODAL.WARNING)}</Body1Styled>
                        </Box>
                        <ModalUserBoxStyled>
                            <AvatarStyled src={getProfilePhotoUrl(reservation.opponent)} />
                            <H5Styled>{t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.CANCEL_PACKAGE_MODAL.REMAINING_LESSONS_WITH_OPPONENT, { count: reservation.remainingInPackage ?? 0, opponent: getUserName(reservation.opponent) })}</H5Styled>
                        </ModalUserBoxStyled>

                        <TextareaBoxStyled>
                            <textarea name="text" placeholder={t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.CANCEL_PACKAGE_MODAL.TEXT_PLACEHOLDER)} />
                        </TextareaBoxStyled>

                        <Body1Styled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.CANCEL_PACKAGE_MODAL.REFUND_TEXT, { amount: localize({ price: reservation.refundPrice ?? 0 }) })}</Body1Styled>
                        <PrimaryButtonRectangleWarningStyled type="submit">
                            {deleteStatus !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.CANCEL_PACKAGE_MODAL.CANCEL_BUTTON) : <></>}
                            {deleteStatus === LoadingStatusEnum.Loading ? <CircularProgress /> : <></>}
                        </PrimaryButtonRectangleWarningStyled>
                    </ModalBoxStyled>
                </form>
            </ModalStyled>
        </CancelLessonModalContext.Provider>
    )
}

export default CancelPackageModalProvider;