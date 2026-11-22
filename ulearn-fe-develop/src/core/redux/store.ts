import {configureStore} from "@reduxjs/toolkit";
import thunk from 'redux-thunk'
import hashReducer from "core/redux/hashSlice";
import codetableReducer from "core/redux/codetableSlice";
import authReducer from "core/redux/authSlice";
import userReducer from "core/redux/userSlice";
import errorReducer from "core/redux/errorSlice";
import userStatisticsReducer from "core/redux/userStatisticsSlice";
import conversationReducer from "core/redux/conversationSlice";
import messageReducer from "core/redux/messageSlice";
import reservationReducer from "core/redux/reservationSlice";
import roomActivityReducer from "core/redux/roomActivitySlice";
import roomReducer from "core/redux/roomSlice";
import userRoomStatisticReducer from "core/redux/userRoomStatisticSlice";
import userRecommendationReducer from "core/redux/userRecommendationSlice";
import timeslotReducer from "core/redux/timeslotSlice";
import recurrentTimeslotReducer from "core/redux/recurrentTimeslotSlice";
import recurrentTimeslotBlacklistReducer from "core/redux/recurrentTimeslotBlacklistSlice";
import calendarReducer from "core/redux/calendarSlice";
import availabilityCalendarReducer from "core/redux/availabilityCalendarSlice";
import registrationReducer from "core/redux/registrationSlice";
import reservationReviewReducer from "core/redux/reservationReviewSlice";
import userReviewReducer from "core/redux/userReviewSlice";
import reservationIssueReducer from "core/redux/reservationIssueSlice";
import reservationPackageReducer from "core/redux/reservationPackageSlice";
import userWalletReducer from "core/redux/userWalletSlice";
import transactionReducer from "core/redux/transactionSlice";
import notificationsReducer from "core/redux/notificationSlice";
import userBehaviourReducer from "core/redux/userBehaviourSlice";
import {queryErrorLogger} from "./middleware/queryErrorLogger";

const reduxLogger = require('redux-logger')
const logger = reduxLogger.createLogger();

export const store = configureStore({
    reducer: {
        hash: hashReducer,
        codetable: codetableReducer,
        auth: authReducer,
        user: userReducer,
        error: errorReducer,
        userStatistics: userStatisticsReducer,
        conversation: conversationReducer,
        message: messageReducer,
        reservation: reservationReducer,
        roomActivity: roomActivityReducer,
        room: roomReducer,
        userRoomStatistic: userRoomStatisticReducer,
        timeslot: timeslotReducer,
        recurrentTimeslot: recurrentTimeslotReducer,
        recurrentTimeslotBlacklist: recurrentTimeslotBlacklistReducer,
        calendar: calendarReducer,
        availabilityCalendar: availabilityCalendarReducer,
        registration: registrationReducer,
        userRecommendation: userRecommendationReducer,
        reservationReview: reservationReviewReducer,
        userReview: userReviewReducer,
        reservationIssue: reservationIssueReducer,
        reservationPackage: reservationPackageReducer,
        userWallet: userWalletReducer,
        transaction: transactionReducer,
        notifications: notificationsReducer,
        userBehaviour: userBehaviourReducer
    },
    middleware: getDefaultMiddleware =>
        getDefaultMiddleware()
            .concat(thunk)
            .concat(logger)
            .concat(queryErrorLogger)
});


export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch