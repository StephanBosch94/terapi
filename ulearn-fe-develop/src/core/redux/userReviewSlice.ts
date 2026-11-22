import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { RootState } from "./store";
import { ErrorResponse } from "core/services/Types/AppTypes";
import { paths } from "apiTypes";
import UserReviewService from "core/services/UserReview";

export type CreateUserReviewRequest = paths['/api/user-review']['post']['requestBody']['content']['application/json'];
export type CreateUserReviewResponse = paths['/api/user-review']['post']['responses']['204'];

export type EditUserReviewRequest = paths['/api/user-review']['put']['requestBody']['content']['application/json'];
export type EditUserReviewResponse = paths['/api/user-review']['put']['responses']['204'];

interface IUserReviewState {
    createUserReviewStatus?: LoadingStatusEnum | undefined,
    editUserReviewStatus?: LoadingStatusEnum | undefined
} 

export const createUserReview = createAsyncThunk<CreateUserReviewResponse | ErrorResponse, CreateUserReviewRequest>(
    'userReview/createReservationReview',
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await UserReviewService.create(requestBody);
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const editUserReview = createAsyncThunk<EditUserReviewResponse | ErrorResponse, EditUserReviewRequest>(
    'userReview/editUserReview',
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await UserReviewService.edit(requestBody);
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

const initialState: IUserReviewState = {};

export const userReviewSlice = createSlice({
    name: "userReview",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(createUserReview.pending, (state) => {
            state.createUserReviewStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(createUserReview.fulfilled, (state, action) => {
            state.createUserReviewStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(createUserReview.rejected, (state, action) => {
            state.createUserReviewStatus = LoadingStatusEnum.Error;
        });
        // -------------------------------------------------------
        builder.addCase(editUserReview.pending, (state) => {
            state.editUserReviewStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(editUserReview.fulfilled, (state, action) => {
            state.editUserReviewStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(editUserReview.rejected, (state, action) => {
            state.editUserReviewStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectCreateUserReviewStatus = (state: RootState) => state.userReview.createUserReviewStatus;
export const selectEditUserReviewStatus = (state: RootState) => state.userReview.editUserReviewStatus;

export default userReviewSlice.reducer;