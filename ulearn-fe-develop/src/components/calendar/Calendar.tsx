import { styled } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { selectGetPrivateCalendarEventsStatus, selectPrivateCalendarEvents } from "core/redux/calendarSlice";
import { useAppSelector } from "core/redux/reduxHooks";
import { DayView, generateCalendarMonthViewData } from "core/services/Utils/calendarUtil";
import moment from "moment";
import { useMemo } from "react";
import { useCalendarContext } from "./contexts/CalendarContext";
import ArrowLeftIcon from "assets/arrow-left.svg";
import ArrowRightIcon from "assets/arrow-right.svg";
import { ArrowBoxStyled } from "./common/styles/ArrowBoxStyled";
import CalendarFreeSlotDot from "./common/CalendarFreeSlotDot";
import CalendarResevationDot from "./common/CalendarResevationDot";
import Box from "@mui/material/Box";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import H5Styled from "components/styles/typography/H5Styled";
import Body1Styled from "components/styles/typography/Body1Styled";

const CalendarDatesBoxStyled = styled(Box)({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "8px",
    width: "100%",
    ".row": {
        display: "flex",
        flexFlow: "row noWrap",
        width: "100%",
        gap: "12px",
        color: COLORS.BLACK_40,
    },
    ".itemParent": {
        display: "inline-flex",
        flex: "1 1 auto",
        justifyContent: "center",
        alignItems: "center",
        width: "32px",
        height: "32px",
        position: "relative",
    },
    ".dateLetterItem": {
        display: "flex",
        width: "32px",
        height: "32px",
        justifyContent: "center",
        alignItems: "center",
        borderRadius: "50%",
        color: COLORS.BLACK
    },
    ".item": {
        display: "flex",
        width: "32px",
        height: "32px",
        justifyContent: "center",
        alignItems: "center",
        borderRadius: "50%",
        color: COLORS.BLACK_10,
        textAlign: "center"
    },
    ".item:hover": {
        backgroundColor: COLORS.PRIMARY_5,
        border: `1px solid ${COLORS.PRIMARY}`,
        cursor: "pointer",
        color: COLORS.BLACK
    },
    ".item p": {
        fontWeight: "400",
        fontSize: "0.875rem",
        lineHeight: "24px"
    },
    ".dotBox": {
        position: "absolute",
        left: "0",
        bottom: "2px",
        width: "100%",
        display: "inline-flex",
        justifyContent: "center",
        gap: "1px"
    },
    ".currentDay": {
        backgroundColor: `${COLORS.PRIMARY_10} !important`,
        color: `${COLORS.BLACK} !important`
    },
    ".active": {
        backgroundColor: `${COLORS.PRIMARY} !important`,
        color: `${COLORS.WHITE} !important`,
    },
    ".currentMonth": {
        color: COLORS.BLACK
    },
    ".currentMonth.active:hover": {
        backgroundColor: COLORS.PRIMARY,
        color: COLORS.WHITE
    }
}) as typeof Box;

const CalendarControllersBoxStyled = styled(Box)({
    display: "flex",
    flexFlow: "row noWrap",
    gap: "4px"
}) as typeof Box;

const MonthTitle = () => {
    const { referenceDate } = useCalendarContext();

    return (
        <Box>
            <H5Styled sx={{color:COLORS.BLACK}}>{referenceDate.format("MMMM YYYY")}</H5Styled>
        </Box>
    )
}

const CalendarWeekDays = () => {
    const calendarWeekDays = useMemo(() => moment.weekdays(true), []);

    return (
        <div className="row">
            {
                calendarWeekDays.map((x, idx) => (
                    <div key={idx} className="itemParent">
                        <div className="dateLetterItem">
                            <Body1Styled sx={{color:COLORS.BLACK}}>{x.slice(0, 1).toUpperCase()}</Body1Styled>
                        </div>
                    </div>
                ))
            }
        </div>
    )
}

const CalendarDots = ({ dayView }: { dayView: DayView }) => {
    return (
        <div className="dotBox">
            {dayView.hasFreeTimeslot ? <CalendarFreeSlotDot width="4px" height="4px" /> : <></>}
            {dayView.hasReservation ? <CalendarResevationDot width="4px" height="4px" /> : <></>}
        </div>
    )
}

const CalendarDate = ({ dayView }: { dayView: DayView }) => {
    const { handleSetActiveDate } = useCalendarContext();

    return (
        <div className="itemParent">
            <div
                onClick={() => handleSetActiveDate(dayView.date)}
                className={[
                    "item",
                    dayView.isCurrent ? "currentDay" : "",
                    dayView.inMonth ? "currentMonth" : "",
                    dayView.isActive ? "active" : ""
                ].filter(x => x !== "").join(" ")}
            >
                <p>{dayView.date.date()}</p>
                {!dayView.isActive ? <CalendarDots dayView={dayView} /> : <></>}
            </div>
        </div>
    )
}

const CalendarDatesLoading = () => (
    <Box sx={{ display: "flex", height: "208px", width: "100%", alignItems: "center", justifyContent: "center" }}>
        <CircularProgressCustom color={COLORS.PRIMARY}/>
    </Box>
)

const CalendarDates = () => {
    const { mode, referenceDate, activeDate } = useCalendarContext();
    const privateCalendarEvents = useAppSelector(selectPrivateCalendarEvents);
    const calendarViewData = useMemo(() => generateCalendarMonthViewData(mode, referenceDate, activeDate, privateCalendarEvents), [mode, privateCalendarEvents, activeDate, referenceDate]);
    const loadingStatus = useAppSelector(selectGetPrivateCalendarEventsStatus);

    return (
        <>
            {loadingStatus === LoadingStatusEnum.Loading ? <CalendarDatesLoading /> : <></>}
            {loadingStatus === LoadingStatusEnum.Loaded ? calendarViewData.map((x, colIdx) => (
                <div className="row" key={colIdx}>
                    {x.map((y, _) => <CalendarDate key={y.idx} dayView={y} />)}
                </div>
            )) : <></>}
        </>
    )
}

const CalendarMonthDatesControllers = () => {
    const { handlePreviousClick, handleNextClick, controllersColor } = useCalendarContext();

    return (
        <CalendarControllersBoxStyled sx={{ color: controllersColor }}>
            <ArrowBoxStyled onClick={handlePreviousClick}><ArrowLeftIcon /></ArrowBoxStyled>
            <ArrowBoxStyled onClick={handleNextClick}><ArrowRightIcon /></ArrowBoxStyled>
        </CalendarControllersBoxStyled>
    )
}

const Calendar = () => {
    return (
        <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "16px", userSelect: "none" }}>
            <Box sx={{ width: "100%", display: "flex", flexFlow: "row noWrap", justifyContent: "space-between", alignItems: "center" }}>
                <MonthTitle />
                <CalendarMonthDatesControllers />
            </Box>

            <CalendarDatesBoxStyled>
                <CalendarWeekDays />
                <CalendarDates />
            </CalendarDatesBoxStyled>
        </Box>
    )
}

export const CalendarMobile = () => {

    return (
        <CalendarDatesBoxStyled>
            <CalendarWeekDays />
            <CalendarDates />
        </CalendarDatesBoxStyled>
    )
}

export default Calendar;