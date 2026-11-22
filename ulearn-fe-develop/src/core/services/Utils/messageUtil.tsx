import { AppMessageVM } from "../Types/AppTypes";
import { MessageStateEnum } from "core/constants/enums/MessageStateEnum";
import moment, { Moment } from "moment";
import { dayEquals, getDiffTimeInM } from "./timeUtil";

export const getGrouppedMessagesByDay = (messages: AppMessageVM[]) => {
    return messages.reduce((acc, curr) => !acc.length || !dayEquals(acc[acc.length - 1].date, moment(curr.created))
        ? [...acc, { date: moment(curr.created), messages: [curr] }]
        : [...acc.slice(0, acc.length - 1), { date: moment(curr.created), messages: [...acc[acc.length - 1].messages, curr] }]
        , [] as { date: Moment, messages: AppMessageVM[] }[])
}

export const getGrouppedMessagesByUserAndTime = (messages: AppMessageVM[], intervalInMinutes = 5) => {
    return messages.reduce((acc, curr) => !acc.length || acc[acc.length - 1].senderId !== curr.senderId! || getDiffTimeInM(acc[acc.length - 1].date, moment(curr.created)) > intervalInMinutes
        ? [...acc, { date: moment(curr.created), senderId: curr.senderId!, messages: [curr] }]
        : [...acc.slice(0, acc.length - 1), { date: acc[acc.length - 1].date, senderId: acc[acc.length - 1].senderId, messages: [...acc[acc.length - 1].messages, curr] }]
        , [] as { senderId: number, date: Moment, messages: AppMessageVM[] }[])
}

export const getUnreadMessages = (messages: AppMessageVM[], currentUserId: number): AppMessageVM[] => {
    return messages.filter(x => x.receiverId === currentUserId && x.messageStateId === MessageStateEnum.Created);
}

export const hasUnreadMessages = (messages: AppMessageVM[], currentUserId: number): boolean => {
    return getUnreadMessages(messages, currentUserId).length > 0;
}

export const calculateContentHeight = (element: HTMLElement, lineHeight: number) => {
    const originalHeight = element.style.height;
    const scrollHeight = element.scrollHeight;
    const offsetHeight = element.offsetHeight;

    if (offsetHeight < scrollHeight) {
        return scrollHeight;
    }
    element.style.height = `${lineHeight}px`;
    const realHeight = element.scrollHeight;
    element.style.height = originalHeight;

    return realHeight;
}

export const calculateHeight = (element: HTMLElement, lineHeight: number) => {
    const taHeight = calculateContentHeight(element, lineHeight);
    return Math.ceil(taHeight ?? 0 / lineHeight);
};