import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { paths } from "apiTypes";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { ErrorResponse} from "core/services/Types/AppTypes";
import { RootState } from "./store";
import AppRepeatingTimeslotService from "core/services/AppRecurrentTimeslotService";

export type GetRecurrentTimeslotsRequest = paths['/api/recurrent-timeslot']['get']['parameters']['query'];
export type GetRecurrentTimeslotsResponse = paths['/api/recurrent-timeslot']['get']['responses']['200']['content']['application/json'];

export type CreateAppRepeatingTimeslotRequest = paths['/api/recurrent-timeslot']['post']['requestBody']['content']['application/json'];
export type CreateAppRepeatingTimeslotResponse = paths['/api/recurrent-timeslot']['post']['responses']['204'];

export type DeleteAppRepeatingTimeslotRequest = paths['/api/recurrent-timeslot/{recurrentTimeslotId}']['delete']['parameters']['path'] & paths['/api/recurrent-timeslot/{recurrentTimeslotId}']['delete']['parameters']['query'];
export type DeleteAppRepeatingTimeslotResponse = paths['/api/recurrent-timeslot/{recurrentTimeslotId}']['delete']['responses']['204'];

interface ITimeslotSliceState{
    recurrentTimeslots: GetRecurrentTimeslotsResponse | undefined,
    getRecurrentTimeslotsStatus: LoadingStatusEnum | undefined,
    createRecurrentTimeslotStatus: LoadingStatusEnum | undefined,
    deleteRecurrentTimeslotStatus: LoadingStatusEnum | undefined
}

const initialState: ITimeslotSliceState = {
    recurrentTimeslots: undefined,
    getRecurrentTimeslotsStatus: undefined,
    createRecurrentTimeslotStatus: undefined,
    deleteRecurrentTimeslotStatus: undefined
};

export const getRecurrentTimeslots = createAsyncThunk<GetRecurrentTimeslotsResponse | ErrorResponse, GetRecurrentTimeslotsRequest>(
    "recurrentTimeslots/get", async (request, {rejectWithValue}) => {
        try{
            const response = await AppRepeatingTimeslotService.getByDate(request);
            return response.data;
        } catch (e: any){
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const createRecurrentTimeslot = createAsyncThunk<CreateAppRepeatingTimeslotResponse | ErrorResponse, CreateAppRepeatingTimeslotRequest>(
    "recurrentTimeslots/create", async (request, {rejectWithValue}) => {
        try{
            const response = await AppRepeatingTimeslotService.create(request);
            return response;
        } catch (e: any){
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const deleteRecurrentTimeslot = createAsyncThunk<DeleteAppRepeatingTimeslotResponse | ErrorResponse, DeleteAppRepeatingTimeslotRequest>(
    "recurrentTimeslots/delete", async (request, {rejectWithValue}) => {
        try{
            const response = await AppRepeatingTimeslotService.delete(
                {recurrentTimeslotId: request.recurrentTimeslotId}, 
                {eventDate: request.eventDate}
            );
            return response;
        } catch (e: any){
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const recurrentTimeslotSlice = createSlice({
    name: "recurrentTimeslots",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(getRecurrentTimeslots.pending, (state) => {
            state.getRecurrentTimeslotsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(getRecurrentTimeslots.fulfilled, (state, action) => {
            state.recurrentTimeslots = action.payload as GetRecurrentTimeslotsResponse;
            state.getRecurrentTimeslotsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(getRecurrentTimeslots.rejected, (state, action) => {
            state.getRecurrentTimeslotsStatus = LoadingStatusEnum.Error;
        });
         //------------------------------------
        builder.addCase(createRecurrentTimeslot.pending, (state) => {
            state.createRecurrentTimeslotStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(createRecurrentTimeslot.fulfilled, (state, action) => {
            state.createRecurrentTimeslotStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(createRecurrentTimeslot.rejected, (state, action) => {
            state.createRecurrentTimeslotStatus = LoadingStatusEnum.Error;
        });
        //------------------------------------
        builder.addCase(deleteRecurrentTimeslot.pending, (state) => {
            state.deleteRecurrentTimeslotStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(deleteRecurrentTimeslot.fulfilled, (state, action) => {
            state.deleteRecurrentTimeslotStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(deleteRecurrentTimeslot.rejected, (state, action) => {
            state.deleteRecurrentTimeslotStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectCreateRecurrentTimeslotStatus = (state: RootState) => state.recurrentTimeslot.createRecurrentTimeslotStatus;
export const deleteCreateRecurrentTimeslotStatus = (state: RootState) => state.recurrentTimeslot.deleteRecurrentTimeslotStatus;

export const selectRecurrentTimeslots = (state: RootState) => state.recurrentTimeslot.recurrentTimeslots;
export const selectGetRecurrentTimeslotsStatus = (state: RootState) => state.recurrentTimeslot.getRecurrentTimeslotsStatus;

export default recurrentTimeslotSlice.reducer;