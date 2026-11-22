import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { ErrorResponse } from "core/services/Types/AppTypes";
import UserStatisticsService from "core/services/UserStatisticsService";
import { paths } from "../../apiTypes";
import { RootState } from "./store";

export type GetUserStatisticsResponse = paths['/api/user-statistics/price']['get']['responses']['200']['content']['application/json'];
export type GetTutorPublicStatisticsRequest = paths['/api/user-statistics/tutor']['get']['responses']['200']['content']['application/json'];
export type GetTutorPublicStatisticsResponse = paths['/api/user-statistics/tutor/{userId}']['get']['responses']['200']['content']['application/json'];
export type GetTutorPrivateStatisticsResponse = paths['/api/user-statistics/tutor']['get']['responses']['200']['content']['application/json'];
export type GetStudentPrivateStatisticsResponse = paths['/api/user-statistics/student']['get']['responses']['200']['content']['application/json'];
export type GetCompanyPrivateStatisticsResponse = paths['/api/user-statistics/company']['get']['responses']['200']['content']['application/json'];

interface IUserStatisticsSlice {
    priceStatistics?: GetUserStatisticsResponse | undefined,
    priceStatisticsStatus?: LoadingStatusEnum | undefined,
    tutorPublicStatisticsStatus?: LoadingStatusEnum | undefined,
    tutorPublicStatistics?: GetTutorPublicStatisticsResponse | undefined,
    tutorPrivateStatisticsStatus?: LoadingStatusEnum | undefined,
    tutorPrivateStatistics?: GetTutorPrivateStatisticsResponse | undefined,
    studentPrivateStatisticsStatus?: LoadingStatusEnum | undefined,
    studentPrivateStatistics?: GetStudentPrivateStatisticsResponse | undefined,
    companyPrivateStatisticsStatus?: LoadingStatusEnum | undefined,
    companyPrivateStatistics?: GetCompanyPrivateStatisticsResponse | undefined
}

export const setPriceStatistics = createAsyncThunk<GetUserStatisticsResponse | ErrorResponse, undefined>(
    "user-statistics/setPriceStatistics",
    async (param, { dispatch, rejectWithValue }) => {
        try {
            const result = await UserStatisticsService.getPriceStatistics();
            return result.data;
        } catch (e: any) {
            const errorResponse = e.response.data as ErrorResponse;
            return rejectWithValue(errorResponse);
        }
    }
);

export const setTutorPublicStatistics = createAsyncThunk<GetTutorPublicStatisticsResponse | ErrorResponse, number>(
    "user-statistics/setTutorPublicStatistics",
    async (param, { dispatch, rejectWithValue }) => {
        try {
            const result = await UserStatisticsService.getTutorPublicStatistics(param);
            return result.data;
        } catch (e: any) {
            const errorResponse = e.response.data as ErrorResponse;
            return rejectWithValue(errorResponse);
        }
    }
);

export const setTutorPrivateStatistics = createAsyncThunk<GetTutorPrivateStatisticsResponse | ErrorResponse, undefined>(
    "user-statistics/setTutorPrivateStatistics",
    async (param, { dispatch, rejectWithValue }) => {
        try {
            const result = await UserStatisticsService.getTutorPrivateStatistics();
            return result.data;
        } catch (e: any) {
            const errorResponse = e.response.data as ErrorResponse;
            return rejectWithValue(errorResponse);
        }
    }
);

export const setStudentPrivateStatistics = createAsyncThunk<GetStudentPrivateStatisticsResponse | ErrorResponse, undefined>(
    "user-statistics/setStudentPrivateStatistics",
    async (param, { dispatch, rejectWithValue }) => {
        try {
            const result = await UserStatisticsService.getStudentPrivateStatistics();
            return result.data;
        } catch (e: any) {
            const errorResponse = e.response.data as ErrorResponse;
            return rejectWithValue(errorResponse);
        }
    }
);

export const setCompanyPrivateStatistics = createAsyncThunk<GetCompanyPrivateStatisticsResponse | ErrorResponse, undefined>(
    "user-statistics/setCompanyPrivateStatistics",
    async (param, { dispatch, rejectWithValue }) => {
        try {
            const result = await UserStatisticsService.getCompanyPrivateStatistics();
            return result.data;
        } catch (e: any) {
            const errorResponse = e.response.data as ErrorResponse;
            return rejectWithValue(errorResponse);
        }
    }
);
const initialState: IUserStatisticsSlice = {};

export const userStatisticsSlice = createSlice({
    name: "user-statistics",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(setPriceStatistics.pending, (state) => {
            state.priceStatisticsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setPriceStatistics.fulfilled, (state, action) => {
            state.priceStatistics = action.payload as GetUserStatisticsResponse;
            state.priceStatisticsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setPriceStatistics.rejected, (state) => {
            state.priceStatisticsStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setTutorPublicStatistics.pending, (state) => {
            state.tutorPublicStatisticsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setTutorPublicStatistics.fulfilled, (state, action) => {
            state.tutorPublicStatistics = action.payload as GetTutorPublicStatisticsResponse;
            state.tutorPublicStatisticsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setTutorPublicStatistics.rejected, (state) => {
            state.tutorPublicStatisticsStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setTutorPrivateStatistics.pending, (state) => {
            state.tutorPrivateStatisticsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setTutorPrivateStatistics.fulfilled, (state, action) => {
            state.tutorPrivateStatistics = action.payload as GetTutorPrivateStatisticsResponse;
            state.tutorPrivateStatisticsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setTutorPrivateStatistics.rejected, (state) => {
            state.tutorPrivateStatisticsStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setStudentPrivateStatistics.pending, (state) => {
            state.studentPrivateStatisticsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setStudentPrivateStatistics.fulfilled, (state, action) => {
            state.studentPrivateStatistics = action.payload as GetStudentPrivateStatisticsResponse;
            state.studentPrivateStatisticsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setStudentPrivateStatistics.rejected, (state) => {
            state.studentPrivateStatisticsStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setCompanyPrivateStatistics.pending, (state) => {
            state.companyPrivateStatisticsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setCompanyPrivateStatistics.fulfilled, (state, action) => {
            state.companyPrivateStatistics = action.payload as GetCompanyPrivateStatisticsResponse;
            state.companyPrivateStatisticsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setCompanyPrivateStatistics.rejected, (state) => {
            state.companyPrivateStatisticsStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectPriceStatisticsStatus = (state: RootState) => state.userStatistics.priceStatisticsStatus;
export const selectPriceStatistics = (state: RootState) => state.userStatistics.priceStatistics;
export const selectTutorPublicStatisticsStatus = (state: RootState) => state.userStatistics.tutorPublicStatisticsStatus;
export const selectTutorPublicStatistics = (state: RootState) => state.userStatistics.tutorPublicStatistics;
export const selectTutorPrivateStatisticsStatus = (state: RootState) => state.userStatistics.tutorPrivateStatisticsStatus;
export const selectTutorPrivateStatistics = (state: RootState) => state.userStatistics.tutorPrivateStatistics;
export const selectStudentPrivateStatisticsStatus = (state: RootState) => state.userStatistics.studentPrivateStatisticsStatus;
export const selectStudentPrivateStatistics = (state: RootState) => state.userStatistics.studentPrivateStatistics;
export const selectCompanyPrivateStatisticsStatus = (state: RootState) => state.userStatistics.companyPrivateStatisticsStatus;
export const selectCompanyPrivateStatistics = (state: RootState) => state.userStatistics.companyPrivateStatistics;

export default userStatisticsSlice.reducer;