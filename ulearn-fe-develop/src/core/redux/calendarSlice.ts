import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { components, paths } from "apiTypes";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { ErrorResponse, PagedVMRequest, PagedVMResponse } from "core/services/Types/AppTypes";
import { RootState } from "./store";
import CalendarService from "core/services/CalendarService";
import { isTimeslot, joinResponses } from "core/services/Utils/calendarUtil";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";

export type PrivateCalendarEventsResponse = paths['/api/calendar']['get']['responses']['200']['content']['application/json'];
export type PrivateCalendarEventsRequest = paths['/api/calendar']['get']['parameters']['query'];
export type PrivateCalendarFreeSlotsResponse = paths['/api/calendar/free-slot']['get']['responses']['200']['content']['application/json'];
export type PrivateCalendarFreeSlotsRequest = paths['/api/calendar/free-slot']['get']['parameters']['query'];
export type PublicCalendarEventsResponse = paths['/api/calendar/user/{userId}']['get']['responses']['200']['content']['application/json'];
export type PublicCalendarEventsRequest = paths['/api/calendar/user/{userId}']['get']['parameters']['query'];
export type PrivateCalendarStatisticsResponse = paths['/api/calendar/statistics']['get']['responses']['200']['content']['application/json'];

export type PrivateCalendarReservationType = components["schemas"]["CalendarReservationVM"];
export type PrivateCalendarRecurrentTimeslotType = components["schemas"]["CalendarRecurrentTimeslotVM"];
export type PrivateCalendarTimeslotType = components["schemas"]["CalendarTimeslotVM"];
export type PrivateCalendarEventBaseType = components["schemas"]["CalendarBaseEventVM"];

interface ICalendarState {
    privateCalendarEvents?: PrivateCalendarEventsResponse | undefined,
    getPrivateCalendarEventsStatus?: LoadingStatusEnum | undefined,

    privateCalendarFreeSlots?: PrivateCalendarFreeSlotsResponse | undefined,
    getPrivateCalendarFreeSlotsStatus?: LoadingStatusEnum | undefined,

    publicCalendarEvents?: PublicCalendarEventsResponse | undefined,
    getPublicCalendarEventsStatus?: LoadingStatusEnum | undefined,

    privateCalendarStatistics?: PrivateCalendarStatisticsResponse | undefined,
    privateCalendarStatisticsStatus?: LoadingStatusEnum | undefined,
}

const initialState: ICalendarState = {};

export const setPrivateCalendarEvents = createAsyncThunk<PagedVMResponse<PrivateCalendarEventsResponse>, PagedVMRequest<PrivateCalendarEventsRequest>>(
    "calendar/setPrivateCalendarEvents", async (requestBody, { rejectWithValue }) => {
        try {
            const response = await CalendarService.getPrivateEvents(requestBody.request);
            return {
                appendType: requestBody.appendType,
                response: response.data
            };
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const refreshPrivateCalendarEvents = createAsyncThunk<PagedVMResponse<PrivateCalendarEventsResponse>, PagedVMRequest<PrivateCalendarEventsRequest>>(
    "calendar/refreshPrivateCalendarEvents", async (requestBody, { rejectWithValue }) => {
        try {
            const response = await CalendarService.getPrivateEvents(requestBody.request);
            return {
                appendType: requestBody.appendType,
                response: response.data
            };
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setPrivateCalendarFreeSlots = createAsyncThunk<PrivateCalendarFreeSlotsResponse, PrivateCalendarFreeSlotsRequest>(
    "calendar/setPrivateCalendarFreeSlots", async (requestBody, { rejectWithValue }) => {
        try {
            const response = await CalendarService.getFreeSlots(requestBody);
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setPublicCalendarEvents = createAsyncThunk<PublicCalendarEventsResponse, PublicCalendarEventsRequest & { userId: number }>(
    "calendar/setPublicCalendarEvents", async (requestBody, { rejectWithValue }) => {
        try {
            const response = await CalendarService.getPublicEvents(requestBody);
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setPrivateCalendarStatistics = createAsyncThunk<PrivateCalendarStatisticsResponse, void>(
    "calendar/setPrivateCalendarStatistics", async (requestBody, { rejectWithValue }) => {
        try {
            const response = await CalendarService.getPrivateCalendarStatistics();
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const refreshPrivateCalendarStatistics = createAsyncThunk<PrivateCalendarStatisticsResponse, void>(
    "calendar/refreshPrivateCalendarStatistics", async (requestBody, { rejectWithValue }) => {
        try {
            const response = await CalendarService.getPrivateCalendarStatistics();
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const calendarSlice = createSlice({
    name: "calendar",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(setPrivateCalendarEvents.pending, (state) => {
            state.getPrivateCalendarEventsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setPrivateCalendarEvents.fulfilled, (state, action) => {
            state.getPrivateCalendarEventsStatus = LoadingStatusEnum.Loaded;
            const payload = action.payload as PagedVMResponse<PrivateCalendarEventsResponse>;
            state.privateCalendarEvents = payload.appendType !== AppendTypeEnum.None
                ? joinResponses(state.privateCalendarEvents, payload.response)
                : payload.response;
        });
        builder.addCase(setPrivateCalendarEvents.rejected, (state, action) => {
            state.getPrivateCalendarEventsStatus = LoadingStatusEnum.Error;
        });
        //-------------------------------------------------
        builder.addCase(refreshPrivateCalendarEvents.fulfilled, (state, action) => {
            state.getPrivateCalendarEventsStatus = LoadingStatusEnum.Loaded;
            const payload = action.payload as PagedVMResponse<PrivateCalendarEventsResponse>;
            state.privateCalendarEvents = payload.appendType !== AppendTypeEnum.None
                ? joinResponses(state.privateCalendarEvents, payload.response)
                : payload.response;
        });
        //-------------------------------------------------
        builder.addCase(setPrivateCalendarFreeSlots.pending, (state) => {
            state.getPrivateCalendarFreeSlotsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setPrivateCalendarFreeSlots.fulfilled, (state, action) => {
            state.getPrivateCalendarFreeSlotsStatus = LoadingStatusEnum.Loaded;
            state.privateCalendarFreeSlots = action.payload;
        });
        builder.addCase(setPrivateCalendarFreeSlots.rejected, (state, action) => {
            state.getPrivateCalendarFreeSlotsStatus = LoadingStatusEnum.Error;
        });
        //-------------------------------------------------
        builder.addCase(setPublicCalendarEvents.pending, (state) => {
            state.getPublicCalendarEventsStatus = LoadingStatusEnum.Loading;
            state.publicCalendarEvents = undefined;
        });
        builder.addCase(setPublicCalendarEvents.fulfilled, (state, action) => {
            state.getPublicCalendarEventsStatus = LoadingStatusEnum.Loaded;
            state.publicCalendarEvents = action.payload;
        });
        builder.addCase(setPublicCalendarEvents.rejected, (state, action) => {
            state.getPublicCalendarEventsStatus = LoadingStatusEnum.Error;
        });
        //-------------------------------------------------
        builder.addCase(setPrivateCalendarStatistics.pending, (state) => {
            state.privateCalendarStatisticsStatus = LoadingStatusEnum.Loading;
            state.privateCalendarStatistics = undefined;
        });
        builder.addCase(setPrivateCalendarStatistics.fulfilled, (state, action) => {
            state.privateCalendarStatisticsStatus = LoadingStatusEnum.Loaded;
            state.privateCalendarStatistics = action.payload;
        });
        builder.addCase(setPrivateCalendarStatistics.rejected, (state, action) => {
            state.privateCalendarStatisticsStatus = LoadingStatusEnum.Error;
        });
        //-------------------------------------------------
        builder.addCase(refreshPrivateCalendarStatistics.fulfilled, (state, action) => {
            state.privateCalendarStatisticsStatus = LoadingStatusEnum.Loaded;
            state.privateCalendarStatistics = action.payload;
        });
    }
});

export const selectGetPrivateCalendarEventsStatus = (state: RootState) => state.calendar.getPrivateCalendarEventsStatus;
export const selectPrivateCalendarEvents = (state: RootState) => state.calendar.privateCalendarEvents;

export const selectGetPrivateCalendarFreeSlotsStatus = (state: RootState) => state.calendar.getPrivateCalendarFreeSlotsStatus;
export const selectPrivateCalendarFreeSlots = (state: RootState) => state.calendar.privateCalendarFreeSlots;

export const selectGetPublicCalendarEventsStatus = (state: RootState) => state.calendar.getPublicCalendarEventsStatus;
export const selectPublicCalendarEvents = (state: RootState) => state.calendar.publicCalendarEvents;

export const selectPrivateCalendarStatistics = (state: RootState) => state.calendar.privateCalendarStatistics;
export const selectPrivateCalendarStatisticsStatus = (state: RootState) => state.calendar.privateCalendarStatisticsStatus;

export default calendarSlice.reducer;