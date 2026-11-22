import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useAppDispatch, useAppSelector, contextFactoryCreate, useUpdateEffect } from "core/redux/reduxHooks";
import { createContext, useContext, useEffect, useMemo, useState } from "react";
import TutorPackageSaleSelectionLarge from "../profile/TutorPackageSaleSelectionLarge";
import AvailabilityCalendar from "../profile/calendar/AvailabilityCalendar";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import CloseIcon from "assets/close.svg";
import { useTranslation } from "components/core/TranslationProvider";
import { selectNumberOfSelectedEvents, selectNumberOfLessons, selectReservationStartTimes, selectDuration, resetAvailabilityCalendarData, setNumberOfLessons, setDuration, selectPackageType } from "core/redux/availabilityCalendarSlice";
import { UIContext } from "components/core/UIContextProvider";
import TutorPackageSaleSelection from "../profile/TutorPackageSaleSelection";
import { PrimaryButton } from "components/styles/PrimaryButton";
import { SecondaryButton } from "components/styles/SecondaryButton";
import Tooltip from "components/styles/Tooltip";
import { ClassDurationEnum, getDurationInMinutes } from "core/constants/enums/ClassDurationEnum";
import { GetCouponInfoResponse, ReservationFilterResponse, ReservationsCreateResponse, createReservation, editReservation, selectCouponInfo, selectCouponInfoStatus, selectCreateReservationsStatus, selectEditReservationStatus, setCouponInfo } from "core/redux/reservationSlice";
import moment from "moment";
import { useSnackbar } from "notistack";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import Navbar, { NavbarSourceEnum } from "components/navbar/Navbar";
import Box from "@mui/material/Box";
import Modal from "@mui/material/Modal";
import H4Styled from "components/styles/typography/H4Styled";
import H3Styled from "components/styles/typography/H3Styled";
import PStyled from "components/styles/typography/PStyled";
import YaspeakAvatar from "assets/yaspeak_avatar.svg"
import { getUserName } from "core/services/Utils/userUtils";
import { getFormattedDate } from "core/services/Utils/reservationUtil";
import { AvatarStyled } from "components/styles/user/AvatarStyled";
import { getProfilePhotoThumbnailUrl, getProfilePhotoUrl } from "core/services/Utils/imageUtil";
import { paths } from "apiTypes";
import H5StyledComponent from "components/styles/typography/H5Styled";
import { GetUserWalletStatisticsResponse, selectUserWalletStatistics, selectUserWalletStatisticsStatus, setUserWalletStatistics } from "core/redux/userWalletSlice";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import ErrorLarge from "components/core/error/ErrorLarge";
import { getAmountToPay, getInternalCreditsAmountToUse, getPrice, getTotalPrice, getDiscountAmount } from "core/services/Utils/walletUtil";
import { MuiCheckboxStyled } from "components/styles/MuiCheckboxStyled";
import { useRouter } from "next/router";
import { snackbarConstants } from "components/core/Snackbar";
import routes from "routes/routes";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { selectShortUserInfo, setChangePassword, setShortUserInfo } from "core/redux/userSlice";
import { useAppDataContext } from "components/core/providers/AppProvider";
import { ContainerStyled } from "components/styles/ContainerStyled";
import { selectIsAuthorized, selectUserStateId } from "core/redux/authSlice";
import Authentication, { PageType } from "components/auth/Authentication";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import ConfirmEmailBox from "components/ConfirmEmailBox";
import SetPasswordBox from "components/tutor/SetPasswordBox";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import Avatar from "@mui/material/Avatar";
import TutorPackageSelection from "components/profile/TutorPackageSelection";
import StaticLinearProgress from "components/styles/StaticLinearProgress";
import { isDispatchFulfilled, isDispatchRejected } from "core/services/Utils/apiUtil";
import { PrimaryButtonCompacted } from "components/styles/PrimaryButtonCompacted";
import { TextFieldStyled } from "components/styles/TextFieldStyled";
import { usePriceLocalization } from "core/hooks/priceHook";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";

const ModalStyled = styled(Modal)(() => sx({
    height: "100%",
    ".MuiBackdrop-root": {
        backgroundColor: "transparent"
    }
})) as typeof Modal;

const ContainerCustomStyled = styled(ContainerStyled)(() => sx({
    height: "100%",
    display: "flex",
    flexFlow: "column noWrap",
    gap: { xs: "16px", md: "24px" }
})) as typeof ContainerStyled;

const FullscreenModalBoxStyled = styled(Box)(() => sx({
    position: "absolute",
    top: "0",
    left: "0",
    width: "100%",
    height: "100%",
    backgroundColor: COLORS.WHITE,
    outline: "none",
    overflowY: "auto",
    display: "flex",
    flexFlow: "column noWrap"
})) as typeof Box;

const FullscreenModalInnerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flex: "1 1 auto",
    flexFlow: "column noWrap",
    overflowY: "auto",
    borderWidth: "1px 1px 0 1px",
    borderStyle: "solid",
    borderColor: COLORS.BLACK_10,
    boxShadow: '0px 5px 4px rgba(0, 0, 0, 0.1)',
    borderRadius: '8px 8px 0px 0px',
    padding: { xs: "16px 0", md: "24px 0" },
    gap: { xs: "16px", md: "24px" },
    margin: "16px 16px 0 16px",
})) as typeof Box;

const HeadingBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    flex: "0 1 auto",
    alignItems: "center",
    justifyContent: "space-between",
    width: "100%",
})) as typeof Box;

const InnerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    gap: { xs: "16px", md: "24px" },
    height: "100%"
})) as typeof Box;

const CloseIconBoxStyled = styled(Box)(() => sx({
    color: COLORS.BLACK_40,
    padding: "4px",
    "svg": {
        transform: "scale(1.25)"
    },
    ":hover": {
        cursor: "pointer",
        color: COLORS.BLACK
    }
})) as typeof Box;

const CenterBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flex: "1 1 auto",
    flexFlow: "column noWrap",
    gap: "16px",
    overflowY: "auto"
})) as typeof Box;

const LessonSchedulingDescriptionTypographyStyled = styled(PStyled)(() => sx({
    flex: "0 1 auto"
})) as typeof PStyled;

const AvailabilityBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flex: "1 1 auto",
    flexFlow: "column noWrap",
    overflowY: "auto",
    border: { xs: "none", md: `1px solid ${COLORS.BLACK_10}` },
    borderRadius: "4px",
    padding: { xs: "0", md: "24px 24px 0" },
})) as typeof Box;

const ButtonsBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    flex: "0 1 auto",
    justifyContent: "space-between",
    alignItems: "center",
    width: "100%"
})) as typeof Box;

const ContinueActiveButtonStyled = styled(PrimaryButtonRectangle)(() => sx({
    padding: "10px 12px"
})) as typeof Box;

const ContinueNonActiveButtonStyled = styled(PrimaryButton)(() => sx({
    borderRadius: "4px",
    padding: "10px 12px",
    backgroundColor: COLORS.BLACK_10,
    color: COLORS.WHITE,
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.BLACK_10,
        color: COLORS.WHITE,
    }
})) as typeof Box;

const BackButtonStyled = styled(SecondaryButton)(() => sx({
    borderRadius: "4px",
    padding: "10px 12px"
})) as typeof Box;

const ItemTypographyStyled = styled(PStyled)(() => sx({
    color: COLORS.BLACK
})) as typeof PStyled;

const ItemBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    justifyContent: "space-between",
    alignItems: "center",
    gap: "4px"
})) as typeof Box;

const InternalCreditsBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    gap: "8px",
    alignItems: "center",
    ":hover": {
        cursor: "pointer"
    }
})) as typeof Box;

const UserAvatarStyled = styled(AvatarStyled)(() => sx({
    width: "48px",
    height: "48px"
})) as typeof AvatarStyled;

const UserBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    gap: "8px",
    alignItems: "center"
})) as typeof Box;

const PayButtonStyled = styled(PrimaryButtonRectangle)(() => sx({
    minWidth: "125px",
    padding: "10px"
})) as typeof PrimaryButton;

const NonActivePayButtonStyled = styled(PayButtonStyled)(() => sx({
    backgroundColor: COLORS.PRIMARY_40,
    ":hover": {
        backgroundColor: COLORS.PRIMARY_40,
    }
})) as typeof PayButtonStyled;

const ExpandableBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: { xs: "flex-start", md: "safe center" },
    justifyContent: "center",
    flex: "1 1 auto",
    height: "100%",
    overflow: "auto"
})) as typeof Box;

const AuthInnerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "row noWrap",
    gap: "40px",
    padding: { md: "32px" },
    borderRadius: { md: "4px" },
    backgroundColor: COLORS.WHITE,
    border: { md: `1px solid ${COLORS.BLACK_10}` },
    a: {
        textDecoration: "underline",
        color: COLORS.PRIMARY,
    }
})) as typeof Box;

const HeaderImageBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "row noWrap",
    gap: "12px",
    alignItems: "center"
})) as typeof Box;

const ButtonsContainerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    justifyContent: "space-between",
    alignItems: "center",
    width: "100%",
    gap: { xs: "16px", md: "24px" },
    flex: "0 1 auto"
})) as typeof Box;

type TutorInfoType = {
    userId: number | null | undefined,
    firstName: string | null | undefined,
    lastName: string | null | undefined,
    hourRate: number | null | undefined,
    currencyCode: string | null | undefined,
    teachesLessonsForChildren: boolean | undefined
    files: paths['/api/user/short-info']['get']['responses']['200']['content']['application/json']['files']
}
type ReservationType = NonNullable<ReservationFilterResponse["data"][0]>;
type ReservationStepType = "init" | "packageTypeSelection" | "packageSaleSelection" | "timeScheduling" | "authorization" | "orderSummary" | "timeRescheduling";
type ReservationModalContextType = {
    tutorInfo: TutorInfoType,
    progress: number,
    handleOpen: () => void,
    handleClose: () => void,
    handleRescheduleClick: () => void,
    handleContinue: () => void,
    handleBack: () => void
} | undefined;

const STEPS: ReservationStepType[] = ["packageTypeSelection", "packageSaleSelection", "timeScheduling", "authorization", "orderSummary"];

const ReservationDetailsModalContext = createContext<ReservationModalContextType>(undefined);
export const useReservationModalContext = contextFactoryCreate('ReservationModal', ReservationDetailsModalContext);

const CloseIconBox = () => {
    const { handleClose } = useReservationModalContext();

    return (
        <CloseIconBoxStyled onClick={handleClose}>
            <CloseIcon />
        </CloseIconBoxStyled>
    )
}

const LineSeparator = () => {
    return (
        <Box height="1px" width="100%" sx={{ display: "flex", flex: "0 1 1px", backgroundColor: COLORS.BLACK_5 }} />
    )
}

const PackageMenuHeader = ({ title }: { title: string }) => {
    const { tutorInfo } = useReservationModalContext();

    return (
        <HeadingBoxStyled>
            <HeaderImageBoxStyled>
                <Avatar src={getProfilePhotoUrl(tutorInfo)} />
                <H3Styled>{title}</H3Styled>
            </HeaderImageBoxStyled>
            <CloseIconBox />
        </HeadingBoxStyled>
    )
}

const ButtonsBoxWithProgress = ({ children }: { children: any }) => {
    const { progress } = useReservationModalContext();

    return (
        <ButtonsContainerBoxStyled>
            <StaticLinearProgress value={progress} sx={{ height: "6px" }} />
            <ContainerCustomStyled>
                <ButtonsBoxStyled>
                    {children}
                </ButtonsBoxStyled>
            </ContainerCustomStyled>
        </ButtonsContainerBoxStyled>
    )
}

const PackageSaleSelection = () => {
    const { t } = useTranslation();
    const { screenType } = useContext(UIContext);
    const { handleBack, handleContinue, tutorInfo } = useReservationModalContext();

    return (
        <>
            <ContainerCustomStyled>
                <PackageMenuHeader title={t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.LESSON_SALE_HEADER)} />
                <LineSeparator />
                <CenterBoxStyled>
                    <PStyled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.LESSON_SALE_DESCRIPTION)}</PStyled>
                    {screenType === "desktop" ? <TutorPackageSaleSelectionLarge tutorInfo={tutorInfo} /> : <></>}
                    {screenType === "mobile" ? <TutorPackageSaleSelection tutorInfo={tutorInfo} /> : <></>}
                </CenterBoxStyled>
            </ContainerCustomStyled>
            <ButtonsBoxWithProgress>
                <BackButtonStyled onClick={handleBack}>{t(TRANSLATION_KEYS.COMMON.BACK)}</BackButtonStyled>
                <ContinueActiveButtonStyled onClick={handleContinue}>{t(TRANSLATION_KEYS.COMMON.CONTINUE)}</ContinueActiveButtonStyled>
            </ButtonsBoxWithProgress>
        </>
    )
}

const PackageTypeSelection = () => {
    const { t } = useTranslation();
    const { screenType } = useContext(UIContext);
    const { handleBack, handleContinue, tutorInfo } = useReservationModalContext();

    return (
        <>
            <ContainerCustomStyled>
                <PackageMenuHeader title={t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.LESSON_OPTIONS_HEADER)} />
                <LineSeparator />
                <CenterBoxStyled>
                    <PStyled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.LESSON_OPTIONS_DESCRIPTION)}</PStyled>
                    {screenType === "desktop" ? <TutorPackageSelection tutorInfo={tutorInfo} variant="large" /> : <></>}
                    {screenType === "mobile" ? <TutorPackageSelection tutorInfo={tutorInfo} variant="small" /> : <></>}
                </CenterBoxStyled>
            </ContainerCustomStyled>
            <ButtonsBoxWithProgress>
                <BackButtonStyled onClick={handleBack}>{t(TRANSLATION_KEYS.COMMON.BACK)}</BackButtonStyled>
                <ContinueActiveButtonStyled onClick={handleContinue}>{t(TRANSLATION_KEYS.COMMON.CONTINUE)}</ContinueActiveButtonStyled>
            </ButtonsBoxWithProgress>
        </>
    )
}

const TimeSchedulingHeader = () => {
    const { t } = useTranslation();
    const selectedNumber = useAppSelector(selectNumberOfSelectedEvents);
    const totalToSelect = useAppSelector(selectNumberOfLessons);
    const title = useMemo(() => t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.TIME_SCHEDULING_HEADER_MULTIPLE, { selected: selectedNumber, total: totalToSelect }), [selectedNumber, totalToSelect]);

    return (
        <PackageMenuHeader title={title} />
    )
}

const SchedulingViewContinueButton = () => {
    const selected = useAppSelector(selectNumberOfSelectedEvents);
    const total = useAppSelector(selectNumberOfLessons);
    const numberToSelect = total - selected;
    const { t } = useTranslation();
    const { handleContinue } = useReservationModalContext();

    const isActive = () => {
        return selected >= total;
    }

    const handleClick = () => {
        if (selected < total) return;
        handleContinue();
    }

    return isActive() ? (
        <ContinueActiveButtonStyled onClick={handleClick}>{t(TRANSLATION_KEYS.COMMON.CONTINUE)}</ContinueActiveButtonStyled>
    ) : (
        <Tooltip text={t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.SELECT_LESSONS_TO_CONTINUE_TOOLTIP, { numberToSelect })} >
            <ContinueNonActiveButtonStyled>{t(TRANSLATION_KEYS.COMMON.CONTINUE)}</ContinueNonActiveButtonStyled>
        </Tooltip>
    )
}

const ReschedulingViewContinueButton = ({ reservation, onSubmit }: { reservation: ReservationType, onSubmit?: () => void }) => {
    const selected = useAppSelector(selectNumberOfSelectedEvents);
    const total = useAppSelector(selectNumberOfLessons);
    const numberToSelect = total - selected;
    const { t } = useTranslation();
    const { handleClose } = useReservationModalContext();
    const reservationStartTimes = useAppSelector(selectReservationStartTimes);
    const rescheduleStatus = useAppSelector(selectEditReservationStatus);
    const { enqueueSnackbar } = useSnackbar();
    const dispatch = useAppDispatch();

    useUpdateEffect(() => {
        if (rescheduleStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.TIME_RESCHEDULING_SUCCESS), { variant: "success" });
        }
        else if (rescheduleStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.TIME_RESCHEDULING_ERROR), { variant: "error" }); //todo separate modal with error reservation message
        }
    }, [rescheduleStatus]);

    const isActive = () => {
        return selected >= total;
    }

    const handleReschedule = () => {
        const callback = async () => {
            await dispatch(editReservation({
                reservationId: reservation.reservationId!,
                newFromDate: moment(reservationStartTimes[0]).utc().format()
            }));
            dispatch(resetAvailabilityCalendarData());
            if (onSubmit) {
                onSubmit();
            }
            handleClose();
        }
        callback();
    }

    const handleClick = () => {
        if (selected < total) return;
        handleReschedule();
    }

    return isActive() ? (
        <ContinueActiveButtonStyled onClick={handleClick}>
            {rescheduleStatus !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.COMMON.SAVE) : <CircularProgressCustom color={COLORS.WHITE} />}
        </ContinueActiveButtonStyled>
    ) : (
        <Tooltip text={t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.SELECT_LESSONS_TO_CONTINUE_TOOLTIP, { numberToSelect })} >
            <ContinueNonActiveButtonStyled>{t(TRANSLATION_KEYS.COMMON.SAVE)}</ContinueNonActiveButtonStyled>
        </Tooltip>
    )
}

const TimeSchedulingView = () => {
    const { t } = useTranslation();
    const { tutorInfo } = useReservationModalContext();
    const { handleBack } = useReservationModalContext();
    return (
        <>
            <ContainerCustomStyled sx={{ flex: "1 1 auto", overflow: "auto" }}>
                <TimeSchedulingHeader />
                <LineSeparator />
                <CenterBoxStyled>
                    <LessonSchedulingDescriptionTypographyStyled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.TIME_SCHEDULING_DESCRIPTION)}</LessonSchedulingDescriptionTypographyStyled>
                    <AvailabilityBoxStyled>
                        <AvailabilityCalendar userId={tutorInfo.userId!} displayMode="fullscreen" />
                    </AvailabilityBoxStyled>
                </CenterBoxStyled>
            </ContainerCustomStyled>
            <ButtonsBoxWithProgress>
                <BackButtonStyled onClick={handleBack}>{t(TRANSLATION_KEYS.COMMON.BACK)}</BackButtonStyled>
                <SchedulingViewContinueButton />
            </ButtonsBoxWithProgress>
        </>
    )
}

type AuthorizationViewType = "authorization" | "email_verification" | "set_password";
const getViewType = (userStateId: UserStateEnum | undefined): AuthorizationViewType => {
    if (!userStateId) {
        return "authorization";
    }
    else if (userStateId === UserStateEnum.EmailVerification) {
        return "email_verification";
    }
    else if (userStateId === UserStateEnum.PasswordSetup) {
        return "set_password";
    }
    return "authorization";
}

const AuthorizationView = () => {
    const userStateId = useAppSelector(selectUserStateId);
    const { handleBack, handleContinue } = useReservationModalContext();
    const { screenType } = useContext(UIContext);
    const dispatch = useAppDispatch();
    const { t } = useTranslation();
    const [authType, setAuthType] = useState<PageType>("registration");
    const [viewType, setViewType] = useState<AuthorizationViewType>(getViewType(userStateId));
    const CURRENT_KEYS = TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS;

    useEffect(() => {
        setViewType(getViewType(userStateId));
    }, [userStateId]);

    const handleAlternative = () => {
        setAuthType(prev => prev === "registration" ? "login" : "registration");
    }

    const handleInternalRegistrationSuccess = () => {
        setViewType("email_verification");
        dispatch(setShortUserInfo());
    }

    const handleConfirmEmailSuccess = () => {
        setViewType("set_password");
        dispatch(setShortUserInfo());
    }

    const handleSetPasswordClick = (newPassword: string) => {
        dispatch(setChangePassword({ newPassword }));
        handleContinue();
    }

    return (
        <>
            <ContainerCustomStyled>
                <PackageMenuHeader title={t(authType === "login" ? CURRENT_KEYS.LOGIN_TITLE : CURRENT_KEYS.SIGNUP_TITLE)} />
                <LineSeparator />
                <CenterBoxStyled>
                    {screenType === "desktop" ? <LessonSchedulingDescriptionTypographyStyled>{t(CURRENT_KEYS.AUTHORIZATION_DESCIPTION)}</LessonSchedulingDescriptionTypographyStyled> : <></>}
                    <ExpandableBoxStyled>
                        <AuthInnerBoxStyled>
                            {viewType === "authorization" ? (
                                <Authentication
                                    pageType={authType}
                                    userRole={UserRoleEnum.Student}
                                    hideTitle={screenType === "mobile"}
                                    hideImage={true}
                                    onSuccessExternal={handleContinue}
                                    onAlternativeClick={handleAlternative}
                                    onSucessInternal={handleInternalRegistrationSuccess}
                                />
                            ) : <></>}
                            {viewType === "email_verification" ? <ConfirmEmailBox onSuccess={handleConfirmEmailSuccess} /> : <></>}
                            {viewType === "set_password" ? <SetPasswordBox type="set_new_password" onClick={handleSetPasswordClick} /> : <></>}
                        </AuthInnerBoxStyled>
                    </ExpandableBoxStyled>
                </CenterBoxStyled>
            </ContainerCustomStyled>
            <ButtonsBoxWithProgress>
                <BackButtonStyled onClick={handleBack}>{t(TRANSLATION_KEYS.COMMON.BACK)}</BackButtonStyled>
                <Tooltip text={t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.REGISTER_CO_CONTINUE_TOOLTIP)} >
                    <ContinueNonActiveButtonStyled>{t(TRANSLATION_KEYS.COMMON.CONTINUE)}</ContinueNonActiveButtonStyled>
                </Tooltip>
            </ButtonsBoxWithProgress>
        </>
    )
}

const TimeReschedulingView = ({ reservation, onSubmit }: { reservation: ReservationType, onSubmit?: () => void }) => {
    const { t } = useTranslation();
    const { tutorInfo } = useReservationModalContext();
    const { handleBack } = useReservationModalContext();

    return (
        <ContainerCustomStyled>
            <HeadingBoxStyled>
                <H3Styled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.TIME_RESCHEDULING_HEADER)}</H3Styled>
                <CloseIconBox />
            </HeadingBoxStyled>
            <LineSeparator />
            <CenterBoxStyled>
                <LessonSchedulingDescriptionTypographyStyled>
                    {t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.TIME_RESCHEDULING_DESCRIPTION, { opponent: getUserName(reservation.opponent), duration: getFormattedDate(reservation.startTime, reservation.endTime) })}
                </LessonSchedulingDescriptionTypographyStyled>
                <AvailabilityBoxStyled>
                    <AvailabilityCalendar userId={tutorInfo.userId!} displayMode="fullscreen" />
                </AvailabilityBoxStyled>
                <ButtonsBoxStyled>
                    <BackButtonStyled onClick={handleBack}>{t(TRANSLATION_KEYS.COMMON.BACK)}</BackButtonStyled>
                    <ReschedulingViewContinueButton reservation={reservation} onSubmit={onSubmit} />
                </ButtonsBoxStyled>
            </CenterBoxStyled>
        </ContainerCustomStyled>
    )
}

const useReservationOrderSummaryContext = ({ userWalletStatistics }: { userWalletStatistics: GetUserWalletStatisticsResponse }) => {
    const [useAppCredits, setUseAppCredits] = useState(false);
    const [usePromoCode, setUsePromoCode] = useState(false);
    const { currencyCode } = useAppDataContext();
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const router = useRouter();

    const { tutorInfo } = useReservationModalContext();
    const { enqueueSnackbar } = useSnackbar();

    const reservationStartTimes = useAppSelector(selectReservationStartTimes);
    const duration = useAppSelector(selectDuration);
    const createStatus = useAppSelector(selectCreateReservationsStatus);
    const numberOfLessons = useAppSelector(selectNumberOfSelectedEvents);
    const couponInfo = useAppSelector(selectCouponInfo);
    const resolvedCoupon = couponInfo?.readyToUse ? couponInfo : undefined;

    const price = useMemo(() => getPrice({ hourRate: tutorInfo.hourRate, numberOfLessons, duration }), [tutorInfo, numberOfLessons, duration]);
    const totalPrice = useMemo(() => getTotalPrice({ hourRate: tutorInfo.hourRate, numberOfLessons, duration, couponDiscountPercentage: resolvedCoupon?.discountPercentage }), [tutorInfo, numberOfLessons, duration, resolvedCoupon]);
    const promocodeDiscount = useMemo(() => getDiscountAmount({ hourRate: tutorInfo.hourRate, numberOfLessons, duration, couponDiscountPercentage: resolvedCoupon?.discountPercentage }), [tutorInfo, numberOfLessons, duration, resolvedCoupon]);
    const amountOfCreditsToUse = useMemo(() => getInternalCreditsAmountToUse(totalPrice, userWalletStatistics.amount, useAppCredits), [totalPrice, promocodeDiscount, userWalletStatistics, useAppCredits])
    const amountToPay = useMemo(() => getAmountToPay(totalPrice, amountOfCreditsToUse), [totalPrice, amountOfCreditsToUse]);

    const handleReservation = async () => {
        if (createStatus === LoadingStatusEnum.Loading) return;

        const result = await dispatch(createReservation({
            tutorId: tutorInfo.userId!,
            durationMinutes: duration,
            fromDateTimes: reservationStartTimes
                .map((x, idx) => ({ x, idx }))
                .filter(x => duration === ClassDurationEnum.Half || x.idx % 2 === 0)
                .map((x, idx) => moment(x.x).utc().format()),
            useWalletCredits: useAppCredits,
            couponCode: resolvedCoupon?.code,
            currencyCode: currencyCode ?? COMMON_CONSTANTS.INTERNAL_CURRENCY_CODE
        }))

        if (isDispatchRejected(result)) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.FAILED_TO_CREATE_RESERVATION), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            return;
        }

        const createReservationResponse = result.payload as ReservationsCreateResponse;
        if (createReservationResponse.payedFullyByCredits) {
            location.replace(routes.reservationPaymentSuccess.path);
            return;
        }
        if (createReservationResponse.redirectUrl) {
            router.push(createReservationResponse.redirectUrl);
        }
    }

    return {
        useAppCredits,
        usePromoCode,
        setUseAppCredits,
        setUsePromoCode,
        price,
        promocodeDiscount,
        promoCodeDiscountPercentage: resolvedCoupon?.discountPercentage,
        amountToPay,
        amountOfCreditsToUse,
        handleReservation
    }
}

const OrderSummaryPrimaryItem = ({ price }: { price: number }) => {
    const { tutorInfo } = useReservationModalContext();
    const { t } = useTranslation();
    const duration = useAppSelector(selectDuration);
    const numberOfLessons = useAppSelector(selectNumberOfSelectedEvents);
    const { localize } = usePriceLocalization();

    return (
        <ItemBoxStyled>
            <UserBoxStyled>
                <UserAvatarStyled src={getProfilePhotoThumbnailUrl(tutorInfo)} alt="Tutor's profile thumbnail" />
                <ItemTypographyStyled>
                    {t(TRANSLATION_KEYS.STRIPE.PACKAGE_PRODUCT_NAME, { count: numberOfLessons, duration: getDurationInMinutes(duration), tutorName: getUserName(tutorInfo) })}
                </ItemTypographyStyled>
            </UserBoxStyled>
            <Box>
                <H5StyledComponent>{localize({ price, round: false })}</H5StyledComponent>
            </Box>
        </ItemBoxStyled>
    )
}

const OrderSummaryYaspeakItem = ({ text, priceText }: { text: string, priceText: string }) => (
    <ItemBoxStyled>
        <UserBoxStyled>
            <YaspeakAvatar />
            <ItemTypographyStyled>{text}</ItemTypographyStyled>
        </UserBoxStyled>
        <H5StyledComponent>{priceText}</H5StyledComponent>
    </ItemBoxStyled>
)

const OrderSummaryHeader = () => {
    const { t } = useTranslation();

    return (
        <>
            <HeadingBoxStyled>
                <H4Styled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.ORDER_SUMMARY_HEADER)}</H4Styled>
                <CloseIconBox />
            </HeadingBoxStyled>
            <LineSeparator />
        </>
    )
}

const OrderSummaryCompanyStudentLoaded = ({ userWalletStatistics }: { userWalletStatistics: GetUserWalletStatisticsResponse }) => {
    const { price, handleReservation } = useReservationOrderSummaryContext({ userWalletStatistics })
    const { handleBack } = useReservationModalContext();
    const { t } = useTranslation();
    const createStatus = useAppSelector(selectCreateReservationsStatus);
    const hasEnoughMoney = (userWalletStatistics.amount ?? 0) >= price;

    return (
        <>
            <ContainerCustomStyled>
                <OrderSummaryHeader />
                <CenterBoxStyled sx={{ gap: "24px" }}>
                    <OrderSummaryPrimaryItem price={price} />
                </CenterBoxStyled>
            </ContainerCustomStyled >
            <ButtonsBoxWithProgress>
                <BackButtonStyled onClick={handleBack}>{t(TRANSLATION_KEYS.COMMON.BACK)}</BackButtonStyled>
                {hasEnoughMoney ? (
                    <PayButtonStyled onClick={handleReservation}>
                        {createStatus !== LoadingStatusEnum.Loading ? (t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.BUY_USING_CREDITS_BUTTON)) : <CircularProgressCustom color={COLORS.WHITE} />}
                    </PayButtonStyled>
                ) : (
                    <Tooltip text={t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.NOT_ENOUGH_CREDITS_TO_BUY)}>
                        <NonActivePayButtonStyled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.BUY_USING_CREDITS_BUTTON)}</NonActivePayButtonStyled>
                    </Tooltip>
                )}
            </ButtonsBoxWithProgress>
        </>
    )
}

const OrderSummaryPromoCode = () => {
    const [promoCode, setPromoCode] = useState<string | undefined>();
    const [error, setError] = useState<string | undefined>();
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const promoCodeInfo = useAppSelector(selectCouponInfoStatus);

    const handleRedeem = async () => {
        const result = await dispatch(setCouponInfo({ code: promoCode ?? "" }));
        if (isDispatchRejected(result)) {
            setError(t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.INVALID_PROMO_CODE));
        }

        if (isDispatchFulfilled(result)) {
            const info = result.payload as GetCouponInfoResponse;
            const expired = moment(info.expireTime).isBefore(moment());
            if (expired) {
                setError(t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.PROMO_CODE_EXPIRED));
            }
            else {
                setPromoCode(undefined);
                setError(undefined);
            }
        }
    }

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setPromoCode(e.target.value);
        if (error) {
            setError(undefined);
        }
    }

    return (
        <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "8px" }}>
            <Box sx={{ display: "flex", flexFlow: "row noWrap", gap: "8px", width: "100%" }}>
                <TextFieldStyled
                    type="text"
                    placeholder={t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.PROMO_CODE_PLACEHOLDER)}
                    value={promoCode ?? ""}
                    onChange={handleChange}
                />
                <PrimaryButtonCompacted onClick={handleRedeem}>
                    {promoCodeInfo !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.PROMO_CODE_REDEEM_BUTTON) : <CircularProgressCustom color={COLORS.WHITE} />}
                </PrimaryButtonCompacted>
            </Box>
            {error ? <PStyled sx={{ color: COLORS.RED }}>{error}</PStyled> : <></>}
        </Box>
    )
}

const OrderSummaryRegularViewLoaded = ({ userWalletStatistics }: { userWalletStatistics: GetUserWalletStatisticsResponse }) => {
    const { handleBack } = useReservationModalContext();
    const { t } = useTranslation();
    const createStatus = useAppSelector(selectCreateReservationsStatus);
    const {
        useAppCredits,
        usePromoCode,
        setUseAppCredits,
        setUsePromoCode,
        price,
        promocodeDiscount,
        promoCodeDiscountPercentage,
        amountToPay,
        amountOfCreditsToUse,
        handleReservation,
    } = useReservationOrderSummaryContext({ userWalletStatistics })
    const { localize } = usePriceLocalization();

    return (
        <>
            <ContainerCustomStyled>
                <OrderSummaryHeader />
                <CenterBoxStyled sx={{ gap: "24px" }}>
                    <OrderSummaryPrimaryItem price={price} />
                    {useAppCredits ? (
                        <OrderSummaryYaspeakItem text={t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.WALLET_CREDITS_OPTION)} priceText={localize({ price: -amountOfCreditsToUse, round: false })} />
                    ) : <></>}
                    {promocodeDiscount ? (
                        <OrderSummaryYaspeakItem
                            text={t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.PROMO_CODE_TITLE, { discountPercentage: promoCodeDiscountPercentage ?? 0 })}
                            priceText={localize({ price: -promocodeDiscount, round: false })}
                        />
                    ) : <></>}
                    {userWalletStatistics.amount ? (
                        <InternalCreditsBoxStyled onClick={() => setUseAppCredits(prev => !prev)}>
                            <MuiCheckboxStyled checked={useAppCredits} />
                            <ItemTypographyStyled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.USE_INTERNAL_CREDITS_DESCRIPTION)}</ItemTypographyStyled>
                        </InternalCreditsBoxStyled>
                    ) : <></>}
                    <InternalCreditsBoxStyled onClick={() => setUsePromoCode(prev => !prev)}>
                        <MuiCheckboxStyled checked={usePromoCode} />
                        <ItemTypographyStyled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.PROMO_CODE_CHECKBOX)}</ItemTypographyStyled>
                    </InternalCreditsBoxStyled>
                    {usePromoCode ? <OrderSummaryPromoCode /> : <></>}
                </CenterBoxStyled>
            </ContainerCustomStyled >
            <ButtonsBoxWithProgress>
                <BackButtonStyled onClick={handleBack}>{t(TRANSLATION_KEYS.COMMON.BACK)}</BackButtonStyled>
                <PayButtonStyled onClick={handleReservation}>
                    {createStatus !== LoadingStatusEnum.Loading ? (
                        t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.MODALS.PAY_BUTTON, { amount: localize({ price: amountToPay, round: false }) })
                    ) : <CircularProgressCustom color={COLORS.WHITE} />}
                </PayButtonStyled>
            </ButtonsBoxWithProgress>
        </>
    )
}

const OrderSummaryViewLoading = () => (
    <ContainerCustomStyled>
        <OrderSummaryHeader />
        <CenterBoxStyled sx={{ alignItems: "center", justifyContent: "center" }}>
            <CircularProgressCustom />
        </CenterBoxStyled>
    </ContainerCustomStyled>
)

const OrderSummaryViewError = () => (
    <ContainerCustomStyled>
        <OrderSummaryHeader />
        <CenterBoxStyled sx={{ alignItems: "center", justifyContent: "center" }}>
            <ErrorLarge />
        </CenterBoxStyled>
    </ContainerCustomStyled>
)

const OrderSummaryView = () => {
    const userWalletStatistics = useAppSelector(selectUserWalletStatistics);
    const userWalletStatisticsStatus = useAppSelector(selectUserWalletStatisticsStatus);
    const userInfo = useAppSelector(selectShortUserInfo);
    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(setUserWalletStatistics());
    }, []);

    return (
        <>
            {userWalletStatisticsStatus === LoadingStatusEnum.Loading ? <OrderSummaryViewLoading /> : <></>}
            {userWalletStatisticsStatus === LoadingStatusEnum.Error ? <OrderSummaryViewError /> : <></>}
            {userWalletStatisticsStatus === LoadingStatusEnum.Loaded && userWalletStatistics && !userInfo?.inviterId ? <OrderSummaryRegularViewLoaded userWalletStatistics={userWalletStatistics} /> : <></>}
            {userWalletStatisticsStatus === LoadingStatusEnum.Loaded && userWalletStatistics && userInfo?.inviterId ? <OrderSummaryCompanyStudentLoaded userWalletStatistics={userWalletStatistics} /> : <></>}
        </>
    )
}

const ReservationModalProvider = ({ children, tutorInfo, reservation, onSubmit }: { children: any, tutorInfo: TutorInfoType, reservation?: ReservationType | undefined, onSubmit?: () => void | undefined }) => {
    const [step, setStep] = useState<ReservationStepType>("init");
    const dispatch = useAppDispatch();
    const isAuthorized = useAppSelector(selectIsAuthorized);
    const userStateId = useAppSelector(selectUserStateId);
    const packageType = useAppSelector(selectPackageType);
    const steps = STEPS
        .filter(x => !(packageType === "trial" && x === "packageSaleSelection"))
        .filter(x => !(isAuthorized && x === "authorization"));

    useEffect(() => {
        if (!reservation) return;
        const durationInMinutes = Math.abs(moment(reservation.endTime).diff(moment(reservation.startTime), 'minutes'));
        const durationEnum = durationInMinutes > 30 ? ClassDurationEnum.Full : ClassDurationEnum.Half;

        dispatch(setNumberOfLessons(1));
        dispatch(setDuration(durationEnum));
    }, [reservation]);

    const handleOpen = () => {
        if (step === "timeScheduling") {
            return; // do nothing
        }
        setStep("packageTypeSelection");
    }

    const handleClose = () => {
        setStep("init");
        dispatch(resetAvailabilityCalendarData());
    }

    const isOpen = () => {
        return step !== "init";
    }

    const handleRescheduleClick = () => {
        setStep("timeRescheduling");
    }

    const handleContinue = () => {
        switch (step) {
            case "init":
                setStep("packageTypeSelection");
                break;
            case "packageTypeSelection":
                if (packageType === "trial") {
                    setStep("timeScheduling");
                    dispatch(setNumberOfLessons(1));
                    dispatch(setDuration(ClassDurationEnum.Half));
                }
                else if (packageType === "kids") {
                    setStep("packageSaleSelection");
                    dispatch(setNumberOfLessons(5));
                    dispatch(setDuration(ClassDurationEnum.Half));
                }
                else {
                    setStep("packageSaleSelection");
                    dispatch(setNumberOfLessons(5));
                    dispatch(setDuration(ClassDurationEnum.Full));
                }
                break;
            case "packageSaleSelection":
                setStep("timeScheduling");
                break;
            case "timeScheduling":
                const authStates = [UserStateEnum.EmailVerification, UserStateEnum.PasswordSetup];
                setStep(isAuthorized && userStateId && !authStates.includes(userStateId) ? "orderSummary" : "authorization");
                break;
            case "authorization":
                setStep("orderSummary");
                break;
            default:
                throw new Error(`unhandeled step ${step}`);
        }
    }

    const handleBack = () => {
        switch (step) {
            case "init":
                setStep("init");
                break;
            case "packageTypeSelection":
                setStep("init");
                dispatch(resetAvailabilityCalendarData());
                break;
            case "packageSaleSelection":
                setStep("packageTypeSelection");
                break;
            case "timeScheduling":
                setStep(packageType === "trial" ? "packageTypeSelection" : "packageSaleSelection");
                break;
            case "timeRescheduling":
                setStep("init");
                dispatch(resetAvailabilityCalendarData());
                break;
            case "authorization":
                setStep("timeScheduling");
                break;
            case "orderSummary":
                setStep("timeScheduling");
                break;
            default:
                throw new Error(`unhandeled step ${step}`);
        }
    }

    return (
        <ReservationDetailsModalContext.Provider value={{
            handleOpen,
            handleClose,
            handleContinue,
            handleBack,
            handleRescheduleClick,
            tutorInfo,
            progress: Math.round((steps.findIndex(x => x === step) ?? 0) * 100 / (steps.length - 1)),
        }}>
            {children}
            <ModalStyled open={isOpen()} onClose={handleClose}>
                <FullscreenModalBoxStyled padding="0">
                    <Box sx={{ display: "flex", flex: "0 1 auto" }}>
                        <Navbar source={NavbarSourceEnum.tutorPage} />
                    </Box>
                    <FullscreenModalInnerBoxStyled>
                        {step === "packageTypeSelection" ? <PackageTypeSelection /> : <></>}
                        {step === "packageSaleSelection" ? <PackageSaleSelection /> : <></>}
                        {step === "timeScheduling" ? <TimeSchedulingView /> : <></>}
                        {step === "authorization" ? <AuthorizationView /> : <></>}
                        {step === "timeRescheduling" && reservation ? <TimeReschedulingView reservation={reservation} onSubmit={onSubmit} /> : <></>}
                        {step === "orderSummary" ? <OrderSummaryView /> : <></>}
                    </FullscreenModalInnerBoxStyled>
                </FullscreenModalBoxStyled>
            </ModalStyled>
        </ReservationDetailsModalContext.Provider >
    )
}

export default ReservationModalProvider;