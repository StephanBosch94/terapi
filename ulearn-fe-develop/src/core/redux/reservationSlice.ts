import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { paths } from "apiTypes";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import ReservationService from "core/services/ReservationService";
import { ErrorResponse, PagedVMRequest, PagedVMResponse } from "core/services/Types/AppTypes";
import { getJoinedPagedVM } from "core/services/Utils/paginationUtil";
import { RootState } from "./store";
import { ReservationTimeEnum } from "core/constants/enums/ReservationTimeEnum";

export type ReservationFilterResponse = paths['/api/reservation/filter']['post']['responses']['200']['content']['application/json'];
export type ReservationsFilterRequest = paths['/api/reservation/filter']['post']['requestBody']['content']['application/json'];
export type ReservationsCreateRequest = paths['/api/reservation']['post']['requestBody']['content']['application/json'];
export type ReservationsCreateResponse = paths['/api/reservation']['post']['responses']['200']['content']['application/json'];
export type ReservationsEditRequest = paths['/api/reservation/{reservationId}']['put']['requestBody']['content']['application/json'];
export type ReservationsEditResponse = paths['/api/reservation/{reservationId}']['put']['responses']['204'];
export type GetCouponInfoRequest = paths['/api/coupon/code/{code}']['get']['parameters']['path'];
export type GetCouponInfoResponse = paths['/api/coupon/code/{code}']['get']['responses']['200']['content']['application/json'];
export type GetDefaultCouponInfoResponse = paths['/api/coupon/default']['get']['responses']['200']['content']['application/json'];

interface IReservationSliceState {
    upcomingReservations?: ReservationFilterResponse | undefined,
    upcomingReservationsStatus?: LoadingStatusEnum | undefined,

    pastReservations?: ReservationFilterResponse | undefined,
    pastReservationsStatus?: LoadingStatusEnum | undefined,

    createReservationStatus?: LoadingStatusEnum | undefined,
    creteReservationResponse?: ReservationsCreateResponse | undefined,

    editReservationStatus?: LoadingStatusEnum | undefined,

    couponInfo?: GetCouponInfoResponse | undefined,
    couponInfoStatus?: LoadingStatusEnum | undefined,

    defaultCouponInfo?: GetDefaultCouponInfoResponse | undefined,
    defaultCouponInfoStatus?: LoadingStatusEnum | undefined,
}

const initialState: IReservationSliceState = {};

export const setUpcomingReservations = createAsyncThunk<PagedVMResponse<ReservationFilterResponse> | ErrorResponse, PagedVMRequest<ReservationsFilterRequest>>(
    "reservations/setUpcomingFilter", async (request, { rejectWithValue }) => {
        try {
            const response = await ReservationService.filter(request.request);
            return {
                response: response.data,
                appendType: request.appendType
            };
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setPastReservations = createAsyncThunk<PagedVMResponse<ReservationFilterResponse> | ErrorResponse, PagedVMRequest<ReservationsFilterRequest>>(
    "reservations/setPastFilter", async (request, { rejectWithValue }) => {
        try {
            const response = await ReservationService.filter(request.request);
            return {
                response: response.data,
                appendType: request.appendType
            };
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const createReservation = createAsyncThunk<ReservationsCreateResponse | ErrorResponse, ReservationsCreateRequest>(
    "reservations/create", async (request, { rejectWithValue }) => {
        try {
            const response = await ReservationService.create(request);
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const editReservation = createAsyncThunk<ReservationsEditResponse | ErrorResponse, ReservationsEditRequest & { reservationId: number }>(
    "reservations/edit", async (request, { rejectWithValue }) => {
        try {
            const response = await ReservationService.edit(request);
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setCouponInfo = createAsyncThunk<GetCouponInfoResponse | ErrorResponse, GetCouponInfoRequest>(
    "reservations/getCouponInfo", async (request, { rejectWithValue }) => {
        try {
            const response = await ReservationService.getCouponInfo(request);
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setDefaultCouponInfo = createAsyncThunk<GetDefaultCouponInfoResponse | ErrorResponse, void>(
    "reservations/getDefaultCouponInfo", async (request, { rejectWithValue }) => {
        try {
            const response = await ReservationService.getDefaultCouponInfo();
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const reservationSlice = createSlice({
    name: "reservations",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(setUpcomingReservations.pending, (state) => {
            state.upcomingReservationsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setUpcomingReservations.fulfilled, (state, action) => {
            const currentPayload = action.payload as PagedVMResponse<ReservationFilterResponse>;
            state.upcomingReservations = getJoinedPagedVM(state.upcomingReservations, currentPayload.response, currentPayload.appendType);
            state.upcomingReservationsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setUpcomingReservations.rejected, (state, action) => {
            state.upcomingReservationsStatus = LoadingStatusEnum.Error;
        });
        /* ************************************ */
        builder.addCase(setPastReservations.pending, (state) => {
            state.pastReservationsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setPastReservations.fulfilled, (state, action) => {
            const currentPayload = action.payload as PagedVMResponse<ReservationFilterResponse>;
            state.pastReservations = getJoinedPagedVM(state.pastReservations, currentPayload.response, currentPayload.appendType);
            state.pastReservationsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setPastReservations.rejected, (state, action) => {
            state.pastReservationsStatus = LoadingStatusEnum.Error;
        });
        /* ************************************ */
        builder.addCase(createReservation.pending, (state) => {
            state.createReservationStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(createReservation.fulfilled, (state, action) => {
            state.creteReservationResponse = action.payload as ReservationsCreateResponse;
            state.createReservationStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(createReservation.rejected, (state) => {
            state.createReservationStatus = LoadingStatusEnum.Error;
        });
        /* ************************************ */
        builder.addCase(editReservation.pending, (state) => {
            state.editReservationStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(editReservation.fulfilled, (state) => {
            state.editReservationStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(editReservation.rejected, (state, action) => {
            state.editReservationStatus = LoadingStatusEnum.Error;
        });
        /* ************************************ */
        builder.addCase(setCouponInfo.pending, (state) => {
            state.couponInfoStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setCouponInfo.fulfilled, (state, action) => {
            state.couponInfo = action.payload as GetCouponInfoResponse;
            state.couponInfoStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setCouponInfo.rejected, (state, action) => {
            state.couponInfoStatus = LoadingStatusEnum.Error;
        });
        /* ************************************ */
        builder.addCase(setDefaultCouponInfo.pending, (state) => {
            state.defaultCouponInfoStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setDefaultCouponInfo.fulfilled, (state, action) => {
            state.defaultCouponInfo = action.payload as GetCouponInfoResponse;
            state.defaultCouponInfoStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setDefaultCouponInfo.rejected, (state, action) => {
            state.defaultCouponInfoStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectUpcomingReservations = (state: RootState) => state.reservation.upcomingReservations;
export const selectUpcomingReservationsStatus = (state: RootState) => state.reservation.upcomingReservationsStatus;

export const selectPastReservations = (state: RootState) => state.reservation.pastReservations;
export const selectPastReservationsStatus = (state: RootState) => state.reservation.pastReservationsStatus;

export const selectReservations = (state: RootState, time: ReservationTimeEnum) => time === ReservationTimeEnum.Past ? selectPastReservations(state) : selectUpcomingReservations(state);
export const selectReservationsStatus = (state: RootState, time: ReservationTimeEnum) => time === ReservationTimeEnum.Past ? selectPastReservationsStatus(state) : selectUpcomingReservationsStatus(state);

export const selectCreateReservationsStatus = (state: RootState) => state.reservation.createReservationStatus;
export const selectCreateReservationsResponse = (state: RootState) => state.reservation.creteReservationResponse;

export const selectEditReservationStatus = (state: RootState) => state.reservation.editReservationStatus;

export const selectCouponInfoStatus = (state: RootState) => state.reservation.couponInfoStatus;
export const selectCouponInfo = (state: RootState) => state.reservation.couponInfo;

export const selectDefaultCouponInfoStatus = (state: RootState) => state.reservation.defaultCouponInfoStatus;
export const selectDefaultCouponInfo = (state: RootState) => state.reservation.defaultCouponInfo;

export default reservationSlice.reducer;