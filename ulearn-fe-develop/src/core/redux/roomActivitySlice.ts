import { PayloadAction, createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { paths } from "apiTypes";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { ErrorResponse, OpponentActivityMessageVM } from "core/services/Types/AppTypes";
import { RootState } from "./store";
import RoomActivityService from "core/services/RoomActivityService";

export type RoomActivityAddRequest = paths['/api/room-activity/room/{roomKey}']['post']['requestBody']['content']['application/json'];

interface IRoomActivityState{
    roomActivityAddStatus: LoadingStatusEnum | undefined,
    latestOpponentActivity: OpponentActivityMessageVM | undefined
}

const initialState: IRoomActivityState = {
    roomActivityAddStatus: undefined,
    latestOpponentActivity: undefined
};

export const addActivity = createAsyncThunk<void,{roomKey: string, input: RoomActivityAddRequest}>(
    "roomActivity/add", async (requestBody, {rejectWithValue}) => {
        try{
            await RoomActivityService.add(requestBody.roomKey, requestBody.input);    
        } catch (e: any){
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const roomAcitiviySlice = createSlice({
    name: "roomActivities",
    initialState,
    reducers: {
        setOpponentActivity(state, action: PayloadAction<OpponentActivityMessageVM>){
            state.latestOpponentActivity = action.payload;
        }
    },
    extraReducers: (builder) => {
        builder.addCase(addActivity.pending, (state) => {
            state.roomActivityAddStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(addActivity.fulfilled, (state, action) => {
            state.roomActivityAddStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(addActivity.rejected, (state, action) => {
            state.roomActivityAddStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectReservationStatus = (state: RootState) => state.roomActivity.roomActivityAddStatus;
export const selectLatestOpponentActivity = (state: RootState) => state.roomActivity.latestOpponentActivity;

export const { setOpponentActivity } = roomAcitiviySlice.actions;
export default roomAcitiviySlice.reducer;