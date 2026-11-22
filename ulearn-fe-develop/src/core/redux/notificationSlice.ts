import { PayloadAction, createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { paths } from "apiTypes";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { ErrorResponse, PagedVMRequest, PagedVMResponse } from "core/services/Types/AppTypes";
import { RootState } from "./store";
import { getJoinedPagedVM } from "core/services/Utils/paginationUtil";
import AppNotificationService from "core/services/AppNotificationService";
import moment from "moment";


export type NotificationFilterRequest = paths['/api/notification/filter']['post']['requestBody']['content']['application/json'];
export type NotificationFilterResponse = paths['/api/notification/filter']['post']['responses']['200']['content']['application/json'];

export type NotificationsSetReadResponse = paths['/api/notification/read']['put']['responses']['204'];
export type NotificationSetReadResponse = paths['/api/notification/{notificationId}/read']['put']['responses']['204'];

export type NotificationsGetSettingsResponse = paths['/api/notification/settings']['get']['responses']['200']['content']['application/json'];

export type NotificationsEditSettingsRequest = paths['/api/notification/settings']['put']['requestBody']['content']['application/json'];
export type NotificationsEditSettingsResponse = paths['/api/notification/settings']['put']['responses']['204'];

type NotificationSliceStateType = {
    notificationsFilter?: NotificationFilterResponse | undefined,
    notificationsFilterStatus?: LoadingStatusEnum | undefined,

    setReadAllStatus?: LoadingStatusEnum | undefined,
    setReadStatus?: LoadingStatusEnum | undefined,

    settings?: NotificationsGetSettingsResponse | undefined,
    settingsStatus?: LoadingStatusEnum | undefined,

    editSettingsStatus?: LoadingStatusEnum | undefined,
}

const initialState: NotificationSliceStateType = {};

export const setNotificationFilter = createAsyncThunk<PagedVMResponse<NotificationFilterResponse> | ErrorResponse, PagedVMRequest<NotificationFilterRequest>>(
    "notifications/filter", async (request, { rejectWithValue }) => {
        try {
            const response = await AppNotificationService.filter(request.request);
            return {
                response: response.data,
                appendType: request.appendType
            };
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setAllNotificationsRead = createAsyncThunk<NotificationsSetReadResponse | ErrorResponse, void>(
    "notifications/readAll",
    async (requestBody, { rejectWithValue }) => {
        try {
            return await AppNotificationService.setReadAll();
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setNotificationRead = createAsyncThunk<NotificationsSetReadResponse | ErrorResponse, number>(
    "notifications/read",
    async (notificationId, { rejectWithValue }) => {
        try {
            return await AppNotificationService.setRead(notificationId);
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setNotificationSettings = createAsyncThunk<NotificationsGetSettingsResponse | ErrorResponse, void>(
    "notifications/settings",
    async (requestBody, { rejectWithValue }) => {
        try {
            const result = await AppNotificationService.getSettings();
            return result.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const editNotificationSettings = createAsyncThunk<NotificationsGetSettingsResponse | ErrorResponse, NotificationsEditSettingsRequest>(
    "notifications/settings/edit",
    async (request, { rejectWithValue }) => {
        try {
            return await AppNotificationService.editSettings(request);
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const notificationSlice = createSlice({
    name: "notifications",
    initialState,
    reducers: {
        addNotification(state, action: PayloadAction<NonNullable<NotificationFilterResponse['data'][0]>>) {
            state.notificationsFilter = state.notificationsFilter ? {
                data: [action.payload, ...state.notificationsFilter.data],
                size: state.notificationsFilter.size
            } : undefined;
        },
        updateLocalNotificationsSetReadAll(state) {
            state.notificationsFilter?.data?.forEach((x) => {
                x.readDateTime = moment().utc().format();
            })
        },
        updateLocalNotificationsSetRead(state, action: PayloadAction<number>) {
            const notification = state.notificationsFilter?.data?.find(x => x.notificationId === action.payload);
            if (notification) {
                notification.readDateTime = moment().utc().format();
            }
        }
    },
    extraReducers: (builder) => {
        builder.addCase(setNotificationFilter.pending, (state) => {
            state.notificationsFilterStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setNotificationFilter.fulfilled, (state, action) => {
            const currentPayload = action.payload as PagedVMResponse<NotificationFilterResponse>;
            state.notificationsFilter = getJoinedPagedVM(state.notificationsFilter, currentPayload.response, currentPayload.appendType);
            state.notificationsFilterStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setNotificationFilter.rejected, (state, action) => {
            state.notificationsFilterStatus = LoadingStatusEnum.Error;
        });
        // -------------------------------------------------------
        builder.addCase(setAllNotificationsRead.pending, (state) => {
            state.setReadAllStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setAllNotificationsRead.fulfilled, (state, action) => {
            state.setReadAllStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setAllNotificationsRead.rejected, (state, action) => {
            state.setReadAllStatus = LoadingStatusEnum.Error;
        });
        // -------------------------------------------------------
        builder.addCase(setNotificationSettings.pending, (state) => {
            state.settingsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setNotificationSettings.fulfilled, (state, action) => {
            state.settings = action.payload as NotificationsGetSettingsResponse;
            state.settingsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setNotificationSettings.rejected, (state, action) => {
            state.settingsStatus = LoadingStatusEnum.Error;
        });
        // -------------------------------------------------------
        builder.addCase(editNotificationSettings.pending, (state) => {
            state.editSettingsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(editNotificationSettings.fulfilled, (state, action) => {
            state.editSettingsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(editNotificationSettings.rejected, (state, action) => {
            state.editSettingsStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectNotificationsFilter = (state: RootState) => state.notifications.notificationsFilter;
export const selectNotificationsFilterStatus = (state: RootState) => state.notifications.notificationsFilterStatus;
export const selectSetReadAllNotificationsStatus = (state: RootState) => state.notifications.setReadAllStatus;
export const selectSetReadNotificationStatus = (state: RootState) => state.notifications.setReadStatus;
export const selectNotificationSettings = (state: RootState) => state.notifications.settings;
export const selectNotificationSettingsStatus = (state: RootState) => state.notifications.settingsStatus;
export const selectEditNotificationSettingsStatus = (state: RootState) => state.notifications.editSettingsStatus;

export const { addNotification, updateLocalNotificationsSetReadAll, updateLocalNotificationsSetRead } = notificationSlice.actions;
export default notificationSlice.reducer;