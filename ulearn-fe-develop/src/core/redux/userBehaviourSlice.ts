
import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { RootState } from "./store";
import { ErrorResponse } from "core/services/Types/AppTypes";
import { paths } from "apiTypes";
import UserReviewService from "core/services/UserReview";
import UserStatisticsService from "core/services/UserStatisticsService";
import AppUserBehaviourService from "core/services/AppUserBehaviourService";

export type CreateTutorProfileVisitedRequest = paths['/api/user-behaviour/tutor-profile/{userId}']['post']['parameters']['path'];
export type CreateTutorProfileVisitedResponse = paths['/api/user-behaviour/tutor-profile/{userId}']['post']['responses']['204'];

interface IUserBehaviourSliceState {
    createTutorProfileVisitedStatus?: LoadingStatusEnum | undefined
} 

export const createTutorProfileVisited = createAsyncThunk<CreateTutorProfileVisitedResponse | ErrorResponse, CreateTutorProfileVisitedRequest>(
    'userReview/createTutorProfileVisited',
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await AppUserBehaviourService.createTutorProfileVisited(requestBody);
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

const initialState: IUserBehaviourSliceState = {};

export const userBehaviourSlice = createSlice({
    name: "userBehaviour",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(createTutorProfileVisited.pending, (state) => {
            state.createTutorProfileVisitedStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(createTutorProfileVisited.fulfilled, (state, action) => {
            state.createTutorProfileVisitedStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(createTutorProfileVisited.rejected, (state, action) => {
            state.createTutorProfileVisitedStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectCreateTutorProfileVisitedStatus = (state: RootState) => state.userBehaviour.createTutorProfileVisitedStatus;

export default userBehaviourSlice.reducer;