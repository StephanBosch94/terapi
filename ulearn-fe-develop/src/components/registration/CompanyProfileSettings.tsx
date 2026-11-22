import LinearProgress from "@mui/material/LinearProgress";
import { useTranslation } from "components/core/TranslationProvider";
import { PrimaryButton } from "components/styles/PrimaryButton";
import { cacheConstants } from "core/constants/cacheConstants";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { selectUserInfo, selectUserInfoStatus, updateProfileSettingsInfo, updateErrors, setShortUserInfo, selectUserInfoErrors, setUserInfo, UserInfoResponse, setChangeLanguage, selectUpdateUserInfoStatus } from "core/redux/userSlice";
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
import { TabsBoxShrinkedStyled } from "components/styles/boxes/TabsBoxShrinkedStyled";
import { TabItemButtonStyled } from "components/styles/TabItemButtonStyled";
import RegistrationStepBoxStyled from "components/styles/boxes/RegistrationStepBoxStyled";
import { UIContext } from "components/core/UIContextProvider";
import SaveButton from "./SaveButton";
import { useRouter } from "next/router";
import routes from "routes/routes";
import RocketIcon from "assets/emoji/rocket.svg";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import React from "react";
import { MuiCheckboxStyled } from "components/styles/MuiCheckboxStyled";
import Body1Styled from "components/styles/typography/Body1Styled";
import { ContainerStyled } from "components/styles/ContainerStyled";
import StaticLinearProgress from "components/styles/StaticLinearProgress";

const AttributeBoxStyled = styled(Box)({
    display: "flex",
    flexDirection: "column",
    gap: "4px",
    marginRight: "16px"
}) as typeof Box;

const PrimaryButtonStyled = styled(PrimaryButton)(() => sx({
    borderRadius: "4px"
})) as typeof PrimaryButton;

const StartLearningButtonStyled = styled(PrimaryButtonStyled)(() => sx({
    display: "inline-flex",
    alignItems: "center",
    gap: "8px"
})) as typeof PrimaryButtonStyled;

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
    flex: "1 1 auto",
})) as typeof Box;

const HorizontalDividerBoxStyled = styled(Box)({
    height: "1px",
    width: "100%",
    margin: "16px 0px",
    backgroundColor: COLORS.BLACK_10
}) as typeof Box;

const AttributeCheckboxBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "row",
    gap: "8px",
    alignItems: "center",
    ":hover": { cursor: "pointer" }
})) as typeof Box;

const RegistrationLabelStyled = styled(Body1Styled)(() => sx({
    fontWeight: "600",
    color: COLORS.BLACK
})) as typeof Body1Styled;

const CheckboxStyled = styled(MuiCheckboxStyled)(() => sx({
    color: COLORS.PRIMARY,
    padding: "0px !important"
})) as typeof MuiCheckboxStyled;

type RegistrationStepType = "companyInfo";
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
    { type: "companyInfo", tabKey: TRANSLATION_KEYS.COMPANY_SIGN_UP.COMPANY_INFORMATION, titleKey: TRANSLATION_KEYS.COMPANY_SIGN_UP.COMPANY_INFORMATION, icon: null, descriptionKey: undefined, pageTypes: ["registration", "settings"] }
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
    handleSetStep: (step: RegistrationStepInfoType) => void
} | undefined;

const RegistrationInternalContextTypeContext = createContext<RegistrationInternalContextType>(undefined);
export const useInternalRegistrationContext = contextFactoryCreate('CompanyRegistration', RegistrationInternalContextTypeContext);

const InputNames = {
    nextButton: "nextBtn",
    submitButton: "submitBtn",
    companyName: "companyName",
    ico: "ico",
    dic: "dic",
    isVatPayer: "isVatPayer",
    placeOfBusiness: "placeOfBusiness",
    billingEmail: "billingEmail"
}
type StepPropsType = {
    userInfo: UserInfoResponse;
}

const RegistrationInternalContextProvider = ({ children, pageType, onFinalSubmit }: { children: any, pageType: PageType, onFinalSubmit?: () => void }) => {
    const [currentStepType, setCurrentStepType] = useState<RegistrationStepType>(STEPS.filter(x => x.pageTypes.includes(pageType))[0].type);
    const steps = useMemo(() => STEPS.filter(x => x.pageTypes.includes(pageType)), [pageType]);
    const dispatch = useAppDispatch();
    const userInfo = useAppSelector(selectUserInfo);
    const userStatus = useAppSelector(selectUserInfoStatus);
    const updateUserInfoStatus = useAppSelector(selectUpdateUserInfoStatus);
    const currentIdx = STEPS.findIndex(x => x.type === currentStepType);
    const currentStep = STEPS.find(x => x.type === currentStepType);
    const router = useRouter();
    const { currentLanguage } = useAppDataContext();
    const errorKeys = useAppSelector(selectUserInfoErrors);
    const { enqueueSnackbar } = useSnackbar();
    const { t } = useTranslation();
    const submitted = useRef(false);

    useEffect(() => {
        setPreviousStepFromStorage();
    }, []);

    useEffect(() => {
        if (!submitted.current) return;
        errorKeys?.forEach(errorKey => {
            enqueueSnackbar(t(errorKey), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        })
    }, [errorKeys]);

    useEffect(() => {
        if (!submitted.current) return;
        if (updateUserInfoStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_SIGN_UP.FAILED_TO_SAVE), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
        else if (pageType === "settings" && updateUserInfoStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_SIGN_UP.SUCCESSFULLY_SAVED), { variant: "success" });
        }
    }, [updateUserInfoStatus])

    const setPreviousStepFromStorage = async () => {
        if (pageType !== "registration") {
            return;
        }
        const storage = await getStorage();
        const value = await storage.getCommonValue<string>(cacheConstants.companyRegistrationLatestStep);
        if (!value) return;
        const parsedValue = parseInt(value);
        setCurrentStepType(STEPS[parsedValue].type)
    }

    const setStepToStorage = async (newValue: number) => {
        if (pageType !== "registration") {
            return;
        }
        const storage = await getStorage();
        await storage.setCommonValue(cacheConstants.companyRegistrationLatestStep, `${newValue}`);
    }

    const handleNextStep = () => {
        const currentIndex = STEPS.findIndex(x => x.type === currentStepType);
        const newValue = STEPS
            .map((step, idx) => ({ step, idx }))
            .filter(x => x.idx > currentIndex)
            .filter(x => x.step.pageTypes.includes(pageType))
            .map(x => x.idx)[0];
        setStepToStorage(newValue);
        setCurrentStepType(STEPS[newValue].type);
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

        if (currentStepType === "companyInfo") {
            const companyName = fieldValues[InputNames.companyName] as string;
            const ico = fieldValues[InputNames.ico] as string;
            const dic = fieldValues[InputNames.dic] as string | undefined;
            const placeOfBusiness = fieldValues[InputNames.placeOfBusiness] as string;
            const billingEmail = fieldValues[InputNames.billingEmail] as string;
            const isVatPayer = fieldValues[InputNames.isVatPayer] !== undefined;
            const userStateId = submitterName === InputNames.submitButton
                ? UserStateEnum.Confirmed
                : userInfo.stateId;

            errors = [
                !companyName.length ? TRANSLATION_KEYS.ERRORS.MISSING_COMPANY_NAME : undefined,
                !ico.length ? TRANSLATION_KEYS.ERRORS.MISSING_ICO : undefined,
                isVatPayer && !dic?.length ? TRANSLATION_KEYS.ERRORS.MISSING_DIC : undefined,
                !placeOfBusiness.length ? TRANSLATION_KEYS.ERRORS.MISSING_COMPANY_ADDRESS : undefined,
                !billingEmail.length ? TRANSLATION_KEYS.ERRORS.MISSING_BILLING_EMAIL : undefined
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    companyName,
                    ico,
                    dic,
                    placeOfBusiness,
                    billingEmail,
                    isVatPayer,
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
                router.push(routes.companyDashboard.path);
            }
        }
    }

    const hasNextStep = () => {
        const index = STEPS.findIndex(x => x.type === currentStepType);
        return index >= 0 && index < STEPS.length - 1;
    }

    return (
        <RegistrationInternalContextTypeContext.Provider value={{
            currentStep: currentStepType,
            progress: Math.round((currentIdx ?? 0) * 100 / (STEPS.length - 1)),
            stepTitleKey: currentStep?.titleKey ?? "",
            stepTabKey: currentStep?.tabKey ?? "",
            steps,
            pageType,
            hasNextStep: hasNextStep(),
            handleSubmit,
            handleSetStep
        }}>
            {children}
        </RegistrationInternalContextTypeContext.Provider>
    )
}

const RegistrationControllers = () => {
    const { progress, hasNextStep } = useInternalRegistrationContext();
    const { t } = useTranslation();

    return (
        <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "24px", flex: "0 1 auto" }}>
            <StaticLinearProgress value={progress} />
            {hasNextStep
                ? <PrimaryButtonStyled fullWidth={true} type="submit" name={InputNames.nextButton}>{t(TRANSLATION_KEYS.COMMON.CONTINUE)}</PrimaryButtonStyled>
                : (
                    <StartLearningButtonStyled fullWidth={true} type="submit" name={InputNames.submitButton} sx={{ display: "inline-flex", alignItems: "center", gap: "8px" }}>
                        {t(TRANSLATION_KEYS.COMPANY_SIGN_UP.FINISH_BUTTON)}
                        <RocketIcon />
                    </StartLearningButtonStyled>
                )
            }
        </Box>
    )
}

const RegistrationHeader = () => {
    const { t } = useTranslation();
    const { steps, currentStep } = useInternalRegistrationContext();
    const step = steps.find(x => x.type == currentStep)!;

    return (
        <Box sx={{ display: "flex", flexFlow: "column noWrap", flex: "0 1 auto", gap: "16px" }}>
            <Box sx={{ display: "inline-flex", alignItems: "center", gap: "8px" }}>
                <H3Styled>{t(step.titleKey)}</H3Styled>
                {step.icon ? <IconBoxLargeStyled>{step.icon}</IconBoxLargeStyled> : <></>}
            </Box>
            {step.descriptionKey ? <CaptionTextStyled sx={{ fontSize: "0.875rem" }}>{t(step.descriptionKey)}</CaptionTextStyled> : <></>}
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

const CompanyInfoStepLoaded = ({ userInfo }: StepPropsType) => {
    const errors = useAppSelector(selectUserInfoErrors);
    const hasCompanyNameError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_COMPANY_NAME);
    const hasIcoError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_ICO);
    const hasDicError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_DIC);
    const hasplaceOfBusinessError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_COMPANY_ADDRESS);
    const hasBillingEmailError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_BILLING_EMAIL);
    const { pageType } = useInternalRegistrationContext();
    const [showDic, setShowDic] = useState(userInfo.isVatPayer ?? false);
    const { t } = useTranslation();

    useEffect(() => {
        setShowDic(userInfo.isVatPayer ?? false);
    }, [userInfo]);

    return (
        <CenterBoxStyled>
            <ProfileImageUploader />
            <AttributeBoxStyled>
                <InputFieldLabel titleKey={TRANSLATION_KEYS.COMPANY_SIGN_UP.ACCOUNT_COMPANY_NAME} isMandatory={true}/>
                <TextField placeholderKey={TRANSLATION_KEYS.COMPANY_SIGN_UP.ACCOUNT_COMPANY_NAME_PLACEHOLDER} defaultValue={userInfo?.companyName ?? ""} name={InputNames.companyName} hasError={hasCompanyNameError} />
            </AttributeBoxStyled>
            <AttributeBoxStyled>
                <InputFieldLabel titleKey={TRANSLATION_KEYS.COMPANY_SIGN_UP.ACCOUNT_COMPANY_ADDRESS} isMandatory={true}/>
                <TextField placeholderKey={TRANSLATION_KEYS.COMPANY_SIGN_UP.ACCOUNT_COMPANY_ADDRESS_PLACEHOLDER} defaultValue={userInfo?.placeOfBusiness ?? ""} name={InputNames.placeOfBusiness} hasError={hasplaceOfBusinessError} />
            </AttributeBoxStyled>
            <AttributeBoxStyled>
                <InputFieldLabel titleKey={TRANSLATION_KEYS.COMPANY_SIGN_UP.ACCOUNT_BILLING_EMAIL} isMandatory={true}/>
                <TextField type="email" placeholderKey={TRANSLATION_KEYS.COMPANY_SIGN_UP.ACCOUNT_BILLING_EMAIL_PLACEHOLDER} defaultValue={userInfo?.billingEmail ?? ""} name={InputNames.billingEmail} hasError={hasBillingEmailError} />
            </AttributeBoxStyled>
            <AttributeBoxStyled>
                <InputFieldLabel titleKey={TRANSLATION_KEYS.COMPANY_SIGN_UP.ACCOUNT_ICO} isMandatory={true}/>
                <TextField type="number" placeholderKey={TRANSLATION_KEYS.COMPANY_SIGN_UP.ACCOUNT_ICO_PLACEHOLDER} defaultValue={userInfo?.ico ?? ""} name={InputNames.ico} hasError={hasIcoError} />
            </AttributeBoxStyled>
            <AttributeCheckboxBoxStyled
                onClick={() => setShowDic(!showDic)}
            >
                <CheckboxStyled checked={showDic} name={InputNames.isVatPayer} />
                <RegistrationLabelStyled>{t(TRANSLATION_KEYS.COMPANY_SIGN_UP.IS_VAT_PAYER)}</RegistrationLabelStyled>
            </AttributeCheckboxBoxStyled>
            {showDic ? (
                <AttributeBoxStyled>
                    <InputFieldLabel titleKey={TRANSLATION_KEYS.COMPANY_SIGN_UP.ACCOUNT_DIC} isMandatory={true}/>
                    <TextField placeholderKey={TRANSLATION_KEYS.COMPANY_SIGN_UP.ACCOUNT_DIC_PLACEHOLDER} defaultValue={userInfo?.dic ?? ""} name={InputNames.dic} hasError={hasDicError} />
                </AttributeBoxStyled>
            ) : <></>}
        </CenterBoxStyled>
    )
}

const CompanyInfoStep = withLoadingAndErrorHandling(CompanyInfoStepLoaded);

const SettingsControllers = () => {
    const { handleSetStep, steps, currentStep } = useInternalRegistrationContext();
    const { t } = useTranslation();

    return (
        <ContainerStyled>
            <TabsBoxShrinkedStyled sx={{
                maxWidth: "100%",
                minWidth: { xs: "100%", md: "560px" },
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
            {currentStep === "companyInfo" ? <CompanyInfoStep /> : <></>}
        </>
    )
}

const Settings = () => {
    const { handleSubmit } = useInternalRegistrationContext();
    const { screenType } = useContext(UIContext);

    return (
        <form
            onSubmit={handleSubmit}
            style={{ width: "100%", display: "flex", flexFlow: "column noWrap", flex: "1 1 auto", overflow: "auto", alignItems: "center", gap: screenType === "desktop" ? "24px" : "16px", padding: "24px 0" }}
        >
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
        <form
            onSubmit={handleSubmit}
            style={{ display: "flex", width: "100%", gap: "24px", flexFlow: "column noWrap", overflow: "auto", maxHeight: "min(80vh, 600px)", overflowY: "auto" }}
        >
            <RegistrationHeader />
            <Views />
            <RegistrationControllers />
        </form>
    )
}

const CompanyProfileSettings = ({ pageType, onFinalSubmit }: { pageType: PageType, onFinalSubmit?: () => void }) => {

    return (
        <RegistrationInternalContextProvider pageType={pageType} onFinalSubmit={onFinalSubmit}>
            {pageType === "registration" ? <Registration /> : <></>}
            {pageType === "settings" ? <Settings /> : <></>}
        </RegistrationInternalContextProvider>
    )
}

export default CompanyProfileSettings;