import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { RootState } from "./store";
import { ErrorResponse } from "core/services/Types/AppTypes";
import { paths } from "apiTypes";
import ReservationReviewService from "core/services/ReservationReviewService";

export type CreateReservationReviewRequest = paths['/api/reservation-review']['post']['requestBody']['content']['application/json'];
export type CreateReservationReviewResponse = paths['/api/reservation-review']['post']['responses']['204'];
export type EditReservationReviewRequest = paths['/api/reservation-review']['put']['requestBody']['content']['application/json'];
export type EditReservationReviewResponse = paths['/api/reservation-review']['put']['responses']['204'];

interface IReservationReviewState {
    createReservationReviewStatus?: LoadingStatusEnum | undefined,
    editReviewStatus?: LoadingStatusEnum | undefined
} 


export const createReservationReview = createAsyncThunk<CreateReservationReviewResponse | ErrorResponse, CreateReservationReviewRequest>(
    'reservationReview/createReservationReview',
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await ReservationReviewService.create(requestBody);
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const editReservationReview = createAsyncThunk<EditReservationReviewResponse | ErrorResponse, EditReservationReviewRequest>(
    'reservationReview/editReservationReview',
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await ReservationReviewService.edit(requestBody);
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

const initialState: IReservationReviewState = {};

export const reservationReviewSlice = createSlice({
    name: "reservationReview",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(createReservationReview.pending, (state) => {
            state.createReservationReviewStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(createReservationReview.fulfilled, (state, action) => {
            state.createReservationReviewStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(createReservationReview.rejected, (state, action) => {
            state.createReservationReviewStatus = LoadingStatusEnum.Error;
        });
        // ---------------------------------------------------
        builder.addCase(editReservationReview.pending, (state) => {
            state.editReviewStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(editReservationReview.fulfilled, (state, action) => {
            state.editReviewStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(editReservationReview.rejected, (state, action) => {
            state.editReviewStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectCreateReservationReviewStatus = (state: RootState) => state.reservationReview.createReservationReviewStatus;
export const selectEditReservationReviewStatus = (state: RootState) => state.reservationReview.editReviewStatus;

export default reservationReviewSlice.reducer;