import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { paths } from "apiTypes";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { ErrorResponse, PagedVMRequest, PagedVMResponse } from "core/services/Types/AppTypes";
import { RootState } from "./store";
import { getJoinedPagedVM } from "core/services/Utils/paginationUtil";
import AppReservationIssueService from "core/services/AppReservationIssueService";

export type CreateReservationIssueRequest = paths['/api/reservation-issue']['post']['requestBody']['content']['application/json'];
export type CreateReservationIssueResponse = paths['/api/reservation-issue']['post']['responses']['204'];

export type DeclineReservationIssueRequest = paths['/api/reservation-issue/decline']['put']['requestBody']['content']['application/json'];
export type DeclineReservationIssueResponse = paths['/api/reservation-issue/decline']['put']['responses']['204'];

export type ApproveReservationIssueRequest = paths['/api/reservation-issue/approve']['put']['requestBody']['content']['application/json'];
export type ApproveReservationIssueResponse = paths['/api/reservation-issue/approve']['put']['responses']['204'];

export type ReservationIssueFilterRequest = paths['/api/reservation-issue/filter']['post']['requestBody']['content']['application/json'];
export type ReservationIssueFilterResponse = paths['/api/reservation-issue/filter']['post']['responses']['200']['content']['application/json'];

interface ITimeslotSliceState {
    createReservationIssueStatus?: LoadingStatusEnum | undefined,

    issuesFilter?: ReservationIssueFilterResponse | undefined,
    issuesFilterStatus?: LoadingStatusEnum | undefined,

    appoveStatus?: LoadingStatusEnum | undefined,
    declineStatus?: LoadingStatusEnum | undefined
}

const initialState: ITimeslotSliceState = {};

export const createReservationIssue = createAsyncThunk<CreateReservationIssueResponse | ErrorResponse, CreateReservationIssueRequest>(
    "reservationIssue/create", async (request, { rejectWithValue }) => {
        try {
            const response = await AppReservationIssueService.create(request);
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setReservationIssuesFilter = createAsyncThunk<PagedVMResponse<ReservationIssueFilterResponse> | ErrorResponse, PagedVMRequest<ReservationIssueFilterRequest>>(
    "reservationIssue/filter", async (request, { rejectWithValue }) => {
        try {
            const response = await AppReservationIssueService.filter(request.request);
            return {
                response: response.data,
                appendType: request.appendType
            }
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const declineReservationIssue = createAsyncThunk<void | ErrorResponse, DeclineReservationIssueRequest>(
    "reservationIssue/decline", async (requestBody, { rejectWithValue }) => {
        try {
            await AppReservationIssueService.decline(requestBody);
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const approveReservationIssue = createAsyncThunk<void | ErrorResponse, ApproveReservationIssueRequest>(
    "reservationIssue/approve", async (requestBody, { rejectWithValue }) => {
        try {
            await AppReservationIssueService.approve(requestBody);
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const reservationIssueSlice = createSlice({
    name: "reservationIssue",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(createReservationIssue.pending, (state) => {
            state.createReservationIssueStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(createReservationIssue.fulfilled, (state, action) => {
            state.createReservationIssueStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(createReservationIssue.rejected, (state, action) => {
            state.createReservationIssueStatus = LoadingStatusEnum.Error;
        });
        //----------------------------------------------------------------
        builder.addCase(setReservationIssuesFilter.pending, (state) => {
            state.issuesFilterStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setReservationIssuesFilter.fulfilled, (state, action) => {
            const currentPayload = action.payload as PagedVMResponse<ReservationIssueFilterResponse>;
            state.issuesFilter = getJoinedPagedVM(state.issuesFilter, currentPayload.response, currentPayload.appendType);
            state.issuesFilterStatus = LoadingStatusEnum.Loaded;

        });
        builder.addCase(setReservationIssuesFilter.rejected, (state, action) => {
            state.issuesFilterStatus = LoadingStatusEnum.Error;
        });
        //----------------------------------------------------------------
        builder.addCase(declineReservationIssue.pending, (state) => {
            state.declineStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(declineReservationIssue.fulfilled, (state, action) => {
            state.declineStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(declineReservationIssue.rejected, (state, action) => {
            state.declineStatus = LoadingStatusEnum.Error;
        });
        //----------------------------------------------------------------
        builder.addCase(approveReservationIssue.pending, (state) => {
            state.appoveStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(approveReservationIssue.fulfilled, (state, action) => {
            state.appoveStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(approveReservationIssue.rejected, (state, action) => {
            state.appoveStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectCreateReservationIssueStatus = (state: RootState) => state.reservationIssue.createReservationIssueStatus;
export const selectReservationIssueFilterStatus = (state: RootState) => state.reservationIssue.issuesFilterStatus;
export const selectReservationIssueFilter = (state: RootState) => state.reservationIssue.issuesFilter;
export const selectReservationIssueApproveStatus = (state: RootState) => state.reservationIssue.appoveStatus;
export const selectReservationIssueDeclineStatus = (state: RootState) => state.reservationIssue.declineStatus;

export default reservationIssueSlice.reducer;