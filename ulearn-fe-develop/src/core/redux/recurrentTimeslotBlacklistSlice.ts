import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { paths } from "apiTypes";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { ErrorResponse} from "core/services/Types/AppTypes";
import { RootState } from "./store";
import AppRecurrentTimeslotBlacklistService from "core/services/AppRecurrentTimeslotBlacklistService";

export type CreateAppRepeatingTimeslotBlacklistRequest = paths['/api/recurrent-timeslot-blacklist']['post']['requestBody']['content']['application/json'];
export type CreateAppRepeatingTimeslotBlacklistResponse = paths['/api/recurrent-timeslot-blacklist']['post']['responses']['204'];

interface ITimeslotSliceState{
    createRecurrentTimeslotBlacklistStatus: LoadingStatusEnum | undefined
}

const initialState: ITimeslotSliceState = {
    createRecurrentTimeslotBlacklistStatus: undefined
};

export const createRepeatingTimeslotBlacklist = createAsyncThunk<CreateAppRepeatingTimeslotBlacklistResponse | ErrorResponse, CreateAppRepeatingTimeslotBlacklistRequest>(
    "recurrentTimeslotBlaclist/create", async (request, {rejectWithValue}) => {
        try{
            const response = await AppRecurrentTimeslotBlacklistService.create(request);
            return response;
        } catch (e: any){
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const recurrentTimeslotBlacklistSlice = createSlice({
    name: "repeatingTimeslots",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(createRepeatingTimeslotBlacklist.pending, (state) => {
            state.createRecurrentTimeslotBlacklistStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(createRepeatingTimeslotBlacklist.fulfilled, (state, action) => {
            state.createRecurrentTimeslotBlacklistStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(createRepeatingTimeslotBlacklist.rejected, (state, action) => {
            state.createRecurrentTimeslotBlacklistStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectCreateRecurrentTimeslotBlacklistStatus = (state: RootState) => state.recurrentTimeslotBlacklist.createRecurrentTimeslotBlacklistStatus;
export default recurrentTimeslotBlacklistSlice.reducer;