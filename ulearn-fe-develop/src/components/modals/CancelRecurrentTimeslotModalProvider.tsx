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
import RadioButton from "components/styles/RadioButton";
import { createRepeatingTimeslotBlacklist, selectCreateRecurrentTimeslotBlacklistStatus } from "core/redux/recurrentTimeslotBlacklistSlice";
import moment from "moment";
import { deleteCreateRecurrentTimeslotStatus, deleteRecurrentTimeslot } from "core/redux/recurrentTimeslotSlice";
import { ModalBoxSmallStyled } from "components/styles/boxes/ModalBoxSmallStyled";
import Box from "@mui/material/Box";
import { ModalStyled } from "components/styles/ModalStyled";
import Body1Styled from "components/styles/typography/Body1Styled";

const CheckboxsBoxStyled = styled(Box)({
    width: "100%",
    display: "flex",
    flexFlow: "column noWrap",
    gap: "16px",
    ":hover": {
        cursor: "pointer"
    }
}) as typeof Box;

const CheckboxBoxStyled = styled(Box)({
    display: "inline-flex",
    alignItems: "center",
    gap: "8px"
}) as typeof Box;

const PrimaryButtonRectangleWarningStyled = styled(PrimaryButtonRectangle)({
    backgroundColor: COLORS.RED,
    padding: "12px 24px",
    ":hover": { backgroundColor: COLORS.RED_HOVER }
}) as typeof PrimaryButtonRectangle;

type CancelTimeslotModalContextType = {
    setOpen: () => void
} | undefined

type DeleteOptionType = "this" | "this_and_following";
type DeleteOptionItemType = { type: DeleteOptionType, translationKey: string };
const DeleteOptions: DeleteOptionItemType[] = [
    {
        type: "this",
        translationKey: TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.DELETE_EVENT.DELETE_THIS_EVENT
    },
    {
        type: "this_and_following",
        translationKey: TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.DELETE_EVENT.DELETE_THIS_EVENT_AND_FOLLOWING
    }
]
const CancelRecurrentTimeslotModalContext = createContext<CancelTimeslotModalContextType>(undefined);
export const useCancelRecurrentTimeslotModalContext = contextFactoryCreate('CancelRecurrentTimeslotModal', CancelRecurrentTimeslotModalContext);

const CancelRecurrentTimeslotModalProvider = ({ recurrentTimeslotId, referenceDate, onSubmit, children }: { recurrentTimeslotId: number, referenceDate: moment.Moment, onSubmit?: () => Promise<void> | void, children: any }) => {
    const [open, setOpen] = useState(false);
    const [deleteOption, setDeleteOption] = useState(DeleteOptions[0]);
    const { enqueueSnackbar } = useSnackbar();
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const deleteRecurrentTimeslotStatus = useAppSelector(deleteCreateRecurrentTimeslotStatus);
    const createRecurrentTimeslotBlacklistStatus = useAppSelector(selectCreateRecurrentTimeslotBlacklistStatus);
    const submitted = useRef(false);

    useUpdateEffect(() => {
        if (!submitted.current) return;
        snackbarSync(deleteRecurrentTimeslotStatus);
    }, [deleteRecurrentTimeslotStatus]);

    useUpdateEffect(() => {
        if (!submitted.current) return;
        snackbarSync(createRecurrentTimeslotBlacklistStatus);
    }, [createRecurrentTimeslotBlacklistStatus]);

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    });

    const snackbarSync = (loadingStatus: LoadingStatusEnum | undefined) => {
        if (loadingStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.DELETE_EVENT.SUCCESS), { variant: "success" });
        }
        else if (loadingStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.DELETE_EVENT.ERROR), { variant: "error" });
        }
    }

    const handleClose = () => {
        setOpen(false);
    }

    const handleOpen = () => {
        setOpen(true);
    }

    const handleRadioButtonClick = (newOption: DeleteOptionItemType) => {
        if (newOption.type === deleteOption.type) return;
        setDeleteOption(newOption);
    }

    const handleDelete = () => {
        const addToBlacklistCallback = async () => {
            await dispatch(createRepeatingTimeslotBlacklist({
                recurrentTimeslotId: recurrentTimeslotId,
                localDay: referenceDate.date(),
                localMonth: referenceDate.month() + 1, //month is zero-based
                localYear: referenceDate.year()
            }))
            if (onSubmit) {
                await onSubmit();
            }
            handleClose();
        }
        const deleteRecurrentEventCallback = async () => {
            await dispatch(deleteRecurrentTimeslot({
                recurrentTimeslotId,
                eventDate: moment(referenceDate).startOf('day').utc().format()
            }));
            if (onSubmit) {
                await onSubmit();
            }
            handleClose();
        }
        submitted.current = true;
        if (deleteOption.type === "this") {
            addToBlacklistCallback();
        }
        else {
            deleteRecurrentEventCallback();
        }
    }

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
        <CancelRecurrentTimeslotModalContext.Provider value={{
            setOpen: handleOpen
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <ModalBoxSmallStyled>
                    <ModalsHeader text={t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.DELETE_EVENT.TITLE)} handleClose={handleClose} />
                    <CheckboxsBoxStyled>
                        {DeleteOptions.map((x, idx) => (
                            <CheckboxBoxStyled key={idx} onClick={() => handleRadioButtonClick(x)}>
                                <RadioButton checked={x.type === deleteOption.type} />
                                <Body1Styled sx={{ color: COLORS.BLACK }}>{t(x.translationKey)}</Body1Styled>
                            </CheckboxBoxStyled>
                        ))}
                    </CheckboxsBoxStyled>
                    <PrimaryButtonRectangleWarningStyled onClick={handleDelete}>
                        {(deleteRecurrentTimeslotStatus !== LoadingStatusEnum.Loading && createRecurrentTimeslotBlacklistStatus !== LoadingStatusEnum.Loading) ? t(TRANSLATION_KEYS.COMMON.DELETE) : <></>}
                        {(deleteRecurrentTimeslotStatus === LoadingStatusEnum.Loading || createRecurrentTimeslotBlacklistStatus === LoadingStatusEnum.Loading) ? <CircularProgress /> : <></>}
                    </PrimaryButtonRectangleWarningStyled>
                </ModalBoxSmallStyled>
            </ModalStyled>
        </CancelRecurrentTimeslotModalContext.Provider>
    )
}

export default CancelRecurrentTimeslotModalProvider;