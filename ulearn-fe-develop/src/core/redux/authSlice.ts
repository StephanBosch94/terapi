import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { AppDispatch, RootState } from "./store";
import AuthService, { ExternalProviderRequestType, IExternalProviderAuthResponse } from "core/services/AuthService";
import { ErrorResponse } from "core/services/Types/AppTypes";
import { ParsedClaimsType } from "core/services/Utils/tokenUtil";
import { paths } from "apiTypes";

export type GoogleAuthResponse = paths['/api/auth/login/google']['post']['responses']['200'];
export type GoogleAuthRequest = paths['/api/auth/login/google']['post']['requestBody']['content']['application/json'];
export type GoogleOneTapAuthResponse = paths['/api/auth/login/google-one-tap']['post']['responses']['200'];
export type GoogleOneTapAuthRequest = paths['/api/auth/login/google-one-tap']['post']['requestBody']['content']['application/json'];
export type InternalAuthResponse = paths['/api/auth/login']['post']['responses']['200'];
export type InternalAuthRequest = paths['/api/auth/login']['post']['requestBody']['content']['application/json'];

interface IAuthState {
    status?: LoadingStatusEnum | undefined,
    internalAuthStatus?: LoadingStatusEnum | undefined,
    errorCodes?: string | undefined,
    claims?: ParsedClaimsType,
    isAuthorized?: boolean | undefined,
}

const initialState: IAuthState = {};

export type ExternalApiProviderRequest = {
    request: ExternalProviderRequestType;
    url: string
}

export const setLogOut = createAsyncThunk(
    "auth/logOut",
    async (requestBody, { rejectWithValue }) => {
        try {
            return await AuthService.logOut();
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setAuthUsingGoogleApi = createAsyncThunk<GoogleAuthResponse, GoogleAuthRequest, { dispatch: AppDispatch, state: RootState }>(
    "auth/google",
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await AuthService.googleAuth(requestBody);
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setAuthUsingGoogleOneTapApi = createAsyncThunk<GoogleOneTapAuthResponse, GoogleOneTapAuthRequest, { dispatch: AppDispatch, state: RootState }>(
    "auth/google-one-tap",
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await AuthService.googleOneTapAuth(requestBody);
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setAuthUsingExternalApi = createAsyncThunk<IExternalProviderAuthResponse, ExternalApiProviderRequest, { dispatch: AppDispatch, state: RootState }>(
    "auth/external",
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await AuthService.externalProviderAuth(requestBody.request, requestBody.url);
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setAuthUsingInternalLogin = createAsyncThunk<InternalAuthResponse | ErrorResponse, InternalAuthRequest, { dispatch: AppDispatch, state: RootState }>(
    "auth/login",
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await AuthService.login(requestBody);
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
        updateClaims(state, action: PayloadAction<ParsedClaimsType>) {
            state.claims = action.payload;
            state.isAuthorized = action.payload !== undefined;
        },
        updateIsAuthorized(state, action: PayloadAction<boolean>){
            state.isAuthorized = action.payload;
        }
    },
    extraReducers: (builder) => {
        builder.addCase(setAuthUsingGoogleApi.pending, (state) => {
            if (state.status === undefined || state.status === null) {
                state.status = LoadingStatusEnum.Loading;
            }
        });
        builder.addCase(setAuthUsingGoogleApi.fulfilled, (state, action) => {
            state.status = LoadingStatusEnum.Loaded;
            state.isAuthorized = true;
        });
        builder.addCase(setAuthUsingGoogleApi.rejected, (state, action) => {
            state.status = LoadingStatusEnum.Error;
        });
        //----------------------------------------------------------
        builder.addCase(setAuthUsingGoogleOneTapApi.pending, (state) => {
            if (state.status === undefined || state.status === null) {
                state.status = LoadingStatusEnum.Loading;
            }
        });
        builder.addCase(setAuthUsingGoogleOneTapApi.fulfilled, (state, action) => {
            state.status = LoadingStatusEnum.Loaded;
            state.isAuthorized = true;
        });
        builder.addCase(setAuthUsingGoogleOneTapApi.rejected, (state, action) => {
            state.status = LoadingStatusEnum.Error;
        });
        //----------------------------------------------------------
        builder.addCase(setAuthUsingExternalApi.pending, (state) => {
            if (state.status === undefined || state.status === null) {
                state.status = LoadingStatusEnum.Loading;
            }
        });
        builder.addCase(setAuthUsingExternalApi.fulfilled, (state, action) => {
            state.status = LoadingStatusEnum.Loaded;
            state.isAuthorized = true;
        });
        builder.addCase(setAuthUsingExternalApi.rejected, (state, action) => {
            state.status = LoadingStatusEnum.Error;
        });
        //----------------------------------------------------------
        builder.addCase(setAuthUsingInternalLogin.pending, (state) => {
            if (state.status === undefined || state.status === null) {
                state.internalAuthStatus = LoadingStatusEnum.Loading;
            }
        });
        builder.addCase(setAuthUsingInternalLogin.fulfilled, (state, action) => {
            state.internalAuthStatus = LoadingStatusEnum.Loaded;
            state.isAuthorized = true;
        });
        builder.addCase(setAuthUsingInternalLogin.rejected, (state, action) => {
            state.internalAuthStatus = LoadingStatusEnum.Error;
            const errorResponse = action.payload as ErrorResponse;
            state.errorCodes = errorResponse.message;
        });
    }
});

export const selectIsAuthorized = (state: RootState) => state.auth.isAuthorized;
export const selectAuthStatus = (state: RootState) => state.auth.status;
export const selectInternalAuthStatus = (state: RootState) => state.auth.internalAuthStatus;
export const selectErrorCode = (state: RootState) => state.auth.errorCodes;
export const selectUserRoleId = (state: RootState) => state.auth.claims?.userRoleId;
export const selectUserId = (state: RootState) => state.auth.claims?.userId;
export const selectUserStateId = (state: RootState) => state.auth.claims?.userStateId;

export const { updateClaims, updateIsAuthorized } = authSlice.actions;
export default authSlice.reducer;