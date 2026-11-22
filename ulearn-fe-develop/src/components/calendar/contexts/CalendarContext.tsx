import { contextFactoryCreate } from "core/redux/reduxHooks";
import { DateEventsType } from "core/services/Utils/calendarUtil";
import { createContext } from "react";

export type CalendarModeType = "month" | "week";

type CalendarContextType = {
    referenceDate: moment.Moment,
    activeDate: moment.Moment,
    showFreeSlots: boolean,
    showReservations: boolean,
    eventsByDate: DateEventsType[],
    controllersColor: string,
    mode: CalendarModeType,
    refreshData: () => void,
    handleNextClick: () => void,
    handlePreviousClick: () => void,
    handleSetActiveDate: (newDate: moment.Moment) => void,
    handleShowReservationsClick: () => void,
    handleShowFreeSlotsClick: () => void,
    handleSetCurrentDate: () => void,
    handleNextDate: () => void,
    handlePreviousDate: () => void,
    handleFetchNextMonthData: () => void,
    handleSwipeLeft: () => void,
    handleSwipeRight: () => void,
    handleSwipeDown: () => void,
    handleSwipeUp: () => void
} | undefined;

export const CalendarContext = createContext<CalendarContextType>(undefined);
export const useCalendarContext = contextFactoryCreate('Calendar', CalendarContext);