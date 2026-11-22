    import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { snackbarConstants } from "components/core/Snackbar";
import { useTranslation } from "components/core/TranslationProvider";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { ModalStyled } from "components/styles/ModalStyled";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { TextFieldStyled } from "components/styles/TextFieldStyled";
import { ModalBoxStyled } from "components/styles/boxes/ModalBoxStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { enqueueSnackbar, useSnackbar } from "notistack";
import { CSSProperties, createContext, useEffect, useRef, useState } from "react";
import { setEmployeesFilter } from "core/redux/userSlice";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import { selectInviteColleaguesStatus, setInviteColleagues } from "core/redux/registrationSlice";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";
import { CodetableWeeklyGoalReponse, selectWeeklyGoals, selectWeeklyGoalsStatus, setWeeklyGoals } from "core/redux/codetableSlice";
import ErrorLarge from "components/core/error/ErrorLarge";
import ModalsHeader from "./common/ModalsHeader";
import { InviteColleaguesRequest } from "core/services/RegistrationService";
import WeeklyGoalInputField from "./common/WeeklyGoal";

const PrimaryButtonActiveStyled = styled(PrimaryButtonRectangle)(() => sx({
    width: "100%"
})) as typeof PrimaryButtonRectangle;

const CaptionBlackStyled = styled(CaptionTextStyled)(() => sx({
    color: COLORS.BLACK
})) as typeof CaptionTextStyled;

const TextFieldBoxStyled = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "space-between",
    width: "100%",
    flexFlow: "column noWrap",
    gap: "4px"
})) as typeof Box;

const CenterBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "16px"
})) as typeof Box;

const formStyles: CSSProperties = {
    backgroundColor: COLORS.WHITE,
    display: "flex",
    flexDirection: "column",
    gap: "40px"
}

type WeeklyGoalIdType = InviteColleaguesRequest['weeklyGoalId'];
type InvitePeopleModalContextType = {
    setOpen: () => void
} | undefined;

const InvitePeopleModalContext = createContext<InvitePeopleModalContextType>(undefined);

const parseEmails = (emails: string) => {
    return emails
        .split(/[ ,]+/)
        .filter(x => x.includes("@"))
        .filter(x => x !== "")
}

const inputFieldNames = {
    emails: "emails",
    weeklyGoal: "weekly_goal"
}

const EmailsInputField = () => {
    const { t } = useTranslation();

    return (
        <TextFieldBoxStyled>
            <CaptionBlackStyled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.INVITE_PEOPLE_MODAL.INVITE_BY_EMAIL)}</CaptionBlackStyled>
            <TextFieldStyled type="text" name={inputFieldNames.emails} placeholder={t(TRANSLATION_KEYS.COMPANY_DASHBOARD.INVITE_PEOPLE_MODAL.INVITE_BY_EMAIL_PLACEHOLDER)} />
        </TextFieldBoxStyled>
    )
}

const InvitePeopleModalProviderLoaded = ({ handleClose, weeklyGoals }: { handleClose: () => void, weeklyGoals: CodetableWeeklyGoalReponse }) => {
    const { t } = useTranslation();
    const setInviteStatus = useAppSelector(selectInviteColleaguesStatus);
    const { enqueueSnackbar } = useSnackbar();
    const formRef = useRef<HTMLFormElement>(null);
    const submitted = useRef(false);
    const dispatch = useAppDispatch();

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    });

    useEffect(() => {
        if (!submitted.current) return;
        if (setInviteStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.COMPANY_DASHBOARD.INVITE_PEOPLE_MODAL.SUCCESSFULLY_SENT_INVITATION), { variant: "success" });
            submitted.current = false;
        }
        else if (setInviteStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.COMPANY_DASHBOARD.INVITE_PEOPLE_MODAL.FAILED_TO_SEND_INVITATION), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            submitted.current = false;
        };
    }, [setInviteStatus]);

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        submitted.current = true;

        const formData = new FormData(event.currentTarget);
        const fieldValues = Object.fromEntries(formData.entries());
        const emails = fieldValues[inputFieldNames.emails] as string;
        const weeklyGoal = fieldValues[inputFieldNames.weeklyGoal] as string;
        const weeklyGoalId = parseInt(weeklyGoal) as WeeklyGoalIdType;

        const parsedEmails = parseEmails(emails);
        await dispatch(setInviteColleagues({ emails: parsedEmails, weeklyGoalId }));
        await dispatch(setEmployeesFilter({
            appendType: AppendTypeEnum.None,
            request: {
                skip: 0,
                take: 200
            }
        }))
        handleClose();
    }

    const handleKeydown = (e: KeyboardEvent) => {
        if (e.repeat) {
            return;
        }
        if (!open) return;
        if (e.key === 'Enter') {
            formRef.current?.submit();
        }
    }

    return (
        <>
            <form ref={formRef} onSubmit={handleSubmit} style={formStyles}>
                <ModalsHeader text={t(TRANSLATION_KEYS.COMPANY_DASHBOARD.INVITE_PEOPLE_MODAL.TITLE)} handleClose={handleClose} />
                <CenterBoxStyled>
                    <EmailsInputField />
                    <WeeklyGoalInputField weeklyGoals={weeklyGoals} name={inputFieldNames.weeklyGoal} />
                </CenterBoxStyled>
                <PrimaryButtonActiveStyled type="submit">
                    {setInviteStatus !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.COMPANY_DASHBOARD.INVITE_PEOPLE_MODAL.INVITE_PEOPLE_BUTTON) : <CircularProgressCustom color={COLORS.WHITE} />}
                </PrimaryButtonActiveStyled>
            </form>
        </>
    )
}

const InvitePeopleModalProviderError = () => (
    <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center" }}>
        <ErrorLarge />
    </Box>
)

const InvitePeopleModalProviderLoading = () => (
    <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", height: "200px" }}>
        <CircularProgressCustom />
    </Box>
)

const InvitePeopleModalProvider = ({ children }: { children: any }) => {
    const [open, setOpen] = useState(false);
    const dispatch = useAppDispatch();
    const weeklyGoalsStatus = useAppSelector(selectWeeklyGoalsStatus);
    const weeklyGoals = useAppSelector(selectWeeklyGoals);

    const handleClose = () => {
        setOpen(false);
    }

    const handleOpen = () => {
        setOpen(true);
    }

    useEffect(() => {
        dispatch(setWeeklyGoals());
    }, []);

    return (
        <InvitePeopleModalContext.Provider value={{
            setOpen: handleOpen
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose} >
                <ModalBoxStyled>
                    {weeklyGoalsStatus === LoadingStatusEnum.Error ? <InvitePeopleModalProviderError /> : <></>}
                    {weeklyGoalsStatus === LoadingStatusEnum.Loading ? <InvitePeopleModalProviderLoading /> : <></>}
                    {weeklyGoalsStatus === LoadingStatusEnum.Loaded && weeklyGoals ? <InvitePeopleModalProviderLoaded weeklyGoals={weeklyGoals} handleClose={handleClose} /> : <></>}
                </ModalBoxStyled>
            </ModalStyled>
        </InvitePeopleModalContext.Provider >
    )
}

export const useInvitePeopleModalContext = contextFactoryCreate('InvitePeopleModal', InvitePeopleModalContext);
export default InvitePeopleModalProvider;