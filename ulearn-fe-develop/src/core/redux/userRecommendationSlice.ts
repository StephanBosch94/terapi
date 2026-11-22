import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { ErrorResponse, PagedVMRequest, PagedVMResponse } from "core/services/Types/AppTypes";
import { RootState } from "./store";
import { paths } from "apiTypes";
import AppUserRecommendationService from "core/services/AppUserRecommendationService";
import { getJoinedPagedVM } from "core/services/Utils/paginationUtil";

export type GetMatchingTutors = paths['/api/user-recommendation/matching-tutors']['get']['responses']['200']['content']['application/json']
export type GetMatchingTutorsParams = paths['/api/user-recommendation/matching-tutors']['get']['parameters']['query'];
export type FilterPopularTutorsRequestBody = paths['/api/user-recommendation/popular-tutors/filter']['post']['requestBody']['content']['application/json'];
export type FilterPopularTutorsResponseBody = paths['/api/user-recommendation/popular-tutors/filter']['post']['responses']['200']['content']['application/json'];
export type FilterSimilarTutorsRequestBody = paths['/api/user-recommendation/tutor/{userId}/similar-tutors/filter']['post']['requestBody']['content']['application/json'];
export type FilterSimilarTutorsResponseBody = paths['/api/user-recommendation/tutor/{userId}/similar-tutors/filter']['post']['responses']['200']['content']['application/json'];

interface IUserStatisticsSlice {
    matchingTutors?: GetMatchingTutors | undefined,
    matchingTutorsStatus?: LoadingStatusEnum | undefined,
    popularTutorsFilter?: FilterPopularTutorsResponseBody | undefined,
    popularTutorsFilterStatus?: LoadingStatusEnum | undefined,
    similarTutorsFilter?: FilterSimilarTutorsResponseBody | undefined,
    similarTutorsFilterStatus?: LoadingStatusEnum | undefined
}

export const setMatchingTutors = createAsyncThunk<GetMatchingTutors | ErrorResponse, GetMatchingTutorsParams>(
    "user-recommendation/setMatchingTutors",
    async (requestBody, { dispatch, rejectWithValue }) => {
        try {
            const result = await AppUserRecommendationService.getMatchingTutors(requestBody);
            return result.data;
        } catch (e: any) {
            const errorResponse = e.response.data as ErrorResponse;
            return rejectWithValue(errorResponse);
        }
    }
);

export const setPopularTutorsFilter = createAsyncThunk<PagedVMResponse<FilterPopularTutorsResponseBody> | ErrorResponse, PagedVMRequest<FilterPopularTutorsRequestBody>>(
    "user/setPopularTutorsFilter",
    async (request, { rejectWithValue }) => {
        try {
            const result = await AppUserRecommendationService.filterPopularTutors(request.request);
            return {
                response: result.data,
                appendType: request.appendType
            };
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setFilterSimilarUsers = createAsyncThunk<PagedVMResponse<FilterPopularTutorsResponseBody> | ErrorResponse, PagedVMRequest<{ userId: number } & FilterPopularTutorsRequestBody>>(
    "user/setSimilarTutorsFilter",
    async (request, { rejectWithValue }) => {
        try {
            const result = await AppUserRecommendationService.filterSimilarUsers(request.request);
            return {
                response: result.data,
                appendType: request.appendType
            };
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

const initialState: IUserStatisticsSlice = {};

export const userRecommendationSlice = createSlice({
    name: "user-recommendation",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(setMatchingTutors.pending, (state) => {
            state.matchingTutorsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setMatchingTutors.fulfilled, (state, action) => {
            state.matchingTutors = action.payload as GetMatchingTutors;
            state.matchingTutorsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setMatchingTutors.rejected, (state) => {
            state.matchingTutorsStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setPopularTutorsFilter.pending, (state, action) => {
            state.popularTutorsFilterStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setPopularTutorsFilter.fulfilled, (state, action) => {
            const payload = action.payload as PagedVMResponse<FilterPopularTutorsResponseBody>;
            const joined = getJoinedPagedVM(state.popularTutorsFilter, payload.response, payload.appendType);
            state.popularTutorsFilter = joined;
            state.popularTutorsFilterStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setPopularTutorsFilter.rejected, (state, action) => {
            state.popularTutorsFilterStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setFilterSimilarUsers.pending, (state, action) => {
            state.similarTutorsFilterStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setFilterSimilarUsers.fulfilled, (state, action) => {
            const payload = action.payload as PagedVMResponse<FilterSimilarTutorsResponseBody>;
            const joined = getJoinedPagedVM(state.popularTutorsFilter, payload.response, payload.appendType);
            state.similarTutorsFilter = joined;
            state.similarTutorsFilterStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setFilterSimilarUsers.rejected, (state, action) => {
            state.similarTutorsFilterStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectMatchingTutorsStatus = (state: RootState) => state.userRecommendation.matchingTutorsStatus;
export const selectMatchingTutors = (state: RootState) => state.userRecommendation.matchingTutors;

export const selectPopularTutorsFilter = (state: RootState) => state.userRecommendation.popularTutorsFilter;
export const selectPopularTutorsFilterStatus = (state: RootState) => state.userRecommendation.popularTutorsFilterStatus;

export const selectSimilarTutorsFilter = (state: RootState) => state.userRecommendation.similarTutorsFilter;
export const selectSimilarTutorsFilterStatus = (state: RootState) => state.userRecommendation.similarTutorsFilterStatus;

export default userRecommendationSlice.reducer;