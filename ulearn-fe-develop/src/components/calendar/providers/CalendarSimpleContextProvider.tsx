import moment from "moment";
import { useState } from "react";
import { CalendarContext } from "../contexts/CalendarContext";
import { COLORS } from "components/styles/constants/ColorsConstatnts";

const CalendarSimpleContextProvider = ({ children }: { children: any }) => {
    const [referenceDate, setReferenceDate] = useState<moment.Moment>(moment().startOf('month'));
    const [activeDate, setActiveDate] = useState<moment.Moment>(moment());
    const handleNextClick = () => {
        setReferenceDate(prevDate => moment(prevDate).add(1, 'month'));
    }

    const handlePreviousClick = () => {
        setReferenceDate(prevDate => moment(prevDate).add(-1, 'month'));
    }

    const handleSetActiveDate = (newDate: moment.Moment) => {
        if (!referenceDate.isSame(newDate, 'month')) {
            setReferenceDate(moment(newDate).startOf('month'));
        }
        if (!activeDate.isSame(newDate, 'date')) {
            setActiveDate(moment(newDate));
        }
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

    return (
        <CalendarContext.Provider value={{
            referenceDate,
            activeDate,
            showFreeSlots: false,
            showReservations: false,
            mode: "month",
            eventsByDate: [],
            refreshData: () => { },
            handleNextClick,
            handlePreviousClick,
            handleSetActiveDate,
            handleShowFreeSlotsClick: () => { },
            handleShowReservationsClick: () => { },
            handleSetCurrentDate,
            handleNextDate,
            handlePreviousDate,
            handleFetchNextMonthData: () => { },
            handleSwipeLeft: () => { },
            handleSwipeRight: () => { },
            handleSwipeUp: () => { },
            handleSwipeDown: () => { },
            controllersColor: COLORS.BLACK
        }}>
            {children}
        </CalendarContext.Provider>
    )
};

export default CalendarSimpleContextProvider;