import { createContext, useEffect, useRef, useState } from "react";
import { useTranslation } from "components/core/TranslationProvider";
import { COLORS } from "../styles/constants/ColorsConstatnts";
import { useAppDispatch, contextFactoryCreate, useAppSelector, useUpdateEffect } from "core/redux/reduxHooks";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import ModalsHeader from "./common/ModalsHeader";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { useSnackbar } from "notistack";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import CircularProgress from "components/styles/CircularProgressStyled";
import { deleteTimeslot, selectDeleteAppTimeslotStatus } from "core/redux/timeslotSlice";
import { ModalBoxSmallStyled } from "components/styles/boxes/ModalBoxSmallStyled";
import Box from "@mui/material/Box";
import { ModalStyled } from "components/styles/ModalStyled";
import Body1Styled from "components/styles/typography/Body1Styled";

const DescriptionBoxStyled = styled(Box)({
    width: "100%",
    display: "inline-flex",
    padding: "16px 0",
    alignItems: "center"
}) as typeof Box;

const PrimaryButtonRectangleWarningStyled = styled(PrimaryButtonRectangle)({
    backgroundColor: COLORS.RED,
    padding: "12px 24px",
    ":hover": { backgroundColor: COLORS.RED_HOVER }
}) as typeof PrimaryButtonRectangle;

type CancelTimeslotModalContextType = {
    setOpen: () => void
} | undefined

const CancelTimeslotModalContext = createContext<CancelTimeslotModalContextType>(undefined);
export const useCancelTimeslotModalContext = contextFactoryCreate('CancelTimeslotModal', CancelTimeslotModalContext);

const CancelTimeslotModalProvider = ({ timeslotId, onSubmit, children }: { timeslotId: number, onSubmit?: () => Promise<void> | void, children: any }) => {
    const [open, setOpen] = useState(false);
    const { enqueueSnackbar } = useSnackbar();
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const deleteStatus = useAppSelector(selectDeleteAppTimeslotStatus);
    const submitted = useRef(false);

    useUpdateEffect(() => {
        if (!submitted.current) return;
        if (deleteStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.DELETE_EVENT.SUCCESS), { variant: "success" });
        }
        else if (deleteStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.DELETE_EVENT.ERROR), { variant: "error" });
        }
    }, [deleteStatus]);

    const handleClose = () => {
        setOpen(false);
    }

    const handleOpen = () => {
        setOpen(true);
    }

    const handleDelete = () => {
        submitted.current = true;
        
        const callback = async () => {
            await dispatch(deleteTimeslot({ timeslotId }));
            if (onSubmit) {
                await onSubmit();
            }
            handleClose();
        }
        
        callback();
    }

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    });

    const handleKeydown = (e: KeyboardEvent) => {
        if (e.repeat) {
            return;
        }
        if (!open) return;
        if (e.key === 'Enter') {
            handleDelete();
        }
    }

    return (
        <CancelTimeslotModalContext.Provider value={{
            setOpen: handleOpen
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <ModalBoxSmallStyled>
                    <ModalsHeader text={t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.DELETE_EVENT.TITLE)} handleClose={handleClose} />
                    <DescriptionBoxStyled>
                        <Body1Styled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.DELETE_EVENT.DELETE_CONFIRMATION)}</Body1Styled>
                    </DescriptionBoxStyled>
                    <PrimaryButtonRectangleWarningStyled onClick={handleDelete}>
                        {deleteStatus !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.COMMON.DELETE) : <></>}
                        {deleteStatus === LoadingStatusEnum.Loading ? <CircularProgress /> : <></>}
                    </PrimaryButtonRectangleWarningStyled>
                </ModalBoxSmallStyled>
            </ModalStyled>
        </CancelTimeslotModalContext.Provider>
    )
}

export default CancelTimeslotModalProvider;