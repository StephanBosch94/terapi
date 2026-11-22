import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { paths } from "apiTypes";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import UserRoomStatisticService from "core/services/UserRoomStatisticService";
import { RootState } from "./store";

export type AddUserRoomStatisticRequestBody = paths['/api/user-room-statistic']['post']['requestBody']['content']['application/json'];

interface IState{
    userRoomStatisticAddStatus: LoadingStatusEnum | undefined
}

const initialState: IState = {
    userRoomStatisticAddStatus: undefined
};

export const addStatistic = createAsyncThunk<void, AddUserRoomStatisticRequestBody>(
    'userRoomStatistic/add', async (requestBody) => {
        await UserRoomStatisticService.add(requestBody);
    }
);

export const userRoomStatisticSlice = createSlice({
    name: "userRoomStatistic",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(addStatistic.pending, (state) => {
            state.userRoomStatisticAddStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(addStatistic.fulfilled, (state, action) => {
            state.userRoomStatisticAddStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(addStatistic.rejected, (state, action) => {
            state.userRoomStatisticAddStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectUserRoomStatisticAddStatus = (state: RootState) => state.userRoomStatistic.userRoomStatisticAddStatus;
export default userRoomStatisticSlice.reducer;