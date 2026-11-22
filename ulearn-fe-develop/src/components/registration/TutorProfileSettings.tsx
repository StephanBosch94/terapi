import { styled, experimental_sx as sx } from "@mui/material/styles";
import React, { createContext, useState, useMemo, useEffect, useRef, useContext } from "react";
import EyesIcon from "assets/emoji/eyes.svg";
import PersonIcon from "assets/emoji/person.svg";
import PhoneIcon from "assets/emoji/phone.svg";
import SearchIcon from "assets/emoji/search.svg";
import MoneyIcon from "assets/emoji/money.svg";
import H4Styled from "components/styles/typography/H4Styled";
import PStyled from "components/styles/typography/PStyled";
import H2Styled from "components/styles/typography/H2Styled";
import { getColor, COLORS } from "components/styles/constants/ColorsConstatnts";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { PrimaryButton } from "components/styles/PrimaryButton";
import { useTranslation } from "components/core/TranslationProvider";
import TrashIcon from "assets/trash.svg";
import PlusIcon from "assets/plus.svg";
import H5Styled from "components/styles/typography/H5Styled";
import { ListItem } from "components/styles/ListItem";
import RadioButton from "components/styles/RadioButton";
import H3Styled from "components/styles/typography/H3Styled";;
import { TertiaryButton } from "components/styles/TertiaryButton";
import Body1Styled from "components/styles/typography/Body1Styled";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import FileIcon from "assets/file.svg";
import MinusIcon from "assets/minus.svg";
import { DeleteIconButton } from "components/styles/DeleteIconButton";
import DropBoxProvider, { useDropBoxContext } from "components/styles/boxes/DropBoxProvider";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { useSnackbar } from "notistack";
import {
    deleteUserFile,
    selectDeleteUserFileStatus,
    selectUploadCertificatesStatus,
    selectUserInfo,
    selectUserInfoErrors,
    selectUserInfoStatus,
    updateErrors,
    setShortUserInfo,
    setUserInfo,
    updateProfileSettingsInfo,
    uploadCertificates,
    selectUpdateUserInfoStatus,
    selectUserInfoFiles
} from "core/redux/userSlice";
import ErrorLarge from "components/core/error/ErrorLarge";
import { UserInfoResponse } from "core/redux/userSlice";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import MunuInputIcon from "components/styles/icons/MenuInputIcon";
import Menu from "components/menu/Menu";
import {
    CodetableLanguageResponse,
    CodetableLanguageLevelResponse,
    selectCountries,
    selectCountriesStatus,
    selectLanguageLevels,
    selectLanguageLevelsStatus,
    selectLanguages,
    selectLanguagesStatus,
    setCountriesAsync,
    setLanguageLevelsAsync,
    setLanguagesAsync,
    setTeachingExperiences,
    selectTeachingExperienceStatus,
    selectTeachingExperiences,
    CodetableTeachingExperienceResponse,
    CodetableSubjectLevelsResponse,
    selectSubjectLevels,
    selectSubjectLevelsStatus,
    setSubjectLevelsAsync,
    CodetableCountryResponse,
    CodetableSubjectsResponse,
    selectInterests,
    selectInterestsStatus,
    CodetableInterestsReponse,
    setInterests
} from "core/redux/codetableSlice";
import { getLoadingStatus } from "core/services/Utils/apiUtil";
import produce, { current } from "immer";
import PhoneInput from 'react-phone-input-2';
import 'react-phone-input-2/lib/style.css';
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import { toAppUserIm } from "core/services/mappers/appUserMapper";
import { useAppDataContext } from "components/core/providers/AppProvider";
import { AttributeTypeEnum } from "core/constants/enums/AttributeTypeEnum";
import { getFileUrl } from "core/services/Utils/imageUtil";
import { FileTypeEnum } from "core/constants/enums/FileTypeEnum";
import FileService from "core/services/FileService";
import { MuiCheckboxStyled } from "components/styles/MuiCheckboxStyled";
import { getStorage } from "core/services/Core/StorageService";
import { cacheConstants } from "core/constants/cacheConstants";
import ProfileImageUploader from "components/registration/ProfileImageUploader";
import { snackbarConstants } from "components/core/Snackbar";
import InputFieldLabel from "components/form/InputFieldLabel";
import TextField from "components/form/TextField";
import RegistrationStepBoxStyled from "components/styles/boxes/RegistrationStepBoxStyled";
import { TabItemButtonStyled } from "components/styles/TabItemButtonStyled";
import { UIContext } from "components/core/UIContextProvider";
import { getEmbededId, getYoutubeVideoUrlFromQuery } from "core/services/Utils/youtubeUtil";
import { TextFieldStyled } from "components/styles/TextFieldStyled";
import { TabsBoxShrinkedStyled } from "components/styles/boxes/TabsBoxShrinkedStyled";
import SaveButton from "./SaveButton";
import Box from "@mui/material/Box";
import Skeleton from "@mui/material/Skeleton";
import Link from "next/link";
import { getTimeZoneName } from "core/services/Utils/timeUtil";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { PhoneInputBoxStyled } from "components/styles/PhoneInputBoxStyled";
import InjectIcon from "components/core/icons/InjectIcon";
import MenuItemsList from "components/menu/MenuItemList";
import { ContainerStyled } from "components/styles/ContainerStyled";
import StaticLinearProgress from "components/styles/StaticLinearProgress";
import { NotificationsGetSettingsResponse, editNotificationSettings, selectNotificationSettings, selectNotificationSettingsStatus, setNotificationSettings } from "core/redux/notificationSlice";
import ItemStateButtonStyled from "components/styles/buttons/ItemStateButtonStyled";
import ItemStateButtonBoxStyled from "components/styles/boxes/ItemStateButtonBoxStyled";
import { usePriceLocalization } from "core/hooks/priceHook";
import { toOptionalPriceFixed } from "core/services/Utils/walletUtil";

const HeadingBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "8px"
})) as typeof Box;

const IconStyledBox = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    height: "100%",
    svg: {
        transform: "scale(3)"
    }
})) as typeof Box;

const RightSideContentBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "row noWrap",
    padding: "24px 24px 24px 0px",
    gap: "40px",
    justifyContent: "space-between",
    borderBottom: `1px solid ${COLORS.BLACK_10}`,
    alignItems: "center",
})) as typeof Box;

const FirstStepBoxStyled = styled(Box)(() => sx({
    flexDirection: { xs: "column", md: "row" },
    alignItems: { xs: "left", md: "center" },
    justifyContent: "space-between",
    maxWidth: "1400px",
    padding: "0px 24px",
    height: "100%",
    gap: { xs: "24px", md: "80px" },
    display: "flex",
    flex: "1 1 auto",
    overflow: "auto"
})) as typeof Box;

const LargeIconBoxStyled = styled(Box)(() => ({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        transform: "scale(1.5)"
    }
})) as typeof Box;

const LanguageIconBoxStyled = styled(Box)(() => ({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        transform: "scale(1.25)"
    }
})) as typeof Box;

const TitleHeaderBoxStyled = styled(Box)(() => ({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "8px"
})) as typeof Box;

const ListItemsBoxStyled = styled(Box)(() => ({
    display: "flex",
    gap: "16px",
    flexDirection: "column",
    height: "100%",
    width: "100%"
})) as typeof Box;

const ButtonsContainerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    justifyContent: "space-between",
    alignItems: "center",
    width: "100%",
    gap: { xs: "16px", md: "24px" }
})) as typeof Box;

const LevelIconBoxStyled = styled(Box)(() => ({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        transform: "scale(1.5)"
    }
})) as typeof Box;

const TextBoxStyled = styled(Box)(() => sx({
    width: "100%",
    "textarea": {
        width: "100%",
        border: `1px solid ${COLORS.BLACK_10}`,
        font: "inherit",
        fontSize: "0.9rem",
        p: "12px",
        minHeight: "320px",
        resize: "none",
        borderRadius: "4px",
    }
})) as typeof Box;

const CurrencyPlaceholderBoxStyled = styled(Box)(() => sx({
    lineHeight: "40px",
    paddingTop: "64px",
    color: COLORS.PRIMARY,
    fontWeight: "700 !important",
    font: "inherit",
    fontSize: "1.5rem",
    userSelect: "none",
    transform: "translate(-50%, -50%)"
})) as typeof Box;

const PricingBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    width: "100%",
    justifyContent: "center",
    position: "relative",
    "textarea": {
        width: "100%",
        textAlign: "center",
        lineHeight: "40px",
        paddingTop: "64px",
        color: COLORS.PRIMARY,
        border: `1px solid ${COLORS.BLACK_10}`,
        fontWeight: "700 !important",
        font: "inherit",
        fontSize: "3rem",
        minHeight: "40px",
        resize: "none",
        borderRadius: "4px",
    },
    "textarea::placeholder": {
        color: COLORS.BLACK_10
    }
})) as typeof Box;

const InsertFileBoxStyled = styled(Box)(() => sx({
    border: `1px dashed ${COLORS.BLACK_10}`,
    display: "flex",
    flexFlow: "column noWrap",
    alignItems: "center",
    justifyContent: "center",
    height: "256px",
    gap: "24px",
    borderRadius: "4px",
    position: "relative"
})) as typeof Box;

const IconCertificationStyled = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    height: "40px",
    svg: {
        transform: "scale(2.5)"
    }
})) as typeof Box;

const CertifiactionHeaderBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    flexDirection: "column",
    gap: "8px",
})) as typeof Box;

const IconPricingStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    padding: "4px",
    border: `1px solid ${COLORS.PRIMARY}`,
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    borderRadius: "50%",
    height: "fit-content",
    svg: {
        color: COLORS.PRIMARY
    },
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_10
    }
})) as typeof Box;

const PriceBoxingStyled = styled(Box)(() => sx({
    width: "100%",
    backgroundColor: COLORS.PRIMARY_10,
    padding: "20px",
    borderRadius: "4px",
    display: "flex",
    flexDirection: "column",
    gap: "8px",
    justifyContent: "center",
    alignItems: "center"
})) as typeof Box;

const PricingInnerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "row",
    gap: "8px",
    alignItems: "center"
})) as typeof Box;

const AttributeBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    gap: "4px"
})) as typeof Box;

const AttributeCheckboxBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "row",
    gap: "8px",
    alignItems: "center",
    ":hover": { cursor: "pointer" }
})) as typeof Box;

const AllAtributesBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "16px"
})) as typeof Box;

const SpecializationTitleAndDescriptionBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    alignItems: "start"
})) as typeof Box;

const IconLargeBoxStyled = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    svg: {
        transform: "scale(1.25)"
    }
})) as typeof Box;

const InputBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    justifyContent: "space-between",
    width: "100%",
    background: COLORS.WHITE,
    border: `1px solid ${(COLORS.BLACK_10)}`,
    borderRadius: "4px",
    boxSizing: "border-box",
    padding: "10px 14px",
    fontWeight: 400,
    color: COLORS.BLACK_60,
    ":hover": {
        borderColor: COLORS.BLACK_40,
        cursor: "pointer"
    },
    ".active": {
        color: COLORS.BLACK
    }
})) as typeof Box;

const RowItemBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "8px",
    flexFlow: "row noWrap",
    alignItems: "center",
    justifyContent: "center"
})) as typeof Box;

const ExtendableBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flex: "1 1 50%",
    gap: "4px"
})) as typeof Box;

const LanguageLevelIconBoxStyled = styled(Box)(() => sx({
    width: "24px",
    height: "24px",
    display: "flex",
    flex: "0 1 auto",
    alignItems: "center",
    justifyContent: "center"
})) as typeof Box;

const ButtonsBoxStyled = styled(Box)(() => sx({
    width: "100%",
    display: "inline-flex",
    alignItems: "center",
    gap: { xs: "16px", md: "24px" },
    maxWidth: "1400px",
    justifyContent: "space-between"
})) as typeof Box;

const YoutubeBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.BLACK_10,
    width: "100%",
    height: "315px"
})) as typeof Box;

const RegistrationLabelStyled = styled(Body1Styled)(() => sx({
    fontWeight: "600",
    color: COLORS.BLACK
})) as typeof Body1Styled;

const CheckboxStyled = styled(MuiCheckboxStyled)(() => sx({
    color: COLORS.PRIMARY,
    padding: "0px !important"
})) as typeof MuiCheckboxStyled;

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

type PageType = "registration" | "settings";
type RegistrationStepType = "overview" | "basicInfo" | "notifications" | "subjectSelect" | "description" | "certificates" | "teachingExpirience" | "subjectLevel" | "specialization" | "interests" | "video" | "pricing" | "billingInformation";
type RegistrationStepInfoType = {
    type: RegistrationStepType,
    titleKey: string,
    pageTypes: PageType[]
}
type CodetableType = { id: number | undefined, nameKey: string | null | undefined };
const STEPS: RegistrationStepInfoType[] = [
    { type: "overview", titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.EASY_TO_GET_STARTED, pageTypes: ["registration"] },
    { type: "basicInfo", titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT, pageTypes: ["registration", "settings"] },
    { type: "notifications", titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.NOTIFICATIONS, pageTypes: ["settings"] },
    { type: "subjectSelect", titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.SUBJECT, pageTypes: ["registration"] },
    { type: "description", titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.DESCRIPTION, pageTypes: ["registration", "settings"] },
    { type: "certificates", titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.UPLOAD_CERTIFICATES, pageTypes: ["registration", "settings"] },
    { type: "teachingExpirience", titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.TEACHING_EXPIRIENCE, pageTypes: ["registration"] },
    { type: "subjectLevel", titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_LEVEL, pageTypes: ["registration", "settings"] },
    { type: "specialization", titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.SPECIALIZATION, pageTypes: ["registration", "settings"] },
    { type: "interests", titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.INTERESTS, pageTypes: ["registration", "settings"] },
    { type: "video", titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.VIDEO_STEP_TITLE, pageTypes: ["registration", "settings"] },
    { type: "pricing", titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.PRICE_LABEL, pageTypes: ["registration", "settings"] },
    { type: "billingInformation", titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.BILLING_INFORMATION, pageTypes: ["registration", "settings"] },
];

export const InputNames = {
    prevButton: "prevBtn",
    nextButton: "nextBtn",
    saveButton: "saveBtn",
    submitButton: "submitBtn",
    firstName: "firstName",
    lastName: "lastName",
    email: "email",
    countryId: "countryId",
    languageId: "languageId",
    languageLevelId: "languageLevelId",
    phone: "phone",
    subjectId: "subjectId",
    description: "description",
    teachingExperienceId: "teachingExperienceId",
    subjectLevelIds: "subjectLevelIds",
    specializationIds: "specializationIds",
    interestIds: "interestIds",
    videoUrl: "videoUrl",
    price: "price",
    ico: "ico",
    placeOfBusiness: "placeOfBusiness",
    dic: "dic",
    isVatPayer: "isVatPayer",
    notificationSetting: "notificationSetting"
}

type RegistrationContextType = {
    steps: RegistrationStepInfoType[],
    handleSetStep: (step: RegistrationStepInfoType) => void,
    currentStep: RegistrationStepType,
    hasPreviousStep: boolean,
    hasNextStep: boolean,
    progress: number,
    pageType: PageType,
    handleSubmit: (event: React.FormEvent<HTMLFormElement>) => void
} | undefined;

type StepPropsType = {
    userInfo: UserInfoResponse;
}

const RegistrationContext = createContext<RegistrationContextType>(undefined);
const useTutorRegistrationContext = contextFactoryCreate('Registration', RegistrationContext);

const TutorRegistrationControllerProvider = ({ children, pageType }: { children: any, pageType: PageType }) => {
    const [currentStep, setCurrentStep] = useState<RegistrationStepType>(STEPS.filter(x => x.pageTypes.includes(pageType))[0].type);
    const steps = useMemo(() => STEPS.filter(x => x.pageTypes.includes(pageType)), [pageType]);
    const submitted = useRef(false);
    const dispatch = useAppDispatch();
    const userInfo = useAppSelector(selectUserInfo);
    const userStatus = useAppSelector(selectUserInfoStatus);
    const userInfoFiles = useAppSelector(selectUserInfoFiles);
    const updateUserInfoStatus = useAppSelector(selectUpdateUserInfoStatus);
    const errorKeys = useAppSelector(selectUserInfoErrors);
    const { convertToInternalCurrency } = usePriceLocalization();
    const { enqueueSnackbar } = useSnackbar();
    const { t } = useTranslation();

    useEffect(() => {
        if (pageType !== "registration") {
            return;
        }
        setPreviousStepFromStorage();
    }, [pageType]);

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
        const value = await storage.getCommonValue<string>(cacheConstants.tutorRegistrationLatestStep);
        if (!value) return;
        const parsedValue = parseInt(value);
        setCurrentStep(STEPS[parsedValue].type)
    }

    const setStepToStorage = async (newValue: number) => {
        if (pageType !== "registration") {
            return;
        }
        const storage = await getStorage();
        await storage.setCommonValue(cacheConstants.tutorRegistrationLatestStep, `${newValue}`);
    }

    const handleNextStep = () => {
        const currentIndex = STEPS.findIndex(x => x.type === currentStep);
        const newValue = STEPS
            .map((step, idx) => ({ step, idx }))
            .filter(x => x.idx > currentIndex)
            .filter(x => x.step.pageTypes.includes(pageType))
            .map(x => x.idx)[0];
        setStepToStorage(newValue);
        setCurrentStep(STEPS[newValue].type);
    }

    const handlePreviousStep = () => {
        const currentIndex = STEPS.findIndex(x => x.type === currentStep);
        const previousSteps = STEPS
            .map((step, idx) => ({ step, idx }))
            .filter(x => x.idx < currentIndex)
            .filter(x => x.step.pageTypes.includes(pageType))
            .map(x => x.idx);

        const newValue = previousSteps[previousSteps.length - 1];
        setStepToStorage(newValue);
        setCurrentStep(STEPS[newValue].type);
    }

    const handleSetStep = (step: RegistrationStepInfoType) => {
        setCurrentStep(step.type);
    }

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        submitted.current = true;

        if (userStatus !== LoadingStatusEnum.Loaded || !userInfo) return;
        let errors = [] as string[];
        const formData = new FormData(event.currentTarget);
        const fieldValues = Object.fromEntries(formData.entries());
        const submitterName = (event.nativeEvent as any).submitter.name;

        if (currentStep === "basicInfo") {
            const firstName = fieldValues[InputNames.firstName] as string;
            const lastName = fieldValues[InputNames.lastName] as string;
            const countryId = parseInt(fieldValues[InputNames.countryId] as string);
            const phone = fieldValues[InputNames.phone] as string;
            const languageKeys = Object.keys(fieldValues).filter(x => x.includes(InputNames.languageId));
            const languageLevelKeys = Object.keys(fieldValues).filter(x => x.includes(InputNames.languageLevelId));
            const languageWithLevels = Array(Math.min(languageKeys.length, languageLevelKeys.length)).fill(1)
                .map((x, idx) => ({
                    languageId: parseInt(fieldValues[languageKeys[idx]] as string),
                    languageLevelId: parseInt(fieldValues[languageLevelKeys[idx]] as string)
                }))
                .filter(x => x.languageId > 0 && x.languageLevelId > 0);
            const profilePhotoAttribute = getFileUrl(userInfoFiles, FileTypeEnum.ProfilePhoto);

            errors = [
                !firstName.length ? TRANSLATION_KEYS.ERRORS.MISSING_FIRST_NAME : undefined,
                !lastName.length ? TRANSLATION_KEYS.ERRORS.MISSING_LAST_NAME : undefined,
                !countryId ? TRANSLATION_KEYS.ERRORS.MISSING_COUNTRY_OF_ORIGIN : undefined,
                !phone.length || phone.length < 5 ? TRANSLATION_KEYS.ERRORS.MISSING_PHONE_NUMBER : undefined,
                !languageWithLevels.length ? TRANSLATION_KEYS.ERRORS.MISSING_SPOKEN_LANGUAGE : undefined,
                !profilePhotoAttribute.length ? TRANSLATION_KEYS.ERRORS.MISSING_PROFILE_PHOTO : undefined
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    firstName,
                    lastName,
                    countryId: countryId > 0 ? countryId : undefined,
                    phone,
                    languages: languageWithLevels,
                    timezone: getTimeZoneName()
                }))
            }
        }
        else if (currentStep === "notifications") {
            const input = Object.keys(fieldValues)
                .filter(x => x.startsWith(InputNames.notificationSetting))
                .map(x => x.replace(InputNames.notificationSetting + '_', ''))
                .map(x => ({ notificationTypeId: parseInt(x) }));

            dispatch(editNotificationSettings(input))
            dispatch(updateProfileSettingsInfo(toAppUserIm(userInfo))) // Update to see notification snackbar
        }
        else if (currentStep === "subjectSelect") {
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
        else if (currentStep === "description") {
            const otherAttributes = userInfo.attributes?.map(x => ({
                attributeTypeId: x.attributeType?.attributeTypeId,
                value: x.value
            }))?.filter(x => x.attributeTypeId !== AttributeTypeEnum.Description) ?? [];
            const descriptionAttribute = {
                attributeTypeId: AttributeTypeEnum.Description,
                value: fieldValues[InputNames.description] as string
            };
            errors = [
                !descriptionAttribute.value.length || descriptionAttribute.value.length < 100 ? TRANSLATION_KEYS.ERRORS.DESCRIPTION_TOO_SHORT : undefined
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    attributes: [...otherAttributes, descriptionAttribute]
                }))
            }
        }
        else if (currentStep === "teachingExpirience") {
            const teachingExperienceId = parseInt(fieldValues[InputNames.teachingExperienceId] as string);
            errors = [
                !teachingExperienceId ? TRANSLATION_KEYS.ERRORS.MISSING_TEACHING_EXPERIENCE : undefined
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    teachingExperienceId: teachingExperienceId > 0 ? teachingExperienceId : undefined
                }))
            }
        }
        else if (currentStep === "subjectLevel") {
            const subjectLevelsJson = fieldValues[InputNames.subjectLevelIds] as string;
            const subjectLevelIds = JSON.parse(subjectLevelsJson) as number[];
            errors = [
                !subjectLevelIds.length ? TRANSLATION_KEYS.ERRORS.MISSING_SUBJECT_LEVEL : undefined
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    subjectLevelIds
                }))
            }
        }
        else if (currentStep === "specialization") {
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
        else if (currentStep === "interests") {
            const interestsJSON = fieldValues[InputNames.interestIds] as string;
            const interestIds = JSON.parse(interestsJSON) as number[];
            errors = [
                !interestIds.length ? TRANSLATION_KEYS.ERRORS.MISSING_INTERESTS : undefined
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    interestIds
                }))
            }
        }
        else if (currentStep === "video") {
            const previousAttributes =
                userInfo?.attributes?.map((att, idx) => ({
                    attributeTypeId: att?.attributeType?.attributeTypeId ?? 0,
                    value: att?.value ?? ''
                }))
                    ?.filter(x => x.attributeTypeId !== AttributeTypeEnum.VideoUrl)
                    ?.filter(x => x.attributeTypeId !== 0)
                    ?.filter(x => x.value !== '')
                ?? [];

            const videoUrl = fieldValues[InputNames.videoUrl] as string;
            errors = [
                !videoUrl.length ? TRANSLATION_KEYS.ERRORS.MISSING_PROFILE_VIDEO_URL : undefined
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    attributes: [
                        ...previousAttributes,
                        {
                            attributeTypeId: AttributeTypeEnum.VideoUrl,
                            value: videoUrl
                        }
                    ].filter(x => x.value !== '')
                }))
            }
        }
        else if (currentStep === "pricing") {
            const price = parseInt(fieldValues[InputNames.price] as string);
            const priceConverted = convertToInternalCurrency({ price });

            errors = [
                isNaN(price) || price <= 0 ? TRANSLATION_KEYS.ERRORS.INVALID_HOUR_RATE : undefined,
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    hourRate: toOptionalPriceFixed(priceConverted)
                }))
            }
        }
        else if (currentStep === "billingInformation") {
            const ico = fieldValues[InputNames.ico] as string;
            const placeOfBusiness = fieldValues[InputNames.placeOfBusiness] as string;
            const dic = fieldValues[InputNames.dic] as string | undefined;
            const isVatPayer = fieldValues[InputNames.isVatPayer] !== undefined;
            const userStateId = submitterName === InputNames.submitButton
                ? UserStateEnum.ProfileValidation
                : userInfo.stateId;

            errors = [
                !ico.length ? TRANSLATION_KEYS.ERRORS.MISSING_ICO : undefined,
                !placeOfBusiness.length ? TRANSLATION_KEYS.ERRORS.MISSING_PLACE_OF_BUSINESS : undefined,
                isVatPayer && !dic?.length ? TRANSLATION_KEYS.ERRORS.MISSING_DIC : undefined,
            ].filter(x => x !== undefined).map(x => x!);

            if (!errors.length) {
                dispatch(updateProfileSettingsInfo({
                    ...toAppUserIm(userInfo),
                    ico,
                    dic,
                    placeOfBusiness,
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
            if (submitterName === InputNames.prevButton) {
                handlePreviousStep();
            }
            else if (submitterName === InputNames.nextButton) {
                handleNextStep();
            }
        }
    }

    const hasPreviousStep = () => {
        const index = steps.findIndex(x => x.type === currentStep);
        return index > 0;
    }

    const hasNextStep = () => {
        const index = steps.findIndex(x => x.type === currentStep);
        return index >= 0 && index < steps.length - 1;
    }

    return (
        <RegistrationContext.Provider value={{
            currentStep: currentStep,
            progress: Math.round((steps.findIndex(x => x.type === currentStep) ?? 0) * 100 / (steps.length - 1)),
            hasPreviousStep: hasPreviousStep(),
            hasNextStep: hasNextStep(),
            handleSubmit,
            steps,
            handleSetStep,
            pageType
        }}>
            {children}
        </RegistrationContext.Provider>
    )
}

const StepLoading = () => {

    return (
        <RegistrationStepBoxStyled sx={{ alignItems: "center", justifyContent: "center" }}>
            <CircularProgressCustom size={40} />
        </RegistrationStepBoxStyled>
    )
}

const StepError = () => {

    return (
        <RegistrationStepBoxStyled sx={{ alignItems: "center", justifyContent: "center" }}>
            <ErrorLarge />
        </RegistrationStepBoxStyled>
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

const OverviewStepLoaded = () => {
    const { t } = useTranslation();
    const overviewSteps: { titleKey: string, descriptionKey: string, icon: JSX.Element }[] = [
        {
            titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.TELL_US_ABOUT_YOURSELF,
            descriptionKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.TELL_US_ABOUT_YOURSELF_DESCRIPTION,
            icon: <PersonIcon />
        },
        {
            titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.STAND_OUT,
            descriptionKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.STAND_OUT_DESCRIPTION,
            icon: <EyesIcon />
        },
        {
            titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.SPECIALIZATION,
            descriptionKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.SPECIALIZATION_DESCRIPTION,
            icon: <SearchIcon />
        },
        {
            titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.PRICE_LABEL,
            descriptionKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.PRICE_LABEL_DESCRIPTION,
            icon: <MoneyIcon />
        },
        {
            titleKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.APPROVALS,
            descriptionKey: TRANSLATION_KEYS.TUTOR_SIGN_UP.APPROVALS_DESCRIPTION,
            icon: <PhoneIcon />
        }
    ]

    return (
        <FirstStepBoxStyled>
            <Box sx={{ paddingTop: { xs: "24px", md: "0px" } }}>
                <H2Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.EASY_TO_GET_STARTED)}</H2Styled>
            </Box>
            <Box sx={{ display: "flex", flexFlow: "column noWrap", overflowY: { md: "scroll" }, height: { md: "100%" }, "div:first-child": { marginTop: "auto" }, "div:last-child": { marginBottom: "auto" } }}>
                {
                    overviewSteps.map((x, idx) => (
                        <RightSideContentBoxStyled key={idx}>
                            <HeadingBoxStyled>
                                <H4Styled>{`${idx + 1}. ${t(x.titleKey)}`}</H4Styled>
                                <PStyled>{t(x.descriptionKey)}</PStyled>
                            </HeadingBoxStyled>
                            <IconStyledBox>{x.icon}</IconStyledBox>
                        </RightSideContentBoxStyled>
                    ))
                }
            </Box>
        </FirstStepBoxStyled>
    )
}
const OverviewStep = withLoadingAndErrorHandling(OverviewStepLoaded);

const CountrySelect = ({ userInfo, countries }: { userInfo: UserInfoResponse, countries: CodetableCountryResponse }) => {
    const { t } = useTranslation();
    const [open, setOpen] = useState(false);
    const [countryId, setCountryId] = useState(0);
    const errors = useAppSelector(selectUserInfoErrors);
    const hasError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_COUNTRY_OF_ORIGIN) ?? false;
    const items: CodetableType[] | undefined = useMemo(() => countries?.map(x => ({ id: x.countryId, nameKey: x.nameKey })), [countries])
    const selectedCountry = countries?.find(x => x.countryId === countryId);

    useEffect(() => {
        setCountryId(userInfo?.country?.countryId ?? 0);
    }, [userInfo]);


    const handleClick = (newValue: boolean) => {
        setOpen(newValue);
    }

    const handleSelectCountryId = (newCountryId: number) => {
        setCountryId(newCountryId);
    }

    return (
        <Menu
            sameWidths={true}
            content={
                <InputBoxStyled sx={{
                    border: hasError && !countryId ? `1px solid ${COLORS.RED}` : `1px solid ${COLORS.BLACK_10}`
                }}
                >
                    <Box className={countryId ? "active" : ""}>
                        {t(selectedCountry?.nameKey ?? TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_ORIGIN_COUNTRY_PLACEHOLDER)}
                    </Box>
                    <MunuInputIcon isOpen={open} />
                    <input type="number" readOnly={true} name={InputNames.countryId} value={countryId} style={{ display: "none" }} />
                </InputBoxStyled>
            }
            innerMenu={(
                <MenuItemsList items={items} onClick={handleSelectCountryId} />
            )}
            onClick={handleClick}
            autoClose={true}
        />
    )
}

const LanguageSelect = ({
    languageId,
    languages,
    idx,
    onLanguageSelect
}: {
    languageId: number,
    languages: CodetableLanguageResponse,
    idx: number,
    onLanguageSelect: (idx: number, newValue: number) => void
}) => {
    const { t } = useTranslation();
    const [open, setOpen] = useState(false);
    const inputNameWithIdx = `${InputNames.languageId}${idx}`;
    const currentLanguage = languages.find(x => x.languageId === languageId);
    const items: CodetableType[] | undefined = useMemo(() => languages?.map(x => ({ id: x.languageId, nameKey: x.nameKey })), [languages]);

    const handleClick = (newValue: boolean) => {
        setOpen(newValue);
    }

    return (
        <Menu
            sameWidths={true}
            content={
                <InputBoxStyled>
                    <Box className={languageId ? "active" : ""}>
                        {t(currentLanguage?.nameKey ?? TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_LANGUAGE_PLACEHOLDER)}
                    </Box>
                    <MunuInputIcon isOpen={open} />
                    <input type="number" readOnly={true} name={inputNameWithIdx} value={languageId} style={{ display: "none" }} />
                </InputBoxStyled>
            }
            innerMenu={<MenuItemsList items={items} onClick={(newValue: number) => onLanguageSelect(idx, newValue)} />}
            sx={{
                width: "100%"
            }}
            onClick={handleClick}
            autoClose={true}
        />
    )
}

const LanguageLevelSelect = ({
    languageLevelId,
    languageLevels,
    idx,
    onLanguageLevelSelect,
}: {
    languageLevelId: number,
    languageLevels: CodetableLanguageLevelResponse,
    idx: number,
    onLanguageLevelSelect: (idx: number, newValue: number) => void
}) => {
    const { t } = useTranslation();
    const [open, setOpen] = useState(false);
    const inputNameWithIdx = `${InputNames.languageLevelId}${idx}`;
    const current = languageLevels.find(x => x.languageLevelId === languageLevelId);
    const items: CodetableType[] | undefined = useMemo(() => languageLevels?.map(x => ({ id: x.languageLevelId, nameKey: x.nameKey })), [languageLevels]);

    const handleClick = (newValue: boolean) => {
        setOpen(newValue);
    }

    return (
        <Menu
            sameWidths={true}
            content={
                <InputBoxStyled>
                    <Box className={languageLevelId ? "active" : ""}>
                        {t(current?.nameKey ?? TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_LANGUAGE_LEVEL_PLACEHOLDER)}
                    </Box>
                    <MunuInputIcon isOpen={open} />
                    <input type="number" readOnly={true} name={inputNameWithIdx} value={languageLevelId} style={{ display: "none" }} />
                </InputBoxStyled>
            }
            innerMenu={<MenuItemsList items={items} onClick={(newValue: number) => onLanguageLevelSelect(idx, newValue)} />}
            sx={{
                width: "100%"
            }}
            onClick={handleClick}
            autoClose={true}
        />
    )
}

const RemoveLanguageLevel = ({ idx, onClick }: { idx: number, onClick: (idx: number) => void }) => {

    return (
        <Box
            sx={{
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                padding: "14px",
                borderRadius: "4px",
                backgroundColor: getColor(COLORS.RED, 0.1),
                color: COLORS.RED,
                ":hover": {
                    cursor: "pointer",
                    backgroundColor: COLORS.RED_HOVER,
                    color: COLORS.WHITE
                }
            }}
            onClick={() => onClick(idx)}>
            <IconLargeBoxStyled><TrashIcon /></IconLargeBoxStyled>
        </Box>
    )
}

const AddLanguageLevel = ({ onClick }: { onClick: () => void }) => {

    return (
        <Box
            sx={{
                width: "fit-content",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                padding: "14px",
                borderRadius: "4px",
                backgroundColor: COLORS.PRIMARY_10,
                color: COLORS.PRIMARY,
                ":hover": {
                    cursor: "pointer",
                    backgroundColor: COLORS.PRIMARY_HOVER,
                    color: COLORS.WHITE
                }
            }}
            onClick={onClick}>
            <IconLargeBoxStyled><PlusIcon /></IconLargeBoxStyled>
        </Box>
    )
}

const LanguageAndLevelSelect = ({ userInfo, languages, languageLevels }: { userInfo: UserInfoResponse, languages: CodetableLanguageResponse, languageLevels: CodetableLanguageLevelResponse }) => {
    const [languageIds, setLanguageIds] = useState<number[]>([0]);
    const [languageLevelIds, setLanguageLevelIds] = useState<number[]>([0]);
    const errors = useAppSelector(selectUserInfoErrors);
    const errorKey = errors?.find(x => x === TRANSLATION_KEYS.ERRORS.MISSING_SPOKEN_LANGUAGE);
    const { t } = useTranslation();

    useEffect(() => {
        if (!userInfo || !userInfo.languages) return;
        const newLanguageIds = userInfo.languages && userInfo.languages.length ? userInfo.languages?.map(x => x.language?.languageId ?? 0) : [0];
        const newLanguageLevelIds = userInfo.languages && userInfo.languages.length ? userInfo.languages?.map(x => x.level?.languageLevelId ?? 0) : [0];
        setLanguageIds(newLanguageIds);
        setLanguageLevelIds(newLanguageLevelIds);
    }, [userInfo]);


    const handleRemove = (idx: number) => {
        setLanguageIds(prev => produce(prev, draft => {
            draft.splice(idx, 1);
        }))
        setLanguageLevelIds(prev => produce(prev, draft => {
            draft.splice(idx, 1);
        }))
    }

    const handleAdd = () => {
        setLanguageIds(prev => [...prev, 0]);
        setLanguageLevelIds(prev => [...prev, 0]);
    }

    const handleLanguageSelect = (idx: number, newValue: number) => {
        setLanguageIds(prev => produce(prev, draft => {
            draft[idx] = newValue;
        }));
    }

    const handleLanguageLevelSelect = (idx: number, newValue: number) => {
        setLanguageLevelIds(prev => produce(prev, draft => {
            draft[idx] = newValue;
        }));
    }

    return (
        <Box sx={{
            display: "flex",
            flexFlow: "column noWrap",
            gap: "8px"
        }}>
            <RowItemBoxStyled>
                <ExtendableBoxStyled>
                    <InputFieldLabel titleKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_LANGUAGE} isMandatory={true} />
                </ExtendableBoxStyled>
                <ExtendableBoxStyled>
                    <InputFieldLabel titleKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_LEVEL} isMandatory={true} />
                </ExtendableBoxStyled>
                <LanguageLevelIconBoxStyled />
            </RowItemBoxStyled>
            {errorKey && !languageIds.filter(x => x !== 0).length && !languageLevelIds.filter(x => x !== 0).length ? <PStyled sx={{ color: COLORS.RED }}>{t(errorKey)}</PStyled> : <></>}
            {Array(Math.max(languageIds.length, languageLevelIds.length)).fill(1).map((x, idx) => (
                <RowItemBoxStyled key={idx}>
                    <LanguageSelect idx={idx} languageId={languageIds[idx]} languages={languages} onLanguageSelect={handleLanguageSelect} />
                    <LanguageLevelSelect idx={idx} languageLevelId={languageLevelIds[idx]} languageLevels={languageLevels} onLanguageLevelSelect={handleLanguageLevelSelect} />
                    <RemoveLanguageLevel idx={idx} onClick={handleRemove} />
                </RowItemBoxStyled>
            ))}
            <Box>
                <AddLanguageLevel onClick={handleAdd} />
            </Box>
        </Box>
    )
}

const PhoneNumber = ({ userInfo }: { userInfo: UserInfoResponse }) => {
    const [value, setValue] = React.useState(userInfo.phone ?? "");
    const errors = useAppSelector(selectUserInfoErrors);
    const hasError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_PHONE_NUMBER) ?? false;

    const handleChange = (newValue: string) => {
        if (!value.length && newValue.length === 9) {
            setValue(`+381${newValue}`)
            return;
        }
        setValue(newValue)
    }

    return (
        <PhoneInputBoxStyled sx={{
            border: hasError ? `1px solid ${COLORS.RED}` : `1px solid ${(COLORS.BLACK_10)}`
        }}>
            <PhoneInput
                value={value}
                country={'rs'}
                alwaysDefaultMask={false}
                placeholder="+(381) 64 123 4567"
                onChange={handleChange}
                inputProps={{
                    name: InputNames.phone,
                }}
            />
        </PhoneInputBoxStyled>
    )
}

const BillingInfoStepLoaded = ({ userInfo }: { userInfo: UserInfoResponse }) => {
    const { t } = useTranslation();
    const errors = useAppSelector(selectUserInfoErrors);
    const [showDic, setShowDic] = useState(userInfo.isVatPayer ?? false);
    const hasIcoError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_ICO);
    const hasPlaceOfBusinessError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_PLACE_OF_BUSINESS);
    const hasDicError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_DIC);

    useEffect(() => {
        setShowDic(userInfo.isVatPayer ?? false);
    }, [userInfo]);

    return (
        <RegistrationStepBoxStyled>
            <TitleHeaderBoxStyled>
                <H3Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.BILLING_INFORMATION)}</H3Styled>
            </TitleHeaderBoxStyled>
            <AllAtributesBoxStyled>
                <AttributeBoxStyled>
                    <InputFieldLabel titleKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_ICO} isMandatory={true} />
                    <TextField type="number" placeholderKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_ICO_PLACEHOLDER} defaultValue={userInfo?.ico ?? ""} name={InputNames.ico} hasError={hasIcoError} />
                </AttributeBoxStyled>
                <AttributeBoxStyled>
                    <InputFieldLabel titleKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_PLACE_OF_BUSINESS} isMandatory={true} />
                    <TextField placeholderKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_PLACE_OF_BUSINESS_PLACEHOLDER} defaultValue={userInfo?.placeOfBusiness ?? ""} name={InputNames.placeOfBusiness} hasError={hasPlaceOfBusinessError} />
                </AttributeBoxStyled>
                <AttributeCheckboxBoxStyled
                    onClick={() => setShowDic(!showDic)}
                >
                    <CheckboxStyled checked={showDic} name={InputNames.isVatPayer} />
                    <RegistrationLabelStyled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_DPH)}</RegistrationLabelStyled>
                </AttributeCheckboxBoxStyled>
                {showDic ? (
                    <AttributeBoxStyled>
                        <InputFieldLabel titleKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_DIC} isMandatory={true} />
                        <TextField placeholderKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_DIC_PLACEHOLDER} defaultValue={userInfo?.dic ?? ""} name={InputNames.dic} hasError={hasDicError} />
                    </AttributeBoxStyled>
                ) : <></>}
            </AllAtributesBoxStyled>
        </RegistrationStepBoxStyled>
    )
}

const BillingInfoStep = () => {
    const userInfo = useAppSelector(selectUserInfo);
    const userInfoStatus = useAppSelector(selectUserInfoStatus);
    const loadingStatus = getLoadingStatus([userInfoStatus]);

    return (
        <>
            {loadingStatus === LoadingStatusEnum.Loading ? <StepLoading /> : <></>}
            {loadingStatus === LoadingStatusEnum.Error ? <StepError /> : <></>}
            {loadingStatus === LoadingStatusEnum.Loaded && userInfo ? <BillingInfoStepLoaded userInfo={userInfo} /> : <></>}
        </>
    )
}


const BasicInfoStepLoaded = ({
    userInfo,
    countries,
    languages,
    languageLevels
}: {
    userInfo: UserInfoResponse,
    countries: CodetableCountryResponse,
    languages: CodetableLanguageResponse,
    languageLevels: CodetableLanguageLevelResponse
}) => {
    const { t } = useTranslation();
    const errors = useAppSelector(selectUserInfoErrors);
    const hasFirstNameError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_FIRST_NAME);
    const hasLastNameError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_LAST_NAME);

    return (
        <RegistrationStepBoxStyled>
            <TitleHeaderBoxStyled>
                <H3Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT)}</H3Styled>
                <PStyled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_DESCRIPTION)}</PStyled>
            </TitleHeaderBoxStyled>
            <AllAtributesBoxStyled>
                <ProfileImageUploader />
                <AttributeBoxStyled>
                    <InputFieldLabel titleKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_FIRST_NAME} isMandatory={true} />
                    <TextField placeholderKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_FIRST_NAME_PLACEHOLDER} defaultValue={userInfo?.firstName ?? ""} name={InputNames.firstName} hasError={hasFirstNameError} />
                </AttributeBoxStyled>
                <AttributeBoxStyled>
                    <InputFieldLabel titleKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_LAST_NAME} isMandatory={true} />
                    <TextField placeholderKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_LAST_NAME_PLACEHOLDER} defaultValue={userInfo?.lastName ?? ""} name={InputNames.lastName} hasError={hasLastNameError} />
                </AttributeBoxStyled>
                <AttributeBoxStyled>
                    <InputFieldLabel titleKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_ORIGIN_COUNTRY} isMandatory={true} />
                    <CountrySelect userInfo={userInfo} countries={countries} />
                </AttributeBoxStyled>
                <LanguageAndLevelSelect userInfo={userInfo} languages={languages} languageLevels={languageLevels} />
                <AttributeBoxStyled>
                    <InputFieldLabel titleKey={TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_PHONE_NUMBER} isMandatory={true} />
                    <PhoneNumber userInfo={userInfo} />
                </AttributeBoxStyled>
            </AllAtributesBoxStyled>
        </RegistrationStepBoxStyled>
    )
}
const BasicInfoStep = () => {
    const dispatch = useAppDispatch();
    const userInfo = useAppSelector(selectUserInfo);
    const userInfoStatus = useAppSelector(selectUserInfoStatus);
    const countries = useAppSelector(selectCountries);
    const countriesStatus = useAppSelector(selectCountriesStatus);
    const languages = useAppSelector(selectLanguages);
    const languagesStatus = useAppSelector(selectLanguagesStatus);
    const languageLevels = useAppSelector(selectLanguageLevels);
    const languageLevelsStatus = useAppSelector(selectLanguageLevelsStatus);

    const loadingStatus = getLoadingStatus([userInfoStatus, countriesStatus, languagesStatus, languageLevelsStatus]);

    useEffect(() => {
        dispatch(setUserInfo());
        dispatch(setLanguagesAsync());
        dispatch(setLanguageLevelsAsync());
        dispatch(setCountriesAsync());
    }, []);

    return (
        <>
            {loadingStatus === LoadingStatusEnum.Loading ? <StepLoading /> : <></>}
            {loadingStatus === LoadingStatusEnum.Error ? <StepError /> : <></>}
            {loadingStatus === LoadingStatusEnum.Loaded && userInfo && countries && languages && languageLevels ? (
                <BasicInfoStepLoaded
                    userInfo={userInfo}
                    countries={countries}
                    languages={languages}
                    languageLevels={languageLevels}
                />
            ) : <></>}
        </>
    )
};

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
        <RegistrationStepBoxStyled>
            <TitleHeaderBoxStyled>
                <H3Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.NOTIFICATIONS)}</H3Styled>
                <PStyled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.NOTIFICATIONS_DESCRIPTION)}</PStyled>
            </TitleHeaderBoxStyled>
            <AllAtributesBoxStyled>
                {notificationSettings.map(x => <NotificationSetting key={x.notificationTypeId!} notificationSetting={x} />)}
            </AllAtributesBoxStyled>
        </RegistrationStepBoxStyled>
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

const Subject = ({ subject, checked, onClick }: { subject: CodetableSubjectsResponse[0], checked: boolean, onClick: () => void }) => {
    const { t } = useTranslation();

    return (
        <ListItem onClick={onClick}>
            <RadioButton checked={checked} />
            <LanguageIconBoxStyled>
                <InjectIcon category="emoji" iconName={subject.iconName ?? ""} />
            </LanguageIconBoxStyled>
            <H5Styled>{t(subject.nameKey ?? "")}</H5Styled>
        </ListItem>
    )
}

const SelectSubjectStepLoaded = ({ userInfo }: { userInfo: UserInfoResponse }) => {
    const { t } = useTranslation();
    const [subjectId, setSubjectId] = useState<number | undefined>(undefined);
    const { subjects } = useAppDataContext();
    const errors = useAppSelector(selectUserInfoErrors);
    const errorKey = errors?.find(x => x === TRANSLATION_KEYS.ERRORS.MISSING_SUBJECT);

    useEffect(() => {
        setSubjectId(userInfo.subject?.subjectId ?? subjects[0].subjectId!);
    }, [userInfo, subjects]);

    return (
        <RegistrationStepBoxStyled>
            <TitleHeaderBoxStyled>
                <H3Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.SUBJECT)}</H3Styled>
                <PStyled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.SUBJECT_DESCRIPTION)}</PStyled>
                {errorKey && !subjectId ? <PStyled sx={{ color: COLORS.RED }}>{t(errorKey)}</PStyled> : <></>}
            </TitleHeaderBoxStyled>
            <ListItemsBoxStyled>
                {subjects.map(x => (
                    <Subject key={x.subjectId!} checked={subjectId === x.subjectId} subject={x} onClick={() => setSubjectId(x.subjectId)} />
                ))}
            </ListItemsBoxStyled>
            <input type="number" readOnly={true} name={InputNames.subjectId} value={subjectId} style={{ display: "none" }} />
        </RegistrationStepBoxStyled>

    )
}
const SelectSubjectStep = withLoadingAndErrorHandling(SelectSubjectStepLoaded);


const DescriptionStepLoaded = ({ userInfo }: { userInfo: UserInfoResponse }) => {
    const [value, setValue] = useState("");
    const { t } = useTranslation();
    const MAX_LENGTH = 1000;
    const errors = useAppSelector(selectUserInfoErrors);
    const errorKey = errors?.find(x => x === TRANSLATION_KEYS.ERRORS.DESCRIPTION_TOO_SHORT);
    const errorFixed = value.length > 100;

    useEffect(() => {
        const description = userInfo.attributes?.find(x => x.attributeType?.attributeTypeId === AttributeTypeEnum.Description)?.value ?? ""
        setValue(description);
    }, [userInfo]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const newValue = e.target.value;
        if (newValue.length > value.length && newValue.length > MAX_LENGTH) {
            return;
        }
        setValue(newValue);
    }

    return (
        <RegistrationStepBoxStyled>
            <TitleHeaderBoxStyled>
                <H3Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.DESCRIPTION)}</H3Styled>
                <PStyled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.DESCRIPTION_DESCRIPTION)}</PStyled>
                {errorKey && !errorFixed ? <PStyled sx={{ color: COLORS.RED }}>{t(errorKey)}</PStyled> : <></>}
            </TitleHeaderBoxStyled>
            <ListItemsBoxStyled sx={{ gap: "4px" }}>
                <TextBoxStyled>
                    <textarea
                        name={InputNames.description}
                        placeholder={t(TRANSLATION_KEYS.TUTOR_SIGN_UP.DESCRIPTION_PLACEHOLDER)}
                        value={value}
                        onChange={handleChange} />
                </TextBoxStyled>
                <Body1Styled>{`${value.length}/${MAX_LENGTH}`}</Body1Styled>
            </ListItemsBoxStyled>
        </RegistrationStepBoxStyled>

    )
}
const DescriptionStep = withLoadingAndErrorHandling(DescriptionStepLoaded);


const CertificateDropBox = ({ onSubmit }: { onSubmit: (files: File[]) => Promise<void> }) => {
    const { t } = useTranslation();
    const { isDragActive } = useDropBoxContext();
    const certificatesUploadStatus = useAppSelector(selectUploadCertificatesStatus);
    const uploadInputRef = useRef<HTMLInputElement | null>(null);

    const handleUploadButtonClick = () => {
        if (!uploadInputRef.current) return;
        uploadInputRef.current.click()
    }

    const handleFilesSelect = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (!event.target || !event.target.files) return;
        onSubmit(Array.from(event.target.files));
    }

    return (
        <InsertFileBoxStyled sx={{ backgroundColor: isDragActive ? COLORS.PRIMARY_10 : COLORS.WHITE }}>
            {certificatesUploadStatus !== LoadingStatusEnum.Loading ? (
                <>
                    <CertifiactionHeaderBoxStyled>
                        <IconCertificationStyled><FileIcon /></IconCertificationStyled>
                        <H5Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.UPLOAD_CERTIFICATES_DRAG_AND_DROP)}</H5Styled>
                        <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.UPLOAD_CERTIFICATES_SELECT_MULTIPLE_FILES)}</Body1Styled>
                    </CertifiactionHeaderBoxStyled>
                    <TertiaryButton onClick={handleUploadButtonClick}>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.UPLOAD_CERTIFICATES_UPLOAD_FROM_DEVICE)}</TertiaryButton>
                </>
            ) : <></>}
            {certificatesUploadStatus === LoadingStatusEnum.Loading ? <Skeleton variant="rectangular" animation="wave" width="100%" height="100%" /> : <></>}
            <input ref={uploadInputRef} accept="image/jpg, image/jpeg, image/heif, image/heic, application/pdf" multiple={true} hidden type="file" onChange={handleFilesSelect} />
        </InsertFileBoxStyled>
    )
}

const Certificate = ({ fileInfo }: { fileInfo: NonNullable<UserInfoResponse["files"]>[0] }) => {
    const dispatch = useAppDispatch();

    const handleDeleteFile = () => {
        dispatch(deleteUserFile({ filepath: fileInfo.filePath ?? "" }))
    }

    return (
        <InsertFileBoxStyled>
            <CertifiactionHeaderBoxStyled>
                <IconCertificationStyled><FileIcon /></IconCertificationStyled>
                <Link href={FileService.getFileUrl(fileInfo.filePath ?? "")} target="_blank" rel="noreferrer">
                    <Body1Styled sx={{ color: COLORS.PRIMARY }}>{fileInfo.fileName ?? ""}</Body1Styled>
                </Link>
                <DeleteIconButton onClick={handleDeleteFile} />
            </CertifiactionHeaderBoxStyled>
        </InsertFileBoxStyled >
    )
}

const CertificationStepLoaded = ({ userInfo }: { userInfo: UserInfoResponse }) => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const certificates = userInfo.files?.filter(x => x.fileTypeId === FileTypeEnum.Certificate);
    const deleteStatus = useAppSelector(selectDeleteUserFileStatus);
    const uploadCertificatesStatus = useAppSelector(selectUploadCertificatesStatus);
    const isLoading = deleteStatus === LoadingStatusEnum.Loading || uploadCertificatesStatus === LoadingStatusEnum.Loading;

    const uploadFiles = async (files: File[]) => {
        await dispatch(uploadCertificates(files));
    }

    return (
        <>
            {isLoading ? <StepLoading /> : <></>}
            {!isLoading ? (
                <RegistrationStepBoxStyled>
                    <TitleHeaderBoxStyled>
                        <H3Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.UPLOAD_CERTIFICATES)}</H3Styled>
                        <PStyled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.UPLOAD_CERTIFICATES_DESCRIPTION)}</PStyled>
                    </TitleHeaderBoxStyled>
                    <DropBoxProvider multiple={true} onFilesDrop={uploadFiles} fileFormats={[".heic", ".heif", ".jpeg", ".jpg", ".pdf", ".png"]}>
                        <CertificateDropBox onSubmit={uploadFiles} />
                    </DropBoxProvider>
                    {certificates ? certificates.map(x => <Certificate key={x.filePath!} fileInfo={x} />) : <></>}
                </RegistrationStepBoxStyled>
            ) : <></>}
        </>
    );
}
const CertificationStep = withLoadingAndErrorHandling(CertificationStepLoaded);


const TeachingExpirienceLoaded = ({ userInfo, teachingExperiences }: { userInfo: UserInfoResponse, teachingExperiences: CodetableTeachingExperienceResponse }) => {
    const [teachingExperienceId, setTeachingExperienceId] = useState<number>(0);
    const { t } = useTranslation();
    const errors = useAppSelector(selectUserInfoErrors);
    const errorKey = errors?.find(x => x === TRANSLATION_KEYS.ERRORS.MISSING_TEACHING_EXPERIENCE);

    useEffect(() => {
        setTeachingExperienceId(userInfo.teachingExperience?.teachingExperienceId ?? 0);
    }, [userInfo]);

    const handleSetTeachingExperience = (teachingExperienceId: number) => {
        setTeachingExperienceId(teachingExperienceId);
    }

    return (
        <RegistrationStepBoxStyled>
            <TitleHeaderBoxStyled>
                <H3Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.TEACHING_EXPIRIENCE)}</H3Styled>
                <PStyled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.TEACHING_EXPIRIENCE_DESCRIPTION)}</PStyled>
                {errorKey && !teachingExperienceId ? <PStyled sx={{ color: COLORS.RED }}>{t(errorKey)}</PStyled> : <></>}
            </TitleHeaderBoxStyled>
            <ListItemsBoxStyled>
                {teachingExperiences.map(x => (
                    <ListItem key={x.teachingExperienceId!} onClick={() => handleSetTeachingExperience(x.teachingExperienceId!)}>
                        <RadioButton checked={x.teachingExperienceId === teachingExperienceId} />
                        <H5Styled sx={{ textAlign: "left" }}>{t(x.nameKey ?? "")}</H5Styled>
                    </ListItem>
                ))}
            </ListItemsBoxStyled>
            <input type="number" readOnly={true} name={InputNames.teachingExperienceId} value={teachingExperienceId} style={{ display: "none" }} />
        </RegistrationStepBoxStyled>

    )
}
const TeachingExpirience = () => {
    const dispatch = useAppDispatch();
    const userInfo = useAppSelector(selectUserInfo);
    const userInfoStatus = useAppSelector(selectUserInfoStatus);
    const teachingExperiencesStatus = useAppSelector(selectTeachingExperienceStatus);
    const teachingExperiences = useAppSelector(selectTeachingExperiences);
    const loadingStatus = getLoadingStatus([userInfoStatus, teachingExperiencesStatus]);

    useEffect(() => {
        dispatch(setUserInfo());
        dispatch(setTeachingExperiences());
    }, []);

    return (
        <>
            {loadingStatus === LoadingStatusEnum.Loading ? <StepLoading /> : <></>}
            {loadingStatus === LoadingStatusEnum.Error ? <StepError /> : <></>}
            {loadingStatus === LoadingStatusEnum.Loaded && userInfo && teachingExperiences ? <TeachingExpirienceLoaded userInfo={userInfo} teachingExperiences={teachingExperiences} /> : <></>}
        </>
    )
};

const SubjectLevelStepLoaded = ({ userInfo, subjectLevels }: { userInfo: UserInfoResponse, subjectLevels: CodetableSubjectLevelsResponse }) => {
    const [subjectLevelIds, setSubjectLevelIds] = useState<number[]>([]);
    const { t } = useTranslation();
    const errors = useAppSelector(selectUserInfoErrors);
    const errorKey = errors?.find(x => x === TRANSLATION_KEYS.ERRORS.MISSING_SUBJECT_LEVEL);

    useEffect(() => {
        setSubjectLevelIds(userInfo.subjectLevels?.map(x => x.subjectLevelId!) ?? []);
    }, [userInfo]);

    const handleClick = (subjectLevelId: number) => {
        const idx = subjectLevelIds.indexOf(subjectLevelId);
        if (idx === -1) {
            const newSubjectLevelIds = [...subjectLevelIds, subjectLevelId];
            setSubjectLevelIds(newSubjectLevelIds);
            return;
        }
        const newSubjectLevelIds = produce(subjectLevelIds, draft => {
            draft.splice(idx, 1);
        });
        setSubjectLevelIds(newSubjectLevelIds);
    }

    return (
        <RegistrationStepBoxStyled>
            <TitleHeaderBoxStyled>
                <H3Styled>{t(TRANSLATION_KEYS.CHOOSE_SUBJECT_LEVEL_BOX.TITLE)}</H3Styled>
                <PStyled>{t(TRANSLATION_KEYS.CHOOSE_SUBJECT_LEVEL_BOX.DESCRIPTION)}</PStyled>
                {errorKey && !subjectLevelIds.length ? <PStyled sx={{ color: COLORS.RED }}>{t(errorKey)}</PStyled> : <></>}
            </TitleHeaderBoxStyled>
            <ListItemsBoxStyled>
                {subjectLevels.map(x => (
                    <ListItem key={x.subjectLevelId!} onClick={() => handleClick(x.subjectLevelId!)}>
                        <MuiCheckboxStyled sx={{ color: COLORS.PRIMARY }} checked={subjectLevelIds.some(y => y === x.subjectLevelId)} />
                        <LevelIconBoxStyled>
                            <InjectIcon category="subjectLevels" iconName={x.iconName ?? ""} />
                        </LevelIconBoxStyled>
                        <H5Styled>{t(x.tutorNameKey ?? "")}</H5Styled>
                    </ListItem>
                ))}
            </ListItemsBoxStyled>
            <input type="text" readOnly={true} name={InputNames.subjectLevelIds} value={JSON.stringify(subjectLevelIds)} style={{ display: "none" }} />
        </RegistrationStepBoxStyled>
    )
}

const SubjectLevelStep = () => {
    const dispatch = useAppDispatch();
    const userInfo = useAppSelector(selectUserInfo);
    const userInfoStatus = useAppSelector(selectUserInfoStatus);
    const subjectLevels = useAppSelector(selectSubjectLevels);
    const subjectLevelsStatus = useAppSelector(selectSubjectLevelsStatus);
    const loadingStatus = getLoadingStatus([userInfoStatus, subjectLevelsStatus]);

    useEffect(() => {
        dispatch(setUserInfo());
        dispatch(setSubjectLevelsAsync());
    }, []);

    return (
        <>
            {loadingStatus === LoadingStatusEnum.Loading ? <StepLoading /> : <></>}
            {loadingStatus === LoadingStatusEnum.Error ? <StepError /> : <></>}
            {loadingStatus === LoadingStatusEnum.Loaded && userInfo && subjectLevels ? <SubjectLevelStepLoaded userInfo={userInfo} subjectLevels={subjectLevels} /> : <></>}
        </>
    )
};

const SpecializationStepLoaded = ({ userInfo }: { userInfo: UserInfoResponse }) => {
    const { t } = useTranslation();
    const { specializations } = useAppDataContext();
    const [specializationIds, setSpecializationIds] = useState<number[]>([]);
    const filteredSpecializations = specializations?.filter(x => x.subjectId === userInfo.subjectId);
    const { enqueueSnackbar } = useSnackbar();
    const maxSpecializationsCount = Math.round(filteredSpecializations.length * COMMON_CONSTANTS.REGISTRATION.TUTOR.MAX_PERCENTAGE_OF_SPECIALIZATIONS / 100);
    const maxCount = Math.max(maxSpecializationsCount, 5);
    const errors = useAppSelector(selectUserInfoErrors);
    const errorKey = errors?.find(x => x === TRANSLATION_KEYS.ERRORS.MISSING_SPECIALIZATIONS);

    useEffect(() => {
        setSpecializationIds(userInfo.specializations?.filter(x => x.subjectId === userInfo.subjectId)?.map(x => x.specializationId!) ?? []);
    }, [userInfo]);

    const handleClick = (specializationId: number) => {
        const idx = specializationIds.indexOf(specializationId);
        if (idx === -1) {
            if (specializationIds.length >= maxCount) {
                enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_SIGN_UP.TOO_MANY_INTERESTS, { count: maxCount }), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
                return;
            }
            setSpecializationIds(prev => [...prev, specializationId]);
            return;
        }
        setSpecializationIds(prev => produce(prev, draft => {
            draft.splice(idx, 1);
        }));
    }

    return (
        <RegistrationStepBoxStyled>
            <TitleHeaderBoxStyled>
                <H3Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.SPECIALIZATION_STEP)}</H3Styled>
                <PStyled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.SPECIALIZATION_DESCRIPTION)}</PStyled>
                {errorKey && !specializationIds.length ? <PStyled sx={{ color: COLORS.RED }}>{t(errorKey)}</PStyled> : <></>}
            </TitleHeaderBoxStyled>
            <ItemStateButtonBoxStyled>
                {filteredSpecializations.map(x => (
                    <ItemStateButtonStyled
                        key={x.specializationId!}
                        onClick={() => handleClick(x.specializationId!)}
                        active={specializationIds.some(y => y === x.specializationId!)}
                    >
                        <LargeIconBoxStyled>
                            <InjectIcon category="specializations" iconName={x.iconName ?? ""} />
                        </LargeIconBoxStyled>
                        <H5Styled>{t(x.nameKey ?? "")}</H5Styled>
                    </ItemStateButtonStyled>
                ))}
                <input type="text" readOnly={true} name={InputNames.specializationIds} value={JSON.stringify(specializationIds)} style={{ display: "none" }} />
            </ItemStateButtonBoxStyled>
        </RegistrationStepBoxStyled>
    )
}
const SpecializationStep = withLoadingAndErrorHandling(SpecializationStepLoaded);

const InterestsStepLoaded = ({ userInfo, interests }: { userInfo: UserInfoResponse, interests: CodetableInterestsReponse }) => {
    const [interestIds, setInterestsIds] = useState<number[]>([]);
    const { t } = useTranslation();
    const errors = useAppSelector(selectUserInfoErrors);
    const errorKey = errors?.find(x => x === TRANSLATION_KEYS.ERRORS.MISSING_INTERESTS);
    const { enqueueSnackbar } = useSnackbar();

    useEffect(() => {
        setInterestsIds(userInfo.interests?.map(x => x.interestId!) ?? []);
    }, [userInfo]);

    const handleClick = (interestId: number) => {
        const idx = interestIds.indexOf(interestId);

        if (idx === -1) {
            if (interestIds.length >= COMMON_CONSTANTS.REGISTRATION.TUTOR.MAX_NUMBER_OF_INTERESTS) {
                enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_SIGN_UP.TOO_MANY_INTERESTS, { count: COMMON_CONSTANTS.REGISTRATION.TUTOR.MAX_NUMBER_OF_INTERESTS }), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
                return;
            }
            setInterestsIds(prev => [...prev, interestId]);
            return;
        }
        setInterestsIds(prev => produce(prev, draft => {
            draft.splice(idx, 1);
        }));
    }

    return (
        <RegistrationStepBoxStyled>
            <TitleHeaderBoxStyled>
                <H3Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.INTERESTS_STEP.TITLE)}</H3Styled>
                <PStyled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.INTERESTS_STEP.DESCRIPTION)}</PStyled>
                {errorKey && !interestIds.length ? <PStyled sx={{ color: COLORS.RED }}>{t(errorKey)}</PStyled> : <></>}
            </TitleHeaderBoxStyled>
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
        </RegistrationStepBoxStyled>
    )
}

const InterestsStep = () => {
    const dispatch = useAppDispatch();
    const userInfo = useAppSelector(selectUserInfo);
    const userInfoStatus = useAppSelector(selectUserInfoStatus);
    const interests = useAppSelector(selectInterests);
    const interestsStatus = useAppSelector(selectInterestsStatus);
    const loadingStatus = getLoadingStatus([userInfoStatus, interestsStatus]);

    useEffect(() => {
        dispatch(setUserInfo());
        dispatch(setInterests());
    }, []);

    return (
        <>
            {loadingStatus === LoadingStatusEnum.Loading ? <StepLoading /> : <></>}
            {loadingStatus === LoadingStatusEnum.Error ? <StepError /> : <></>}
            {loadingStatus === LoadingStatusEnum.Loaded && userInfo && interests ? <InterestsStepLoaded userInfo={userInfo} interests={interests} /> : <></>}
        </>
    )
};

const VideoStepLoaded = ({ userInfo }: { userInfo: UserInfoResponse }) => {
    const { t } = useTranslation();
    const [query, setQuery] = React.useState<string>('');
    const [enteredEmbedId, setEnteredEmbedId] = React.useState('');
    const errors = useAppSelector(selectUserInfoErrors);
    const errorKey = errors?.find(x => x === TRANSLATION_KEYS.ERRORS.MISSING_PROFILE_VIDEO_URL);
    const errorFixed = enteredEmbedId.length > 0;

    useEffect(() => {
        const videoLink = userInfo?.attributes?.find(x => x?.attributeType?.attributeTypeId === AttributeTypeEnum.VideoUrl)?.value ?? '';
        setQuery(getEmbededId(videoLink ?? "") ?? "");
        setEnteredEmbedId(videoLink);
    }, [userInfo]);

    useEffect(() => {
        const timeOutId = setTimeout(() => {
            setQuery(getEmbededId(enteredEmbedId) ?? "");
        }, 500);
        return () => clearTimeout(timeOutId);
    }, [enteredEmbedId]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const value = e.target.value;
        setEnteredEmbedId(value as string);
    }

    return (
        <RegistrationStepBoxStyled>
            <TitleHeaderBoxStyled>
                <H3Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.VIDEO_STEP_TITLE)}</H3Styled>
                <PStyled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.VIDEO_STEP_DESCRIPTION)}</PStyled>
                {errorKey && !errorFixed ? <PStyled sx={{ color: COLORS.RED }}>{t(errorKey)}</PStyled> : <></>}
            </TitleHeaderBoxStyled>

            <AttributeBoxStyled>
                <TextFieldStyled
                    fullWidth
                    placeholder={t(TRANSLATION_KEYS.TUTOR_SIGN_UP.VIDEO_TEXT_PLACEHOLDER)}
                    name={InputNames.videoUrl}
                    type="text"
                    value={enteredEmbedId}
                    onChange={handleChange}
                    sx={{
                        border: `1px solid ${(COLORS.BLACK_10)}`
                    }}
                />
            </AttributeBoxStyled>

            <YoutubeBoxStyled>
                {query.length ? (
                    <iframe
                        width="100%"
                        height="315"
                        src={getYoutubeVideoUrlFromQuery(query ?? "")}
                        frameBorder="0"
                        allowFullScreen={true}
                        title="YouTube video player"
                    />) : <></>}
            </YoutubeBoxStyled>
        </RegistrationStepBoxStyled>
    );
}
const VideoStep = withLoadingAndErrorHandling(VideoStepLoaded);

const PricingStepLoaded = ({ userInfo }: { userInfo: UserInfoResponse }) => {
    const { t } = useTranslation();
    const [price, setPrice] = useState<string>('');
    const { currentCurrency } = useAppDataContext();
    const { convertToUserCurrency } = usePriceLocalization();
    const textareaRef = useRef<HTMLTextAreaElement>(null);
    const errors = useAppSelector(selectUserInfoErrors);
    const errorKey = errors?.find(x => x === TRANSLATION_KEYS.ERRORS.INVALID_HOUR_RATE);

    useEffect(() => {
        setPrice(userInfo.hourRate ? '' + toOptionalPriceFixed(convertToUserCurrency({ price: userInfo.hourRate })) : '')
    }, [userInfo, currentCurrency]);

    const isNumeric = (num: any) => (typeof (num) === 'number' || typeof (num) === "string" && num.trim() !== '') && !isNaN(num as number);
    const handleSetValue = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const value = event.target.value;

        if (isNumeric(value) && parseInt(value) >= 0) {
            setPrice(value);
            return;
        }
        if (value.length === 0) {
            setPrice('');
        }
    }

    const handlePlusClick = () => {
        const prev = parseInt(price);
        if (isNaN(prev)) {
            setPrice('1');
            return;
        };
        setPrice(`${prev + 1}`);
    }

    const handleMinusClick = () => {
        const prev = parseInt(price);
        if (isNaN(prev)) return;
        if (prev === 0) return;
        setPrice(`${prev - 1}`);
    }

    return (
        <RegistrationStepBoxStyled>
            <TitleHeaderBoxStyled>
                <H3Styled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.PRICE_STEP)}</H3Styled>
                <PStyled>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.PRICE_STEP_DESCRIPTION)}</PStyled>
                {errorKey && !(parseInt(price) > 0) ? <PStyled sx={{ color: COLORS.RED }}>{t(errorKey)}</PStyled> : <></>}
            </TitleHeaderBoxStyled>
            <PriceBoxingStyled>
                <PricingInnerBoxStyled>
                    <IconPricingStyled onClick={handleMinusClick}><MinusIcon /></IconPricingStyled>
                    <PricingBoxStyled>
                        <textarea
                            ref={textareaRef}
                            name={InputNames.price}
                            placeholder={'0 ' + currentCurrency?.symbol ?? ""}
                            onChange={handleSetValue}
                            value={price}
                            inputMode="numeric"
                        />
                        {price.length ? (
                            <CurrencyPlaceholderBoxStyled sx={{
                                position: "absolute",
                                top: "79px",
                                left: `calc(50% + 20px + ${price.length * 16}px)`
                            }}>
                                {currentCurrency?.symbol ?? ""}
                            </CurrencyPlaceholderBoxStyled>
                        ) : <></>}
                    </PricingBoxStyled>
                    <IconPricingStyled onClick={handlePlusClick}><PlusIcon /></IconPricingStyled>
                </PricingInnerBoxStyled>
                <PStyled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.PRICE_PER_LESSON)}</PStyled>
            </PriceBoxingStyled>
        </RegistrationStepBoxStyled>
    )
}
const PricingStep = withLoadingAndErrorHandling(PricingStepLoaded);

const RegistrationButtonControllers = () => {
    const { progress, hasPreviousStep, hasNextStep } = useTutorRegistrationContext();
    const { t } = useTranslation();

    return (
        <ButtonsContainerBoxStyled>
            {(!hasPreviousStep && hasNextStep)
                ? (
                    <>
                        <StaticLinearProgress value={progress} />
                        <ContainerStyled>
                            <ButtonsBoxStyled sx={{ justifyContent: "flex-end" }}>
                                <PrimaryButton type="submit" name={InputNames.nextButton}>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.GET_STARTED)}</PrimaryButton>
                            </ButtonsBoxStyled>
                        </ContainerStyled>
                    </>

                ) : (<></>)
            }
            {(hasPreviousStep && hasNextStep)
                ? (
                    <>
                        <StaticLinearProgress value={progress} />
                        <ContainerStyled>
                            <ButtonsBoxStyled>
                                <TertiaryButton sx={{ borderRadius: "20px", padding: "12px 24px" }} type="submit" name={InputNames.prevButton}>
                                    {t(TRANSLATION_KEYS.TUTOR_SIGN_UP.BACK)}
                                </TertiaryButton>
                                <PrimaryButton type="submit" name={InputNames.nextButton}>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.NEXT)}</PrimaryButton>
                            </ButtonsBoxStyled>
                        </ContainerStyled>
                    </>

                ) : (<></>)
            }
            {(hasPreviousStep && !hasNextStep)
                ? (
                    <>
                        <StaticLinearProgress value={progress} />
                        <ContainerStyled>
                            <ButtonsBoxStyled>
                                <TertiaryButton sx={{ borderRadius: "20px", padding: "12px 24px" }} type="submit" name={InputNames.prevButton}>
                                    {t(TRANSLATION_KEYS.TUTOR_SIGN_UP.BACK)}
                                </TertiaryButton>
                                <PrimaryButton
                                    type="submit"
                                    name={InputNames.submitButton}
                                >
                                    {t(TRANSLATION_KEYS.TUTOR_SIGN_UP.SUBMIT)}
                                </PrimaryButton>
                            </ButtonsBoxStyled>
                        </ContainerStyled>
                    </>
                ) : (<></>)
            }
            { /* Show nothing when no button is available*/}
            {!hasPreviousStep && !hasNextStep ? <></> : <></>}
        </ButtonsContainerBoxStyled>
    )
}

const SettingsControllers = () => {
    const { handleSetStep, steps, currentStep } = useTutorRegistrationContext();
    const { t } = useTranslation();

    return (
        <ContainerStyled>
            <TabsBoxShrinkedStyled >
                {steps.map((x, idx) => (
                    <TabItemButtonStyled
                        key={idx}
                        onClick={() => handleSetStep(x)}
                        sx={{
                            backgroundColor: currentStep === x.type ? COLORS.PRIMARY : "transparent",
                            color: currentStep === x.type ? COLORS.WHITE : COLORS.BLACK,
                            ":hover": { backgroundColor: currentStep === x.type ? COLORS.PRIMARY : COLORS.PRIMARY_10, color: currentStep === x.type ? COLORS.WHITE : COLORS.BLACK }
                        }}>
                        {t(x.titleKey)}
                    </TabItemButtonStyled>
                ))}
            </TabsBoxShrinkedStyled>
        </ContainerStyled>
    )
}

const TutorProfileSettingsViews = ({ pageType }: { pageType: PageType }) => {
    const { currentStep, handleSubmit } = useTutorRegistrationContext();
    const { screenType } = useContext(UIContext);
    return (
        <form onSubmit={handleSubmit}
            style={{ width: "100%", display: "flex", flexFlow: "column noWrap", flex: "1 1 auto", overflow: "auto", alignItems: "center", gap: screenType === "desktop" ? "24px" : "16px", padding: "24px 0" }}
        >
            {pageType === "settings" ? <SettingsControllers /> : <></>}
            {currentStep === "overview" ? <OverviewStep /> : <></>}
            {currentStep === "basicInfo" ? <BasicInfoStep /> : <></>}
            {currentStep === "notifications" ? <NotificationsStep /> : <></>}
            {currentStep === "subjectSelect" ? <SelectSubjectStep /> : <></>}
            {currentStep === "description" ? <DescriptionStep /> : <></>}
            {currentStep === "certificates" ? <CertificationStep /> : <></>}
            {currentStep === "teachingExpirience" ? <TeachingExpirience /> : <></>}
            {currentStep === "subjectLevel" ? <SubjectLevelStep /> : <></>}
            {currentStep === "specialization" ? <SpecializationStep /> : <></>}
            {currentStep === "interests" ? <InterestsStep /> : <></>}
            {currentStep === "video" ? <VideoStep /> : <></>}
            {currentStep === "pricing" ? <PricingStep /> : <></>}
            {currentStep === "billingInformation" ? <BillingInfoStep /> : <></>}
            {pageType === "registration" ? <RegistrationButtonControllers /> : <></>}
            {pageType === "settings" ? <SaveButton /> : <></>}
        </form>
    )
}

const TutorProfileSettings = ({ pageType }: { pageType: PageType }) => {

    return (
        <TutorRegistrationControllerProvider pageType={pageType}>
            <TutorProfileSettingsViews pageType={pageType} />
        </TutorRegistrationControllerProvider>
    )
};

export default TutorProfileSettings;