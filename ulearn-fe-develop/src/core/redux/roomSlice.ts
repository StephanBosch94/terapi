import { PayloadAction, createAsyncThunk, createSelector, createSlice } from "@reduxjs/toolkit";
import { paths } from "apiTypes";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { ErrorResponse } from "core/services/Types/AppTypes";
import { RootState } from "./store";
import RoomService from "core/services/RoomService";
import { RoomStateEnum } from "core/constants/enums/RoomStateEnum";
import { selectShortUserInfo } from "core/redux/userSlice";
export type RoomInfoResponse = paths['/api/room/{key}']['get']['responses']['200']['content']['application/json'];

interface IRoomState{
    roomInfoStatus: LoadingStatusEnum | undefined,
    roomInfo: RoomInfoResponse | undefined,
    websocketStatus: LoadingStatusEnum | undefined
}

const initialState: IRoomState = {
    roomInfoStatus: undefined,
    roomInfo: undefined,
    websocketStatus: undefined
};

export const setRoomInfo = createAsyncThunk<RoomInfoResponse,{roomKey: string}>(
    "room/setInfo", async (requestBody, {rejectWithValue}) => {
        try{
            const response = await RoomService.get(requestBody.roomKey);    
            return response.data;
        } catch (e: any){
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const roomSlice = createSlice({
    name: "room",
    initialState,
    reducers: {
        setWebsocketStatus(state, action: PayloadAction<LoadingStatusEnum | undefined>){
            state.websocketStatus = action.payload;
        } 
    },
    extraReducers: (builder) => {
        builder.addCase(setRoomInfo.pending, (state) => {
            state.roomInfoStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setRoomInfo.fulfilled, (state, action) => {
            state.roomInfo = action.payload;
            state.roomInfoStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setRoomInfo.rejected, (state, action) => {
            state.roomInfoStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectRoomInfo = (state: RootState) => state.room.roomInfo;
export const selectRoomState = (state: RootState): RoomStateEnum | undefined => state.room.roomInfo?.roomStateId;
export const selectRoomOpponent = createSelector(selectRoomInfo, selectShortUserInfo, (room, user) => room?.host?.userId === user?.userId ? room?.attender : room?.host);
export const selectRoomInfoStatus = (state: RootState) => state.room.roomInfoStatus;
export const selectWebsocketStatus = (state: RootState) => state.room.websocketStatus;

export const { setWebsocketStatus } = roomSlice.actions;
export default roomSlice.reducer;