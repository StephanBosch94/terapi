import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { ModalStyled } from "components/styles/ModalStyled";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { ModalBoxStyled } from "components/styles/boxes/ModalBoxStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { CSSProperties, createContext, useEffect, useRef, useState } from "react";
import { FilterEmployeesResponseBody, selectEditEmployeeAccountStatus, setEmployeesFilter, updateEmployeeAccountInfo } from "core/redux/userSlice";
import { useSnackbar } from "notistack";
import { snackbarConstants } from "components/core/Snackbar";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";
import ModalsHeader from "./common/ModalsHeader";
import { CodetableWeeklyGoalReponse, selectWeeklyGoals, selectWeeklyGoalsStatus, setWeeklyGoals } from "core/redux/codetableSlice";
import ErrorLarge from "components/core/error/ErrorLarge";
import { InviteColleaguesRequest } from "core/services/RegistrationService";
import WeeklyGoalInputField from "./common/WeeklyGoal";

const PrimaryButtonActiveStyled = styled(PrimaryButtonRectangle)(() => sx({
    width: "100%"
})) as typeof PrimaryButtonRectangle;

type EditStudentModalContextType = {
    setOpen: () => void
} | undefined;

type WeeklyGoalIdType = InviteColleaguesRequest['weeklyGoalId'];

const formStyles: CSSProperties = {
    backgroundColor: COLORS.WHITE,
    display: "flex",
    flexDirection: "column",
    gap: "40px"
}

const inputFieldNames = {
    weeklyGoal: "weekly_goal"
}
const EditEmployeeModalContext = createContext<EditStudentModalContextType>(undefined);

const EditEmployeeModalProviderLoaded = ({ user, weeklyGoals, handleClose }: { user: FilterEmployeesResponseBody['data'][0], weeklyGoals: CodetableWeeklyGoalReponse, handleClose: () => void }) => {
    const { t } = useTranslation();
    const submitted = useRef(false);
    const formRef = useRef<HTMLFormElement>(null);
    const { enqueueSnackbar } = useSnackbar();
    const editAccountStatus = useAppSelector(selectEditEmployeeAccountStatus);
    const dispatch = useAppDispatch();

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    });

    useEffect(() => {
        if (!submitted.current) return;
        if (editAccountStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.COMPANY_DASHBOARD.EDIT_ACCOUNT_MODAL.SUCCESS), { variant: "success" });
            submitted.current = false;
        }
        else if (editAccountStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.COMPANY_DASHBOARD.EDIT_ACCOUNT_MODAL.FAILED), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            submitted.current = false;
        };
    }, [editAccountStatus]);

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        submitted.current = true;

        const formData = new FormData(event.currentTarget);
        const fieldValues = Object.fromEntries(formData.entries());
        const weeklyGoal = fieldValues[inputFieldNames.weeklyGoal] as string;
        const weeklyGoalId = parseInt(weeklyGoal) as WeeklyGoalIdType;

        await dispatch(updateEmployeeAccountInfo({
            userId: user.userId!,
            weeklyGoalId
        }));
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
        <form ref={formRef} onSubmit={handleSubmit} style={formStyles}>
            <ModalsHeader text={t(TRANSLATION_KEYS.COMPANY_DASHBOARD.EDIT_ACCOUNT_MODAL.TITLE)} handleClose={handleClose} />
            <WeeklyGoalInputField weeklyGoals={weeklyGoals} name={inputFieldNames.weeklyGoal} prevValue={user.weeklyGoalId} />
            <PrimaryButtonActiveStyled type="submit">
                {editAccountStatus !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.COMPANY_DASHBOARD.EDIT_ACCOUNT_MODAL.BUTTON) : <CircularProgressCustom color={COLORS.WHITE} />}
            </PrimaryButtonActiveStyled>
        </form>
    )
}

const EditEmployeeModalProviderError = () => (
    <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center" }}>
        <ErrorLarge />
    </Box>
)

const EditEmployeeModalProviderLoading = () => (
    <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", height: "200px" }}>
        <CircularProgressCustom />
    </Box>
)

const EditEmployeeModalProvider = ({ children, user }: { children: any, user: FilterEmployeesResponseBody['data'][0] }) => {
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
        <EditEmployeeModalContext.Provider value={{
            setOpen: handleOpen
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose} >
                <ModalBoxStyled>
                    {weeklyGoalsStatus === LoadingStatusEnum.Error ? <EditEmployeeModalProviderError /> : <></>}
                    {weeklyGoalsStatus === LoadingStatusEnum.Loading ? <EditEmployeeModalProviderLoading /> : <></>}
                    {weeklyGoalsStatus === LoadingStatusEnum.Loaded && weeklyGoals ? <EditEmployeeModalProviderLoaded user={user} weeklyGoals={weeklyGoals} handleClose={handleClose} /> : <></>}
                </ModalBoxStyled>
            </ModalStyled>
        </EditEmployeeModalContext.Provider >
    )
}

export const useEditEmployeeModalContext = contextFactoryCreate('EditEmployeeModal', EditEmployeeModalContext);
export default EditEmployeeModalProvider;