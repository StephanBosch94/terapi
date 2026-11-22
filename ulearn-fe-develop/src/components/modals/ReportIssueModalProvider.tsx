import { ModalBoxSmallStyled } from "components/styles/boxes/ModalBoxSmallStyled";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { createContext, useEffect, useRef, useState } from "react";
import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { ModalStyled } from "components/styles/ModalStyled";
import RadioButton from "components/styles/RadioButton";
import Body1Styled from "components/styles/typography/Body1Styled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import ModalsHeader from "./common/ModalsHeader";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { useTranslation } from "components/core/TranslationProvider";
import { CodetableReservationIssueTypes, selectReservationIssueTypes, selectReservationIssueTypesStatus } from "core/redux/codetableSlice";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import ErrorLarge from "components/core/error/ErrorLarge";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { ModalBoxStyled } from "components/styles/boxes/ModalBoxStyled";
import { TextareaBoxStyled } from "components/styles/boxes/TextareaBoxStyled";
import { createReservationIssue, selectCreateReservationIssueStatus } from "core/redux/reservationIssueSlice";
import { paths } from "apiTypes";
import { useSnackbar } from "notistack";
import { snackbarConstants } from "components/core/Snackbar";

const ModalCenterBoxStyled = styled(Box)({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "16px"
}) as typeof Box;

const RadioButtonBox = styled(Box)(() => sx({
    display: "inline-flex",
    gap: "8px",
    alignItems: "center",
    ":hover": {
        cursor: "pointer",
    }
})) as typeof Box;

const OptionTypographyStyled = styled(Body1Styled)(() => sx({
    color: COLORS.BLACK
})) as typeof Body1Styled;

const PrimaryButtonStyled = styled(PrimaryButtonRectangle)(() => sx({
    width: "100%"
})) as typeof PrimaryButtonRectangle;

type ReportIssueModalContextType = {
    setOpen: () => void,
} | undefined;

const InputNames = {
    text: "text",
    issueTypeId: "issueTypeId",
};
const ReportIssueModalContext = createContext<ReportIssueModalContextType>(undefined);

const ReservationIssueContent = ({ issueTypes }: { issueTypes: CodetableReservationIssueTypes }) => {
    const [issueTypeId, setIssueTypeId] = useState<number | undefined>(issueTypes[0]?.issueTypeId);
    const hasDescription = issueTypes.some(x => x.issueTypeId === issueTypeId && x.hasDescription);
    const { t } = useTranslation();

    return (
        <ModalCenterBoxStyled>
            {issueTypes.map(x => (
                <RadioButtonBox key={x.issueTypeId!} onClick={() => setIssueTypeId(x.issueTypeId!)}>
                    <RadioButton checked={issueTypeId === x.issueTypeId} />
                    <OptionTypographyStyled>{t(x.nameKey ?? "")}</OptionTypographyStyled>
                </RadioButtonBox>
            ))}
            {hasDescription ? (
                <TextareaBoxStyled><textarea name={InputNames.text} placeholder={t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.REPORT_ISSUE_MODAL.PLACEHOLDER)} /></TextareaBoxStyled>
            ) : <></>}
            <input style={{ display: "none" }} type="number" name={InputNames.issueTypeId} value={issueTypeId} readOnly={true} />
        </ModalCenterBoxStyled >
    )
}

const ReservationsLoading = () => {
    return (
        <ModalCenterBoxStyled sx={{ height: "300px", alignItems: "center", justifyContent: "center" }}>
            <CircularProgressCustom color={COLORS.PRIMARY} />
        </ModalCenterBoxStyled>
    )
}

const ReportIssueModalProvider = ({ reservationId, children }: { reservationId: number, children: any }) => {
    const [open, setOpen] = useState(false);
    const { t } = useTranslation();
    const snackbarPending = useRef(false);

    const dispatch = useAppDispatch();
    const reservationIssueTypes = useAppSelector(selectReservationIssueTypes);
    const reservationIssueTypeStatus = useAppSelector(selectReservationIssueTypesStatus);

    const { enqueueSnackbar } = useSnackbar();
    const createIssueStatus = useAppSelector(selectCreateReservationIssueStatus);

    useEffect(() => {
        if (!snackbarPending.current) return;
        if (createIssueStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.REPORT_ISSUE_MODAL.SUCCESSFULLY_CREATED), { variant: "success" });
            snackbarPending.current = false;
            setOpen(false);
        }
        else if (createIssueStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.REPORT_ISSUE_MODAL.FAILED_TO_CREATE), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            snackbarPending.current = false;
            setOpen(false);
        };
    }, [createIssueStatus]);

    const handleOpen = () => {
        setOpen(true);
    }

    const handleClose = () => {
        setOpen(false);
    }

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        
        const formData = new FormData(event.currentTarget);
        const fieldValues = Object.fromEntries(formData.entries());
        const text = fieldValues[InputNames.text] as string;
        const issueTypeId = parseInt(fieldValues[InputNames.issueTypeId] as string) as paths["/api/reservation-issue"]['post']['requestBody']['content']['application/json']['issueTypeId'];

        snackbarPending.current = true;
        dispatch(createReservationIssue({ reservationId, issueTypeId, description: text }));
    }

    return (
        <ReportIssueModalContext.Provider value={{
            setOpen: handleOpen
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <ModalBoxStyled>
                    <form onSubmit={handleSubmit} style={{ display: "flex", flexFlow: "column noWrap", gap: "32px" }}>
                        <ModalsHeader text={t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.REPORT_ISSUE_MODAL.TITLE)} handleClose={handleClose} />
                        {reservationIssueTypeStatus === LoadingStatusEnum.Loading ? <ReservationsLoading /> : <></>}
                        {reservationIssueTypeStatus === LoadingStatusEnum.Error ? <ErrorLarge /> : <></>}
                        {reservationIssueTypeStatus === LoadingStatusEnum.Loaded && reservationIssueTypes ? <ReservationIssueContent issueTypes={reservationIssueTypes} /> : <></>}
                        <PrimaryButtonStyled type="submit">
                            {createIssueStatus !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.REPORT_ISSUE_MODAL.SUBMIT_BUTTON) : <CircularProgressCustom color={COLORS.WHITE} />}
                        </PrimaryButtonStyled>
                    </form>
                </ModalBoxStyled>
            </ModalStyled>
        </ReportIssueModalContext.Provider>
    )
}

export const userReportIssueModalConttext = contextFactoryCreate('ReportIssueModal', ReportIssueModalContext);
export default ReportIssueModalProvider;