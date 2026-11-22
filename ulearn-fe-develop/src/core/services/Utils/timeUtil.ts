import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { Moment } from "moment";

export const getTimeZoneName = () => {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
}

export const getTimeZoneOffset = (timeZone = 'UTC') => {
    const date = new Date()
    const utcDate = new Date(date.toLocaleString('en-US', { timeZone: 'UTC' }));
    const tzDate = new Date(date.toLocaleString('en-US', { timeZone }));
    return (tzDate.getTime() - utcDate.getTime()) / 6e4;
}

export const getCurrentOffset = () => {
    return (new Date()).getTimezoneOffset();
}

export const getTotalMinutesFromTimestampId = (timestampId: number) => {
    return (timestampId - 1) * COMMON_CONSTANTS.TIMESTAMP_DURATION_MIN; //-1 since timeslots start from 1
}

export const dayEquals = (day1: Moment, day2: Moment) => {
    const dayFormat = "YYYY-MM-DD"
    return day1.format(dayFormat) === day2.format(dayFormat);
}


export const getDiffTimeInM = (startTime: Moment, currentTime: Moment) => {
    return Math.abs(currentTime.diff(startTime, 'minutes'));
}

export const getFromToTime = (startTime: Moment, currentTime: Moment) => {
    return currentTime.isAfter(startTime)
        ? currentTime.from(startTime)
        : currentTime.to(startTime);
}