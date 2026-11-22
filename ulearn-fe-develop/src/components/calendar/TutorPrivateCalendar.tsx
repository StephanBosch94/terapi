import { styled, experimental_sx as sx } from "@mui/material/styles";
import { isRecurrentTimeslot, isReservation, isTimeslot } from "core/services/Utils/calendarUtil";
import ArrowLeftIcon from "assets/arrow-left.svg";
import ArrowRightIcon from "assets/arrow-right.svg";
import PlusBoldIcon from "assets/plus-bold.svg";
import RepeatIcon from "assets/repeat.svg";
import PlusIcon from "assets/plus.svg";
import CancelLessonIcon from "assets/cancel_lesson.svg";
import JoinLessonIcon from "assets/join_lesson.svg";
import ChatIcon from "assets/chat.svg";
import EmptyCalendarIcon from "assets/empty_calendar.svg";
import CalendarIcon from "assets/calendar.svg";
import moment from 'moment';
import { createContext, useCallback, useEffect, useContext, useState, useRef } from "react";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { contextFactoryCreate, useAppSelector } from "core/redux/reduxHooks";
import { PrivateCalendarEventsResponse, PrivateCalendarRecurrentTimeslotType, PrivateCalendarReservationType, PrivateCalendarTimeslotType, selectGetPrivateCalendarEventsStatus, selectPrivateCalendarStatistics, selectPrivateCalendarStatisticsStatus } from "core/redux/calendarSlice";
import { useTranslation } from "components/core/TranslationProvider";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import CheckBoxItem from "components/styles/CheckBoxItem";
import { PrimaryButton } from "components/styles/PrimaryButton";
import Tooltip from "components/styles/Tooltip";
import React from "react";
import { getProfilePhotoThumbnailUrl } from "core/services/Utils/imageUtil";
import { getUserName } from "core/services/Utils/userUtils";
import InfiniteScroll from "react-infinite-scroll-component";
import { MenuBoxStyled } from "components/styles/boxes/MenuBoxStyled";
import MenuItem from "components/menu/MenuItem";
import MenuLinkItem from "components/menu/MenuLinkItem";
import routes from 'routes/routes';
import ContactUserModalProvider, { useContactUserModalContext } from "components/modals/ContactUserModalProvider";
import CancelTimeslotModalProvider, { useCancelTimeslotModalContext } from "components/modals/CancelTimeslotModalProvider";
import CancelRecurrentTimeslotModalProvider, { useCancelRecurrentTimeslotModalContext } from "components/modals/CancelRecurrentTimeslotModalProvider";
import NewTimeslotModalProvider, { useNewTimeslotModalContext } from "components/modals/NewTimeslotModalProvider";
import { useCalendarContext } from "./contexts/CalendarContext";
import CalendarContextProvider from "./providers/CalendarContextProvider";
import Calendar, { CalendarMobile } from "./Calendar";
import { UIContext } from "components/core/UIContextProvider";
import { ArrowBoxStyled } from "./common/styles/ArrowBoxStyled";
import { DotsBoxStyled as DotsDefinitionBoxStyled } from "./common/styles/DotsBoxStyled";
import CalendarFreeSlotDot from "./common/CalendarFreeSlotDot";
import CalendarResevationDot from "./common/CalendarResevationDot";
import { useSwipeable } from "react-swipeable";
import CircularProgress from "components/styles/CircularProgressStyled";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import ErrorLarge from "components/core/error/ErrorLarge";
import H3Styled from "components/styles/typography/H3Styled";
import PStyled from "components/styles/typography/PStyled";
import { getLoadingStatus } from "core/services/Utils/apiUtil";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import Avatar from "@mui/material/Avatar";
import Body1Styled from "components/styles/typography/Body1Styled";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import ReservationModalProvider, { useReservationModalContext } from "components/modals/ReservationModalProvider";
import ThreeDotsSmallMenu from "components/menu/ThreeDotsSmallMenu";
import { ContainerStyled } from "components/styles/ContainerStyled";

const CalendarDektopBoxStyled = styled(Box)({
    width: "100%",
    display: "flex",
    flexFlow: "row noWrap",
    padding: "16px 6px 16px 16px",
    gap: "24px",
    border: `1px solid ${COLORS.BLACK_10}`,
    borderRadius: "4px"
}) as typeof Box;

const CalendarMobileBoxStyled = styled(Box)(() => sx({
    width: "100%",
    height: "100%",
    display: "flex",
    flexFlow: "column noWrap",
    overflow: "auto",
    position: "relative",
    borderRadius: "4px",
    gap: { xs: "0", md: "8px" }
})) as typeof Box;

const VerticalDividerBoxStyled = styled(Box)({
    width: "1px",
    minHeight: "100%",
    display: "flex",
    flexFlow: "column noWrap",
    flex: "0 1 auto",
    backgroundColor: COLORS.BLACK_10
}) as typeof Box;

const HorizontalDividerBoxStyled = styled(Box)({
    width: "100%",
    minHeight: "1px",
    display: "flex",
    flexFlow: "column noWrap",
    backgroundColor: COLORS.BLACK_10
}) as typeof Box;

const CalendarFiltersBoxStyled = styled(Box)({
    display: "flex",
    gap: "16px",
    flexFlow: "column noWrap",
    userSelect: "none"
}) as typeof Box;

const CalendarFiltersInnerBoxStyled = styled(Box)({
    display: "flex",
    gap: "16px",
    flexFlow: "column noWrap"
}) as typeof Box;

const CalendarFiltersTypographyStyled = styled(Box)({
    fontSize: "0.875rem",
    fontWeight: "500",
    lineHeight: "15px"
}) as typeof Box;

const CalendarControllersBoxStyled = styled(Box)({
    display: "flex",
    flexFlow: "row noWrap",
    gap: "8px"
}) as typeof Box;

const CalendarTitleTypographyStyled = styled(Body1Styled)({
    fontWeight: "500",
    color: COLORS.BLACK
}) as typeof CaptionTextStyled;

const DaySequenceTypographyStyled = styled(Body1Styled)({
    fontWeight: "500",
    color: COLORS.BLACK
}) as typeof Body1Styled;

const EventDateHeaderBox = styled(Box)(() => sx({
    width: "100%",
    display: "inline-flex",
    padding: { xs: "6px 16px", md: "12px 0" },
    gap: "10px",
    borderBottom: { xs: "none", md: `1px solid ${COLORS.BLACK_10}` },
    backgroundColor: { xs: COLORS.BLACK_5, md: "transparent" }
})) as typeof Box;

const NewAvailabilityTextButtonStyled = styled(PrimaryButton)({
    width: "auto",
    borderRadius: "4px",
    display: "inline-flex",
    gap: "8px",
    padding: "6px 12px !important"
}) as typeof Button;

const TodayButtonStyled = styled(Button)({
    display: "inline-flex",
    padding: "6px 12px",
    borderRadius: "4px",
    border: `1px solid ${COLORS.BLACK_10}`,
    color: COLORS.BLACK,
    ":hover": {
        border: `1px solid ${COLORS.PRIMARY}`,
        cursor: "pointer"
    }
}) as typeof Button;

const EventDotsIconBox = styled(Box)({
    border: `1px solid ${COLORS.PRIMARY}`,
    padding: "4px",
    borderRadius: "4px",
    alignItems: "center",
    display: "flex",
    color: COLORS.PRIMARY,
    svg: {
        transform: "scale(1.25)"
    }
}) as typeof Box;

const ScrollableEventBox = styled(Box)(() => sx({
    height: { xs: "100%", md: "600px" },
    overflowY: "auto",
    position: "relative",
    paddingRight: { xs: "0px", md: "18px" },
    "div.eventBox": {
        display: "flex",
        flexFlow: "row noWrap",
        justifyContent: "space-between",
        alignItems: "center",
        padding: { xs: "8px 16px", md: "12px 8px" },
        ":hover": {
            background: COLORS.PRIMARY_LIGHT,
            cursor: "pointer"
        }
    },
    "div.eventInnerBox": {
        display: "flex",
        flexFlow: "row noWrap",
        gap: "32px",
        alignItems: "center"
    },
    "div.eventInnerTimeBox": {
        display: "flex",
        flexFlow: "column noWrap",
        "p.fromTime": {
            fontSize: "0.75rem",
            lineHeight: "16px",
            fontWeight: "500",
            margin: 0
        },
        "p.toTime": {
            fontSize: "0.75rem",
            lineHeight: "16px",
            fontWeight: "400",
            color: COLORS.BLACK_40,
            margin: 0
        }
    },
    "div.eventCenterBox": {
        display: "inline-flex",
        gap: "8px",
        alignItems: "center"
    },
    "div.noEventsBox": {
        width: "100%",
        padding: { xs: "8px 16px", md: "4px 0" }
    },
    "p.eventPrimaryText": {
        fontSize: "0.875rem",
        lineHeight: "16px",
        fontWeight: "500",
        margin: 0
    },
    "p.eventSecondaryText": {
        fontSize: "0.875rem",
        lineHeight: "20px",
        fontWeight: "400",
        color: COLORS.BLACK_60,
        margin: 0
    },
    "p.noEventsText": {
        fontSize: "0.75rem",
        lineHeight: "16px",
        padding: "8px",
        fontWeight: "400",
        color: COLORS.BLACK,
        margin: 0
    }
})) as typeof Box;

const hiddenScrollbarStyles = {
    "::-webkit-scrollbar": {
        display: "none"
    },
    msOverflowStyle: "none",
    scrollbarWidth: "none"
}

const PlusIconBox = styled(Box)({
    position: "absolute",
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    bottom: "24px",
    right: "24px",
    padding: "8px",
    color: COLORS.WHITE,
    backgroundColor: COLORS.PRIMARY,
    borderRadius: "50%",
    zIndex: "100",
    boxShadow: "2px 3px 6px rgba(0, 0, 0, 0.15), -1px -1px 6px rgba(0, 0, 0, 0.08)",
    "svg": {
        transform: "scale(1.5)",
    },
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_HOVER
    }
}) as typeof Box;

const CancelIconBoxStyled = styled(Box)({
    display: "inline-flex",
    alignItems: "center",
    color: COLORS.RED
}) as typeof Box;

const EmptyCalendarBox = styled(Box)(() => sx({
    height: "100%",
    display: "flex",
    alignItems: "center",
    padding: "0px 16px",
    gap: "56px",
    justifyContent: "center",
    flexFlow: "column noWrap"
})) as typeof Box;

const EmptyCalendarHeader = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    gap: "16px",
    justifyContent: "center",
    flexFlow: "column noWrap"
})) as typeof Box;

const EmptyCalendarContent = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    gap: "24px",
    justifyContent: "center",
    flexFlow: "column noWrap"
})) as typeof Box;

type ScrollContextType = {
    isScrolling: boolean,
    scrollHeight: number,
    setScrollHeight: (newValue: number) => void,
    setIsScrolling: (newValue: boolean) => void
} | undefined;

const ScrollContext = createContext<ScrollContextType>(undefined);
const useScrollContext = contextFactoryCreate('Scroll', ScrollContext);

const ScrollContextProvider = ({ children }: { children: any }) => {
    const [scrollHeight, setScrollHeight] = useState(0);
    const [isScrolling, setIsScrolling] = useState(false);

    return (
        <ScrollContext.Provider value={{
            scrollHeight,
            setScrollHeight: (newValue: number) => setScrollHeight(newValue),
            isScrolling,
            setIsScrolling
        }}>
            {children}
        </ScrollContext.Provider>
    )
}

const ActiveDateTitle = () => {
    const { activeDate } = useCalendarContext();

    return (
        <Box sx={{ display: "inline-flex", justifyContent: "center" }}>
            <CalendarTitleTypographyStyled>{activeDate.format("MMMM Do, YYYY")}</CalendarTitleTypographyStyled>
        </Box>
    )
}

const CalendarActiveDatesDesktopControllers = () => {
    const { handleNextDate, handlePreviousDate } = useCalendarContext();

    return (
        <CalendarControllersBoxStyled>
            <ArrowBoxStyled onClick={handlePreviousDate}><ArrowLeftIcon /></ArrowBoxStyled>
            <ArrowBoxStyled onClick={handleNextDate}><ArrowRightIcon /></ArrowBoxStyled>
        </CalendarControllersBoxStyled>
    )
}

const CalendarActiveDatesMobileControllers = () => {
    const { handlePreviousClick, handleNextClick } = useCalendarContext();

    return (
        <CalendarControllersBoxStyled>
            <ArrowBoxStyled onClick={handlePreviousClick}><ArrowLeftIcon /></ArrowBoxStyled>
            <ArrowBoxStyled onClick={handleNextClick}><ArrowRightIcon /></ArrowBoxStyled>
        </CalendarControllersBoxStyled>
    )
}

const TodayBadgeLinkWithTooltip = () => {
    const { t } = useTranslation();
    const { handleSetCurrentDate } = useCalendarContext();
    const currentDate = moment().startOf('day').format("MMMM DD, YYYY");

    return (
        <Tooltip text={<div dangerouslySetInnerHTML={{ __html: t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.TODAY_BUTTON_TOOLTIP, { date: currentDate }) }} />}>
            <TodayButtonStyled onClick={handleSetCurrentDate}>
                <Typography sx={{ fontSize: "0.875rem", lineHeight: "20px", fontWeight: "500" }}>
                    {t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.DAY_SEQUENCE.TODAY)}
                </Typography>
            </TodayButtonStyled>
        </Tooltip>
    )
}

const NewAvailabilityTextButton = () => {
    const { t } = useTranslation();
    const { setOpen } = useNewTimeslotModalContext();

    return (
        <NewAvailabilityTextButtonStyled onClick={setOpen}>
            <PlusBoldIcon />
            <Typography sx={{ fontSize: "0.875rem", fontWeight: "500", lineHeight: "20px" }}>
                {t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.NEW_AVAILABILITY)}
            </Typography>
        </NewAvailabilityTextButtonStyled>
    )
}

const NewAvailabilityTextButtonWithContext = () => {
    const { refreshData } = useCalendarContext();
    return (
        <NewTimeslotModalProvider onSubmit={refreshData}>
            <NewAvailabilityTextButton />
        </NewTimeslotModalProvider>
    )
}

const CalendarTitleSectionDesktop = () => {

    return (
        <Box sx={{ display: "inline-flex", width: "100%", justifyContent: "space-between", alignItems: "center", paddingRight: "18px" }}>
            <Box sx={{ display: "inline-flex", gap: "16px", alignItems: "center" }}>
                <TodayBadgeLinkWithTooltip />
                <CalendarActiveDatesDesktopControllers />
                <ActiveDateTitle />
            </Box>
            <NewAvailabilityTextButtonWithContext />
        </Box>
    )
}

const CalendarTitleSectionMobile = () => {

    return (
        <ContainerStyled sx={{ display: "inline-flex", justifyContent: "space-between", gap: "16px", alignItems: "center" }}>
            <TodayBadgeLinkWithTooltip />
            <Box sx={{ display: "inline-flex", gap: "8px", alignItems: "center" }}>
                <ActiveDateTitle />
                <CalendarActiveDatesMobileControllers />
            </Box>
        </ContainerStyled>
    )
}

const EventDateHeader = ({ date }: { date: moment.Moment }) => {
    const { t } = useTranslation();
    const isToday = moment().isSame(date, 'date');
    const isTomorrow = moment().add(1, 'day').isSame(date, 'day');

    return (
        <EventDateHeaderBox>
            {isToday ? <DaySequenceTypographyStyled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.DAY_SEQUENCE.TODAY)}</DaySequenceTypographyStyled> : <></>}
            {isTomorrow ? <DaySequenceTypographyStyled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.DAY_SEQUENCE.TOMORROW)}</DaySequenceTypographyStyled> : <></>}
            <Body1Styled sx={{ color: COLORS.BLACK }}>{date.format("ddd D.M")}</Body1Styled>
        </EventDateHeaderBox>
    )
}

const EventTimeBox = ({ fromTime, toTime }: { fromTime: moment.Moment, toTime: moment.Moment }) => {

    return (
        <div className="eventInnerTimeBox">
            <p className="fromTime">{fromTime.format("HH:mm")}</p>
            <p className="toTime">{toTime.format("HH:mm")}</p>
        </div>
    )
}

const AvailableForLesson = ({ showRecurrentIcon, date }: { showRecurrentIcon?: boolean, date?: moment.Moment }) => {
    const { t } = useTranslation();

    return (

        <div className="eventCenterBox">
            <CalendarFreeSlotDot width="8px" height="8px" />
            <p className="eventPrimaryText">{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.FREE_SLOT_TITLE)}</p>
            {showRecurrentIcon ? (
                <Tooltip text={<div dangerouslySetInnerHTML={{ __html: t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.REPEATING_TIMESLOT_TOOLTIP, { dayOfWeek: date?.format("dddd") }) }} />}>
                    <div style={{ display: "inline-flex", alignItems: "center" }}>
                        <RepeatIcon />
                    </div>
                </Tooltip>
            ) : <></>}
        </div>

    )
}

const ReservationClassOpponent = ({ reservation }: { reservation: NonNullable<PrivateCalendarReservationType["reservation"]> }) => {
    const { t } = useTranslation();

    return (
        <div className="eventCenterBox">
            <CalendarResevationDot width="8px" height="8px" />
            <Avatar sx={{ width: "40px", height: "40px" }} src={getProfilePhotoThumbnailUrl(reservation?.opponent)} />
            <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "4px", justifyContent: "flex-start" }}>
                <p className="eventPrimaryText">
                    {t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.USER_NAME_WITH_PACKAGE, { opponent: getUserName(reservation.opponent), orderNumber: reservation.orderNumber ?? 0, totalNumber: reservation.totalInPackage ?? 0 })}
                </p>
                <p className="eventSecondaryText">{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.LESSON)}</p>
            </Box>
        </div>
    )
}

const CancelIconBox = () => {
    return (
        <CancelIconBoxStyled><CancelLessonIcon /></CancelIconBoxStyled>
    )
}

const ReservationMenu = ({ reservation }: { reservation: PrivateCalendarReservationType["reservation"] }) => {
    const { t } = useTranslation();
    const { setOpen: setContactUserModalOpen } = useContactUserModalContext();
    const { handleRescheduleClick } = useReservationModalContext();


    return (
        <MenuBoxStyled gap="4px">
            {reservation?.room ? <MenuLinkItem icon={<JoinLessonIcon />} text={t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.JOIN_LESSON)} href={routes.meetingPage.path(reservation.room.roomKey ?? "")} /> : <></>}
            <MenuItem icon={<ChatIcon />} text={t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.WRITE_A_MESSAGE)} onClick={setContactUserModalOpen} />
            {reservation?.canReschedule ? (
                <>
                    <HorizontalDividerBoxStyled />
                    <MenuItem
                        onClick={handleRescheduleClick}
                        icon={<CalendarIcon />}
                        text={t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.RESCHEDULE_LESSON)}
                    />
                </>
            ) : <></>}
        </MenuBoxStyled>
    )
}

const ReservationMenuWithContexts = ({ reservation }: { reservation: NonNullable<PrivateCalendarReservationType["reservation"]> }) => {
    const { refreshData } = useCalendarContext();

    return (
        <ContactUserModalProvider userInfo={reservation!.opponent!}>
            <ReservationModalProvider
                tutorInfo={{
                    userId: reservation.hostId,
                    currencyCode: undefined,
                    hourRate: undefined,
                    files: undefined,
                    firstName: undefined,
                    lastName: undefined,
                    teachesLessonsForChildren: false
                }}
                reservation={reservation}
                onSubmit={refreshData}
            >
                <ReservationMenu reservation={reservation} />
            </ReservationModalProvider>
        </ContactUserModalProvider>
    )
}

const Reservation = ({ event }: { event: PrivateCalendarReservationType }) => {
    return (
        <div className="eventBox">
            <div className="eventInnerBox">
                <EventTimeBox fromTime={moment(event.fromTime)} toTime={moment(event.reservation?.expectedEndTime)} />
                {event.reservation ? <ReservationClassOpponent reservation={event.reservation} /> : <></>}
            </div>
            {event.reservation ? <ThreeDotsSmallMenu innerMenu={<ReservationMenuWithContexts reservation={event.reservation} />} /> : <></>}
        </div>
    )
}

const RecurrentTimeslotMenu = () => {
    const { t } = useTranslation();
    const { setOpen } = useCancelRecurrentTimeslotModalContext();

    return (
        <MenuBoxStyled gap="4px">
            <MenuItem icon={<CancelIconBox />} text={t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.DELETE_EVENT.TITLE)} onClick={setOpen} />
        </MenuBoxStyled>
    )
}

const RecurrentTimeslotMenuWithContexts = ({ event, date }: { event: PrivateCalendarRecurrentTimeslotType, date: moment.Moment }) => {
    const { refreshData } = useCalendarContext();
    return (
        <CancelRecurrentTimeslotModalProvider recurrentTimeslotId={event.recurrentTimeslotId!} referenceDate={date} onSubmit={refreshData}>
            <RecurrentTimeslotMenu />
        </CancelRecurrentTimeslotModalProvider>
    )
}

const RecurrentTimeslot = ({ event, date }: { event: PrivateCalendarRecurrentTimeslotType, date: moment.Moment }) => {
    return (
        <div className="eventBox">
            <div className="eventInnerBox">
                <EventTimeBox fromTime={moment(event.fromTime)} toTime={moment(event.toTime)} />
                <AvailableForLesson showRecurrentIcon={true} date={date} />
            </div>
            <ThreeDotsSmallMenu innerMenu={<RecurrentTimeslotMenuWithContexts event={event} date={date} />} />
        </div>
    )
}

const TimeslotMenu = () => {
    const { t } = useTranslation();
    const { setOpen } = useCancelTimeslotModalContext();
    return (
        <MenuBoxStyled>
            <MenuItem icon={<CancelIconBox />} text={t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.DELETE_EVENT.TITLE)} onClick={setOpen} />
        </MenuBoxStyled>
    )
}

const TimeslotMenuWithContexts = ({ event }: { event: PrivateCalendarTimeslotType }) => {
    const { refreshData } = useCalendarContext();

    return (
        <CancelTimeslotModalProvider timeslotId={event.timeslotId!} onSubmit={refreshData}>
            <TimeslotMenu />
        </CancelTimeslotModalProvider>
    )
}

const Timeslot = ({ event }: { event: PrivateCalendarTimeslotType }) => {
    return (
        <div className="eventBox">
            <div className="eventInnerBox">
                <EventTimeBox fromTime={moment(event.fromTime)} toTime={moment(event.toTime)} />
                <AvailableForLesson />
            </div>
            <ThreeDotsSmallMenu innerMenu={<TimeslotMenuWithContexts event={event} />} />
        </div>
    )
}

const NoEvents = () => {
    const { t } = useTranslation();

    return (
        <div className="noEventsBox">
            <p className="noEventsText">{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.NO_EVENTS)}</p>
        </div>
    )
}

const isInCurrentHeightRange = (scrollTop: number, currentOffset: number, currentHeight: number) => {
    const threshold = 40;
    return scrollTop >= currentOffset - threshold && scrollTop <= currentOffset + currentHeight - threshold;
}

const CalendarDayEvents = ({ date, events }: { date: moment.Moment, events: PrivateCalendarEventsResponse["data"][0]["events"] }) => {
    const { scrollHeight, setScrollHeight, isScrolling } = useScrollContext();
    const [parentOffset, setParentOffset] = useState(0);
    const [height, setHeight] = useState(0);
    const { handleSetActiveDate, activeDate } = useCalendarContext();

    // When events are changed => positions should be recalculated
    const refCallback = useCallback((node: HTMLElement | null) => {
        if (node !== null) {
            setParentOffset(node.offsetTop);
            setHeight(node.scrollHeight);
        }
    }, [events]);

    //Sync activeDate when user is scrolling
    useEffect(() => {
        if (isScrolling && isInCurrentHeightRange(scrollHeight, parentOffset, height)) {
            handleSetActiveDate(date);
        }
    }, [date, isScrolling, scrollHeight, parentOffset, height]);

    // Sync scroll when user selecting date
    useEffect(() => {
        if (!isScrolling && activeDate.isSame(date, 'date') && !isInCurrentHeightRange(scrollHeight, parentOffset, height)) {
            setScrollHeight(parentOffset);
        }
    }, [date, isScrolling, activeDate, scrollHeight, parentOffset, height]);

    return (
        <Box ref={refCallback}>
            <EventDateHeader date={date} />
            {events?.length === 0 ? <NoEvents /> : <></>}
            {events?.map((x, idx) =>
                <React.Fragment key={idx}>
                    {isReservation(x) ? <Reservation event={x} /> : <></>}
                    {isTimeslot(x) ? <Timeslot event={x} /> : <></>}
                    {isRecurrentTimeslot(x) ? <RecurrentTimeslot event={x} date={date} /> : <></>}
                </React.Fragment>
            )}
        </Box>
    )
}

const PlusButton = () => {
    const { setOpen } = useNewTimeslotModalContext();

    return (
        <PlusIconBox onClick={setOpen}>
            <PlusIcon />
        </PlusIconBox>
    )
}

const PlusButtonWithProviders = () => {
    const { refreshData } = useCalendarContext();

    return (
        <NewTimeslotModalProvider onSubmit={refreshData}>
            <PlusButton />
        </NewTimeslotModalProvider>
    )
}

const InfinityLoadSkeleton = () => {

    return (
        <Box sx={{ position: "absolute", top: "50%", left: "50%", transform: "translate(-50%, -50%)", display: "flex", alignItems: "center", justifyContent: "center" }}>
            <CircularProgress />
        </Box>
    )
}

const EmptyCalendarEvents = () => {
    const { t } = useTranslation();
    
    return (
        <EmptyCalendarBox>
            <EmptyCalendarContent>
                <EmptyCalendarHeader>
                    <H3Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.EMPTY_CALENDAR_TITLE)}</H3Styled>
                    <PStyled sx={{ textAlign: "center" }}>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.EMPTY_CALENDAR_DESCRIPTION)}</PStyled>
                </EmptyCalendarHeader>
            </EmptyCalendarContent>
            <EmptyCalendarIcon />
        </EmptyCalendarBox>
    )
}

const EmptyCalendarEventsWithProviders = () => {
    const { refreshData } = useCalendarContext();

    return (
        <NewTimeslotModalProvider onSubmit={refreshData}>
            <EmptyCalendarEvents />
        </NewTimeslotModalProvider>
    )
}

const CalendarEventSectionLoading = () => (
    <Box sx={{ display: "flex", height: "100%", alignItems: "center", justifyContent: "center" }}>
        <CircularProgress color={COLORS.PRIMARY} />
    </Box>
)

const CalendarEventsSection = () => {
    const [autoScrollInProgress, setAutoScrollInProgress] = useState(false);
    const { eventsByDate, handleFetchNextMonthData } = useCalendarContext();
    const { scrollHeight, setScrollHeight, setIsScrolling, isScrolling } = useScrollContext();
    const { screenType } = useContext(UIContext);
    const fetchEventsStatus = useAppSelector(selectGetPrivateCalendarEventsStatus);
    const fetchStatisticsStatus = useAppSelector(selectPrivateCalendarStatisticsStatus);
    const calendarStatistics = useAppSelector(selectPrivateCalendarStatistics);
    const fetchStatus = getLoadingStatus([fetchEventsStatus, fetchStatisticsStatus]);

    const ref = useRef<HTMLElement | null>(null);
    const timerDebounceRef = useRef<NodeJS.Timeout>();
    const scrollingTimerRef = useRef<NodeJS.Timeout>();

    const handleDebounedScroll = () => {
        if (autoScrollInProgress) {
            return;
        }
        if (fetchStatus !== LoadingStatusEnum.Loaded){
            return;
        }
        if (!isScrolling) {
            setIsScrolling(true);
        }
        if (timerDebounceRef.current) {
            clearTimeout(timerDebounceRef.current);
        }
        if (scrollingTimerRef.current) {
            clearTimeout(scrollingTimerRef.current);
        }
        timerDebounceRef.current = setTimeout(() => {
            setScrollHeight(ref.current?.scrollTop ?? 0);
        }, 500);
        scrollingTimerRef.current = setTimeout(() => {
            setIsScrolling(false);
        }, 1500);
    }

    // Update the scroll position after the autoScrollInProgress is set to true
    useEffect(() => {
        if (!autoScrollInProgress) return;
        if (fetchStatus !== LoadingStatusEnum.Loaded){
            return;
        }
        ref.current?.scrollTo({
            top: scrollHeight
        });
    }, [scrollHeight, autoScrollInProgress]);

    useEffect(() => {
        if (isScrolling) {
            return;
        }
        if ((ref.current?.scrollTop ?? 0) === scrollHeight) {
            return;
        }

        setAutoScrollInProgress(true);

        setTimeout(() => {
            setAutoScrollInProgress(false);
        }, 100);
    }, [isScrolling, scrollHeight]);

    return (
        <ScrollableEventBox id="scrollableDiv" ref={ref} onScroll={handleDebounedScroll}
            sx={screenType === "mobile" ? hiddenScrollbarStyles : undefined}
        >
            {fetchStatus === LoadingStatusEnum.Error ? <ErrorLarge /> : <></>}
            {fetchStatus === LoadingStatusEnum.Loading ? <CalendarEventSectionLoading /> : <></>}
            {fetchStatus === LoadingStatusEnum.Loaded && calendarStatistics && !calendarStatistics.hasConfiguredAvailability ? <EmptyCalendarEventsWithProviders /> : <></>}
            {fetchStatus === LoadingStatusEnum.Loaded && calendarStatistics && calendarStatistics.hasConfiguredAvailability
                ? (
                    <InfiniteScroll
                        dataLength={eventsByDate.length}
                        style={{ display: "flex", flexFlow: "column noWrap" }}
                        next={handleFetchNextMonthData}
                        scrollThreshold="30px"
                        hasMore={true}
                        loader={<InfinityLoadSkeleton />}
                        inverse={false}
                        scrollableTarget="scrollableDiv"
                    >
                        {eventsByDate.map((x, idx) => <CalendarDayEvents key={idx} date={x.date} events={x.events} />)}
                    </InfiniteScroll>
                ) : <></>}
        </ScrollableEventBox>
    )
}

const CalendarFilter = ({ titleKey, colorComponent, defaultChecked, onClick }: { titleKey: string, colorComponent: JSX.Element, defaultChecked: boolean, onClick: () => void }) => {
    const { t } = useTranslation();

    return (
        <CheckBoxItem defaultChecked={defaultChecked} onClick={onClick}>
            {colorComponent}
            <CalendarFiltersTypographyStyled>{t(titleKey)}</CalendarFiltersTypographyStyled>
        </CheckBoxItem>
    )
}

const CalendarAvailabeTimeFilter = () => {
    const { handleShowFreeSlotsClick } = useCalendarContext();

    return (
        <CalendarFilter
            titleKey={TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.FILTERS.AVAILABLE_TIME}
            colorComponent={<CalendarFreeSlotDot width="6px" height="6px" />}
            defaultChecked={true}
            onClick={handleShowFreeSlotsClick}
        />
    )
}

const CalendarReservationFilter = () => {
    const { handleShowReservationsClick } = useCalendarContext();

    return (
        <CalendarFilter
            titleKey={TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.FILTERS.RESERVED_LESSONS}
            colorComponent={<CalendarResevationDot width="6px" height="6px" />}
            defaultChecked={true}
            onClick={handleShowReservationsClick}
        />
    )
}

const CalendarFilters = () => {
    const { t } = useTranslation();

    return (
        <CalendarFiltersBoxStyled>
            <CalendarTitleTypographyStyled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.EVENTS)}</CalendarTitleTypographyStyled>
            <CalendarFiltersInnerBoxStyled>
                <CalendarAvailabeTimeFilter />
                <CalendarReservationFilter />
            </CalendarFiltersInnerBoxStyled>
        </CalendarFiltersBoxStyled>
    )
}

const TutorPrivateCalendarDesktop = () => {
    return (
        <CalendarDektopBoxStyled>
            <Box sx={{ display: "flex", flexFlow: "column noWrap", flex: "0 1 auto", gap: "16px" }}>
                <Calendar />
                <HorizontalDividerBoxStyled />
                <CalendarFilters />
            </Box>
            <VerticalDividerBoxStyled />
            <Box sx={{ display: "flex", flexFlow: "column noWrap", flex: "1 1 auto", gap: "8px", position: "relative" }}>
                <CalendarTitleSectionDesktop />
                <ScrollContextProvider>
                    <CalendarEventsSection />
                </ScrollContextProvider>
            </Box>
        </CalendarDektopBoxStyled>
    )
}

const CalendarSwipableIcon = () => {
    return (
        <Box sx={{ display: "inline-flex", width: "100%", justifyContent: "center" }} >
            <Box sx={{ width: "32px", height: "4px", borderRadius: "18px", background: COLORS.BLACK_10 }} />
        </Box>
    )
}

const TutorPrivateCalendarMobile = () => {
    const { handleSwipeUp, handleSwipeRight, handleSwipeDown, handleSwipeLeft } = useCalendarContext();

    const handlers = useSwipeable({
        onSwipedUp: handleSwipeUp,
        onSwipedRight: handleSwipeRight,
        onSwipedDown: handleSwipeDown,
        onSwipedLeft: handleSwipeLeft
    });

    return (
        <Box {...handlers} sx={{ width: "100%", backgroundColor: COLORS.BLACK_5 }}>
            <ContainerStyled sx={{ width: "100%", justifyContent: "center", borderRadius: "0px 0px 8px 8px", backgroundColor: COLORS.WHITE }}>
                <CalendarMobile />
                <CalendarSwipableIcon />
            </ContainerStyled>
        </Box>
    )
}

const TutorPrivateCalendarMobileWithEvenets = () => (
    <CalendarMobileBoxStyled>
        <Box sx={{ display: "flex", flexFlow: "column noWrap", flex: "0 1 auto", gap: "8px" }}>
            <CalendarTitleSectionMobile />
            <TutorPrivateCalendarMobile />
        </Box>
        <Box sx={{ display: "flex", flexFlow: "column noWrap", flex: "1 1 auto", overflowY: "auto", position: "relative" }}>
            <ScrollContextProvider>
                <CalendarEventsSection />
            </ScrollContextProvider>
            <PlusButtonWithProviders />
        </Box>
    </CalendarMobileBoxStyled>
)

const TutorPrivateDesktopCalendarWithContainer = () => (
    <CalendarContextProvider defaultMode="month">
        <TutorPrivateCalendarDesktop />
    </CalendarContextProvider>
)

const TutorPrivateMobileCalendarWithContainer = () => (
    <CalendarContextProvider defaultMode="week">
        <TutorPrivateCalendarMobileWithEvenets />
    </CalendarContextProvider>
)

const TutorPrivateCalendarWithContexts = () => {
    const { screenType } = useContext(UIContext);

    return (
        <DotsDefinitionBoxStyled sx={{ height: "100%" }}>
            {screenType === "desktop" ? <TutorPrivateDesktopCalendarWithContainer /> : <></>}
            {screenType === "mobile" ? <TutorPrivateMobileCalendarWithContainer /> : <></>}
        </DotsDefinitionBoxStyled>
    )
}

export default TutorPrivateCalendarWithContexts;