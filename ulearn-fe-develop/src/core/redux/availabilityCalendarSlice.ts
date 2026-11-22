import { createSelector, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RootState } from "./store";
import moment from "moment";
import { ClassDurationEnum } from "core/constants/enums/ClassDurationEnum";

export type NumberOfLessonsType = 1 | 5 | 10;
export type PackageType = "trial" | "standard" | "kids";
type AvailabilityCalendarStateType = {
    referenceDate: string,
    reservationStartTimes: string[],
    duration: ClassDurationEnum,
    numberOfLessons: NumberOfLessonsType,
    packageType: PackageType
}

const initialState: AvailabilityCalendarStateType = {
    referenceDate: moment().startOf('isoWeek').format(),
    reservationStartTimes: [],
    duration: ClassDurationEnum.Full,
    numberOfLessons: 1,
    packageType: "trial"
};

export const availabilityCalendarSlice = createSlice({
    name: "availabilityCalendar",
    initialState,
    reducers: {
        setPackageType(state, action: PayloadAction<PackageType>) {
            state.packageType = action.payload;
        },
        setRerefenceDate(state, action: PayloadAction<string>) {
            state.referenceDate = action.payload;
        },
        setReservationStartTimes(state, action: PayloadAction<string[]>) {
            const reference = moment(state.referenceDate);
            const notThisWeekEvents = state.reservationStartTimes
                .map((x, idx) => moment(x)).filter(x => !x.isSame(reference, 'week'))
                .map((x, idx) => x.format());

            state.reservationStartTimes = [...notThisWeekEvents, ...action.payload];
        },
        setDuration(state, action: PayloadAction<ClassDurationEnum>) {
            state.reservationStartTimes = []; // reset indexes on duration change
            state.duration = action.payload;
        },
        setNumberOfLessons(state, action: PayloadAction<NumberOfLessonsType>) {
            if (action.payload < state.numberOfLessons) {
                state.reservationStartTimes = [];
            }
            state.numberOfLessons = action.payload;
        },
        resetAvailabilityCalendarData(state) {
            state.duration = initialState.duration;
            state.numberOfLessons = initialState.numberOfLessons;
            state.reservationStartTimes = initialState.reservationStartTimes;
            state.referenceDate = initialState.referenceDate;
            state.packageType = initialState.packageType;
        }
    }
});

export const selectReferenceDate = (state: RootState) => state.availabilityCalendar.referenceDate;
export const selectReservationStartTimes = (state: RootState) => state.availabilityCalendar.reservationStartTimes;
export const selectDuration = (state: RootState) => state.availabilityCalendar.duration;
export const selectNumberOfLessons = (state: RootState) => state.availabilityCalendar.numberOfLessons;
export const selectPackageType = (state: RootState) => state.availabilityCalendar.packageType;
export const selectNumberOfSelectedEvents = createSelector(selectDuration, selectReservationStartTimes, (duration, reservationStateTimes) =>
    duration === ClassDurationEnum.Full ? Math.round(reservationStateTimes.length / 2) : reservationStateTimes.length
)

export const { setPackageType, setRerefenceDate, setReservationStartTimes, setDuration, setNumberOfLessons, resetAvailabilityCalendarData } = availabilityCalendarSlice.actions;
export default availabilityCalendarSlice.reducer;