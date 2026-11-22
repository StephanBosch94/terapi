import { PrivateCalendarEventBaseType, PrivateCalendarEventsResponse, PrivateCalendarFreeSlotsResponse, PrivateCalendarRecurrentTimeslotType, PrivateCalendarReservationType, PrivateCalendarTimeslotType } from 'core/redux/calendarSlice';
import moment from 'moment';
import { PRIVATE_CALENDAR_EVENT_TYPES } from 'core/constants/calendarEventTypesConstants';
import { getJoinedPagedVM } from './paginationUtil';
import { AppendTypeEnum } from 'core/constants/enums/AppendTypeEnum';
import { GetRecurrentTimeslotsResponse } from 'core/redux/recurrentTimeslotSlice';
import { CalendarModeType } from 'components/calendar/contexts/CalendarContext';

export type DayView = {
    idx: number;
    date: moment.Moment;
    inMonth: boolean;
    isCurrent: boolean;
    isActive: boolean;
    hasReservation: boolean;
    hasFreeTimeslot: boolean;
}

export type FromToTimeslotType = {
    from: moment.Moment,
    to: moment.Moment
}

type CalendarMonthView = DayView[][];

export type MomentEventType = {
    from: moment.Moment,
    to: moment.Moment
}

type EventType = {
    date: moment.Moment,
    hasFreeSlot: boolean,
    hasReservation: boolean
}

export type DateEventsType = {
    date: moment.Moment,
    events: PrivateCalendarEventsResponse["data"][0]["events"]
}

const hasReservation = (date: moment.Moment, events: EventType[]) => {
    return events.find(x => date.isSame(x.date, 'date'))?.hasReservation ?? false;
}

const hasFreeSlot = (date: moment.Moment, events: EventType[]) => {
    return events.find(x => date.isSame(x.date, 'date'))?.hasFreeSlot ?? false;
}

const getEventTypesByDay = (events: PrivateCalendarEventsResponse | undefined): EventType[] => {
    return (
        events?.data
            ?.filter((x) => x.events?.length)
            ?.map((x, idx) => ({
                date: moment(x.date),
                hasFreeSlot: x.events?.some(y => y.type === PRIVATE_CALENDAR_EVENT_TYPES.TIMESLOT || y.type === PRIVATE_CALENDAR_EVENT_TYPES.RECURRENT_TIMESLOT) ?? false,
                hasReservation: x.events?.some(y => y.type === PRIVATE_CALENDAR_EVENT_TYPES.RESERVATION) ?? false
            }))
    ) ?? [] as EventType[];
}

export const getEventsByDate = (events: PrivateCalendarEventsResponse | undefined): DateEventsType[] => {
    return (
        events?.data
            ?.map((x, idx) => ({
                date: moment(x.date),
                events: x.events
            }))
    ) ?? [] as DateEventsType[];
}

export const filterEventsByDate = (dateEvents: DateEventsType[], activeDate: moment.Moment) => {
    return dateEvents.filter(x => x.date.diff(activeDate, 'day') >= 0);
}

export const generateCalendarMonthViewData = (mode: CalendarModeType, referenceDate: moment.Moment, activeDate: moment.Moment, events: PrivateCalendarEventsResponse | undefined): CalendarMonthView => {
    const startDate = moment(referenceDate);
    const current = moment();
    const firstWeekStart = moment(startDate).startOf('week');
    const numRows = mode === "month" ? 6 : 1;
    const numberOfDaysInWeek = 7;
    const eventsByDay = getEventTypesByDay(events);

    return Array(numRows).fill(0)
        .map((x, idx) => moment(firstWeekStart).add(idx, 'week'))
        .map((x, idx) => Array(numberOfDaysInWeek).fill(0)
            .map((_, idx) => moment(x).add(idx, 'days'))
            .map((y, idx2) => ({
                idx: idx * numRows + idx2,
                date: y,
                inMonth: y.month() === referenceDate.month(),
                isCurrent: y.isSame(current, 'date'),
                isActive: y.isSame(activeDate, 'date'),
                hasReservation: y.month() === referenceDate.month() && hasReservation(y, eventsByDay),
                hasFreeTimeslot: y.month() === referenceDate.month() && hasFreeSlot(y, eventsByDay)
            })));
}


export const isReservation = (calendarEvent: PrivateCalendarEventBaseType): calendarEvent is PrivateCalendarReservationType => {
    return calendarEvent.type === PRIVATE_CALENDAR_EVENT_TYPES.RESERVATION;
}

export const isRecurrentTimeslot = (calendarEvent: PrivateCalendarEventBaseType): calendarEvent is PrivateCalendarRecurrentTimeslotType => {
    return calendarEvent.type === PRIVATE_CALENDAR_EVENT_TYPES.RECURRENT_TIMESLOT;
}

export const isTimeslot = (calendarEvent: PrivateCalendarEventBaseType): calendarEvent is PrivateCalendarTimeslotType => {
    return calendarEvent.type === PRIVATE_CALENDAR_EVENT_TYPES.TIMESLOT;
}

export const joinResponses = (prevData: PrivateCalendarEventsResponse | undefined, newData: PrivateCalendarEventsResponse) => {
    if (prevData === undefined || !prevData.data.length) {
        return newData;
    }
    if (newData === undefined || !newData.data.length) {
        return prevData;
    }
    const newDataEventDate = moment(newData.data[0].date);
    if (prevData.data.some(x => moment(x.date).isSame(newDataEventDate, 'date'))) {
        return prevData;
    }
    const latestPrevDataDate = moment(prevData.data[prevData.data.length - 1].date);
    if (newDataEventDate.isAfter(latestPrevDataDate)) {
        return getJoinedPagedVM(prevData, newData, AppendTypeEnum.End);
    }
    return getJoinedPagedVM(prevData, newData, AppendTypeEnum.Start);
}

export const getFreeSlots = (slots: PrivateCalendarFreeSlotsResponse["data"] | undefined): FromToTimeslotType[] => {
    return slots?.map((x) => ({
        from: moment(x.fromLocalTime),
        to: moment(x.toLocalTime)
    })) ?? [];
}

export const getFromSlots = (slots: PrivateCalendarFreeSlotsResponse["data"] | undefined): moment.Moment[] => {
    return getFreeSlots(slots)
        .map((x) => x.from);
}

export const getToSlots = (from: moment.Moment, slots: PrivateCalendarFreeSlotsResponse["data"] | undefined): moment.Moment[] => {
    return getFreeSlots(slots)
        .map((x) => x.to)
        .filter((x) => x.isAfter(from))
        .reduce((acc, curr) =>
            !acc.length || curr.diff(acc[acc.length - 1], "minutes") <= 30
                ? [...acc, curr]
                : acc,
            [] as moment.Moment[]
        );
};

const isSame = (interval1Start: moment.Moment, interval1End: moment.Moment, interval2Start: moment.Moment, interval2End: moment.Moment) => {
    return interval1Start.isSame(interval2Start, 'minute') && interval1End.isSame(interval2End, 'minute');
}

const hasInterception = (interval1Start: moment.Moment, interval1End: moment.Moment, interval2Start: moment.Moment, interval2End: moment.Moment) => {
    return interval1Start.isBetween(interval2Start, interval2End) ||
        interval1End.isBetween(interval2Start, interval2End) ||
        interval2Start.isBetween(interval1Start, interval1End) ||
        interval2End.isBetween(interval1Start, interval1End)
}

export const getConflict = (from: moment.Moment, to: moment.Moment, recurrentTimeslots: GetRecurrentTimeslotsResponse | undefined): FromToTimeslotType | undefined => {
    if (!recurrentTimeslots) return undefined;

    return recurrentTimeslots
        .map(x => ({ from: moment(x.fromTime), to: moment(x.toTime) }))
        .find(x => hasInterception(from, to, x.from, x.to) || isSame(from, to, x.from, x.to));
}

export const getDefaultReferenceDate = (defaultMode: CalendarModeType) => {
    return defaultMode === "month" ? moment().startOf('month') : moment().startOf('week');
}