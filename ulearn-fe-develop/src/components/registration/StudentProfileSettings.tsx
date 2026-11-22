import { useTranslation } from "components/core/TranslationProvider";
import { PrimaryButton } from "components/styles/PrimaryButton";
import { cacheConstants } from "core/constants/cacheConstants";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { selectUserInfo, selectUserInfoStatus, updateProfileSettingsInfo, updateErrors, setShortUserInfo, selectUserInfoErrors, setUserInfo, UserInfoResponse, setChangeLanguage, selectUpdateUserInfoStatus, selectShortUserInfo } from "core/redux/userSlice";
import { getStorage } from "core/services/Core/StorageService";
import { toAppUserIm } from "core/services/mappers/appUserMapper";
import { createContext, useContext, useEffect, useMemo, useRef, useState } from "react";
import { snackbarConstants } from "components/core/Snackbar";
import { useSnackbar } from "notistack";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import ErrorLarge from "components/core/error/ErrorLarge";
import Box from "@mui/material/Box";
import ProfileImageUploader from "components/registration/ProfileImageUploader";
import InputFieldLabel from "components/form/InputFieldLabel";
import TextField from "components/form/TextField";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import H3Styled from "components/styles/typography/H3Styled";
import { useAppDataContext } from "components/core/providers/AppProvider";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import PStyled from "components/styles/typography/PStyled";
import { CodetableInterestsReponse, CodetableSubjectLevelsResponse, CodetableWeeklyGoalReponse, selectInterests, selectInterestsStatus, selectSubjectLevels, selectSubjectLevelsStatus, selectWeeklyGoals, selectWeeklyGoalsStatus, setInterests, setSubjectLevelsAsync, setWeeklyGoals } from "core/redux/codetableSlice";
import { getLoadingStatus } from "core/services/Utils/apiUtil";
import { TabsBoxShrinkedStyled } from "components/styles/boxes/TabsBoxShrinkedStyled";
import { TabItemButtonStyled } from "components/styles/TabItemButtonStyled";
import RegistrationStepBoxStyled from "components/styles/boxes/RegistrationStepBoxStyled";
import { UIContext } from "components/core/UIContextProvider";
import SaveButton from "./SaveButton";
import produce from "immer";
import Typography from "@mui/material/Typography";
import { useRouter } from "next/router";
import { StudentDashboardTabPathParsedType } from "pages/student/dashboard";
import routes from "routes/routes";
import CupIcon from "assets/emoji/cup.svg";
import RocketIcon from "assets/emoji/rocket.svg";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import InjectIcon, { IconCategoryType } from "components/core/icons/InjectIcon";
import { ContainerStyled } from "components/styles/ContainerStyled";
import { CloseIconButton } from "components/styles/CloseIconModalStyled";
import StaticLinearProgress from "components/styles/StaticLinearProgress";
import { TertiaryButton } from "components/styles/TertiaryButton";
import { NotificationsGetSettingsResponse, editNotificationSettings, selectNotificationSettings, selectNotificationSettingsStatus, setNotificationSettings } from "core/redux/notificationSlice";
import { MuiCheckboxStyled } from "components/styles/MuiCheckboxStyled";
import H5Styled from "components/styles/typography/H5Styled";
import Body1Styled from "components/styles/typography/Body1Styled";
import ItemStateButtonBoxStyled from "components/styles/boxes/ItemStateButtonBoxStyled";
import ItemStateButtonStyled from "components/styles/buttons/ItemStateButtonStyled";

const AttributeBoxStyled = styled(Box)({
    display: "flex",
    flexDirection: "column",
    gap: "4px"
}) as typeof Box;

const ItemTypography = styled(PStyled)({
    color: "inherit",
    fontWeight: "500"
}) as typeof Typography;

const ItemDescriptionTypography = styled(PStyled)({
    color: COLORS.BLACK_60,
    fontWeight: "400"
}) as typeof Typography;

const PrimaryButtonStyled = styled(PrimaryButton)(() => sx({
    borderRadius: "4px"
})) as typeof PrimaryButton;

const StartLearningButtonStyled = styled(PrimaryButtonStyled)(() => sx({
    display: "inline-flex",
    alignItems: "center",
    gap: "8px"
})) as typeof PrimaryButtonStyled;

const IconBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    alignItems: "center",
    svg: { transform: "scale(1.25)" }
})) as typeof Box;

const IconBoxLargeStyled = styled(Box)(() => sx({
    display: "inline-flex",
    alignItems: "center",
    svg: { transform: "scale(1.5)" }
})) as typeof Box;

const CenterBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "8px",
    width: "100%",
    overflowY: "auto",
    flex: "1 1 auto"
})) as typeof Box;

const LargeIconBoxStyled = styled(Box)(() => ({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        transform: "scale(1.5)"
    }
})) as typeof Box;

const ItemBoxStyled = styled(Box)(() => sx({
    width: "100%",
    "div:hover": {
        cursor: "pointer",
    },
    ".passive": {
        border: `1px solid ${COLORS.BLACK_10}`,
        color: COLORS.BLACK,
        width: "100%",
        display: "inline-flex",
        padding: "12px 16px",
        gap: "16px",
        borderRadius: "4px",
        alignItems: "center",
        justifyContent: "flex-start",
    },
    ".active": {
        border: `1px solid ${COLORS.PRIMARY}`,
        backgroundColor: COLORS.PRIMARY_10,
        width: "100%",
        display: "inline-flex",
        padding: "12px 16px",
        gap: "16px",
        borderRadius: "4px",
        alignItems: "center",
        justifyContent: "flex-start",
    },
    ".passive:hover": {
        border: `1px solid ${COLORS.PRIMARY_LIGHT}`,
        backgroundColor: COLORS.PRIMARY_LIGHT
    },
    ".active:hover": {
    }
})) as typeof Box;

const TertiaryButtonStyled = styled(TertiaryButton)({
    padding: "12px 0px",
    ":hover": {
        backgroundColor: COLORS.WHITE,
        textDecoration: "underline"
    }
}) as typeof TertiaryButton;

const CheckboxStyled = styled(MuiCheckboxStyled)(() => sx({
    color: COLORS.PRIMARY,
    padding: "0px !important"
})) as typeof MuiCheckboxStyled;

const AllAtributesBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "16px"
})) as typeof Box;

const NotificationSettingBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "row noWrap",
    gap: "16px",
    alignItems: "flex-start",
    border: `1px solid ${COLORS.BLACK_10}`,
    padding: "12px 16px",
    borderRadius: "4px",
    ":hover": { cursor: "pointer" }
})) as typeof Box;

type RegistrationStepType = "basicInfo" | "notifications" | "subjectSelect" | "subjectLevel" | "specialization" | "interests" | "weeklyGoal";
type PageType = "registration" | "settings";
type RegistrationStepInfoType = {
    type: RegistrationStepType,
    titleKey: string,
    icon: JSX.Element | null,
    descriptionKey: string | undefined,
    tabKey: string,
    pageTypes: PageType[]
};

const STEPS: RegistrationStepInfoType[] = [
    { type: "basicInfo", tabKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT, titleKey: TRANSLATION_KEYS.STUDENT_SIGN_UP.BASIC_INFO_TITLE, icon: null, descriptionKey: undefined, pageTypes: ["registration", "settings"] },
    { type: "notifications", tabKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.NOTIFICATIONS, titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.NOTIFICATIONS, icon: null, descriptionKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.NOTIFICATIONS_DESCRIPTION, pageTypes: ["settings"] },
    { type: "subjectSelect", tabKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.SUBJECT, titleKey: TRANSLATION_KEYS.STUDENT_SIGN_UP.SUBJECT_SELECT_TITLE, icon: null, descriptionKey: undefined, pageTypes: ["registration", "settings"] },
    { type: "subjectLevel", tabKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_LEVEL, titleKey: TRANSLATION_KEYS.STUDENT_SIGN_UP.SUBJECT_LEVEL_TITLE, icon: null, descriptionKey: undefined, pageTypes: ["registration", "settings"] },
    { type: "specialization", tabKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.SPECIALIZATION, titleKey: TRANSLATION_KEYS.STUDENT_SIGN_UP.SPECIALIZATION_TITLE, icon: null, descriptionKey: undefined, pageTypes: ["registration", "settings"] },
    { type: "interests", tabKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.INTERESTS, titleKey: TRANSLATION_KEYS.STUDENT_SIGN_UP.INTERESTS_TITLE, icon: null, descriptionKey: TRANSLATION_KEYS.STUDENT_SIGN_UP.INTERESTS_DESCRIPTION, pageTypes: ["registration", "settings"] },
    { type: "weeklyGoal", tabKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.WEEKLY_GOAL_TAB, titleKey: TRANSLATION_KEYS.STUDENT_SIGN_UP.WEEKLY_GOAL_TITLE, icon: <CupIcon />, descriptionKey: TRANSLATION_KEYS.STUDENT_SIGN_UP.WEEKLY_GOAL_DESCRIPTION, pageTypes: ["registration", "settings"] },
];

type RegistrationInternalContextType = {
    steps: RegistrationStepInfoType[],
    currentStep: RegistrationStepType,
    stepTitleKey: string,
    stepTabKey: string,
    hasNextStep: boolean,
    progress: number,
    pageType: PageType,
    handleSubmit: (event: React.FormEvent<HTMLFormElement>) => void,
    handleSetStep: (step: RegistrationStepInfoType) => void,
    handleClose?: () => void
} | undefined;

const RegistrationInternalContextTypeContext = createContext<RegistrationInternalContextType>(undefined);
export const useInternalRegistrationContext = contextFactoryCreate('StudentRegistration', RegistrationInternalContextTypeContext);

const InputNames = {
    nextButton: "nextBtn",
    submitButton: "submitBtn",
    firstName: "firstName",
    lastName: "lastName",
    subjectId: "subjectId",
    subjectLevelId: "subjectLevelId",
    specializationIds: "specializationIds",
    interestIds: "interestIds",
    weeklyGoalId: "weeklyGoalId",
    notificationSetting: "notificationSetting"
}
type StepPropsType = {
    userInfo: UserInfoResponse;
}

const RegistrationInternalContextProvider = ({ children, pageType, onFinalSubmit, onClose }: { children: any, pageType: PageType, onFinalSubmit?: () => void, onClose?: () => void }) => {
    const [currentStepType, setCurrentStepType] = useState<RegistrationStepType>(STEPS.filter(x => x.pageTypes.includes(pageType))[0].type);
    const submitted = useRef(false);
    const dispatch = useAppDispatch();
    const shortUserInfo = useAppSelector(selectShortUserInfo);
    const userInfo = useAppSelector(selectUserInfo);
    const userStatus = useAppSelector(selectUserInfoStatus);
    const updateUserInfoStatus = useAppSelector(selectUpdateUserInfoStatus);
    const errorKeys = useAppSelector(selectUserInfoErrors);
    const { enqueueSnackbar } = useSnackbar();
    const steps = useMemo(() => STEPS
        .filter(x => x.pageTypes.includes(pageType))
        .filter(x => !shortUserInfo?.inviterId || x.type !== "weeklyGoal")
        , [pageType]);
    const currentIdx = steps.findIndex(x => x.type === currentStepType);
    const currentStep = steps.find(x => x.type === currentStepType);
    const router = useRouter();
    const { t } = useTranslation();

    useEffect(() => {
        setPreviousStepFromStorage();
    }, []);

    useEffect(() => {
        if (!submitted.current) return;
        if (updateUserInfoStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_SIGN_UP.FAILED_TO_SAVE), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
        else if (pageType === "settings" && updateUserInfoStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_SIGN_UP.SUCCESSFULLY_SAVED), { variant: "success" });
        }
    }, [updateUserInfoStatus])

    useEffect(() => {
        if (!submitted.current) return;
        errorKeys?.forEach(errorKey => {
            enqueueSnackbar(t(errorKey), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        })
    }, [errorKeys]);

    const setPreviousStepFromStorage = async () => {
        if (pageType !== "registration") {
            return;
        }
        const storage = await getStorage();
        const value = await storage.getCommonValue<string>(cacheConstants.studentRegistrationLatestStep);
        if (!value) return;
        const parsedValue = parseInt(value);
        setCurrentStepType(STEPS[parsedValue].type)
    }

    const setStepToStorage = async (newValue: number) => {
        if (pageType !== "registration") {
            return;
        }
        const storage = await getStorage();
        await storage.setCommonValue(cacheConstants.studentRegistrationLatestStep, `${newValue}`);
    }

    const handleNextStep = () => {
        const currentIndex = steps.findIndex(x => x.type === currentStepType);
        const newValue = steps
            .map((step, idx) => ({ step, idx }))
            .filter(x => x.idx > currentIndex)
            .filter(x => x.step.pageTypes.includes(pageType))
            .map(x => x.idx)[0];
        setStepToStorage(newValue);
        setCurrentStepType(steps[newValue].type);
    }

    const handleSetStep = (step: RegistrationStepInfoType) => {
        setCurrentStepType(step.type);
    }

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        submitted.current = true;

        const submitterName = (event.nativeEvent as any).submitter.name;
        if (userStatus !== LoadingStatusEnum.Loaded || !userInfo) return;
        let errors = [] as string[];
        const formData = new FormData(event.currentTarget);
        const fieldValues = Object.fromEntries(formData.entries());

        if (currentStepType === "basicInfo") {
            const firstName = fieldValues[InputNames.firstName] as string;
            const lastName = fieldValues[InputNames.lastName] as string;

            errors = [
                !firstName.length ? TRANSLATION_KEYS.ERRORS.MISSING_FIRST_NAME : undefined,
                !lastName.length ? TRANSLATION_KEYS.ERRORS.MISSING_LAST_NAME : undefined
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    firstName,
                    lastName
                }))
            }
        }
        else if (currentStepType === "notifications") {
            const input = Object.keys(fieldValues)
                .filter(x => x.startsWith(InputNames.notificationSetting))
                .map(x => x.replace(InputNames.notificationSetting + '_', ''))
                .map(x => ({ notificationTypeId: parseInt(x) }));

            dispatch(editNotificationSettings(input))
            dispatch(updateProfileSettingsInfo(toAppUserIm(userInfo))) // Update to see notification snackbar
        }
        else if (currentStepType === "subjectSelect") {
            const subjectId = parseInt(fieldValues[InputNames.subjectId] as string);
            errors = [
                !subjectId ? TRANSLATION_KEYS.ERRORS.MISSING_SUBJECT : undefined
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    subjectId: subjectId > 0 ? subjectId : undefined
                }))
            }
        }
        else if (currentStepType === "subjectLevel") {
            const subjectLevelId = parseInt(fieldValues[InputNames.subjectLevelId] as string);
            errors = [
                !subjectLevelId ? TRANSLATION_KEYS.ERRORS.MISSING_SUBJECT_LEVEL : undefined
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    subjectLevelIds: [subjectLevelId]
                }))
            }
        }
        else if (currentStepType === "specialization") {
            const specializationIdsJSON = fieldValues[InputNames.specializationIds] as string;
            const specializationIds = JSON.parse(specializationIdsJSON) as number[];
            errors = [
                !specializationIds.length ? TRANSLATION_KEYS.ERRORS.MISSING_SPECIALIZATIONS : undefined
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    specializationIds
                }))
            }
        }
        else if (currentStepType === "interests") {
            const interestsJSON = fieldValues[InputNames.interestIds] as string;
            const interestIds = JSON.parse(interestsJSON) as number[];
            const userStateId = submitterName === InputNames.submitButton
                ? UserStateEnum.Confirmed
                : userInfo.stateId;

            errors = [
                !interestIds.length ? TRANSLATION_KEYS.ERRORS.MISSING_INTERESTS : undefined
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    interestIds,
                    userStateId
                }))
            }
        }
        else if (currentStepType === "weeklyGoal") {
            const weeklyGoalId = parseInt(fieldValues[InputNames.weeklyGoalId] as string);
            const userStateId = submitterName === InputNames.submitButton
                ? UserStateEnum.Confirmed
                : userInfo.stateId;

            errors = [
                !weeklyGoalId ? TRANSLATION_KEYS.ERRORS.MISSING_WEEKLY_GOAL : undefined
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    weeklyGoalId,
                    userStateId
                })).then(() => {
                    if (submitterName === InputNames.submitButton) {
                        dispatch(setShortUserInfo());
                    }
                })
            }
        }

        dispatch(updateErrors(errors));

        if (!errors.length) {
            if (submitterName === InputNames.nextButton) {
                handleNextStep();
            }
            if (!hasNextStep() && onFinalSubmit) {
                onFinalSubmit()
                const query: StudentDashboardTabPathParsedType = { tab: "matching_tutors" };
                router.push({
                    pathname: routes.studentDashboard.path,
                    query
                });
            }
        }
    }

    const hasNextStep = () => {
        const index = steps.findIndex(x => x.type === currentStepType);
        return index >= 0 && index < steps.length - 1;
    }

    return (
        <RegistrationInternalContextTypeContext.Provider value={{
            currentStep: currentStepType,
            progress: Math.round((currentIdx ?? 0) * 100 / (steps.length - 1)),
            stepTitleKey: currentStep?.titleKey ?? "",
            stepTabKey: currentStep?.tabKey ?? "",
            steps,
            pageType,
            hasNextStep: hasNextStep(),
            handleSubmit,
            handleSetStep,
            handleClose: onClose
        }}>
            {children}
        </RegistrationInternalContextTypeContext.Provider>
    )
}


const RegistrationControllers = () => {
    const { progress, hasNextStep } = useInternalRegistrationContext();
    const { t } = useTranslation();
    const { handleClose } = useInternalRegistrationContext();

    return (
        <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "24px", flex: "0 1 auto" }}>
            <StaticLinearProgress value={progress} />
            <Box sx={{ display: "flex", alignItems: "center", gap: "8px", justifyContent: "space-between" }}>
                <TertiaryButtonStyled onClick={handleClose}>{t(TRANSLATION_KEYS.COMMON.SKIP_FOR_NOW)}</TertiaryButtonStyled>
                {hasNextStep
                    ? <PrimaryButtonStyled type="submit" name={InputNames.nextButton}>{t(TRANSLATION_KEYS.COMMON.CONTINUE)}</PrimaryButtonStyled>
                    : (
                        <StartLearningButtonStyled type="submit" name={InputNames.submitButton} sx={{ display: "inline-flex", alignItems: "center", gap: "8px" }}>
                            {t(TRANSLATION_KEYS.STUDENT_SIGN_UP.START_LEARNING_BUTTON)}
                            <RocketIcon />
                        </StartLearningButtonStyled>
                    )
                }
            </Box>
        </Box>
    )
}

const RegistrationHeader = () => {
    const { t } = useTranslation();
    const { steps, currentStep, handleClose, pageType } = useInternalRegistrationContext();
    const step = steps.find(x => x.type == currentStep)!;

    const handleCloseOptional = () => {
        if (handleClose) {
            handleClose();
        }
    }

    return (
        <Box sx={{ display: "flex", flexFlow: "row noWrap", justifyContent: "space-between", flex: "0 1 auto" }}>
            <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "16px" }}>
                <Box sx={{ display: "inline-flex", alignItems: "center", gap: "8px" }}>
                    <H3Styled>{t(step.titleKey)}</H3Styled>
                    {step.icon ? <IconBoxLargeStyled>{step.icon}</IconBoxLargeStyled> : <></>}
                </Box>
                {step.descriptionKey ? <CaptionTextStyled sx={{ fontSize: "0.875rem" }}>{t(step.descriptionKey)}</CaptionTextStyled> : <></>}
            </Box>
            {pageType === "registration" ? <CloseIconButton onClick={handleCloseOptional} /> : <></>}
        </Box>
    )
}

const StepLoading = () => {

    return (
        <Box sx={{ display: "flex", minHeight: "300px", alignItems: "center", justifyContent: "center" }}>
            <CircularProgressCustom size={40} />
        </Box>
    )
}

const StepError = () => {

    return (
        <Box sx={{ display: "flex", height: "100%", alignItems: "center", justifyContent: "center" }}>
            <ErrorLarge />
        </Box>
    )
}

const withLoadingAndErrorHandling = <P extends object>(WrappedComponent: React.ComponentType<P & StepPropsType>) => {
    const HOC = (props: any) => {
        const dispatch = useAppDispatch();
        const userInfo = useAppSelector(selectUserInfo);
        const userInfoStatus = useAppSelector(selectUserInfoStatus);

        useEffect(() => {
            dispatch(setUserInfo());
        }, []);

        return (
            <>
                {userInfoStatus === LoadingStatusEnum.Loading ? <StepLoading /> : <></>}
                {userInfoStatus === LoadingStatusEnum.Error ? <StepError /> : <></>}
                {userInfoStatus === LoadingStatusEnum.Loaded && userInfo ? <WrappedComponent userInfo={userInfo} {...props} /> : <></>}
            </>
        )
    };

    const displayName = WrappedComponent.displayName || WrappedComponent.name || 'Component';
    HOC.displayName = `withLoadingAndErrorHandling(${displayName})`;

    return HOC;
};

const BasicInfoStepLoaded = ({ userInfo }: StepPropsType) => {
    const errors = useAppSelector(selectUserInfoErrors);
    const hasFirstNameError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_FIRST_NAME);
    const hasLastNameError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_LAST_NAME);
    const { pageType } = useInternalRegistrationContext();

    return (
        <CenterBoxStyled>
            <ProfileImageUploader />
            <AttributeBoxStyled>
                <InputFieldLabel titleKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_FIRST_NAME} isMandatory={true} />
                <TextField placeholderKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_FIRST_NAME_PLACEHOLDER} defaultValue={userInfo?.firstName ?? ""} name={InputNames.firstName} hasError={hasFirstNameError} />
            </AttributeBoxStyled>
            <AttributeBoxStyled>
                <InputFieldLabel titleKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_LAST_NAME} isMandatory={true} />
                <TextField placeholderKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_LAST_NAME_PLACEHOLDER} defaultValue={userInfo?.lastName ?? ""} name={InputNames.lastName} hasError={hasLastNameError} />
            </AttributeBoxStyled>
        </CenterBoxStyled>
    )
}

const BasicInfoStep = withLoadingAndErrorHandling(BasicInfoStepLoaded);

const ItemWithIcon = ({ category, iconName, textKey, onClick, isActive }: { category: IconCategoryType, iconName: string | null | undefined, textKey: string | null | undefined, onClick: () => void, isActive: boolean }) => {
    const { t } = useTranslation();
    return (
        <ItemBoxStyled onClick={onClick}>
            <Box className={isActive ? "active" : "passive"}>
                <IconBoxStyled><InjectIcon category={category} iconName={iconName ?? ""} /></IconBoxStyled>
                <ItemTypography>{t(textKey ?? "")}</ItemTypography>
            </Box>
        </ItemBoxStyled>
    )
}

const NotificationSetting = ({ notificationSetting }: { notificationSetting: NotificationsGetSettingsResponse[0] }) => {
    const [checked, setChecked] = useState(!notificationSetting.excluded);
    const { t } = useTranslation();

    useEffect(() => {
        setChecked(!notificationSetting.excluded);
    }, [notificationSetting]);

    const handleClick = () => setChecked(prev => !prev);
    return (
        <NotificationSettingBoxStyled onClick={handleClick}>
            <CheckboxStyled checked={checked} name={InputNames.notificationSetting + '_' + notificationSetting.notificationTypeId!} />
            <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "4px" }}>
                <H5Styled>{t(notificationSetting.notificationTypeSettingTitleKey ?? "")}</H5Styled>
                <Body1Styled>{t(notificationSetting.notificationTypeSettingDescriptionKey ?? "")}</Body1Styled>
            </Box>
        </NotificationSettingBoxStyled>
    )
}

const NotificationStepLoaded = ({ notificationSettings }: { notificationSettings: NotificationsGetSettingsResponse }) => {
    const { t } = useTranslation();

    return (
        <CenterBoxStyled>
            <AllAtributesBoxStyled>
                {notificationSettings.map(x => <NotificationSetting key={x.notificationTypeId!} notificationSetting={x} />)}
            </AllAtributesBoxStyled>
        </CenterBoxStyled>
    )
}

const NotificationsStep = () => {
    const dispatch = useAppDispatch();
    const loadingStatus = useAppSelector(selectNotificationSettingsStatus);
    const notificationSettings = useAppSelector(selectNotificationSettings);

    useEffect(() => {
        dispatch(setNotificationSettings());
    }, []);

    return (
        <>
            {loadingStatus === LoadingStatusEnum.Loading ? <StepLoading /> : <></>}
            {loadingStatus === LoadingStatusEnum.Error ? <StepError /> : <></>}
            {loadingStatus === LoadingStatusEnum.Loaded && notificationSettings ? <NotificationStepLoaded notificationSettings={notificationSettings} /> : <></>}
        </>
    )
}

const ItemWithDescription = ({ textKey, descriptionKey, onClick, isActive }: { textKey: string | null | undefined, descriptionKey: string | null | undefined, onClick: () => void, isActive: boolean }) => {
    const { t } = useTranslation();
    return (
        <ItemBoxStyled onClick={onClick}>
            <Box className={isActive ? "active" : "passive"} sx={{ justifyContent: "space-between !important" }}>
                <ItemTypography>{t(textKey ?? "")}</ItemTypography>
                <ItemDescriptionTypography>{t(descriptionKey ?? "")}</ItemDescriptionTypography>
            </Box>
        </ItemBoxStyled>
    )
}

const SelectSubjectStepLoaded = ({ userInfo }: StepPropsType) => {
    const { subjects } = useAppDataContext();
    const [subjectId, setSubjectId] = useState<number | undefined>(undefined);

    useEffect(() => {
        setSubjectId(userInfo.subject?.subjectId);
    }, [userInfo]);

    return (
        <CenterBoxStyled>
            {subjects.map(x => <ItemWithIcon key={x.subjectId!} category="emoji" isActive={subjectId === x.subjectId} iconName={x.iconName} textKey={x.nameKey} onClick={() => setSubjectId(x.subjectId)} />)}
            <input type="number" readOnly={true} name={InputNames.subjectId} value={subjectId ?? 0} style={{ display: "none" }} />
        </CenterBoxStyled>
    )
}
const SelectSubjectStep = withLoadingAndErrorHandling(SelectSubjectStepLoaded);

const SubjectLevelStepLoaded = ({ userInfo, subjectLevels }: { userInfo: UserInfoResponse, subjectLevels: CodetableSubjectLevelsResponse }) => {
    const [subjectLevelId, setSubjectLevelId] = useState<number | undefined>(undefined);

    useEffect(() => {
        if (userInfo.subjectLevels && userInfo.subjectLevels.length) {
            setSubjectLevelId(userInfo.subjectLevels[0].subjectLevelId);
        }
    }, [userInfo]);

    return (
        <CenterBoxStyled>
            {subjectLevels.map(x =>
                <ItemWithIcon
                    key={x.subjectLevelId!}
                    category="subjectLevels"
                    isActive={subjectLevelId === x.subjectLevelId}
                    iconName={subjectLevelId === x.subjectLevelId ? x.invertedIconName : x.iconName}
                    textKey={x.studentNameKey ?? ""}
                    onClick={() => setSubjectLevelId(x.subjectLevelId)}
                />)}
            <input type="number" readOnly={true} name={InputNames.subjectLevelId} value={subjectLevelId ?? 0} style={{ display: "none" }} />
        </CenterBoxStyled>
    )
}

const SubjectLevelStep = () => {
    const dispatch = useAppDispatch();
    const userInfo = useAppSelector(selectUserInfo);
    const userInfoStatus = useAppSelector(selectUserInfoStatus);
    const subjectLevelStatus = useAppSelector(selectSubjectLevelsStatus);
    const subjectLevels = useAppSelector(selectSubjectLevels);
    const status = getLoadingStatus([userInfoStatus, subjectLevelStatus]);

    useEffect(() => {
        dispatch(setUserInfo());
        dispatch(setSubjectLevelsAsync());
    }, []);

    return (
        <>
            {status === LoadingStatusEnum.Loading ? <StepLoading /> : <></>}
            {status === LoadingStatusEnum.Error ? <StepError /> : <></>}
            {status === LoadingStatusEnum.Loaded && userInfo && subjectLevels ? <SubjectLevelStepLoaded userInfo={userInfo} subjectLevels={subjectLevels} /> : <></>}
        </>
    )
};

const SpecializationStepLoaded = ({ userInfo }: StepPropsType) => {
    const { specializations } = useAppDataContext();
    const [specializationIds, setSpecializationIds] = useState<number[]>([]);
    const filteredSpecializations = specializations?.filter(x => x.subjectId === userInfo.subjectId);
    const { t } = useTranslation();

    useEffect(() => {
        if (userInfo.specializations && userInfo.specializations.length) {
            setSpecializationIds(userInfo?.specializations?.map(x => x.specializationId!) ?? []);
        }
    }, [userInfo]);

    const handleSpecializationClick = (specializationId: number) => {
        const idx = specializationIds.indexOf(specializationId);
        if (idx === -1) {
            setSpecializationIds(prev => [...prev, specializationId]);
            return;
        }
        setSpecializationIds(prev => produce(prev, draft => {
            draft.splice(idx, 1);
        }));
    }

    return (
        <CenterBoxStyled>
            <ItemStateButtonBoxStyled>
                {filteredSpecializations.map(x => (
                    <ItemStateButtonStyled
                        key={x.specializationId!}
                        onClick={() => handleSpecializationClick(x.specializationId!)}
                        active={specializationIds.some(y => y === x.specializationId!)}
                    >
                        <LargeIconBoxStyled>
                            <InjectIcon category="specializations" iconName={x.iconName ?? ""} />
                        </LargeIconBoxStyled>
                        <H5Styled>{t(x.nameKey ?? "")}</H5Styled>
                    </ItemStateButtonStyled>
                ))}
            </ItemStateButtonBoxStyled>
            <input type="text" readOnly={true} name={InputNames.specializationIds} value={JSON.stringify(specializationIds)} style={{ display: "none" }} />
        </CenterBoxStyled >
    )
}
const SpecializationStep = withLoadingAndErrorHandling(SpecializationStepLoaded);

const InterestsStepLoaded = ({ userInfo, interests }: { userInfo: UserInfoResponse, interests: CodetableInterestsReponse }) => {
    const [interestIds, setInterestIds] = useState<number[]>([]);
    const { t } = useTranslation();

    useEffect(() => {
        if (userInfo.specializations && userInfo.specializations.length) {
            setInterestIds(userInfo?.interests?.map(x => x.interestId!) ?? []);
        }
    }, [userInfo]);

    const handleClick = (interestId: number) => {
        const idx = interestIds.indexOf(interestId);
        if (idx === -1) {
            setInterestIds(prev => [...prev, interestId]);
            return;
        }
        setInterestIds(prev => produce(prev, draft => {
            draft.splice(idx, 1);
        }));
    }

    return (
        <CenterBoxStyled>
            <ItemStateButtonBoxStyled>
                {interests.map(x => (
                    <ItemStateButtonStyled
                        key={x.interestId!}
                        onClick={() => handleClick(x.interestId!)}
                        active={interestIds.some(y => y === x.interestId)}
                    >
                        <LargeIconBoxStyled>
                            <InjectIcon category="interests" iconName={x.iconName ?? ""} />
                        </LargeIconBoxStyled>
                        <H5Styled>{t(x.nameKey ?? "")}</H5Styled>
                    </ItemStateButtonStyled>
                ))}
            </ItemStateButtonBoxStyled>
            <input type="text" readOnly={true} name={InputNames.interestIds} value={JSON.stringify(interestIds)} style={{ display: "none" }} />
        </CenterBoxStyled>
    )
}

const InterestsStep = () => {
    const dispatch = useAppDispatch();
    const userInfo = useAppSelector(selectUserInfo);
    const userInfoStatus = useAppSelector(selectUserInfoStatus);
    const interestsStatus = useAppSelector(selectInterestsStatus);
    const interests = useAppSelector(selectInterests);
    const status = getLoadingStatus([userInfoStatus, interestsStatus]);

    useEffect(() => {
        dispatch(setUserInfo());
        dispatch(setInterests());
    }, []);

    return (
        <>
            {status === LoadingStatusEnum.Loading ? <StepLoading /> : <></>}
            {status === LoadingStatusEnum.Error ? <StepError /> : <></>}
            {status === LoadingStatusEnum.Loaded && userInfo && interests ? <InterestsStepLoaded userInfo={userInfo} interests={interests} /> : <></>}
        </>
    )
};

const WeeklyGoalStepLoaded = ({ userInfo, weeklyGoals }: { userInfo: UserInfoResponse, weeklyGoals: CodetableWeeklyGoalReponse }) => {
    const [weeklyGoalId, setWeeklyGoalId] = useState<number | undefined>(undefined);

    useEffect(() => {
        setWeeklyGoalId(userInfo.weeklyGoal?.weeklyGoalId);
    }, [userInfo]);

    return (
        <CenterBoxStyled>
            {weeklyGoals.map(x => <ItemWithDescription key={x.weeklyGoalId!} isActive={weeklyGoalId === x.weeklyGoalId} textKey={x.nameKey} descriptionKey={x.descriptionKey} onClick={() => setWeeklyGoalId(x.weeklyGoalId)} />)}
            <input type="number" readOnly={true} name={InputNames.weeklyGoalId} value={weeklyGoalId ?? 0} style={{ display: "none" }} />
        </CenterBoxStyled>
    )
}
const WeeklyGoalStep = () => {
    const dispatch = useAppDispatch();
    const userInfo = useAppSelector(selectUserInfo);
    const userInfoStatus = useAppSelector(selectUserInfoStatus);
    const weeklyGoalsStatus = useAppSelector(selectWeeklyGoalsStatus);
    const weeklyGoals = useAppSelector(selectWeeklyGoals);
    const status = getLoadingStatus([userInfoStatus, weeklyGoalsStatus]);

    useEffect(() => {
        dispatch(setUserInfo());
        dispatch(setWeeklyGoals());
    }, []);

    return (
        <CenterBoxStyled>
            {status === LoadingStatusEnum.Loading ? <StepLoading /> : <></>}
            {status === LoadingStatusEnum.Error ? <StepError /> : <></>}
            {status === LoadingStatusEnum.Loaded && userInfo && weeklyGoals ? <WeeklyGoalStepLoaded userInfo={userInfo} weeklyGoals={weeklyGoals} /> : <></>}
        </CenterBoxStyled>
    )
};

const SettingsControllers = () => {
    const { handleSetStep, steps, currentStep } = useInternalRegistrationContext();
    const { t } = useTranslation();

    return (
        <ContainerStyled>
            <TabsBoxShrinkedStyled sx={{
                maxWidth: "100%",
                minWidth: { xs: "100%", md: "640px" },
                overflow: "auto",

            }}>
                {steps.map((x, idx) => (
                    <TabItemButtonStyled
                        key={idx}
                        onClick={() => handleSetStep(x)}
                        sx={{
                            backgroundColor: currentStep === x.type ? COLORS.PRIMARY : "transparent",
                            color: currentStep === x.type ? COLORS.WHITE : COLORS.BLACK,
                            ":hover": { backgroundColor: currentStep === x.type ? COLORS.PRIMARY : COLORS.PRIMARY_10, color: currentStep === x.type ? COLORS.WHITE : COLORS.BLACK }
                        }}>
                        {t(x.tabKey)}
                    </TabItemButtonStyled>
                ))}
            </TabsBoxShrinkedStyled>
        </ContainerStyled>
    )
}

const Views = () => {
    const { currentStep } = useInternalRegistrationContext();

    return (
        <>
            {currentStep === "basicInfo" ? <BasicInfoStep /> : <></>}
            {currentStep === "notifications" ? <NotificationsStep /> : <></>}
            {currentStep === "subjectSelect" ? <SelectSubjectStep /> : <></>}
            {currentStep === "subjectLevel" ? <SubjectLevelStep /> : <></>}
            {currentStep === "specialization" ? <SpecializationStep /> : <></>}
            {currentStep === "interests" ? <InterestsStep /> : <></>}
            {currentStep === "weeklyGoal" ? <WeeklyGoalStep /> : <></>}
        </>
    )
}

const Settings = () => {
    const { handleSubmit } = useInternalRegistrationContext();
    const { screenType } = useContext(UIContext);

    return (
        <form onSubmit={handleSubmit} style={{ width: "100%", display: "flex", flexFlow: "column noWrap", flex: "1 1 auto", overflow: "auto", alignItems: "center", gap: screenType === "desktop" ? "24px" : "16px", padding: "24px 0" }}>
            <SettingsControllers />
            <RegistrationStepBoxStyled>
                <RegistrationHeader />
                <Views />
            </RegistrationStepBoxStyled>
            <SaveButton />
        </form>
    )
}

const Registration = () => {
    const { handleSubmit } = useInternalRegistrationContext();

    return (
        <form onSubmit={handleSubmit} style={{ display: "flex", width: "100%", gap: "24px", flexFlow: "column noWrap", maxHeight: "min(80vh, 600px)" }}>
            <RegistrationHeader />
            <Views />
            <RegistrationControllers />
        </form>
    )
}

const StudentProfileSettings = ({ pageType, onFinalSubmit, onClose }: { pageType: PageType, onFinalSubmit?: () => void, onClose?: () => void }) => (
    <RegistrationInternalContextProvider pageType={pageType} onFinalSubmit={onFinalSubmit} onClose={onClose}>
        {pageType === "registration" ? <Registration /> : <></>}
        {pageType === "settings" ? <Settings /> : <></>}
    </RegistrationInternalContextProvider>
)

export default StudentProfileSettings;