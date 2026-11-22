import moment from "moment";

export const getFormattedDate = (startTime: string | undefined, endTime: string | undefined) => {
    const firstDate = moment(startTime);
    const secondDate = moment(endTime);
    const firstDateFormat = "dddd, MMM D, HH:mm";
    const secondDateFormat = "HH:mm";
    return `${firstDate.format(firstDateFormat)} - ${secondDate.format(secondDateFormat)}`;
}