import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { paths } from "apiTypes";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { ErrorResponse, PagedVMRequest, PagedVMResponse } from "core/services/Types/AppTypes";
import { RootState } from "./store";
import AppTransactionService from "core/services/AppTransactionService";
import { ReservationIssueFilterResponse } from "./reservationIssueSlice";
import { getJoinedPagedVM } from "core/services/Utils/paginationUtil";

export type TransactionFilterRequest = paths['/api/transaction/filter']['post']['requestBody']['content']['application/json'];
export type TransactionFilterResponse = paths['/api/transaction/filter']['post']['responses']['200']['content']['application/json'];

export type TransactionCompleteRequest = paths['/api/transaction/{transactionId}/complete']['put']['parameters']['path'];
export type TransactionCompleteResponse = paths['/api/transaction/{transactionId}/complete']['put']['responses']['204'];

export type TransactionWithdrawFilterRequest = paths['/api/transaction/withdraw/filter']['post']['requestBody']['content']['application/json'];
export type TransactionWithdrawFilterResponse = paths['/api/transaction/withdraw/filter']['post']['responses']['200']['content']['application/json'];

type TranslactionSliceState = {
    transactionFilter?: TransactionFilterResponse,
    transactionFilterStatus?: LoadingStatusEnum,

    completeTrasactionStatus?: LoadingStatusEnum,

    transactionWithdrawFilter?: TransactionWithdrawFilterResponse,
    transactionWithdrawFilterStatus?: LoadingStatusEnum
}

const initialState: TranslactionSliceState = {};

export const setTransactionFilter = createAsyncThunk<PagedVMResponse<TransactionFilterResponse> | ErrorResponse, PagedVMRequest<TransactionFilterRequest>>(
    "transaction/filter", async (requestBody, {rejectWithValue}) => {
        try{
            const response = await AppTransactionService.filter(requestBody.request);    
            return {
                response: response.data,
                appendType: requestBody.appendType
            };
        } catch (e: any){
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setCompleteTrasnsatction = createAsyncThunk<TransactionCompleteResponse | ErrorResponse, TransactionCompleteRequest>(
    "transaction/complete",
    async (requestBody, { rejectWithValue }) => {
        try {
            return await AppTransactionService.complete(requestBody);
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setWithdrawFilter = createAsyncThunk<PagedVMResponse<TransactionWithdrawFilterResponse> | ErrorResponse, PagedVMRequest<TransactionWithdrawFilterRequest>>(
    "transaction/withdraw/filter",
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await AppTransactionService.withdrawFilter(requestBody.request);
            return {
                response: response.data,
                appendType: requestBody.appendType
            }
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);


export const transactionSlice = createSlice({
    name: "room",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(setTransactionFilter.pending, (state) => {
            state.transactionFilterStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setTransactionFilter.fulfilled, (state, action) => {
            const currentPayload = action.payload as PagedVMResponse<TransactionFilterResponse>;
            state.transactionFilter = getJoinedPagedVM(state.transactionFilter, currentPayload.response, currentPayload.appendType);
            state.transactionFilterStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setTransactionFilter.rejected, (state, action) => {
            state.transactionFilterStatus = LoadingStatusEnum.Error;
        });
        // -----------------------------------------------------------------
        builder.addCase(setCompleteTrasnsatction.pending, (state) => {
            state.completeTrasactionStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setCompleteTrasnsatction.fulfilled, (state, action) => {
            state.completeTrasactionStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setCompleteTrasnsatction.rejected, (state, action) => {
            state.completeTrasactionStatus = LoadingStatusEnum.Error;
        });
        // -----------------------------------------------------------------
        builder.addCase(setWithdrawFilter.pending, (state) => {
            state.transactionWithdrawFilterStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setWithdrawFilter.fulfilled, (state, action) => {
            const currentPayload = action.payload as PagedVMResponse<ReservationIssueFilterResponse>;
            state.transactionWithdrawFilter = getJoinedPagedVM(state.transactionWithdrawFilter, currentPayload.response, currentPayload.appendType);
            state.transactionWithdrawFilterStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setWithdrawFilter.rejected, (state, action) => {
            state.transactionWithdrawFilterStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectTransactionFilter = (state: RootState) => state.transaction.transactionFilter;
export const selectTransactionFilterStatus = (state: RootState) => state.transaction.transactionFilterStatus;
export const selectWithdrawTransactionFilter = (state: RootState) => state.transaction.transactionWithdrawFilter;
export const selectWithdrawTransactionFilterStatus = (state: RootState) => state.transaction.transactionWithdrawFilterStatus;
export const selectCompleteTransactionStatus = (state: RootState) => state.transaction.completeTrasactionStatus;

export default transactionSlice.reducer;