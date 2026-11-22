import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { AppDispatch, RootState } from "./store";
import { ErrorResponse } from "core/services/Types/AppTypes";
import RegistrationService,
{
    ActivateEmailRequest,
    ActivateEmailResponse,
    ForgottenPasswordRequest,
    ForgottenPasswordResponse,
    InviteColleaguesRequest,
    InviteColleaguesResponse,
    RegistrationRequest,
    RegistrationResponse,
    ResendCodeLatestInfoResponse,
    ResendCodeResponse
} from "core/services/RegistrationService";

export type RegisterErrorType = "already_exists";

type RegistrationSliceStateType = {
    registerStatus?: LoadingStatusEnum | undefined,
    registerErrorType?: RegisterErrorType | undefined,
    resendEmailStatus?: LoadingStatusEnum | undefined,
    resendEmailStatusCode?: number | undefined,
    activateStatus?: LoadingStatusEnum | undefined,
    sendedCodeLatestInfo?: ResendCodeLatestInfoResponse | undefined,
    sendedCodeLatestInfoStatus?: LoadingStatusEnum | undefined,
    forgottenPasswordStatus?: LoadingStatusEnum | undefined,
    inviteColleaguesStatus?: LoadingStatusEnum | undefined
}

const initialState: RegistrationSliceStateType = {};

export const setRegister = createAsyncThunk<RegistrationResponse | ErrorResponse, RegistrationRequest, { dispatch: AppDispatch, state: RootState }>(
    "registration/register",
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await RegistrationService.register(requestBody);
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setResendCode = createAsyncThunk<ResendCodeResponse | ErrorResponse, void, { dispatch: AppDispatch, state: RootState }>(
    "registration/resendCode",
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await RegistrationService.resendCode();
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setActivateEmailByLink = createAsyncThunk<ActivateEmailResponse | ErrorResponse, { token: string } & ActivateEmailRequest, { dispatch: AppDispatch, state: RootState }>(
    "registration/link/activate",
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await RegistrationService.linkActivate(requestBody);
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setActivateEmail = createAsyncThunk<ActivateEmailResponse | ErrorResponse, ActivateEmailRequest, { dispatch: AppDispatch, state: RootState }>(
    "registration/activate",
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await RegistrationService.activate(requestBody);
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setSendedCodeLatestInfo = createAsyncThunk<ResendCodeLatestInfoResponse | ErrorResponse, void, { dispatch: AppDispatch, state: RootState }>(
    "registration/setSendedCodeLatestInfo",
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await RegistrationService.getCodeInfo();
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setForgottenPasword = createAsyncThunk<ForgottenPasswordResponse | ErrorResponse, ForgottenPasswordRequest, { dispatch: AppDispatch, state: RootState }>(
    "registration/setForgottenPasword",
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await RegistrationService.forgottenPassword(requestBody);
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setInviteColleagues = createAsyncThunk<InviteColleaguesResponse | ErrorResponse, InviteColleaguesRequest, { dispatch: AppDispatch, state: RootState }>(
    "registration/setInviteColleagues",
    async (requestBody, { rejectWithValue }) => {
        try {
            const response = await RegistrationService.inviteColleagues(requestBody);
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const registrationSlice = createSlice({
    name: "registration",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(setRegister.pending, (state) => {
            state.registerStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setRegister.fulfilled, (state, action) => {
            state.registerStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setRegister.rejected, (state, action) => {
            state.registerStatus = LoadingStatusEnum.Error;
            const errorResponse = action.payload as ErrorResponse;
            state.registerErrorType = errorResponse?.status === 409 ? "already_exists" : undefined;
        });
        /* ------------------------------------------ */
        builder.addCase(setResendCode.pending, (state) => {
            state.resendEmailStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setResendCode.fulfilled, (state, action) => {
            state.resendEmailStatus = LoadingStatusEnum.Loaded;
            state.sendedCodeLatestInfo = action.payload as ResendCodeResponse;
        });
        builder.addCase(setResendCode.rejected, (state, action) => {
            const response = action.payload as ErrorResponse;
            state.resendEmailStatus = LoadingStatusEnum.Error;
            state.resendEmailStatusCode = response.status;
        });
        /* ------------------------------------------ */
        builder.addCase(setActivateEmailByLink.pending, (state) => {
            state.activateStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setActivateEmailByLink.fulfilled, (state, action) => {
            state.activateStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setActivateEmailByLink.rejected, (state, action) => {
            state.activateStatus = LoadingStatusEnum.Error;
        });
        /* ------------------------------------------ */
        builder.addCase(setActivateEmail.pending, (state) => {
            state.activateStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setActivateEmail.fulfilled, (state, action) => {
            state.activateStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setActivateEmail.rejected, (state, action) => {
            state.activateStatus = LoadingStatusEnum.Error;
        });
        /* ------------------------------------------ */
        builder.addCase(setSendedCodeLatestInfo.pending, (state) => {
            state.sendedCodeLatestInfoStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setSendedCodeLatestInfo.fulfilled, (state, action) => {
            state.sendedCodeLatestInfo = action.payload as ResendCodeLatestInfoResponse;
            state.sendedCodeLatestInfoStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setSendedCodeLatestInfo.rejected, (state, action) => {
            state.sendedCodeLatestInfoStatus = LoadingStatusEnum.Error;
        });
        /* ------------------------------------------ */
        builder.addCase(setForgottenPasword.pending, (state) => {
            state.forgottenPasswordStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setForgottenPasword.fulfilled, (state, action) => {
            state.forgottenPasswordStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setForgottenPasword.rejected, (state, action) => {
            state.forgottenPasswordStatus = LoadingStatusEnum.Error;
        });
        /* ------------------------------------------ */
        builder.addCase(setInviteColleagues.pending, (state) => {
            state.inviteColleaguesStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setInviteColleagues.fulfilled, (state, action) => {
            state.inviteColleaguesStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setInviteColleagues.rejected, (state, action) => {
            state.inviteColleaguesStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectRegisterStatus = (state: RootState) => state.registration.registerStatus;
export const selectRegisterErrorType = (state: RootState) => state.registration.registerErrorType;
export const selectResendEmailStatus = (state: RootState) => state.registration.resendEmailStatus;
export const selectResendEmailStatusCode = (state: RootState) => state.registration.resendEmailStatusCode;
export const selectActivateEmailStatus = (state: RootState) => state.registration.activateStatus;
export const selectSendedCodeLatestInfo = (state: RootState) => state.registration.sendedCodeLatestInfo;
export const selectSendedSendedCodeLatestInfoStatus = (state: RootState) => state.registration.sendedCodeLatestInfoStatus;
export const selectForgottenPasswordStatus = (state: RootState) => state.registration.forgottenPasswordStatus;
export const selectInviteColleaguesStatus = (state: RootState) => state.registration.inviteColleaguesStatus;

export default registrationSlice.reducer;