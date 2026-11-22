import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { useCallback, useEffect, useRef, useState } from "react";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { getProfilePhotoThumbnailUrl } from "core/services/Utils/imageUtil";
import React from "react";
import { useTranslation } from "components/core/TranslationProvider";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import ArrowRight from "assets/arrow-right.svg";
import H5Styled from "components/styles/typography/H5Styled";
import PStyled from "../styles/typography/PStyled";
import Body1Styled from "../styles/typography/Body1Styled";
import { TertiaryButton } from "../styles/TertiaryButton";
import CaptionTextStyled from "../styles/typography/CaptionTextStyled";
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import { NotificationFilterResponse, selectNotificationsFilter, selectNotificationsFilterStatus, selectSetReadAllNotificationsStatus, setNotificationFilter, setAllNotificationsRead, updateLocalNotificationsSetReadAll, selectSetReadNotificationStatus, setNotificationRead, updateLocalNotificationsSetRead } from "core/redux/notificationSlice";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";
import { getUserName } from "core/services/Utils/userUtils";
import moment from "moment";
import Link from "next/link";
import routes from "routes/routes";
import StarsRating from "components/profile/reviews/StarsRating";
import InfiniteScroll from 'react-infinite-scroll-component';
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { isNewReservationType, isNewPackageReservationType, isReservationCancellationType, isPackageCancellationType, isReservationRescheduleType, isRefundRequestedType, isNewReviewType, isWithdrawalApprovedType, isRefundApprovedType, isRefundDeclinedType, isReservationReminderType, NewReviewNotificationType, NewReservationNotificationType, NewPkgReservationNotificationType, ReservationRescheduleNotificationType, RefundRequestedNotificationType, WithdrawalApprovedNotificationType, isTutorProfileVisitedType, TutorProfileVisitedNotificationType } from "core/services/Utils/notificationsUtil";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { useSnackbar } from "notistack";
import { snackbarConstants } from "components/core/Snackbar";
import { updateShortUserInfoNotificationsOneMoreRead, updateShortUserInfoNotificationsRead } from "core/redux/userSlice";
import Tooltip from "components/styles/Tooltip";
import EmptyNotificationsIcon from "assets/empty_notifications.svg";
import H4Styled from "components/styles/typography/H4Styled";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { selectUserRoleId } from "core/redux/authSlice";
import { usePriceLocalization } from "core/hooks/priceHook";

const InnerMenuStyled = styled(Box)(() => sx({
    width: { xs: "100%", md: "540px" },
    height: { xs: "100%", md: "calc(100vh - 100px)" },
    minWidth: { xs: "0", md: "440px" },
    display: "flex",
    flexFlow: "column noWrap",
    padding: { xs: "0px 16px", md: "0px" },
    overflowY: "auto",
    position: "relative"
})) as typeof Box;

const AvatarStyledBox = styled(Avatar)(() => sx({
    width: 40,
    height: 40,
})) as typeof Avatar;

const NOTIFICATIONS_TITLE_HIGHT = 54.5;

const TitleStyledBox = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    flexDirection: "row",
    padding: "16px",
    width: "100%",
    borderBottom: `1px solid ${COLORS.BLACK_10}`,
    flex: "0 1 auto",
    height: `${NOTIFICATIONS_TITLE_HIGHT}px`
})) as typeof Box;

const NotificationItemBoxStyled = styled(Box)(() => sx({
    padding: { xs: "16px 0px", md: "16px" },
    borderBottom: `1px solid ${COLORS.BLACK_10}`,
    display: "flex",
    flexDirection: "row",
    justifyContent: "start",
    gap: "8px"
})) as typeof Box;

const RightContentBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "4px",
    flexDirection: "column",
    width: "100%"
})) as typeof Box;

const DotBoxStyled = styled(Box)(() => sx({
    width: "8px",
    height: "8px",
    borderRadius: "50%",
    backgroundColor: COLORS.PRIMARY
})) as typeof Box;

const DotOuterBoxStyled = styled(Box)(() => sx({
    padding: "4px",
    borderRadius: "50%",
    ":hover": {
        cursor: "pointer",
        padding: "3px",
        border: `1px solid ${COLORS.PRIMARY}`
    }
})) as typeof Box;

const ReadButton = styled(TertiaryButton)(() => sx({
    padding: "0px",
    display: "flex",
    justifyContent: "start",
    ":hover": { backgroundColor: COLORS.WHITE, textDecoration: "underline" }
})) as typeof TertiaryButton;

const ActionItemButton = styled(TertiaryButton)(() => sx({
    padding: "0px",
    display: "flex",
    justifyContent: "start",
    textDecoration: "none",
    width: "fit-content",
    ":hover": {
        backgroundColor: COLORS.WHITE,
        textDecoration: "underline"
    },
    "a": {
        textDecoration: "none"
    }
})) as typeof TertiaryButton;

const ItemTitleBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "space-between",
})) as typeof Box;

const ItemTitleTypographyStyled = styled(PStyled)(() => sx({
    color: COLORS.BLACK,
    b: {
        fontWeight: "600"
    }
})) as typeof PStyled;

const ItemDescriptionTitleTypographyStyled = styled(Body1Styled)(() => sx({
    color: COLORS.BLACK,
    fontWeight: "500",
    "b": {
        fontWeight: "600"
    }
})) as typeof PStyled;

const ItemDescriptionTypographyStyled = styled(Body1Styled)(() => sx({
    color: COLORS.BLACK,
    "b": {
        fontWeight: "600"
    }
})) as typeof PStyled;

const EmptyNotificationsHeader = styled(Box)({
    display: "flex",
    alignItems: "center",
    gap: "8px",
    justifyContent: "center",
    flexFlow: "column noWrap",
    width: "100%",
}) as typeof Box;

const EmptyNotificaitonsWrapperStyled = styled(Box)(() => sx({
    display: "flex",
    height: "100%",
    gap: "24px",
    width: "100%",
    flexFlow: "column noWrap",
    justifyContent: "center",
    padding: "0 16px",
    alignItems: "center",
})) as typeof Box;

const NotificationsInnerBoxTitle = () => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const setNotificationsReadAllStatus = useAppSelector(selectSetReadAllNotificationsStatus);
    const setNotificationsReadStatus = useAppSelector(selectSetReadNotificationStatus);
    const { enqueueSnackbar } = useSnackbar();

    useEffect(() => {
        if (setNotificationsReadAllStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.FAILED_TO_LOAD_NOTIFICATIONS), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
    }, [setNotificationsReadAllStatus]);

    useEffect(() => {
        if (setNotificationsReadStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.FAILED_TO_LOAD_NOTIFICATIONS), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
    }, [setNotificationsReadStatus]);

    const handleReadClick = () => {
        dispatch(updateLocalNotificationsSetReadAll());
        dispatch(updateShortUserInfoNotificationsRead());
        dispatch(setAllNotificationsRead());
    }

    return (
        <TitleStyledBox>
            <H5Styled>{t(TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS_TITLE)}</H5Styled>
            <ReadButton onClick={handleReadClick}>{t(TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.MARK_ALL_AS_READ_BUTTON)}</ReadButton>
        </TitleStyledBox>
    )
}

const ActionButton = ({ titleKey, href }: { titleKey: string, href: string }) => {
    const { t } = useTranslation();

    return (
        <ActionItemButton>
            <Link href={href}>{t(titleKey)}</Link>
        </ActionItemButton>
    )
}

const ActionButtonRole = ({ tabName, titleKey }: { tabName: string, titleKey: string }) => {
    const userRoleId = useAppSelector(selectUserRoleId);
    const dashboardPath = userRoleId === UserRoleEnum.Student ? routes.studentDashboard.path : routes.tutorDashboard.path;

    return (
        <ActionButton href={`${dashboardPath}?tab=${tabName}`} titleKey={titleKey} />
    )
}

const NotificationReview = ({ data }: { data: NewReviewNotificationType }) => {

    return (
        <>
            <StarsRating rating={data?.rate ?? 0} />
            <ItemDescriptionTypographyStyled>{data?.comment ?? ""}</ItemDescriptionTypographyStyled>
        </>
    )
}

const NotificationReservation = ({ data }: { data: NewReservationNotificationType }) => {
    return (
        <>
            <ItemDescriptionTypographyStyled>{moment(data.startTime).format("DD.MM.yyyy HH:mm")} - {moment(data.expectedEndTime).format("HH:mm")}</ItemDescriptionTypographyStyled>
            <ActionButtonRole tabName="overview" titleKey={TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.VIEW_CALENDAR_BUTTON} />
        </>
    )
}

const NotificationPackageReservation = ({ data }: { data: NewPkgReservationNotificationType }) => {
    const { t } = useTranslation();
    return (
        <>
            <ItemDescriptionTypographyStyled dangerouslySetInnerHTML={{ __html: t(TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.NEW_PACKAGE_RESERVATION_DESCRIPION, { count: data.numberOfLessons }) }} />
            <ActionButtonRole tabName="overview" titleKey={TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.VIEW_CALENDAR_BUTTON} />
        </>
    )
}

const NotificationReservationReschedule = ({ data }: { data: ReservationRescheduleNotificationType }) => {
    return (
        <>
            <Box sx={{ display: "inline-flex", gap: "2px", alignContent: "center" }}>
                <ItemDescriptionTypographyStyled>{moment(data.startTime).format("DD.MM.yyyy HH:mm")} - {moment(data.expectedEndTime).format("HH:mm")}</ItemDescriptionTypographyStyled>
                <Box sx={{ svg: { transform: "scale(0.875)" } }}><ArrowRight /></Box>
                <ItemDescriptionTypographyStyled>{moment(data.sourceStartTime).format("DD.MM.yyyy HH:mm")} - {moment(data.sourceExpectedEndTime).format("HH:mm")}</ItemDescriptionTypographyStyled>
            </Box>
            <ActionButtonRole tabName="overview" titleKey={TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.VIEW_CALENDAR_BUTTON} />
        </>
    )
}

const NotificationRefundRequested = ({ data }: { data: RefundRequestedNotificationType }) => {
    const { t } = useTranslation();
    return (
        <>
            <ItemDescriptionTypographyStyled>{moment(data.reservationStartTime).format("DD.MM.yyyy HH:mm")} - {moment(data.reservationExpectedEndTime).format("HH:mm")}</ItemDescriptionTypographyStyled>
            <ItemDescriptionTitleTypographyStyled>{t(data.issueTypeNameKey ?? "")}</ItemDescriptionTitleTypographyStyled>
            <ItemDescriptionTypographyStyled>{data.issueDescription ?? ""}</ItemDescriptionTypographyStyled>
            <ActionButtonRole tabName="overview" titleKey={TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.VIEW_CALENDAR_BUTTON} />
        </>
    )
}

const NotificationWithdrawalApproved = ({ data }: { data: WithdrawalApprovedNotificationType }) => {

    return (
        <>
            <ItemDescriptionTypographyStyled />
            <ActionButtonRole tabName="transactions" titleKey={TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.VIEW_TRANSACTIONS_BUTTON} />
        </>
    )
}

const NotificationRefundCompleted = ({ data }: { data: RefundRequestedNotificationType }) => {
    const { t } = useTranslation();
    const { localize } = usePriceLocalization();

    return (
        <>
            <ItemDescriptionTypographyStyled>{moment(data.reservationStartTime).format("DD.MM.yyyy HH:mm")} - {moment(data.reservationExpectedEndTime).format("HH:mm")}</ItemDescriptionTypographyStyled>
            <ItemDescriptionTitleTypographyStyled>{t(data.issueTypeNameKey ?? "")}</ItemDescriptionTitleTypographyStyled>
            <ItemDescriptionTypographyStyled>{data.issueDescription ?? ""}</ItemDescriptionTypographyStyled>

            {data.decisionText && data.decisionText.length ? (
                <>
                    <ItemDescriptionTitleTypographyStyled>{t(TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.ADMIN_COMMENT_TITLE)}</ItemDescriptionTitleTypographyStyled>
                    <ItemDescriptionTypographyStyled>{data.decisionText ?? ""}</ItemDescriptionTypographyStyled>
                </>
            ) : <></>}

            {data.refundAmount && data.refundAmount > 0 ? (
                <ItemDescriptionTypographyStyled dangerouslySetInnerHTML={{ __html: t(TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.REFUND_AMOUNT_DESCRIPTION, { amount: localize({ price: data.refundAmount }) }) }} />
            ) : <></>}
            <ActionButtonRole tabName="transactions" titleKey={TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.VIEW_TRANSACTIONS_BUTTON} />
        </>
    )
}

const NotificationTutorProfileVisited = ({ data }: { data: TutorProfileVisitedNotificationType }) => {
    const { t } = useTranslation();

    return (
        <>
            <ItemDescriptionTitleTypographyStyled dangerouslySetInnerHTML={{ __html: t(TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.TUTOR_PROFILE_VISITED_DESCRIPTION, { username: data.userName ?? "" }) }} />
            <ActionButton titleKey={TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.WRITE_MESSAGE_BUTTON} href={data.url ?? routes.messagesPage.path} />
        </>
    )
}

const NotificationItemTitle = ({ notification }: { notification: NotificationFilterResponse['data'][0] }) => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();

    const handleClick = () => {
        dispatch(setNotificationRead(notification.notificationId ?? 0));
        dispatch(updateLocalNotificationsSetRead(notification.notificationId ?? 0));
        dispatch(updateShortUserInfoNotificationsOneMoreRead());
    }

    return (
        <Box>
            <ItemTitleBoxStyled >
                <ItemTitleTypographyStyled dangerouslySetInnerHTML={{ __html: t(notification.notificationType?.titleKey ?? "", { username: getUserName(notification.sender) }) }} />
                {!notification.readDateTime ? (
                    <Tooltip text={t(TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.MARK_THIS_NOTIFICATION_AS_READ)}>
                        <DotOuterBoxStyled onClick={handleClick}>
                            <DotBoxStyled />
                        </DotOuterBoxStyled>
                    </Tooltip>
                ) : <></>}
            </ItemTitleBoxStyled>
            <CaptionTextStyled>{moment(notification.created).fromNow()}</CaptionTextStyled>
        </Box>
    )
}

const NotificationItem = ({ notification }: { notification: NotificationFilterResponse['data'][0] }) => {

    return (
        <NotificationItemBoxStyled>
            <AvatarStyledBox src={getProfilePhotoThumbnailUrl(notification.sender)} />
            <RightContentBoxStyled>
                <NotificationItemTitle notification={notification} />
                {isNewReservationType(notification.data) ? <NotificationReservation data={notification.data} /> : <></>}
                {isNewPackageReservationType(notification.data) ? <NotificationPackageReservation data={notification.data} /> : <></>}
                {isReservationCancellationType(notification.data) ? <NotificationReservation data={notification.data} /> : <></>}
                {isPackageCancellationType(notification.data) ? <NotificationPackageReservation data={notification.data} /> : <></>}
                {isReservationRescheduleType(notification.data) ? <NotificationReservationReschedule data={notification.data} /> : <></>}
                {isRefundRequestedType(notification.data) ? <NotificationRefundRequested data={notification.data} /> : <></>}
                {isNewReviewType(notification.data) ? <NotificationReview data={notification.data} /> : <></>}
                {isWithdrawalApprovedType(notification.data) ? <NotificationWithdrawalApproved data={notification.data} /> : <></>}
                {isRefundApprovedType(notification.data) ? <NotificationRefundCompleted data={notification.data} /> : <></>}
                {isRefundDeclinedType(notification.data) ? <NotificationRefundCompleted data={notification.data} /> : <></>}
                {isReservationReminderType(notification.data) ? <NotificationReservation data={notification.data} /> : <></>}
                {isTutorProfileVisitedType(notification.data) ? <NotificationTutorProfileVisited data={notification.data} /> : <></>}
            </RightContentBoxStyled>
        </NotificationItemBoxStyled>
    )
}

const NotificationsLoader = () => {
    return (
        <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center", padding: "12px 0", height: "100%" }}>
            <CircularProgressCustom />
        </Box>
    )
}

const NotificationsEmpty = () => {
    const { t } = useTranslation();

    return (
        <EmptyNotificaitonsWrapperStyled>
            <EmptyNotificationsHeader>
                <H4Styled sx={{ textAlign: "center" }}>{t(TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.EMPTY_TITLE)}</H4Styled>
                <PStyled sx={{ textAlign: "center" }}>{t(TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.EMPTY_DESCRIPTION)}</PStyled>
            </EmptyNotificationsHeader>
            <EmptyNotificationsIcon />
        </EmptyNotificaitonsWrapperStyled>
    )
}

const Notifications = () => {
    const { t } = useTranslation();
    const [height, setHeight] = useState(0);
    const dispatch = useAppDispatch();
    const skipRef = useRef(0);
    const notifications = useAppSelector(selectNotificationsFilter);
    const notificationsStatus = useAppSelector(selectNotificationsFilterStatus);
    const { enqueueSnackbar } = useSnackbar();

    useEffect(() => {
        if (notificationsStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.NAVBAR.NOTIFICATIONS.FAILED_TO_LOAD_NOTIFICATIONS), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
    }, [notificationsStatus]);

    const heightRef = useCallback((node: HTMLElement | null) => {
        if (node !== null) {
            setHeight(node.clientHeight);
        }
    }, []);

    const fetchNext = (appendType: AppendTypeEnum, skip: number) => {
        dispatch(setNotificationFilter({
            appendType: appendType,
            request: {
                skip,
                take: COMMON_CONSTANTS.DEFAULT_NUMBER_OF_ITEMS_TO_SHOW
            }
        }));
    }

    const handleFetchNext = () => {
        skipRef.current += COMMON_CONSTANTS.DEFAULT_NUMBER_OF_ITEMS_TO_SHOW;
        fetchNext(AppendTypeEnum.End, skipRef.current);
    }

    return (
        <InnerMenuStyled>
            <NotificationsInnerBoxTitle />
            <Box ref={heightRef} sx={{ display: "flex", height: "100%", flexFlow: "column noWrap", flex: "1 1 auto", overflow: { xs: "none", md: "auto" } }}>
                <InfiniteScroll
                    dataLength={notifications?.data?.length ?? 0}
                    next={handleFetchNext}
                    hasMore={!notifications || notifications.data.length < notifications.size!}
                    loader={<NotificationsLoader />}
                    inverse={false}
                    scrollableTarget="scrollableDiv"
                    height={`${height}px`}
                >
                    {notifications && !notifications.size ? <NotificationsEmpty /> : <></>}
                    {notifications?.data.map(x => <NotificationItem key={x.notificationId} notification={x} />)}
                </InfiniteScroll>
            </Box>
        </InnerMenuStyled >
    )
}

export default Notifications;