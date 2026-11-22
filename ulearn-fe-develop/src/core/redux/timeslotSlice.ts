import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { paths } from "apiTypes";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { ErrorResponse} from "core/services/Types/AppTypes";
import { RootState } from "./store";
import AppTimeslotService from "core/services/AppTimeslotService";

export type CreateAppTimeslotRequest = paths['/api/timeslot']['post']['requestBody']['content']['application/json'];
export type CreateAppTimeslotResponse = paths['/api/timeslot']['post']['responses']['204'];
export type DeleteAppTimeslotRequest = paths['/api/timeslot/{timeslotId}']['delete']['parameters']['path'];
export type DeleteAppTimeslotResponse = paths['/api/timeslot/{timeslotId}']['delete']['responses']['204'];

interface ITimeslotSliceState{
    createAppTimeslotStatus: LoadingStatusEnum | undefined,
    deleteAppTimeslotStatus: LoadingStatusEnum | undefined
}

const initialState: ITimeslotSliceState = {
    createAppTimeslotStatus: undefined,
    deleteAppTimeslotStatus: undefined
};

export const createTimeslot = createAsyncThunk<CreateAppTimeslotResponse | ErrorResponse, CreateAppTimeslotRequest>(
    "timeslots/createTimeslot", async (request, {rejectWithValue}) => {
        try{
            const response = await AppTimeslotService.create(request);
            return response;
        } catch (e: any){
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const deleteTimeslot = createAsyncThunk<DeleteAppTimeslotResponse | ErrorResponse, DeleteAppTimeslotRequest>(
    "timeslots/deleteTimeslot", async (request, {rejectWithValue}) => {
        try{
            const response = await AppTimeslotService.delete(request);
            return response;
        } catch (e: any){
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const timeslotSlice = createSlice({
    name: "timeslots",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(createTimeslot.pending, (state) => {
            state.createAppTimeslotStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(createTimeslot.fulfilled, (state, action) => {
            state.createAppTimeslotStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(createTimeslot.rejected, (state, action) => {
            state.createAppTimeslotStatus = LoadingStatusEnum.Error;
        });
        /* ************************************ */
        builder.addCase(deleteTimeslot.pending, (state) => {
            state.deleteAppTimeslotStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(deleteTimeslot.fulfilled, (state, action) => {
            state.deleteAppTimeslotStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(deleteTimeslot.rejected, (state, action) => {
            state.deleteAppTimeslotStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectCreateAppTimeslotStatus = (state: RootState) => state.timeslot.createAppTimeslotStatus;
export const selectDeleteAppTimeslotStatus = (state: RootState) => state.timeslot.deleteAppTimeslotStatus;
export const selectDeleteTimeslot = (state: RootState) => state.timeslot.deleteAppTimeslotStatus;

export default timeslotSlice.reducer;