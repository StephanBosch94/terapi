import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { useAppDispatch, useAppSelector, contextFactoryCreate } from "core/redux/reduxHooks";
import moment from "moment";
import { useTranslation } from "components/core/TranslationProvider";
import LeftIcon from 'assets/arrow-left.svg';
import InfoIcon from 'assets/info.svg';
import RightIcon from 'assets/arrow-right.svg';
import React, { useEffect, useState, useMemo, createContext, memo, useCallback, useContext } from "react";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { PublicCalendarEventsResponse, selectPublicCalendarEvents } from "core/redux/calendarSlice";
import { setPublicCalendarEvents, selectGetPublicCalendarEventsStatus } from "core/redux/calendarSlice";
import { ClassDurationEnum } from "core/constants/enums/ClassDurationEnum";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { customSleep, isDispatchFulfilled } from "core/services/Utils/apiUtil";
import { selectDuration, selectNumberOfLessons, selectReferenceDate, selectReservationStartTimes, setRerefenceDate, setReservationStartTimes } from "core/redux/availabilityCalendarSlice";
import { useReservationModalContext } from "../../modals/ReservationModalProvider";
import CircularProgress from "components/styles/CircularProgressStyled";
import { UIContext } from "components/core/UIContextProvider";
import AuthModalProvider, { useAuthModalContext } from "components/modals/AuthModalProvider";
import { selectIsAuthorized } from "core/redux/authSlice";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Body1Styled from "components/styles/typography/Body1Styled";
import PStyled from "components/styles/typography/PStyled";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";

const CELL_HEIGHT = 32;
const CELL_BORDER_STYLE = `1px solid ${COLORS.BLACK_10}`;

const getDefaultHeight = () => {
    return CELL_HEIGHT * 8 * 2;
}

const MainBoxStyled = styled(Box)(() => sx({
    display: "flex",
    width: "100%",
    height: "100%",
    overflow: "auto",
    flexFlow: "column noWrap",
    userSelect: "none",
    WebkitUserSelect: "none",
    gap: "12px",
    "::-webkit-scrollbar": {
        display: "none"
    },
    ".available.selected.hovered": {
        backgroundColor: COLORS.PRIMARY_HOVER
    },
    ".selected.hovered": {
        backgroundColor: COLORS.PRIMARY_HOVER
    },
    ".available.selected": {
        backgroundColor: COLORS.PRIMARY
    },
    ".selected": {
        backgroundColor: COLORS.PRIMARY
    },
    ".available": {
        backgroundColor: COLORS.GREEN
    },
    ".hovered": {
        backgroundColor: COLORS.GREEN_20
    }
})) as typeof Box;

const SelectDateBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    justifyContent: "space-between",
    alignItems: "center",
    minWidth: "240px",
    gap: "4px",
    color: COLORS.BLACK_60,
})) as typeof Box;

const CalendarBoxStyled = styled(Box)(() => sx({
    width: "100%",
    display: "flex",
    flexFlow: "column noWrap",
    flex: "1 1 auto",
    overflow: "auto",
    position: "relative"
})) as typeof Box;

const CalendarScrollableBoxStyled = styled(Box)(() => sx({
    overflowY: "auto",
    scrollbarWidth: "none",
    display: "flex",
    flexFlow: "column noWrap",
    flex: "1 1 auto",
    width: "100%",
    "::-webkit-scrollbar": {
        display: "none"
    }
})) as typeof Box;

const ClickableBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    alignItems: "center",
    gap: "8px",
    ".passive": {
        border: `1px solid ${COLORS.BLACK_10} !important`,
        color: `${COLORS.BLACK_10} !important`,
        backgroundColor: "transparent !important",
        cursor: "default"
    }
})) as typeof Box;

const SelectDateTypographyStyled = styled(CaptionTextStyled)(() => sx({
    fontWeight: 400,
    color: COLORS.BLACK,
    textAlign: "center",
})) as typeof CaptionTextStyled;

const SectionBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    flex: "0 1 auto",
    justifyContent: "space-between",
    width: "100%",
    flexDirection: { xs: "column", md: "row" },
    gap: "16px"
})) as typeof Box;

const SectionTitleStyled = styled(PStyled)(() => sx({
    fontWeight: 600,
    display: { xs: "none", md: "flex" },
    color: COLORS.BLACK
})) as typeof PStyled;

const DateTypographyStyled = styled(CaptionTextStyled)(() => sx({
    color: COLORS.BLACK,
    fontWeight: "600",
    textAlign: "center",
    width: "100%"
})) as typeof CaptionTextStyled;

const DateBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    flexFlow: "row noWrap",
    justifyContent: "center",
    margin: "0 auto",
    padding: "4px",
    width: "26px"
})) as typeof Box;

const CurrentDateBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    flexFlow: "row noWrap",
    margin: "0 auto",
    justifyContent: "center",
    backgroundColor: COLORS.PRIMARY,
    borderRadius: "50%",
    padding: "4px",
    width: "26px"
})) as typeof Box;

const CurrentDateTypographyStyled = styled(CaptionTextStyled)(() => sx({
    color: COLORS.WHITE,
    fontWeight: "600",
    textAlign: "center"
})) as typeof CaptionTextStyled;

const DayOfWeekTypographyStyled = styled(CaptionTextStyled)(() => sx({
    textAlign: "center",
    width: "100%"
})) as typeof CaptionTextStyled;

const LeftCellBoxStyled = styled(Box)(() => sx({
    height: CELL_HEIGHT,
    padding: "4px 0 0 0",
    textAlign: "center",
    borderLeft: CELL_BORDER_STYLE
})) as typeof Box;

const TopLeftEmptyCellBoxStyled = styled(Box)(() => sx({
    borderLeft: CELL_BORDER_STYLE,
    borderTop: CELL_BORDER_STYLE,
    width: "100%",
    height: "100%"
})) as typeof Box;

const GridCalendarColumnStyled = styled(Grid)(() => sx({
    position: "relative"
})) as typeof Grid;

const HeaderCellBoxStyled = styled(Box)(() => sx({
    width: "100%",
    display: "flex",
    flexFlow: "column noWrap",
    justifyContent: "center",
    alignItems: "space-between",
    padding: "8px 0 8px 0",
    borderLeft: CELL_BORDER_STYLE,
    borderTop: CELL_BORDER_STYLE
})) as typeof Box;

const TimestampTypographyStyled = styled(CaptionTextStyled)(() => sx({
    color: COLORS.BLACK,
})) as typeof CaptionTextStyled;

const TooltipTypographyStyled = styled(Body1Styled)(() => sx({
    colors: COLORS.BLACK,
    textAlign: "center"
})) as typeof Body1Styled;

const TooltipBoxStyled = styled(Box)(() => sx({
    position: "absolute",
    display: "inline-flex",
    alignItems: "center",
    justifyContent: "center",
    padding: "8px",
    background: COLORS.WHITE,
    boxShadow: `2px 3px 6px rgba(0, 0, 0, 0.15), -1px -1px 6px rgba(0, 0, 0, 0.08)`,
    borderRadius: "4px",
    zIndex: 50
})) as typeof Box;

const IconBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    padding: "4px",
    border: `1px solid ${COLORS.BLACK_10}`,
    borderRadius: "50%",
    color: COLORS.PRIMARY,
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_5,
        border: `1px solid ${COLORS.PRIMARY}`,
    }
})) as typeof Box;


type TimestampType = { time: moment.Moment, isCurrentTime: boolean, isVisible: boolean, topBorderStyle: BorderStyle, hasBottomBorder: boolean };
type DateType = { time: moment.Moment, isCurrent: boolean };
type BorderStyle = "solid" | "dashed";
type AvailabilityCellType = { idx: number, startTime: moment.Moment, isAvailable: boolean, isNextAvailable: boolean, topBorderStyle: BorderStyle, hasRightBorder: boolean, hasBottomBorder: boolean }
type TooltipType = { text: { date: string, timeInterval: string }, position: { top: number, left: number } };
type CalendarDisplayModeType = "regular" | "fullscreen";

type AvailabilityContextType = {
    handleNextWeek: () => void,
    handlePreviousWeek: () => void,
    handleSelection: (cell: AvailabilityCellType) => void,
    isSelected: (cell: AvailabilityCellType) => boolean,
    hasTopBorder: (cell: AvailabilityCellType) => boolean,
    selectedIndexes: number[],
    prevButtonActive: boolean,
    nextButtonActive: boolean,
    dates: DateType[],
    timestamps: TimestampType[],
    cells: AvailabilityCellType[][] | undefined,
    startOfWeek: moment.Moment,
    showSelectionWarning: boolean
} | undefined;

type AvailabilityStaticContextType = {
    displayMode: CalendarDisplayModeType,
    scrollableBoxHeightCss: string
} | undefined;

type AvailabilityColumnContextType = {
    handleMouseEnter: (cell: AvailabilityCellType) => void,
    handleMouseLeave: () => void,
    isHovered: (cell: AvailabilityCellType) => boolean,
    tooltip: TooltipType | undefined
} | undefined;

const getAllTimestampsForLeftTableHeading = (): TimestampType[] => {
    const start = moment().startOf('day').add(COMMON_CONSTANTS.CALENDAR.MIN_POSSIBLE_HOUR, 'hours');
    const end = moment().startOf('day').add(COMMON_CONSTANTS.CALENDAR.MAX_POSSIBLE_HOUR, 'hours');
    const diffHours = moment(end).diff(start, 'hours');
    const currentRoundedMoment = moment().startOf('hour');
    const numberOfItems = diffHours * 2;
    return Array(numberOfItems).fill(1)
        .map(((_, idx) => moment(start).add(idx * 30, 'minutes')))
        .map((x, idx) => ({
            time: moment(x),
            isCurrentTime: moment(currentRoundedMoment).isSame(x, 'hour'),
            isVisible: x.minutes() === 0,
            hasBottomBorder: idx === numberOfItems - 1,
            topBorderStyle: idx % 2 === 0 ? "solid" : "dashed"
        }));
}

// Returs dates based on a current time 
// Triggered when the user clicks on the next (or previous) week button
const getDates = (
    referenceDate: moment.Moment,
    take = 7
): DateType[] => {
    const startOfWeek = moment(referenceDate).startOf('week');
    const currentTime = moment();
    return Array(take).fill(0)
        .map((x, idx) => idx)
        .map((weekDayId,) => moment(startOfWeek).add(weekDayId, 'days'))
        .map((x, idx) => ({
            time: x,
            isCurrent: x.isSame(currentTime, 'date')
        }));
}

const isAvailable = (
    current: moment.Moment,
    freeEvents: PublicCalendarEventsResponse | undefined
) => {
    return freeEvents?.data?.find(e => moment(e.date).isSame(current, 'date'))?.events?.some(e => moment(e.fromLocalTime).isSame(current, 'minutes')) ?? false;
}

const getCells = (
    freeEvents: PublicCalendarEventsResponse | undefined,
    dates: DateType[],
    timestamps: TimestampType[]
): AvailabilityCellType[][] => {
    return dates.map((date, dateIdx) =>
        timestamps
            .map((timestamp, timestampIdx) => ({
                time: moment(date.time).add(timestamp.time.hours() * 60 + timestamp.time.minutes(), 'minutes'),
                idx: ((dateIdx * timestamps.length) + timestampIdx)
            }))
            .map((x, idx) => ({
                idx: x.idx,
                startTime: x.time,
                isAvailable: isAvailable(moment(x.time), freeEvents),
                isNextAvailable: isAvailable(moment(x.time).add(30, 'minutes'), freeEvents),
                topBorderStyle: idx % 2 === 0 ? "solid" : "dashed",
                hasRightBorder: dateIdx === dates.length - 1,
                hasBottomBorder: idx === timestamps.length - 1
            }))
    )
}

const getSelectedIndexes = (
    referenceDate: moment.Moment,
    reservationStartTimes: string[]
) => {
    const startOfWeek = moment(referenceDate).startOf('week');
    return reservationStartTimes
        .map((x, idx) => moment(x))
        .filter(x => moment(x).isSame(referenceDate, 'week'))
        .map(x => (moment(x).diff(startOfWeek, 'minutes') / 30));
}

const getReservationStartTimes = (
    referenceDate: moment.Moment,
    selectedIndexes: number[]
) => {
    const startOfWeek = moment(referenceDate).startOf('week');
    return selectedIndexes
        .map((x, idx) => moment(startOfWeek).add(x * 30, 'minutes').format());
}

// Gets selected end indexes for drawing top borders
// When duration is 30 min => it returns an empty array
// When duration is 60 min => it returns an array that contints the second indexes
const getSelectedEndIndexes = (
    duration: ClassDurationEnum,
    selectedIndexes: number[]
) => {
    return (duration === ClassDurationEnum.Full && selectedIndexes)
        ? selectedIndexes.map((x, idx) => ({ x, idx })).filter(x => x.idx % 2 === 1).map((x, idx) => x.x)
        : [];
}

const AvailabilityContext = createContext<AvailabilityContextType>(undefined);
const AvailabilityStaticContext = createContext<AvailabilityStaticContextType>(undefined);
const AvailabilityColumnContext = createContext<AvailabilityColumnContextType>(undefined);

const useAvailabilityContext = contextFactoryCreate('Availability', AvailabilityContext);
const useAvailabilityStaticContext = contextFactoryCreate('AvailabilityStaticContext', AvailabilityStaticContext);
const useAvailabilityColumnContext = contextFactoryCreate('AvailabilityColumnContext', AvailabilityColumnContext);

const AvailabilityContextProvider = ({ children, userId }: { children: any, userId: number }) => {
    const [showSelectionWarning, setShowSelectionWarning] = useState(false);

    const calendarEvents = useAppSelector(selectPublicCalendarEvents);
    const duration = useAppSelector(selectDuration);
    const numberOfLessons = useAppSelector(selectNumberOfLessons);
    const dispatch = useAppDispatch();

    const referenceDate = useAppSelector(selectReferenceDate);
    const reservationStartTimes = useAppSelector(selectReservationStartTimes);
    const timestamps = useMemo(() => getAllTimestampsForLeftTableHeading(), []);
    const selectedIndexes = useMemo(() => getSelectedIndexes(moment(referenceDate), reservationStartTimes), [referenceDate, reservationStartTimes]);
    const selectedEndIndexes = useMemo(() => getSelectedEndIndexes(duration, selectedIndexes), [selectedIndexes]);
    const dates = useMemo(() => getDates(moment(referenceDate)), [referenceDate]);
    const prevActive = useMemo(() => isPrevBtnActive(moment(referenceDate)), [referenceDate]);
    const nextActive = useMemo(() => isNextBtnActive(moment(referenceDate)), [referenceDate]);
    const startOfWeek = useMemo(() => moment(referenceDate).startOf('week'), [referenceDate]);

    const cells = useMemo(() => {
        if (!dates || !timestamps) return undefined;
        const result = getCells(calendarEvents, dates, timestamps);
        return result;
    }, [calendarEvents, dates, timestamps]);

    useEffect(() => {
        handleSetInitialData();
    }, []);

    const handleSetInitialData = async () => {
        const startOfWeek = moment().startOf('isoWeek');

        dispatch(setRerefenceDate(startOfWeek.format()));
        const data = await dispatch(setPublicCalendarEvents({
            userId: userId,
            from: moment(startOfWeek).format(),
            to: moment(startOfWeek).add(1, 'month').add(1, 'hour').format(),
            duration: ClassDurationEnum.Half
        }))

        if (!isDispatchFulfilled(data)) return;
        const result = data.payload as PublicCalendarEventsResponse;
        const firstEventDay = result.data[0];
        if (!firstEventDay) return;

        dispatch(setRerefenceDate(moment(firstEventDay.date).startOf('week').format()));
    }

    const handleNextWeek = () => {
        const newReferenceDate = moment(referenceDate).add(COMMON_CONSTANTS.CALENDAR.DAYS_TO_SHOW, 'days');
        if (!nextActive) {
            return;
        }
        dispatch(setPublicCalendarEvents({
            userId: userId,
            from: moment(newReferenceDate).format(),
            to: moment(newReferenceDate).add(1, 'week').add(1, 'hour').format(),
            duration: ClassDurationEnum.Half
        }))
        dispatch(setRerefenceDate(newReferenceDate.format()));
    }

    const handlePreviousWeek = () => {
        const newReferenceDate = moment(referenceDate).add(-COMMON_CONSTANTS.CALENDAR.DAYS_TO_SHOW, 'days');
        if (!prevActive) {
            return;
        }
        dispatch(setPublicCalendarEvents({
            userId: userId,
            from: moment(newReferenceDate).format(),
            to: moment(newReferenceDate).add(1, 'week').add(1, 'hour').format(),
            duration: ClassDurationEnum.Half
        }))
        dispatch(setRerefenceDate(newReferenceDate.format()));
    }

    const isSelectedIdx = (idx: number) => {
        return selectedIndexes.some((x) => x === idx);
    }

    const isSelected = (cell: AvailabilityCellType) => {
        return isSelectedIdx(cell.idx);
    }

    const hasTopBorder = (cell: AvailabilityCellType) => {
        return !selectedEndIndexes.some(y => cell.idx === y);
    }

    const handleSelectionHalfDuration = (cell: AvailabilityCellType) => {
        if (!cell.isAvailable) return;
        const currentSelected = isSelectedIdx(cell.idx);
        if (!currentSelected && reservationStartTimes.length >= numberOfLessons) {
            return;
        }

        if (currentSelected) {
            const newSelected = selectedIndexes.filter(x => x !== cell.idx);
            const newReservationStartTimes = getReservationStartTimes(moment(referenceDate), newSelected);
            dispatch(setReservationStartTimes(newReservationStartTimes));
            return;
        }
        const newSelected = [...selectedIndexes, cell.idx];
        const newReservationStartTimes = getReservationStartTimes(moment(referenceDate), newSelected);
        dispatch(setReservationStartTimes(newReservationStartTimes));
    }

    const temporarlyShowWarning = async () => {
        setShowSelectionWarning(true);
        await customSleep(2000);
        setShowSelectionWarning(false);
    }

    const areIndexesValid = (indexes: number[]) => {
        if (indexes.length % 2 !== 0) return false;
        const indexesWithPositions = indexes.map((x, idx) => ({ index: x, pos: idx }));
        const startIndexes = indexesWithPositions.filter(x => x.pos % 2 === 0).map((x, idx) => ({ index: x.index, pos: idx }));
        const endIndexes = indexesWithPositions.filter(x => x.pos % 2 === 1).map((x, idx) => ({ index: x.index, pos: idx }));
        return !startIndexes.some(x => x.index + 1 !== endIndexes[x.pos].index);
    }

    const handleSelectionFullDuration = (cell: AvailabilityCellType) => {
        if (!cell.isAvailable) return;
        if (!cell.isNextAvailable) {
            temporarlyShowWarning();
            return;
        };
        const currentSelected = isSelectedIdx(cell.idx);
        const numberOfLessonsSelected = Math.round(reservationStartTimes.length / 2);
        if (!currentSelected && numberOfLessonsSelected >= numberOfLessons) {
            return;
        }
        if (currentSelected !== isSelectedIdx(cell.idx + 1)) {
            temporarlyShowWarning();
            return;
        }
        const indexes = [cell.idx, cell.idx + 1];
        if (currentSelected) {
            const newSelected = selectedIndexes.filter(x => !indexes.some(y => x === y));
            if (!areIndexesValid(newSelected)) return;

            const newReservationStartTimes = getReservationStartTimes(moment(referenceDate), newSelected);
            dispatch(setReservationStartTimes(newReservationStartTimes));
            return;
        }
        const newSelected = [...selectedIndexes, ...indexes];
        const newReservationStartTimes = getReservationStartTimes(moment(referenceDate), newSelected);
        dispatch(setReservationStartTimes(newReservationStartTimes));
    }

    const handleSelection = (cell: AvailabilityCellType) => {
        if (duration === ClassDurationEnum.Half) {
            return handleSelectionHalfDuration(cell);
        }
        return handleSelectionFullDuration(cell);
    }

    return (
        <AvailabilityContext.Provider value={{
            handleNextWeek,
            handlePreviousWeek,
            handleSelection,
            startOfWeek,
            isSelected,
            prevButtonActive: prevActive,
            nextButtonActive: nextActive,
            dates,
            timestamps,
            cells,
            showSelectionWarning,
            selectedIndexes,
            hasTopBorder
        }}>
            {children}
        </AvailabilityContext.Provider>
    )
};

const AvailabilityStaticContextProvider = ({ children, displayMode }: { children: any, displayMode: CalendarDisplayModeType }) => {

    return (
        <AvailabilityStaticContext.Provider value={{
            displayMode,
            scrollableBoxHeightCss: displayMode === "fullscreen" ? "auto" : `${getDefaultHeight()}px`
        }}>
            {children}
        </AvailabilityStaticContext.Provider>
    )
}

const AvailabilityColumnContextProvider = ({ children, startOfWeek, columnIdx, columnWidth }: { children: any, startOfWeek: moment.Moment, columnIdx: number, columnWidth: number }) => {
    const [hoveredIndexes, setHoveredIndexes] = useState<number[]>([]);
    const duration = useAppSelector(selectDuration);

    const getTooltip = () => {
        if (!hoveredIndexes.length) return undefined;
        const startTime = moment(startOfWeek).add((hoveredIndexes[hoveredIndexes.length - 1] - 1) * 30, 'minutes');
        const endTime = duration === ClassDurationEnum.Full ? moment(startTime).add(60, 'minutes') : moment(startTime).add(30, 'minutes');
        const startRowIdx = hoveredIndexes[hoveredIndexes.length - 1] % 48;

        const result: TooltipType = {
            text: {
                date: startTime.format("ddd, MMM DD"),
                timeInterval: `${startTime.format("HH:mm")} - ${endTime.format("HH:mm")}`
            },
            position: {
                top: (startRowIdx - 1) * CELL_HEIGHT,
                left: columnIdx !== 6 ? columnWidth * (columnIdx + 1) : columnWidth * (columnIdx - 1)
            }
        }
        return result;
    }

    const tooltip = useMemo(() => getTooltip(), [hoveredIndexes]);

    const handleMouseEnter = (cell: AvailabilityCellType) => {
        if (!cell.isAvailable) return;
        if (duration === ClassDurationEnum.Full && !cell.isNextAvailable) return;

        if (duration === ClassDurationEnum.Full) {
            setHoveredIndexes([cell.idx, cell.idx + 1]);
            return;
        }
        setHoveredIndexes([cell.idx]);
    }

    const handleMouseLeave = () => {
        if (hoveredIndexes.length === 0) return;
        setHoveredIndexes([]);
    }

    const isHovered = (cell: AvailabilityCellType) => {
        return hoveredIndexes.some(x => x === cell.idx);
    }

    return (
        <AvailabilityColumnContext.Provider value={{
            handleMouseEnter,
            handleMouseLeave,
            isHovered,
            tooltip
        }}>
            {children}
        </AvailabilityColumnContext.Provider>
    )
}

const withCellsAndStartOfWeekFromAvailabilityContext = (Component: any) => {
    const ComponentMemo = memo(Component);

    const WithCellsAndStartOfWeek = () => {
        const { cells, startOfWeek } = useAvailabilityContext();
        return <ComponentMemo cells={cells} startOfWeek={startOfWeek} />
    }
    WithCellsAndStartOfWeek.displayName = `WithCellsAndStartOfWeek(${Component.displayName || Component.name})`;

    return WithCellsAndStartOfWeek;
}

const withDatesFromAvailabilityContext = (Component: any) => {
    const ComponentMemo = memo(Component);

    const WithDatesFromAvailabilityContext = () => {
        const { dates } = useAvailabilityContext();
        return <ComponentMemo dates={dates} />
    }
    WithDatesFromAvailabilityContext.displayName = `WithDatesFromAvailabilityContext(${Component.displayName || Component.name})`;

    return WithDatesFromAvailabilityContext;
}

const withTimestampsFromAvailabilityContext = (Component: any) => {
    const ComponentMemo = memo(Component);

    const WithTimestampsFromAvailabilityContext = () => {
        const { timestamps } = useAvailabilityContext();
        return <ComponentMemo timestamps={timestamps} />
    }
    WithTimestampsFromAvailabilityContext.displayName = `WithTimestampsFromAvailabilityContext(${Component.displayName || Component.name})`;
    return WithTimestampsFromAvailabilityContext;
}

const withShowSelectionWarningFromAvailabilityContext = (Component: any) => {
    const ComponentMemo = memo(Component);

    const WithShowSelectionWarningFromAvailabilityContext = () => {
        const { showSelectionWarning } = useAvailabilityContext();
        return <ComponentMemo showSelectionWarning={showSelectionWarning} />
    }
    WithShowSelectionWarningFromAvailabilityContext.displayName = `WithShowSelectionWarningFromAvailabilityContext(${Component.displayName || Component.name})`;
    return WithShowSelectionWarningFromAvailabilityContext;
}

const isNextBtnActive = (referenceDate: moment.Moment) => {
    const NUMBER_OF_WEEKS_TO_SHOW = 4;
    const current = moment()

    return moment(referenceDate).diff(current, 'week') < NUMBER_OF_WEEKS_TO_SHOW;
}

const isPrevBtnActive = (referenceDate: moment.Moment) => {
    const current = moment().startOf('week');

    return moment(referenceDate).diff(current) > 0;
}

const SelectDateBox = () => {
    const { handleNextWeek, handlePreviousWeek, prevButtonActive, nextButtonActive, dates } = useAvailabilityContext();

    return (
        <SelectDateBoxStyled>
            <ClickableBoxStyled onClick={handlePreviousWeek}>
                <IconBoxStyled className={!prevButtonActive ? "passive" : ""}><LeftIcon /></IconBoxStyled>
            </ClickableBoxStyled>
            <Box sx={{ display: "inline-flex", justifyContent: "center", width: "auto", margin: "0 auto", gap: "8px" }}>
                <SelectDateTypographyStyled>{dates[0].time.format("MMM D, YYYY")}</SelectDateTypographyStyled>
                <SelectDateTypographyStyled>-</SelectDateTypographyStyled>
                <SelectDateTypographyStyled>{dates[dates.length - 1].time.format("MMM D, YYYY")}</SelectDateTypographyStyled>
            </Box>
            <ClickableBoxStyled onClick={handleNextWeek}>
                <IconBoxStyled className={!nextButtonActive ? "passive" : ""}><RightIcon /></IconBoxStyled>
            </ClickableBoxStyled>
        </SelectDateBoxStyled>
    )
}

const TimestampsColumn = ({ timestamps }: { timestamps: TimestampType[] }) => {
    return (
        <Grid container>
            {
                timestamps.map((x, idx) => (
                    <Grid item key={idx} xs={12}>
                        <LeftCellBoxStyled sx={{
                            borderTop: `1px ${x.topBorderStyle} ${COLORS.BLACK_10}`,
                            borderBottom: x.hasBottomBorder ? CELL_BORDER_STYLE : 'none',
                            paddingBottom: x.hasBottomBorder ? "1px" : "0"
                        }}>
                            <TimestampTypographyStyled sx={{
                                fontWeight: x.isCurrentTime ? "600" : "400"
                            }}>
                                {x.isVisible ? x.time.format("HH:mm") : ""}
                            </TimestampTypographyStyled>
                        </LeftCellBoxStyled>
                    </Grid>
                ))
            }
        </Grid>
    )
}
const TimestampsColumnWithTimestamps = withTimestampsFromAvailabilityContext(TimestampsColumn);

const DatesRow = ({ dates }: { dates: DateType[] }) => {
    return (
        <Box sx={{ display: "flex", flexFlow: "column nowrap", flex: "0 1 auto" }}>
            <Grid container columns={15}>
                <Grid item xs={2} sm={1}><TopLeftEmptyCellBoxStyled /></Grid>
                <Grid item xs={13} sm={14}>
                    <Grid container columns={7}>
                        {
                            dates.map((dayOfWeek, idx) => (
                                <GridCalendarColumnStyled item xs={1} key={idx}>
                                    <HeaderCellBoxStyled sx={{
                                        borderRight: idx === dates.length - 1 ? CELL_BORDER_STYLE : 'none'
                                    }}>
                                        {dayOfWeek.isCurrent ? (
                                            <CurrentDateBoxStyled><CurrentDateTypographyStyled>{dayOfWeek.time.format("D")}</CurrentDateTypographyStyled></CurrentDateBoxStyled>
                                        ) : (
                                            <DateBoxStyled><DateTypographyStyled>{dayOfWeek.time.format("D")}</DateTypographyStyled></DateBoxStyled>
                                        )}
                                        <Box><DayOfWeekTypographyStyled>{dayOfWeek.time.format("ddd")}</DayOfWeekTypographyStyled></Box>
                                    </HeaderCellBoxStyled>
                                </GridCalendarColumnStyled>
                            ))
                        }
                    </Grid>
                </Grid>
            </Grid>
        </Box>
    )
}
const DatesRowWithDates = withDatesFromAvailabilityContext(DatesRow);

const CalendarLoadingCircle = () => {
    return (
        <Box sx={{ position: "absolute", top: "calc(50% - 40px)", left: "calc(50% - 40px)" }}>
            <CircularProgress />
        </Box>
    )
}

const CalendarCell = ({ calendarCell }: { calendarCell: AvailabilityCellType }) => {
    const { isHovered, handleMouseEnter, handleMouseLeave } = useAvailabilityColumnContext();
    const { isSelected, hasTopBorder, handleSelection } = useAvailabilityContext();
    const { handleOpen: handleReservationModalOpen } = useReservationModalContext();
    const { displayMode } = useAvailabilityStaticContext();

    const onClick = () => {
        if (displayMode === "fullscreen") {
            handleSelection(calendarCell);
            return;
        }

        handleReservationModalOpen();
    }

    const onMouseEnter = () => {
        if (displayMode === "regular") return;
        handleMouseEnter(calendarCell)
    }

    const onMouseLeave = () => {
        if (displayMode === "regular") return;
        handleMouseLeave();
    }

    return (
        <div
            onClick={onClick}
            onMouseEnter={onMouseEnter}
            onMouseLeave={onMouseLeave}
            style={{
                flexBasis: "100%",
                flexGrow: "0",
                maxWidth: "100%",
                height: CELL_HEIGHT,
                backgroundColor: COLORS.BLACK_2,
                borderLeft: CELL_BORDER_STYLE,
                borderTop: hasTopBorder(calendarCell) ? `1px ${calendarCell.topBorderStyle} ${COLORS.BLACK_10}` : 'none',
                borderBottom: calendarCell.hasBottomBorder ? CELL_BORDER_STYLE : 'none',
                borderRight: calendarCell.hasRightBorder ? CELL_BORDER_STYLE : 'none',
                paddingRight: calendarCell.hasRightBorder ? "1px" : "0",
                paddingBottom: calendarCell.hasBottomBorder ? "1px" : "0"
            }}>
            <div
                className={[
                    calendarCell.isAvailable ? "available" : "",
                    isHovered(calendarCell) ? "hovered" : "",
                    isSelected(calendarCell) ? "selected" : ""
                ].filter(x => x !== "").join(" ")}
                style={{
                    width: "100%",
                    height: "100%",
                    cursor: isHovered(calendarCell) ? "pointer" : "regular"
                }} />
        </div>
    )
}

const HoveredTimeTooltip = () => {
    const { tooltip } = useAvailabilityColumnContext();

    return tooltip ? (
        <TooltipBoxStyled top={tooltip.position.top} left={tooltip.position.left} >
            <Box sx={{ width: "100%", display: "flex", flexFlow: "column noWrap" }}>
                <TooltipTypographyStyled>{tooltip.text.date}</TooltipTypographyStyled>
                <TooltipTypographyStyled>{tooltip.text.timeInterval}</TooltipTypographyStyled>
            </Box>
        </TooltipBoxStyled>
    ) : <></>
}

const CalendarCells = ({ cells, startOfWeek }: { cells: AvailabilityCellType[][], startOfWeek: moment.Moment }) => {
    const [columnWidth, setColumnWidth] = useState(0);
    const { displayMode } = useAvailabilityStaticContext();
    const { screenType } = useContext(UIContext);
    const columnRefCallback = useCallback((node: HTMLElement | null) => {
        if (node !== null) {
            setColumnWidth(node.clientWidth);
        }
    }, []);

    return cells ? (
        <>
            {cells.map((x, xIdx) => (
                <Grid item key={xIdx} xs={1} ref={columnRefCallback}>
                    <AvailabilityColumnContextProvider startOfWeek={startOfWeek} columnIdx={xIdx} columnWidth={columnWidth}>
                        {displayMode === "fullscreen" && screenType === "desktop" ? <HoveredTimeTooltip /> : <></>}
                        <Grid container >
                            {x.map((y, yIdx) => (
                                <CalendarCell key={y.idx} calendarCell={y} />
                            ))}
                        </Grid>
                    </AvailabilityColumnContextProvider>
                </Grid>
            ))}
        </>
    ) : <></>
}
const CalendarCellsWithCells = withCellsAndStartOfWeekFromAvailabilityContext(CalendarCells);

const SelectionWarningTooltip = ({ showSelectionWarning }: { showSelectionWarning: boolean }) => {
    const { t } = useTranslation();

    return showSelectionWarning ? (
        <TooltipBoxStyled top="30px" left="calc(50% - 165px)" width="330px" sx={{ alignItems: "flex-start", gap: "8px" }}>
            <Box sx={{ height: "100%", display: "inline-flex", alignItems: "flex-start", padding: "2px 0", color: COLORS.PRIMARY }}>
                <InfoIcon />
            </Box>
            <Box>
                <TooltipTypographyStyled sx={{ textAlign: "left" }}>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.AVAILABILITY_CALENDAR.INVALID_SELECTION_WARNING)}</TooltipTypographyStyled>
            </Box>
        </TooltipBoxStyled>
    ) : (<></>);
}
const SelectionWarningTooltipWithSelectionWarning = withShowSelectionWarningFromAvailabilityContext(SelectionWarningTooltip);

const AvailabilityCalendar = () => {
    const calendarEventsStatus = useAppSelector(selectGetPublicCalendarEventsStatus);
    const calendarEvents = useAppSelector(selectPublicCalendarEvents);
    const { scrollableBoxHeightCss } = useAvailabilityStaticContext();
    const { displayMode } = useAvailabilityStaticContext();
    const { t } = useTranslation();
    const { screenType } = React.useContext(UIContext);

    const scrollableRefCallback = useCallback((node: HTMLElement | null) => {
        if (node !== null && calendarEvents) {
            const firstEvent = calendarEvents?.data?.[0]?.events?.[0]?.fromLocalTime;
            const fromHour = firstEvent ? moment(firstEvent).startOf('hour') : moment().startOf('hour');
            const hourOffset = fromHour.hour() * 2 * CELL_HEIGHT;
            node.scrollTop = hourOffset;
        }
    }, [calendarEvents]);

    return (
        <MainBoxStyled>
            <SectionBoxStyled>
                {screenType === "desktop" ? <SectionTitleStyled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.AVAILABILITY)}</SectionTitleStyled> : <></>}
                <SelectDateBox />
            </SectionBoxStyled>
            <CalendarBoxStyled sx={{
                ":hover": {
                    cursor: displayMode === "regular" ? "pointer" : "regular"
                }
            }}>
                <SelectionWarningTooltipWithSelectionWarning />
                <DatesRowWithDates />
                <CalendarScrollableBoxStyled ref={scrollableRefCallback} height={scrollableBoxHeightCss}>
                    <Grid container columns={15} width="100%">
                        <Grid item xs={2} sm={1}>
                            <TimestampsColumnWithTimestamps />
                        </Grid>
                        <Grid item xs={13} sm={14}>
                            <Grid container columns={7} position="relative">
                                {calendarEventsStatus === LoadingStatusEnum.Loading ? <CalendarLoadingCircle /> : <></>}
                                <CalendarCellsWithCells />
                            </Grid>
                        </Grid>
                    </Grid>
                </CalendarScrollableBoxStyled>
            </CalendarBoxStyled>
        </MainBoxStyled>
    )
}


const AvailabilityCalendarRaw = ({ userId, displayMode }: { userId: number, displayMode: CalendarDisplayModeType }) => {
    return (
        <AvailabilityStaticContextProvider displayMode={displayMode}>
            <AvailabilityContextProvider userId={userId}>
                <AvailabilityCalendar />
            </AvailabilityContextProvider>
        </AvailabilityStaticContextProvider>
    )
}

export default AvailabilityCalendarRaw;