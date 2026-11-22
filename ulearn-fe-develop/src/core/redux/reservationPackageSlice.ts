import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { paths } from "apiTypes";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { ErrorResponse } from "core/services/Types/AppTypes";
import { RootState } from "./store";
import AppReservationPackageService from "core/services/AppReservationPackageService";

export type DeleteReservationPackageRequest =
    paths['/api/reservation-package/{reservationPackageId}']['delete']['requestBody']['content']['application/json'] &
    paths['/api/reservation-package/{reservationPackageId}']['delete']['parameters']['path'];
export type DeleteReservationPackageResponse = paths['/api/reservation-package/{reservationPackageId}']['delete']['responses']['204'];

interface ITimeslotSliceState {
    deleteReservationPackageStatus?: LoadingStatusEnum | undefined
}

const initialState: ITimeslotSliceState = {};

export const deleteReservationPackage = createAsyncThunk<DeleteReservationPackageResponse | ErrorResponse, DeleteReservationPackageRequest>(
    "reservationPackage/delete", async (request, { rejectWithValue }) => {
        try {
            const { reservationPackageId, ...rest } = request;
            const response = await AppReservationPackageService.delete({ reservationPackageId }, rest);
            return response;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const reservationPackageSlice = createSlice({
    name: "reservationIssue",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(deleteReservationPackage.pending, (state) => {
            state.deleteReservationPackageStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(deleteReservationPackage.fulfilled, (state, action) => {
            state.deleteReservationPackageStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(deleteReservationPackage.rejected, (state, action) => {
            state.deleteReservationPackageStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectDeleteReservationPackageStatus = (state: RootState) => state.reservationPackage.deleteReservationPackageStatus;
export default reservationPackageSlice.reducer;