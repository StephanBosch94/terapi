import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";
import { refreshPrivateCalendarEvents, refreshPrivateCalendarStatistics, selectPrivateCalendarEvents, setPrivateCalendarEvents, setPrivateCalendarStatistics } from "core/redux/calendarSlice";
import { useAppSelector, useAppDispatch } from "core/redux/reduxHooks";
import { getDefaultReferenceDate, getEventsByDate } from "core/services/Utils/calendarUtil";
import moment from "moment";
import { useState, useMemo, useEffect, useRef } from "react";
import { CalendarContext, CalendarModeType } from "../contexts/CalendarContext";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
type RequestBodyType = { referenceDate?: moment.Moment, showFreeSlots: boolean, showReservations: boolean, useRefreshApi?: boolean };

const CalendarContextProvider = ({ children, defaultMode = "month" }: { children: any, defaultMode?: CalendarModeType }) => {
    const [referenceDate, setReferenceDate] = useState<moment.Moment>(getDefaultReferenceDate(defaultMode));
    const [activeDate, setActiveDate] = useState<moment.Moment>(moment());
    const [showFreeSlots, setShowFreeSlots] = useState(true);
    const [showReservations, setShowReservations] = useState(true);
    const [mode, setMode] = useState(defaultMode);
    const events = useAppSelector(selectPrivateCalendarEvents);
    const eventsByDate = useMemo(() => getEventsByDate(events), [events]);
    const dispatch = useAppDispatch();

    useEffect(() => {
        apiFetch({ showFreeSlots, showReservations });
    }, [dispatch]);

    const apiFetch = async (params: RequestBodyType) => {
        const { referenceDate: paramsReferenceDate, showFreeSlots, showReservations, useRefreshApi } = params;
        const currentReference = paramsReferenceDate ?? referenceDate;
        const from = moment(currentReference).startOf('month').utc().add(moment(referenceDate).utcOffset(), 'm');
        const to = moment(currentReference).startOf('month').add(2, 'month').utc();

        const eventsF = useRefreshApi ? refreshPrivateCalendarEvents : setPrivateCalendarEvents;
        const statisticsF = useRefreshApi ? refreshPrivateCalendarStatistics : setPrivateCalendarStatistics;

        await dispatch(statisticsF());
        await dispatch(eventsF({
            appendType: AppendTypeEnum.None,
            request: {
                from: from.format(),
                to: to.format(),
                includeFreeSlots: showFreeSlots,
                includeReservations: showReservations
            }
        }));
    }

    const apiFetchConditional = (params: RequestBodyType) => {
        const referenceStartOfMonth = moment(params.referenceDate).startOf('month');
        const isAlreadyFetched = eventsByDate.some(x => x.date.isSame(referenceStartOfMonth, 'month'));
        if (isAlreadyFetched) return;
        apiFetch(params);
    }

    const handleSetActiveDate = (newDate: moment.Moment) => {
        apiFetchConditional({ referenceDate: newDate, showFreeSlots, showReservations });
        if (!referenceDate.isSame(newDate, mode)) {
            setReferenceDate(moment(newDate).startOf(mode));
        }
        if (!activeDate.isSame(newDate, 'date')) {
            setActiveDate(moment(newDate));
        }
    }

    const handleNextClick = () => {
        handleSetActiveDate(moment(referenceDate).add(1, mode));
    }

    const handlePreviousClick = () => {
        handleSetActiveDate(moment(referenceDate).add(-1, mode));
    }

    const handleSetCurrentDate = () => {
        handleSetActiveDate(moment().startOf('day'));
    }

    const handleNextDate = () => {
        handleSetActiveDate(moment(activeDate).add(1, 'day'));
    }

    const handlePreviousDate = () => {
        handleSetActiveDate(moment(activeDate).add(-1, 'day'));
    }

    const handleFetchNextMonthData = () => {
        const latestEvent = eventsByDate[eventsByDate.length - 1];
        const latestEventMonth = moment(latestEvent.date).startOf('month');
        apiFetch({ referenceDate: latestEventMonth.add(1, 'month'), showFreeSlots, showReservations });
    }

    const refreshData = () => {
        apiFetch({ referenceDate, showFreeSlots, showReservations, useRefreshApi: true });
    }

    const handleShowFreeSlotsClick = () => {
        const newValue = !showFreeSlots;
        setShowFreeSlots(newValue);
        apiFetch({ referenceDate, showFreeSlots: newValue, showReservations });
    }

    const handleShowReservationsClick = () => {
        const newValue = !showReservations;
        setShowReservations(newValue);
        apiFetch({ referenceDate, showFreeSlots, showReservations: newValue });
    }

    const handleSwipeUp = () => {
        const newMode: CalendarModeType = "week";
        if (newMode === mode) return;
        setMode(newMode);
    }

    const handleSwipeDown = () => {
        const newMode: CalendarModeType = "month";
        if (newMode === mode) return;
        setMode(newMode);
    }

    const handleSwipeLeft = () => {
        const newReferenceDate = moment(referenceDate).add(1, mode);
        setReferenceDate(newReferenceDate);
        apiFetchConditional({ referenceDate: newReferenceDate, showFreeSlots, showReservations });
    }

    const handleSwipeRight = () => {
        const newReferenceDate = moment(referenceDate).add(-1, mode);
        setReferenceDate(newReferenceDate);
        apiFetchConditional({ referenceDate: newReferenceDate, showFreeSlots, showReservations });
    }

    return (
        <CalendarContext.Provider value={{
            referenceDate,
            activeDate,
            showFreeSlots,
            showReservations,
            eventsByDate,
            mode,
            refreshData,
            handleNextClick,
            handlePreviousClick,
            handleSetActiveDate,
            handleShowFreeSlotsClick,
            handleShowReservationsClick,
            handleSetCurrentDate,
            handleNextDate,
            handlePreviousDate,
            handleFetchNextMonthData,
            handleSwipeUp,
            handleSwipeDown,
            handleSwipeLeft,
            handleSwipeRight,
            controllersColor: COLORS.PRIMARY
        }}>
            {children}
        </CalendarContext.Provider>
    )
};

export default CalendarContextProvider;