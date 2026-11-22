import { styled, experimental_sx as sx } from "@mui/material/styles";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";
import { ReservationTimeEnum } from "core/constants/enums/ReservationTimeEnum";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import {
    setUpcomingReservations,
    setPastReservations,
    selectReservations,
    selectReservationsStatus,
    ReservationFilterResponse
} from "core/redux/reservationSlice";
import React, { useRef } from "react";
import { useEffect, useState } from "react";
import { useTranslation } from "components/core/TranslationProvider";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import ErrorLarge from "components/core/error/ErrorLarge";
import { getProfilePhotoThumbnailUrl } from 'core/services/Utils/imageUtil';
import { TRANSLATION_KEYS } from 'core/constants/translationConstants';
import routes from 'routes/routes';
import { selectShortUserInfo } from "core/redux/userSlice";
import Link from 'next/link';
import { UserRoleEnum } from 'core/constants/enums/UserRoleEnum';
import H5Styled from "components/styles/typography/H5Styled";
import DotsIcon from "assets/3_dots.svg";
import { IconButton } from 'components/styles/IconButton';
import { MenuStyled } from "components/styles/MenuStyled";
import StarIcon from "assets/star.svg";
import ReportIssueIcon from "assets/report_issue.svg";
import Body1Styled from 'components/styles/typography/Body1Styled';
import CancelLessonIcon from "assets/cancel_lesson.svg";
import CalendarIcon from "assets/calendar.svg";
import JoinLessonIcon from "assets/join_lesson.svg";
import ChatIcon from "assets/chat.svg";
import { getUserName } from 'core/services/Utils/userUtils';
import ContactUserModalProvider, { useContactUserModalContext } from 'components/modals/ContactUserModalProvider';
import { getFormattedDate } from "core/services/Utils/reservationUtil";
import MenuLinkItem from 'components/menu/MenuLinkItem';
import MenuItem from "components/menu/MenuItem";
import { MenuBoxStyled } from "components/styles/boxes/MenuBoxStyled";
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import Divider from '@mui/material/Divider';
import InfiniteScroll from 'react-infinite-scroll-component';
import CircularProgressStyled from 'components/styles/CircularProgressStyled';
import StarsRating from './reviews/StarsRating';
import ReservationReviewModalProvider, { useReservationReviewContext } from 'components/modals/ReservationReviewModalProvider';
import ReservationModalProvider, { useReservationModalContext } from 'components/modals/ReservationModalProvider';
import ReportIssueModalProvider, { userReportIssueModalConttext } from 'components/modals/ReportIssueModalProvider';
import { setReservationIssueTypes } from 'core/redux/codetableSlice';
import EmptyCalendarIcon from "assets/empty_calendar.svg";
import CancelPackageModalProvider, { useCancelPackageModalContext } from "components/modals/CancelPackageModalProvider";
import ThreeDotsMenu from "components/menu/ThreeDotsMenu";

export type ReservationTranslationKeys = {
    noEvents: string,
    noEventsDescription: string,
    showMoreBtnText: string
}

const ReservationsBoxStyled = styled(Box)(() => sx({
    display: "flex",
    width: "100%",
    border: `1px solid ${COLORS.BLACK_10}`,
    borderRadius: "8px",
    height: "100%",
    padding: "16px 8px 16px 16px",
    ".infinite-scroll-component__outerdiv": {
        width: "100%"
    }
})) as typeof Box

const ReservationItemBoxStyled = styled(Box)(() => sx({
    background: COLORS.WHITE,
    borderRadius: "8px",
    alignItems: "flex-start",
    display: "flex",
    gap: "16px",
    width: "100%"
})) as typeof Box

const ReservationItemInnerTextBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "4px",
    flexDirection: "column",
    width: "100%",
    alignItems: "left",
})) as typeof Box

const EmptyReservationsBoxStyled = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "center",
    textAlign: "center",
    alignItems: "center",
    flexDirection: "column",
    width: "100%",
    height: "100%",
    padding: "40px 0px",
    gap: "24px",
    ".inline": {
        width: "100%"
    }
})) as typeof Box

const AvatarBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    gap: "12px"
})) as typeof Box

const AvatarStyled = styled(Avatar)(() => sx({
    width: "48px",
    height: "48px"
})) as typeof Avatar;

const PrimaryButtonBoxStyled = styled(Box)(() => sx({
    height: "100%",
    display: "flex",
    alignItems: "center",
    justifyContent: "flex-end",
    gap: "4px"
})) as typeof Box

const IconDotsStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    svg: {
        color: COLORS.PRIMARY,
        transform: "scale(1.5)",
    }
})) as typeof Box;

const MainBoxStyled = styled(Box)(() => sx({
    userSelect: "false"
})) as typeof Box;

const UserOuterBoxStyled = styled(Box)(() => sx({
    userSelect: "false",
    display: 'flex',
    alignItems: 'center',
    textAlign: 'center'
})) as typeof Box;

const UserBoxStyled = styled(Box)(() => sx({
    ":hover": {
        cursor: "pointer"
    }
})) as typeof Box;

const IconBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        color: COLORS.PRIMARY,
    }
})) as typeof Box;

const WarningIconStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    svg: {
        color: COLORS.RED
    }
})) as typeof Box;

const ReservationContainerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    width: "100%",
    paddingRight: "8px"
})) as typeof Box;

const RESERVATION_BOX_HEIGHT = "300px";
const NUMBER_OF_RESERVATIONS_TO_SHOW = 20;

const UpcomingLessonsStudentActions = ({ reservation, onSubmit }: { reservation: ReservationFilterResponse["data"][0], onSubmit: () => void }) => {
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const { setOpen: setContactUserModalOpen } = useContactUserModalContext();
    const { setOpen: setCancelReservationModalOpen } = useCancelPackageModalContext();
    const { handleRescheduleClick } = useReservationModalContext();
    const open = Boolean(anchorEl);
    const { t } = useTranslation();

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    return (
        <React.Fragment>
            <MainBoxStyled>
                <UserOuterBoxStyled>
                    <UserBoxStyled
                        onClick={handleClick}
                        aria-controls={open ? 'past-lessons' : undefined}
                        aria-haspopup="true"
                        aria-expanded={open ? 'true' : undefined}
                    >
                        <IconButton><IconDotsStyled><DotsIcon /></IconDotsStyled></IconButton>
                    </UserBoxStyled>
                </UserOuterBoxStyled>
                <MenuStyled
                    anchorEl={anchorEl}
                    open={open}
                    onClose={handleClose}
                    onClick={handleClose}
                    transformOrigin={{ horizontal: 'right', vertical: 'top' }}
                    anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
                >
                    <MenuBoxStyled gap="4px">
                        {reservation?.room ? <MenuLinkItem icon={<JoinLessonIcon />} text={t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.JOIN_LESSON)} href={routes.meetingPage.path(reservation.room.roomKey ?? "")} /> : <></>}
                        <MenuItem onClick={setContactUserModalOpen} icon={<ChatIcon />} text={t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.WRITE_A_MESSAGE)} />
                        {reservation?.canCancel ? (
                            <>
                                <Divider sx={{ color: COLORS.BLACK_5 }} />
                                <MenuItem
                                    onClick={setCancelReservationModalOpen}
                                    icon={<WarningIconStyled><CancelLessonIcon /></WarningIconStyled>}
                                    text={reservation.totalInPackage === 1 ? (t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.CANCEL_LESSON)) : (t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.CANCEL_PACKAGE))}
                                />
                            </>
                        ) : <></>}
                        {reservation?.canReschedule ? (
                            <>
                                <Divider sx={{ color: COLORS.BLACK_5 }} />
                                <MenuItem
                                    onClick={handleRescheduleClick}
                                    icon={<CalendarIcon />}
                                    text={t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.RESCHEDULE_LESSON)}
                                />
                            </>
                        ) : <></>}
                    </MenuBoxStyled>
                </MenuStyled>
            </MainBoxStyled>
        </React.Fragment>
    )
}

const UpcomingLessonsStudentActionWithProviders = ({ reservation, onSubmit }: { reservation: ReservationFilterResponse["data"][0], onSubmit: () => void }) => {

    return (
        <ContactUserModalProvider userInfo={reservation!.opponent!}>
            <CancelPackageModalProvider reservation={reservation} onSubmit={onSubmit}>
                <UpcomingLessonsStudentActions reservation={reservation} onSubmit={onSubmit} />
            </CancelPackageModalProvider>
        </ContactUserModalProvider>
    )
}

const PastLessonsStudentActions = ({ reservation }: { reservation: ReservationFilterResponse["data"][0] }) => {
    const { t } = useTranslation();
    const { setOpen: setReservationReviewModalOpen } = useReservationReviewContext();
    const { setOpen: setReportIssueModalOpen } = userReportIssueModalConttext();


    return (
        <React.Fragment>
            <ThreeDotsMenu
                innerMenu={
                    <MenuBoxStyled gap="4px">
                        <MenuItem
                            onClick={setReservationReviewModalOpen}
                            icon={<StarIcon />}
                            text={t(reservation.reservationReview ? TRANSLATION_KEYS.PROFILE.RESERVATIONS.EDIT_REVIEW_BUTTON : TRANSLATION_KEYS.PROFILE.RESERVATIONS.WRITE_A_REVIEW)}
                        />
                        {reservation.canReportIssue ? (
                            <>
                                <Divider sx={{ color: COLORS.BLACK_5 }} />
                                <MenuItem
                                    onClick={setReportIssueModalOpen}
                                    icon={<ReportIssueIcon />}
                                    text={t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.REPORT_A_ISSUE)}
                                />
                            </>
                        ) : <></>}
                    </MenuBoxStyled>
                }
                autoClose={true}
            />
        </React.Fragment>
    )
}

const UserNameWithLink = ({ reservation }: { reservation: ReservationFilterResponse["data"][0] }) => {
    const { t } = useTranslation();
    return (
        <>
            <Link style={{ textDecoration: "none", color: COLORS.BLACK }} href={routes.tutorIdPage.path(reservation.opponent?.userId ?? 0)}>
                <H5Styled sx={{ ":hover": { textDecoration: "underline" } }}>
                    {reservation?.totalInPackage && reservation.totalInPackage <= 1
                        ? getUserName(reservation.opponent)
                        : t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.USER_NAME_WITH_PACKAGE, { opponent: getUserName(reservation.opponent), orderNumber: reservation.orderNumber ?? 0, totalNumber: reservation.totalInPackage ?? 0 })
                    }</H5Styled>
            </Link>
        </>
    )
}

const ReservationReview = ({ reservation }: { reservation: ReservationFilterResponse["data"][0] }) => {
    const { setOpen: setReservationReviewModalOpen, selectedRating } = useReservationReviewContext();

    const handleClick = (newRating: number) => {
        setReservationReviewModalOpen(newRating);
    }

    return (
        <StarsRating rating={selectedRating ?? 0} onClick={!reservation.reservationReview ? handleClick : undefined} />
    )
}

const ReservationItem = ({ reservation, reservationTime, onSubmit }: { reservation: ReservationFilterResponse["data"][0], reservationTime: ReservationTimeEnum, onSubmit: () => void }) => {
    const { t } = useTranslation();
    const userInfo = useAppSelector(selectShortUserInfo);

    return (
        <ReservationItemBoxStyled>
            <ReservationReviewModalProvider reservation={reservation} onSubmit={onSubmit}>
                <ReservationModalProvider tutorInfo={{
                    userId: reservation.hostId,
                    firstName: undefined,
                    lastName: undefined,
                    currencyCode: undefined,
                    hourRate: undefined,
                    files: undefined,
                    teachesLessonsForChildren: false
                }} reservation={reservation} onSubmit={onSubmit}>
                    <ReportIssueModalProvider reservationId={reservation.reservationId!}>
                        <AvatarBoxStyled>
                            <AvatarStyled src={getProfilePhotoThumbnailUrl(reservation.opponent)} />
                        </AvatarBoxStyled>
                        <ReservationItemInnerTextBoxStyled>
                            {userInfo?.userRoleId === UserRoleEnum.Student ? <UserNameWithLink reservation={reservation} /> : <H5Styled>{getUserName(reservation.opponent)}</H5Styled>}
                            <Body1Styled>{getFormattedDate(reservation.startTime, reservation.expectedEndTime)}</Body1Styled>
                            {reservationTime === ReservationTimeEnum.Past ? <ReservationReview reservation={reservation} /> : <></>}
                        </ReservationItemInnerTextBoxStyled>
                        <PrimaryButtonBoxStyled>
                            {reservationTime === ReservationTimeEnum.Upcoming && userInfo?.userRoleId === UserRoleEnum.Student ? <UpcomingLessonsStudentActionWithProviders reservation={reservation} onSubmit={onSubmit} /> : <></>}
                            {reservationTime === ReservationTimeEnum.Past && userInfo?.userRoleId === UserRoleEnum.Student ? <PastLessonsStudentActions reservation={reservation} /> : <></>}
                        </PrimaryButtonBoxStyled>
                    </ReportIssueModalProvider>
                </ReservationModalProvider>
            </ReservationReviewModalProvider>
        </ReservationItemBoxStyled>
    )
}

const EmptyResevations = ({ translationKeys }: { translationKeys: ReservationTranslationKeys }) => {
    const { t } = useTranslation();

    return (
        <EmptyReservationsBoxStyled>
            <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", gap: "4px", flexDirection: "column" }}>
                <H5Styled>{t(translationKeys.noEvents)}</H5Styled>
                <Body1Styled>{t(translationKeys.noEventsDescription)}</Body1Styled>
            </Box>
            <IconBoxStyled><EmptyCalendarIcon /></IconBoxStyled>
        </EmptyReservationsBoxStyled>
    )
}

const CircularProgress = () => (<CircularProgressStyled size={30} />);

const ReservationsLoading = () => (
    <Box sx={{ display: "flex", width: "100%", justifyContent: "center" }}>
        <CircularProgress />
    </Box>
)

const ReservationsInitLoading = () => (
    <Box sx={{ display: "flex", height: RESERVATION_BOX_HEIGHT, width: "100%", justifyContent: "center", alignItems: "center" }}>
        <CircularProgress />
    </Box>
);

const Reservations = ({ time, translationKeys }: { time: ReservationTimeEnum, translationKeys: ReservationTranslationKeys }) => {
    const dispatch = useAppDispatch();
    const [skip, setSkip] = useState<number>(0);
    const reservationsStatus = useAppSelector((state) => selectReservationsStatus(state, time))
    const reservations = useAppSelector((state) => selectReservations(state, time));

    useEffect(() => {
        fetchNext(AppendTypeEnum.None, 0);
        dispatch(setReservationIssueTypes());
    }, []);

    const fetchNext = (appendType: AppendTypeEnum, skip: number) => {
        var request = {
            request: {
                filter: { time: time },
                sort: {
                    predicate: "startTime",
                    reverse: time === ReservationTimeEnum.Past
                },
                skip: skip,
                take: NUMBER_OF_RESERVATIONS_TO_SHOW,
            },
            appendType: appendType
        }

        switch (time) {
            case ReservationTimeEnum.Upcoming:
                return dispatch(setUpcomingReservations(request));
            case ReservationTimeEnum.Past:
                return dispatch(setPastReservations(request));
            default:
                throw new Error("Invalid time");
        }
    }

    const handleFetchNext = () => {
        const newSkip = skip + NUMBER_OF_RESERVATIONS_TO_SHOW;
        fetchNext(AppendTypeEnum.End, newSkip);
        setSkip(newSkip);
    }

    const handleRefresh = () => {
        fetchNext(AppendTypeEnum.None, 0);
    }

    return (
        <ReservationsBoxStyled>
            {reservationsStatus === LoadingStatusEnum.Error ? <ErrorLarge /> : <></>}
            {!reservations ? <ReservationsInitLoading /> : <></>}
            {reservations && !reservations.size && <EmptyResevations translationKeys={translationKeys} />}
            {reservations && reservations.size ? (
                <InfiniteScroll
                    dataLength={reservations.data.length}
                    style={{ width: "100%" }}
                    next={handleFetchNext}
                    height="500px"
                    hasMore={!reservations || reservations.data.length < reservations.size!}
                    loader={<ReservationsLoading />}
                    scrollableTarget="scrollableDiv"
                >
                    <ReservationContainerBoxStyled sx={{ gap: time === ReservationTimeEnum.Past ? { xs: "16px", md: "8px" } : { xs: "16px", md: "34px" } }}>
                        {reservations.data?.map((reservation, idx) => <ReservationItem reservation={reservation} reservationTime={time} key={reservation.reservationId!} onSubmit={handleRefresh} />)}
                    </ReservationContainerBoxStyled>
                </InfiniteScroll>
            ) : <></>}
        </ReservationsBoxStyled>
    )
}

export default Reservations;