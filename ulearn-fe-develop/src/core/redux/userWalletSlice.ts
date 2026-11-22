import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { ErrorResponse } from "core/services/Types/AppTypes";
import { paths } from "../../apiTypes";
import { RootState } from "./store";
import UserWalletStatisticsService from "core/services/UserWalletStatisticsService";

export type GetUserWalletStatisticsResponse = paths['/api/user-wallet']['get']['responses']['200']['content']['application/json'];
export type DepositMoneyRequest = paths['/api/user-wallet/deposit']['put']['requestBody']['content']['application/json'];
export type DepositMoneyResponse = paths['/api/user-wallet/deposit']['put']['responses']['200']['content']['application/json'];
export type WithdrawMoneyRequest = paths['/api/user-wallet/withdrawal']['put']['requestBody']['content']['application/json'];
export type WithdrawMoneyResponse = paths['/api/user-wallet/withdrawal']['put']['responses']['204'];
export type TopUpPaymentRequest = paths['/api/user-wallet/top-up']['put']['requestBody']['content']['application/json'];
export type TopUpPaymentResponse = paths['/api/user-wallet/top-up']['put']['responses']['204'];
export type TopUpUserPaymentRequest = paths['/api/user-wallet/user/top-up']['put']['requestBody']['content']['application/json'];
export type TopUpUserPaymentResponse = paths['/api/user-wallet/user/top-up']['put']['responses']['204'];
export type ExtractMoneyRequest = paths['/api/user-wallet/user/extract']['put']['requestBody']['content']['application/json'];
export type ExtractMoneyResponse = paths['/api/user-wallet/user/extract']['put']['responses']['204'];

interface IUserStatisticsSlice {
    userWalletStatistics?: GetUserWalletStatisticsResponse | undefined,
    userWalletStatisticsStatus?: LoadingStatusEnum | undefined,
    withdrawStatus?: LoadingStatusEnum | undefined,
    depositMoneyReponse?: DepositMoneyResponse | undefined,
    depositMoneyStatus?: LoadingStatusEnum | undefined,
    topUpPaymentStatus?: LoadingStatusEnum | undefined,
    topUpUserPaymentStatus?: LoadingStatusEnum | undefined,
    extractMoneyStatus?: LoadingStatusEnum | undefined
}

export const setUserWalletStatistics = createAsyncThunk<GetUserWalletStatisticsResponse | ErrorResponse, undefined>(
    "user-wallet/getStatistics",
    async (param, { dispatch, rejectWithValue }) => {
        try {
            const result = await UserWalletStatisticsService.getUserWalletStatistics();
            return result.data;
        } catch (e: any) {
            const errorResponse = e.response.data as ErrorResponse;
            return rejectWithValue(errorResponse);
        }
    }
);

export const depositMoney = createAsyncThunk<DepositMoneyResponse | ErrorResponse, DepositMoneyRequest>(
    "user-wallet/depositMoney",
    async (param, { dispatch, rejectWithValue }) => {
        try {
            const result = await UserWalletStatisticsService.depositMoney(param);
            return result.data;
        } catch (e: any) {
            const errorResponse = e.response.data as ErrorResponse;
            return rejectWithValue(errorResponse);
        }
    }
);

export const widthdrawMoney = createAsyncThunk<WithdrawMoneyResponse | ErrorResponse, WithdrawMoneyRequest>(
    "user-wallet/withdrawMoney",
    async (param, { dispatch, rejectWithValue }) => {
        try {
            const result = await UserWalletStatisticsService.withdrawMoney(param);
            return result.data;
        } catch (e: any) {
            const errorResponse = e.response.data as ErrorResponse;
            return rejectWithValue(errorResponse);
        }
    }
);

export const topUpPayment = createAsyncThunk<TopUpPaymentResponse | ErrorResponse, TopUpPaymentRequest>(
    "user-wallet/topUpPayment",
    async (param, { dispatch, rejectWithValue }) => {
        try {
            const result = await UserWalletStatisticsService.topUpPayment(param);
            return result.data;
        } catch (e: any) {
            const errorResponse = e.response.data as ErrorResponse;
            return rejectWithValue(errorResponse);
        }
    }
);

export const topUpUserPayment = createAsyncThunk<TopUpUserPaymentResponse | ErrorResponse, TopUpUserPaymentRequest>(
    "user-wallet/topUpUserPayment",
    async (param, { dispatch, rejectWithValue }) => {
        try {
            const result = await UserWalletStatisticsService.topUpUserPayment(param);
            return result.data;
        } catch (e: any) {
            const errorResponse = e.response.data as ErrorResponse;
            return rejectWithValue(errorResponse);
        }
    }
);

export const extractMoney = createAsyncThunk<ExtractMoneyResponse | ErrorResponse, ExtractMoneyRequest>(
    "user-wallet/extractMoney",
    async (param, { dispatch, rejectWithValue }) => {
        try {
            const result = await UserWalletStatisticsService.extractMoney(param);
            return result.data;
        } catch (e: any) {
            const errorResponse = e.response.data as ErrorResponse;
            return rejectWithValue(errorResponse);
        }
    }
);

const initialState: IUserStatisticsSlice = {};

export const userStatisticsSlice = createSlice({
    name: "user-wallet",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(setUserWalletStatistics.pending, (state) => {
            state.userWalletStatisticsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setUserWalletStatistics.fulfilled, (state, action) => {
            state.userWalletStatistics = action.payload as GetUserWalletStatisticsResponse;
            state.userWalletStatisticsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setUserWalletStatistics.rejected, (state) => {
            state.userWalletStatisticsStatus = LoadingStatusEnum.Error;
        });
        //===============================================================
        builder.addCase(depositMoney.pending, (state) => {
            state.depositMoneyStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(depositMoney.fulfilled, (state, action) => {
            state.depositMoneyReponse = action.payload as DepositMoneyResponse;
            state.depositMoneyStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(depositMoney.rejected, (state) => {
            state.depositMoneyStatus = LoadingStatusEnum.Error;
        });
        //===============================================================
        builder.addCase(widthdrawMoney.pending, (state) => {
            state.withdrawStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(widthdrawMoney.fulfilled, (state, action) => {
            state.withdrawStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(widthdrawMoney.rejected, (state) => {
            state.withdrawStatus = LoadingStatusEnum.Error;
        });
        //===============================================================
        builder.addCase(topUpPayment.pending, (state) => {
            state.topUpPaymentStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(topUpPayment.fulfilled, (state, action) => {
            state.topUpPaymentStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(topUpPayment.rejected, (state) => {
            state.topUpPaymentStatus = LoadingStatusEnum.Error;
        });
        //===============================================================
        builder.addCase(topUpUserPayment.pending, (state) => {
            state.topUpUserPaymentStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(topUpUserPayment.fulfilled, (state, action) => {
            state.topUpUserPaymentStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(topUpUserPayment.rejected, (state) => {
            state.topUpUserPaymentStatus = LoadingStatusEnum.Error;
        });
        //===============================================================
        builder.addCase(extractMoney.pending, (state) => {
            state.extractMoneyStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(extractMoney.fulfilled, (state, action) => {
            state.extractMoneyStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(extractMoney.rejected, (state) => {
            state.extractMoneyStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectUserWalletStatisticsStatus = (state: RootState) => state.userWallet.userWalletStatisticsStatus;
export const selectUserWalletStatistics = (state: RootState) => state.userWallet.userWalletStatistics;
export const selectDepositMoneyStatus = (state: RootState) => state.userWallet.depositMoneyStatus;
export const selectDepositMoneyResponse = (state: RootState) => state.userWallet.depositMoneyReponse;
export const selectWithdrawStatus = (state: RootState) => state.userWallet.withdrawStatus;
export const selectTopUpPaymentStatus = (state: RootState) => state.userWallet.topUpPaymentStatus;
export const selectTopUpUserPaymentStatus = (state: RootState) => state.userWallet.topUpUserPaymentStatus;
export const selectExtractMoneyStatus = (state: RootState) => state.userWallet.extractMoneyStatus;

export default userStatisticsSlice.reducer;