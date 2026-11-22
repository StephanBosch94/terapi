import { createContext, useEffect, useState, useMemo, useCallback } from "react";
import { useTranslation } from "components/core/TranslationProvider";
import { COLORS } from "../styles/constants/ColorsConstatnts";
import { useAppDispatch, contextFactoryCreate, useAppSelector, useUpdateEffect } from "core/redux/reduxHooks";
import { styled } from "@mui/material/styles";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import ModalsHeader from "./common/ModalsHeader";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { useSnackbar } from "notistack";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import CircularProgress from "components/styles/CircularProgressStyled";
import { createTimeslot, selectCreateAppTimeslotStatus } from "core/redux/timeslotSlice";
import moment from "moment";
import { createRecurrentTimeslot, getRecurrentTimeslots, selectCreateRecurrentTimeslotStatus, selectGetRecurrentTimeslotsStatus, selectRecurrentTimeslots } from "core/redux/recurrentTimeslotSlice";
import { selectPrivateCalendarEvents, selectPrivateCalendarFreeSlots, setPrivateCalendarFreeSlots } from "core/redux/calendarSlice";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import ClockIcon from "assets/clock.svg";
import CalendarIcon from "assets/calendar-blank.svg";
import ArrowDownIcon from "assets/arrow-down.svg";
import CalendarSimpleContextProvider from "components/calendar/providers/CalendarSimpleContextProvider";
import { useCalendarContext } from "components/calendar/contexts/CalendarContext";
import Menu from "components/menu/Menu";
import Calendar from "components/calendar/Calendar";
import { FromToTimeslotType, getConflict, getFromSlots, getToSlots } from "core/services/Utils/calendarUtil";
import { getTimeZoneName } from "core/services/Utils/timeUtil";
import Tooltip from "components/styles/Tooltip";
import { ModalBoxSmallStyled } from "components/styles/boxes/ModalBoxSmallStyled";
import Box from "@mui/material/Box";
import Modal from "@mui/material/Modal";
import Typography from "@mui/material/Typography";
import { ModalStyled } from "components/styles/ModalStyled";

const CenterBoxStyled = styled(Box)({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "8px"
}) as typeof Box;

const CalendarBoxStyled = styled(Box)({
    padding: "16px"
}) as typeof Box;

const CenterBoxRowStyled = styled(Box)({
    width: "100%"
}) as typeof Box;

const ColumnBoxStyled = styled(Box)({
    display: "flex",
    alignItems: "center",
    justifyContent: "start",
    flexFlow: "row noWrap",
    gap: "8px",
    width: "100%"
}) as typeof Box;

const TimepickerBoxStyled = styled(Box)({
    maxHeight: "170px",
    minWidth: "180px",
    display: "flex",
    flexFlow: "column noWrap",
    overflowY: "auto"
}) as typeof Box;

const TimepickerItemBoxStyled = styled(Box)({
    padding: "6px 12px",
    width: "100%",
    ":hover": {
        backgroundColor: COLORS.PRIMARY_LIGHT,
        cursor: "pointer",
    }
}) as typeof Box;

const SelectBoxStyled = styled(Box)({
    display: "flex",
    padding: "10px 12px",
    border: `1px solid ${COLORS.BLACK_10}`,
    flexFlow: "row noWrap",
    borderRadius: "4px",
    width: "100%",
    justifyContent: "space-between",
    gap: "8px",
    flex: "1 1 auto",
    alignItems: "center",
    ":hover": {
        cursor: "pointer",
        border: `1px solid ${COLORS.PRIMARY}`
    }
}) as typeof Box;

const DateTypographyStyled = styled(CaptionTextStyled)({
    color: COLORS.BLACK
}) as typeof Typography;

const PrimaryButtonRectangeSaveStyled = styled(PrimaryButtonRectangle)({
    backgroundColor: COLORS.PRIMARY,
    padding: "12px 24px",
    ":hover": { backgroundColor: COLORS.PRIMARY_HOVER }
}) as typeof PrimaryButtonRectangle;

const PrimaryButtonRectangeNonActiveSaveStyled = styled(PrimaryButtonRectangle)({
    backgroundColor: COLORS.BLACK_10,
    color: COLORS.WHITE,
    padding: "12px 24px",
    ":hover": { backgroundColor: COLORS.BLACK_10 }
}) as typeof PrimaryButtonRectangle;

type TimeslotType = "regular" | "recurrent";
type DayOfWeekId = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7;
type DayOfWeekType = { type: TimeslotType, id: DayOfWeekId, title: string };

type NewTimeslotModalContextType = {
    setOpen: () => void
} | undefined

type InternalComponentContextType = {
    fromTime: moment.Moment,
    toTime: moment.Moment,
    timeslotType: TimeslotType,
    fromSlots: moment.Moment[],
    toSlots: moment.Moment[],
    conflict: FromToTimeslotType | undefined,
    setTimeslotType: (newType: TimeslotType) => void,
    setFromTime: (newTime: moment.Moment) => void,
    setToTime: (newTime: moment.Moment) => void,
    handleSaveClick: () => Promise<void>
} | undefined

const DaysOfWeek: DayOfWeekType[] = [
    {
        type: "regular",
        id: 0,
        title: TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.NEW_AVAILABILITY.NON_RECURRENT
    },
    {
        type: "recurrent",
        id: 1,
        title: TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.NEW_AVAILABILITY.RECURRENT_DAYS.WEEKLY_ON_MONDAY
    },
    {
        type: "recurrent",
        id: 2,
        title: TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.NEW_AVAILABILITY.RECURRENT_DAYS.WEEKLY_ON_TUESDAY
    },
    {
        type: "recurrent",
        id: 3,
        title: TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.NEW_AVAILABILITY.RECURRENT_DAYS.WEEKLY_ON_WEDNESDAY
    },
    {
        type: "recurrent",
        id: 4,
        title: TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.NEW_AVAILABILITY.RECURRENT_DAYS.WEEKLY_ON_THURSDAY
    },
    {
        type: "recurrent",
        id: 5,
        title: TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.NEW_AVAILABILITY.RECURRENT_DAYS.WEEKLY_ON_FRIDAY
    },
    {
        type: "recurrent",
        id: 6,
        title: TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.NEW_AVAILABILITY.RECURRENT_DAYS.WEEKLY_ON_SATURDAY
    },
    {
        type: "recurrent",
        id: 7,
        title: TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.NEW_AVAILABILITY.RECURRENT_DAYS.WEEKLY_ON_SUNDAY
    }
]

const getRepetingOption = (referenceDate: moment.Moment, timeslotType: TimeslotType) => {
    const regularOption = DaysOfWeek[0];
    const recurrentOption = DaysOfWeek[referenceDate.isoWeekday()];
    return timeslotType === "regular"
        ? regularOption
        : recurrentOption;
}

const getAlternativeRepetingOption = (referenceDate: moment.Moment, timeslotType: TimeslotType) => {
    const regularOption = DaysOfWeek[0];
    const recurrentOption = DaysOfWeek[referenceDate.isoWeekday()];

    return timeslotType === "regular"
        ? recurrentOption
        : regularOption;
}

const NewTimeslotModalContext = createContext<NewTimeslotModalContextType>(undefined);
const InternalComponentContext = createContext<InternalComponentContextType>(undefined);

export const useNewTimeslotModalContext = contextFactoryCreate('NewTimeslotModal', NewTimeslotModalContext);
const userInternalComponentContext = contextFactoryCreate('InternalComponent', InternalComponentContext);

const IternalComponentContextProvider = ({ children }: { children: any }) => {
    const recurrentTimeslots = useAppSelector(selectRecurrentTimeslots);
    const dispatch = useAppDispatch();
    const [fromTime, setFromTime] = useState(moment().startOf('day').add(7, 'hour'));
    const [toTime, setToTime] = useState(moment().startOf('day').add(15, 'hour'));
    const [timeslotType, setTimeslotType] = useState<TimeslotType>("regular");
    const freeSlots = useAppSelector(selectPrivateCalendarFreeSlots);
    const fromSlots = useMemo(() => getFromSlots(freeSlots?.data), [freeSlots]);
    const toSlots = useMemo(() => getToSlots(fromTime, freeSlots?.data), [freeSlots, fromTime]);
    const conflict = useMemo(() => {
        if (timeslotType === "regular") return undefined;
        return getConflict(fromTime, toTime, recurrentTimeslots)
    }, [timeslotType, recurrentTimeslots, fromTime, toTime]);
    const { activeDate } = useCalendarContext();
    const events = useAppSelector(selectPrivateCalendarEvents);

    // Sync free slots based on selected date
    useEffect(() => {
        dispatch(setPrivateCalendarFreeSlots({
            day: moment(activeDate).startOf('day').utc().format(),
            timezone: getTimeZoneName()
        }))
        setFromTime(moment(activeDate).startOf('day').add(fromTime.hours(), 'hours').add(fromTime.minutes(), 'minutes'));
        setToTime(moment(activeDate).startOf('day').add(toTime.hours(), 'hours').add(toTime.minutes(), 'minutes'));
    }, [activeDate, events]);

    // Recurrent events conflict prevention
    useEffect(() => {
        if (timeslotType === "regular") return;
        dispatch(getRecurrentTimeslots({
            date: activeDate.utc().format(),
            timezone: getTimeZoneName()
        }))
    }, [timeslotType, activeDate]);

    // Sync default fromTime based on selected date
    useEffect(() => {
        if (!fromSlots) return;
        if (!fromSlots.length) return;
        const preferredTime = fromSlots.find(x => x.hour() > 6);
        const newFromTime = preferredTime ?? fromSlots[0];
        setFromTime(newFromTime);
    }, [fromSlots]);

    // Sync default toTime based on selected date
    useEffect(() => {
        if (!fromTime) return;
        const preferredTime = toSlots.find(x => x.hour() > 15);
        const newFromTime = preferredTime ?? moment(fromTime).add(30, 'minutes');
        setToTime(newFromTime);
    }, [fromTime, toSlots]);

    const handleSetFromTime = (newDate: moment.Moment) => {
        setFromTime(newDate);
        if (newDate.isAfter(toTime)) {
            setToTime(moment(newDate).add(30, 'minutes'));
        }
    }

    const handleSetToTime = (newDate: moment.Moment) => {
        setToTime(newDate);
    }

    const handleSetTimeslotType = (newType: TimeslotType) => {
        if (timeslotType === newType) return;
        setTimeslotType(newType);
    }

    const handleCreateTimeslot = async () => {
        await dispatch(createTimeslot({
            ianaTimezone: getTimeZoneName(),
            fromLocalYear: fromTime.year(),
            fromLocalMonth: fromTime.month() + 1, // zero-based
            fromLocalDay: fromTime.date(),
            fromLocalHour: fromTime.hour(),
            fromLocalMinute: fromTime.minute(),
            durationInMinutes: Math.round(toTime.diff(fromTime, 'minutes'))
        }))
    }

    const handleCreateRecurrentTimeslot = async () => {
        await dispatch(createRecurrentTimeslot({
            ianaTimezone: getTimeZoneName(),
            localIsoWeekDay: fromTime.isoWeekday(),
            fromLocalHour: fromTime.hour(),
            fromLocalMinute: fromTime.minute(),
            durationInMinutes: Math.round(toTime.diff(fromTime, 'minutes'))
        }));
    }

    const handleSaveClick = (): Promise<void> => {
        return timeslotType === "regular"
            ? handleCreateTimeslot()
            : handleCreateRecurrentTimeslot()
    }

    return (
        <InternalComponentContext.Provider value={{
            fromTime,
            toTime,
            timeslotType,
            fromSlots,
            toSlots,
            conflict,
            setTimeslotType: handleSetTimeslotType,
            setFromTime: handleSetFromTime,
            setToTime: handleSetToTime,
            handleSaveClick
        }}>
            {children}
        </InternalComponentContext.Provider>
    )
}

const DateSelect = () => {
    const { activeDate } = useCalendarContext();

    return (
        <Menu
            content={
                <SelectBoxStyled>
                    <DateTypographyStyled>{activeDate.format("dddd, D MMMM")}</DateTypographyStyled>
                    <CalendarIcon />
                </SelectBoxStyled>
            }
            innerMenu={
                <CalendarBoxStyled>
                    <Calendar />
                </CalendarBoxStyled>
            }
            sx={{
                display: "inline-flex",
                width: "100%"
            }}
        />
    )
}

const LoadingBox = () => {
    return (
        <SelectBoxStyled sx={{ justifyContent: "center", alignItems: "center" }}>
            <CircularProgress />
        </SelectBoxStyled>
    )
}

const Timepicker = ({ defaultItem, timestamps, onClick }: { defaultItem?: moment.Moment, timestamps: moment.Moment[], onClick: (newTime: moment.Moment) => void }) => {
    const ref = useCallback((node: HTMLElement | null) => {
        if (node !== null) {
            if (!defaultItem) return;
            if (!node.children.length) return;
            const childrenHeight = node.children[0].scrollHeight;
            const defaultItemIdx = timestamps.findIndex(x => x.isSame(defaultItem, 'minutes'));
            node.scrollTo({
                top: childrenHeight * defaultItemIdx
            })
        }
    }, []);

    return (
        <TimepickerBoxStyled ref={ref}>
            {timestamps.map((x, idx) => (
                <TimepickerItemBoxStyled key={idx} onClick={() => onClick(x)}>
                    <DateTypographyStyled>{x.format("HH:mm")}</DateTypographyStyled>
                </TimepickerItemBoxStyled>
            ))}
        </TimepickerBoxStyled>
    )
}

const TimeSelect = ({ time, slots, onClick }: { time: moment.Moment, slots: moment.Moment[], onClick: (newTime: moment.Moment) => void }) => {

    return (
        <>
            {slots && slots.length ?
                (
                    <Menu
                        content={
                            <SelectBoxStyled>
                                <DateTypographyStyled>{time.local().format("HH:mm")}</DateTypographyStyled>
                                <ClockIcon />
                            </SelectBoxStyled>
                        }
                        innerMenu={<Timepicker defaultItem={time} timestamps={slots} onClick={onClick} />}
                        autoClose={true}
                        sx={{ width: "100%" }}
                    />

                ) : <></>
            }
            {!slots || !slots.length ? <LoadingBox /> : <></>}
        </>
    )
}

const FromTimeSelect = () => {
    const { fromSlots, fromTime, setFromTime } = userInternalComponentContext();


    return (
        <TimeSelect
            time={fromTime}
            slots={fromSlots}
            onClick={setFromTime}
        />
    )
}

const ToTimeSelect = () => {
    const { toSlots, toTime, setToTime } = userInternalComponentContext();

    return (
        <TimeSelect
            time={toTime}
            slots={toSlots}
            onClick={setToTime}
        />
    )
}

const TimeslotTypeMenu = () => {
    const { activeDate } = useCalendarContext();
    const { timeslotType, setTimeslotType } = userInternalComponentContext();
    const repeatingOption = useMemo(() => getAlternativeRepetingOption(activeDate, timeslotType), [activeDate]);
    const { t } = useTranslation();

    const handleSetTimeslotType = () => {
        setTimeslotType(repeatingOption.type);
    }

    return (
        <TimepickerBoxStyled onClick={handleSetTimeslotType}>
            <TimepickerItemBoxStyled>
                <DateTypographyStyled>{t(repeatingOption.title)}</DateTypographyStyled>
            </TimepickerItemBoxStyled>
        </TimepickerBoxStyled>
    )
}

const TimeslotTypeSelect = () => {
    const { activeDate } = useCalendarContext();
    const { timeslotType } = userInternalComponentContext();
    const repeatingOption = useMemo(() => getRepetingOption(activeDate, timeslotType), [timeslotType, activeDate]);
    const { t } = useTranslation();

    return (
        <Menu
            content={
                <SelectBoxStyled sx={{ flex: "0 0 auto" }}>
                    <DateTypographyStyled>{t(repeatingOption.title)}</DateTypographyStyled>
                    <ArrowDownIcon />
                </SelectBoxStyled>
            }
            innerMenu={<TimeslotTypeMenu />}
            autoClose={true}
            sameWidths={true}
        />
    )
}

const SaveButton = ({ onSubmit, handleClose }: { onSubmit?: () => Promise<void> | void, handleClose: () => void }) => {
    const createRecurrentTimeslotStatus = useAppSelector(selectCreateRecurrentTimeslotStatus);
    const createTimeslotStatus = useAppSelector(selectCreateAppTimeslotStatus);
    const recurrentTimeslotsStatus = useAppSelector(selectGetRecurrentTimeslotsStatus);
    const { conflict, handleSaveClick } = userInternalComponentContext();
    const { t } = useTranslation();

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    });

    const handleSave = () => {
        const callback = async () => {
            await handleSaveClick();
            if (onSubmit) {
                onSubmit();
            }
            handleClose();
        }
        callback();
    }

    const handleKeydown = (e: KeyboardEvent) => {
        if (e.repeat) {
            return;
        }
        if (e.key === 'Enter') {
            handleSave();
        }
    }

    return (
        <>
            {!conflict ? (
                <PrimaryButtonRectangeSaveStyled onClick={handleSave}>
                    {(createTimeslotStatus !== LoadingStatusEnum.Loading && createRecurrentTimeslotStatus !== LoadingStatusEnum.Loading && recurrentTimeslotsStatus !== LoadingStatusEnum.Loading) ? t(TRANSLATION_KEYS.COMMON.SAVE) : <></>}
                    {(createTimeslotStatus === LoadingStatusEnum.Loading || createRecurrentTimeslotStatus === LoadingStatusEnum.Loading || recurrentTimeslotsStatus === LoadingStatusEnum.Loading) ? <CircularProgress /> : <></>}
                </PrimaryButtonRectangeSaveStyled>
            ) : <></>}
            {conflict ? (
                <Tooltip text={<div dangerouslySetInnerHTML={{ __html: t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.NEW_AVAILABILITY.COLISION, { from: conflict.from.format("HH:mm"), to: conflict.to.format("HH:mm") }) }} />}>
                    <PrimaryButtonRectangeNonActiveSaveStyled>{t(TRANSLATION_KEYS.COMMON.SAVE)}</PrimaryButtonRectangeNonActiveSaveStyled>
                </Tooltip>
            ) : <></>}
        </>
    )
}

const NewTimeslotModalProvider = ({ onSubmit, children }: { onSubmit?: () => Promise<void> | void, children: any }) => {
    const [open, setOpen] = useState(false);

    const { enqueueSnackbar } = useSnackbar();
    const { t } = useTranslation();

    const createRecurrentTimeslotStatus = useAppSelector(selectCreateRecurrentTimeslotStatus);
    const createTimeslotStatus = useAppSelector(selectCreateAppTimeslotStatus);


    useUpdateEffect(() => {
        snackbarSync(createRecurrentTimeslotStatus);
    }, [createRecurrentTimeslotStatus]);

    useUpdateEffect(() => {
        snackbarSync(createTimeslotStatus);
    }, [createTimeslotStatus]);

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    });

    const snackbarSync = (loadingStatus: LoadingStatusEnum | undefined) => {
        if (loadingStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.NEW_AVAILABILITY.SUCCESS), { variant: "success" });
        }
        else if (loadingStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.NEW_AVAILABILITY.ERROR), { variant: "error" });
        }
    }

    const handleClose = () => {
        setOpen(false);
    }

    const handleOpen = () => {
        setOpen(true);
    }


    const handleKeydown = (e: KeyboardEvent) => {
        if (e.repeat) {
            return;
        }
        if (!open) return;
    }

    return (
        <NewTimeslotModalContext.Provider value={{
            setOpen: handleOpen
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <ModalBoxSmallStyled>
                    <ModalsHeader text={t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.MODALS.NEW_AVAILABILITY.TITLE)} handleClose={handleClose} />
                    <CenterBoxStyled>
                        <CenterBoxRowStyled>
                            <DateSelect />
                        </CenterBoxRowStyled>
                        <ColumnBoxStyled>
                            <FromTimeSelect />
                            <Box sx={{ display: "inline-flex", alignItems: "center" }}>-</Box>
                            <ToTimeSelect />
                        </ColumnBoxStyled>
                        <CenterBoxRowStyled>
                            <TimeslotTypeSelect />
                        </CenterBoxRowStyled>
                    </CenterBoxStyled>
                    <SaveButton onSubmit={onSubmit} handleClose={handleClose} />
                </ModalBoxSmallStyled>
            </ModalStyled>
        </NewTimeslotModalContext.Provider>
    )
}

const NewTimeslotModalProviderWithContexts = ({ onSubmit, children }: { onSubmit?: () => Promise<void> | void, children: any }) => {

    return (
        <CalendarSimpleContextProvider>
            <IternalComponentContextProvider>
                <NewTimeslotModalProvider onSubmit={onSubmit} >
                    {children}
                </NewTimeslotModalProvider>
            </IternalComponentContextProvider>
        </CalendarSimpleContextProvider>
    )
}

export default NewTimeslotModalProviderWithContexts;