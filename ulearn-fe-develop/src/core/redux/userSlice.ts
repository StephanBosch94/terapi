import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { LoadingStatusEnum } from "../constants/enums/LoadingStatusEnum";
import { paths } from "../../apiTypes";
import { AppDispatch, RootState } from "./store";
import UserService, { ChangePasswordRequest, ChangePasswordResponse, ResetPasswordRequest, ResetPasswordResponse } from "../services/UserService";
import { ErrorResponse, IErrorCode, OpponentActivityMessageVM, PagedVMRequest, PagedVMResponse } from "core/services/Types/AppTypes";
import { setError } from "./errorSlice";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { getJoinedPagedVM } from "core/services/Utils/paginationUtil";
import { ActivityTypeEnum } from "core/constants/enums/ActivityTypeEnum";
import UserFileService from "core/services/UserFileService";

export type UserInfoResponse = paths['/api/user']['get']['responses']['200']['content']['application/json'];
export type UserShortInfoResponse = paths['/api/user/short-info']['get']['responses']['200']['content']['application/json'];
export type UpdateUserInfoResponse = paths['/api/user']['put']['responses']['200']
export type UpdateUserInfoRequestBody = paths['/api/user']['put']['requestBody']['content']['application/json']
export type UpdateUserInfoErrorResponseBody = paths['/api/user']['put']['responses']['409']['content']['application/json']
export type FilterTutorsRequestBody = paths['/api/user/tutor/filter']['post']['requestBody']['content']['application/json'];
export type FilterTutorsResponseBody = paths['/api/user/tutor/filter']['post']['responses']['200']['content']['application/json'];
export type FilterStudentsRequestBody = paths['/api/user/student/filter']['post']['requestBody']['content']['application/json'];
export type FilterStudentsResponseBody = paths['/api/user/student/filter']['post']['responses']['200']['content']['application/json'];
export type FilterEmployeesRequestBody = paths['/api/user/employee/filter']['post']['requestBody']['content']['application/json'];
export type FilterEmployeesResponseBody = paths['/api/user/employee/filter']['post']['responses']['200']['content']['application/json'];
export type GetMyStudentsResponseBody = paths['/api/user/students']['get']['responses']['200']['content']['application/json'];
export type GetMyTutorsResponseBody = paths['/api/user/tutors']['get']['responses']['200']['content']['application/json'];
export type TutorAdminInfoResponseBody = paths['/api/user/admin/{userId}']['get']['responses']['200']['content']['application/json'];
export type TutorAdminInfoParams = paths['/api/user/admin/{userId}']['get']['parameters']['path'];
export type PublicTutorInfoResponse = paths['/api/user/{userId}']['get']['responses']['200']['content']['application/json'];
export type DeleteFileRequestParams = paths['/api/user-file/filepath/{filePath}']['delete']['parameters']['path'];
export type ApproveUserResponse = paths['/api/user/{userId}']['put']['responses']['200'];
export type ChangeLanguageRequestParams = paths['/api/user/change-language/{languageCode}']['put']['parameters']['path'];
export type ChangeLanguageResponse = paths['/api/user/change-language/{languageCode}']['put']['responses']['200'];
export type DeleteEmployeeAccountRequest = paths['/api/user/employee']['delete']['requestBody']['content']['application/json'];
export type DeleteEmployeeAccountResponse = paths['/api/user/employee']['delete']['responses']['204'];
export type RefuseUserResponse = paths['/api/user/{userId}/refuse']['put']['responses']['200'];
export type EditEmpoyeeAccountRequest = paths['/api/user/employee']['put']['requestBody']['content']['application/json'];
export type EditEmpoyeeResponse = paths['/api/user/employee']['put']['responses']['204'];
export type AdminEditAccountRequest = paths['/api/user/admin']['put']['requestBody']['content']['application/json'];
export type AdminEditAccountResponse = paths['/api/user/admin']['put']['responses']['204'];

interface IUserState {
    errorCodes?: string[] | undefined,
    userInfo?: UserInfoResponse | undefined,
    userStatus?: LoadingStatusEnum | undefined,
    tutorAdminInfo?: TutorAdminInfoResponseBody | undefined,
    tutorAdminInfoStatus?: LoadingStatusEnum | undefined,
    userInfoFiles?: UserInfoResponse['files'] | undefined,
    userInfoFilesStatus?: LoadingStatusEnum | undefined,
    updateUserInfoStatus?: LoadingStatusEnum | undefined,
    setProfileInValidationStatus?: LoadingStatusEnum | undefined,
    userShortInfo?: UserShortInfoResponse | undefined,
    userShortInfoStatus?: LoadingStatusEnum | undefined,
    tutors?: FilterTutorsResponseBody | undefined,
    tutorsStatus?: LoadingStatusEnum | undefined,
    students?: FilterStudentsResponseBody | undefined,
    studentsStatus?: LoadingStatusEnum | undefined,
    employeesFilter?: FilterEmployeesResponseBody | undefined,
    employeesFilterStatus?: LoadingStatusEnum | undefined,
    tutorInfo?: PublicTutorInfoResponse | undefined,
    tutorInfoStatus?: LoadingStatusEnum | undefined,
    myTutors?: GetMyTutorsResponseBody | undefined,
    myTutorsStatus?: LoadingStatusEnum | undefined,
    myStudents?: GetMyStudentsResponseBody | undefined,
    myStudentsStatus?: LoadingStatusEnum | undefined,
    uploadProfilePhotoStatus?: LoadingStatusEnum | undefined,
    uploadCertificatesStatus?: LoadingStatusEnum | undefined,
    deleteUserFileStatus?: LoadingStatusEnum | undefined,
    approveUserStatus?: LoadingStatusEnum | undefined,
    refuseUserStatus?: LoadingStatusEnum | undefined,
    changeLanguageStatus?: LoadingStatusEnum | undefined,
    changePasswordStatus?: LoadingStatusEnum | undefined,
    resetPasswordStatus?: LoadingStatusEnum | undefined,
    deleteEmployeeAccountStatus?: LoadingStatusEnum | undefined,
    editEmployeeAccountStatus?: LoadingStatusEnum | undefined,
    adminEditAccountStatus?: LoadingStatusEnum | undefined
}

export const setUserInfo = createAsyncThunk<UserInfoResponse>(
    "user/setUserInfo",
    async (requestBody, { rejectWithValue }) => {
        try {
            const result = await UserService.getUserInfo();
            return result.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setStudents = createAsyncThunk<GetMyStudentsResponseBody>(
    "user/setStudents",
    async (requestBody, { rejectWithValue }) => {
        try {
            const result = await UserService.getStudents();
            return result.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setTutors = createAsyncThunk<GetMyTutorsResponseBody>(
    "user/setTutors",
    async (requestBody, { rejectWithValue }) => {
        try {
            const result = await UserService.getTutors();
            return result.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setShortUserInfo = createAsyncThunk<UserShortInfoResponse | ErrorResponse, undefined, { dispatch: AppDispatch, state: RootState }>(
    "user/setUserShortInfo",
    async (param, { dispatch, rejectWithValue }) => {
        try {
            const result = await UserService.getShortUserInfo();
            return result.data;
        } catch (e: any) {
            const errorResponse = e.response.data as ErrorResponse;
            dispatch(setError(TRANSLATION_KEYS.ERRORS.FAILED_TO_GET_USER_INFO));
            return rejectWithValue(errorResponse);
        }
    }
);

export const updateUserShortInfoStatistics = createAsyncThunk<UserShortInfoResponse | ErrorResponse, undefined, { dispatch: AppDispatch, state: RootState }>(
    "user/updateUserShortInfoCountOfUnreadMessages",
    async (param, { dispatch, rejectWithValue }) => {
        try {
            const result = await UserService.getShortUserInfo();
            return result.data;
        } catch (e: any) {
            const errorResponse = e.response.data as ErrorResponse;
            dispatch(setError(TRANSLATION_KEYS.ERRORS.FAILED_TO_GET_USER_INFO));
            return rejectWithValue(errorResponse);
        }
    }
);

export const setProfileInValidation = createAsyncThunk<UserInfoResponse | UpdateUserInfoErrorResponseBody, UpdateUserInfoRequestBody>(
    "user/updateUserInfo/in-validation",
    async (requestBody, { rejectWithValue }) => {
        try {
            await UserService.updateUserInfo(requestBody);
            const result = await UserService.getUserInfo();
            return result.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as UpdateUserInfoErrorResponseBody);
        }
    }
);

export const updateProfileSettingsInfo = createAsyncThunk<UserInfoResponse | UpdateUserInfoErrorResponseBody, UpdateUserInfoRequestBody>(
    "user/updateUserInfo",
    async (requestBody, { rejectWithValue }) => {
        try {
            await UserService.updateUserInfo(requestBody);
            const result = await UserService.getUserInfo();
            return result.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as UpdateUserInfoErrorResponseBody);
        }
    }
);

export const updateEmployeeAccountInfo = createAsyncThunk<EditEmpoyeeResponse | ErrorResponse, EditEmpoyeeAccountRequest>(
    "user/updateEmployeeAccountInfo",
    async (requestBody, { rejectWithValue }) => {
        try {
            return await UserService.editEmployeeAccountInfo(requestBody);
        } catch (e: any) {
            return rejectWithValue(e.response.data as UpdateUserInfoErrorResponseBody);
        }
    }
);

export const uploadProfilePhoto = createAsyncThunk<UserInfoResponse | ErrorResponse, File>('user-file/profile-photo/upload',
    async (requestBody, { rejectWithValue }) => {
        try {
            await UserFileService.uploadProfilePhoto(requestBody);
            const result = await UserService.getUserInfo();
            return result.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const uploadCertificates = createAsyncThunk<UserInfoResponse | ErrorResponse, File[]>('user-file/certificates/upload',
    async (requestBody, { rejectWithValue }) => {
        try {
            await UserFileService.uploadCetrificates(requestBody);
            const result = await UserService.getUserInfo();
            return result.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const deleteUserFile = createAsyncThunk<UserInfoResponse | ErrorResponse, DeleteFileRequestParams>('user-file/delete',
    async (requestBody, { rejectWithValue }) => {
        try {
            await UserFileService.deleteUserFile(requestBody);
            const result = await UserService.getUserInfo();
            return result.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setTutorsFilter = createAsyncThunk<PagedVMResponse<FilterTutorsResponseBody> | ErrorResponse, PagedVMRequest<FilterTutorsRequestBody>>(
    "user/setTutorsFilter",
    async (request, { rejectWithValue }) => {
        try {
            const result = await UserService.filterTutors(request.request);
            return {
                response: result.data,
                appendType: request.appendType
            };
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setStudentsFilter = createAsyncThunk<PagedVMResponse<FilterTutorsResponseBody> | ErrorResponse, PagedVMRequest<FilterTutorsRequestBody>>(
    "user/setStudentsFilter",
    async (request, { rejectWithValue }) => {
        try {
            const result = await UserService.filterStudents(request.request);
            return {
                response: result.data,
                appendType: request.appendType
            };
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setEmployeesFilter = createAsyncThunk<PagedVMResponse<FilterEmployeesResponseBody> | ErrorResponse, PagedVMRequest<FilterEmployeesRequestBody>>(
    "user/setEmployeesFilter",
    async (request, { rejectWithValue }) => {
        try {
            const result = await UserService.filterEmployees(request.request);
            return {
                response: result.data,
                appendType: request.appendType
            };
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setPublicTutorInfo = createAsyncThunk<PublicTutorInfoResponse | ErrorResponse, { userId: number }>(
    "user/setPublicTutorInfo",
    async (requestBody, { rejectWithValue }) => {
        try {
            const result = await UserService.getPublicTutorInfoById(requestBody.userId);
            return result.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setTutorAdminInfo = createAsyncThunk<TutorAdminInfoResponseBody | ErrorResponse, TutorAdminInfoParams>(
    "user/setTutorAdminInfo",
    async (requestBody, { rejectWithValue }) => {
        try {
            const result = await UserService.getTutorAdminInfo(requestBody);
            return result.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const approveUser = createAsyncThunk<ApproveUserResponse | ErrorResponse, { userId: number }>(
    "user/approve",
    async (requestBody, { rejectWithValue }) => {
        try {
            await UserService.approve(requestBody.userId);
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const refuseUser = createAsyncThunk<RefuseUserResponse | ErrorResponse, { userId: number }>(
    "user/refuse",
    async (requestBody, { rejectWithValue }) => {
        try {
            await UserService.refuse(requestBody.userId);
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setChangeLanguage = createAsyncThunk<ChangeLanguageResponse | ErrorResponse, ChangeLanguageRequestParams>(
    "user/changeLanguage",
    async (requestBody, { rejectWithValue }) => {
        try {
            await UserService.changeLanguage(requestBody);
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setChangePassword = createAsyncThunk<ChangePasswordResponse | ErrorResponse, ChangePasswordRequest>(
    "user/changePassword",
    async (requestBody, { rejectWithValue }) => {
        try {
            await UserService.changePassword(requestBody);
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setResetPassword = createAsyncThunk<ResetPasswordResponse | ErrorResponse, ResetPasswordRequest & { token: string }>(
    "user/resetPassword",
    async (requestBody, { rejectWithValue }) => {
        try {
            await UserService.resetPassword({ newPassword: requestBody.newPassword }, requestBody.token);
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const deleteEmployeeAccount = createAsyncThunk<DeleteEmployeeAccountResponse | ErrorResponse, DeleteEmployeeAccountRequest>(
    "user/deleteEmployeesAccount",
    async (requestBody, { rejectWithValue }) => {
        try {
            return await UserService.deleteEmployeeAccount(requestBody);
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);


export const adminEditAccountAccount = createAsyncThunk<AdminEditAccountResponse | ErrorResponse, AdminEditAccountRequest>(
    "user/adminEditAccountAccount",
    async (requestBody, { rejectWithValue }) => {
        try {
            return await UserService.adminEditAccount(requestBody);
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

const initialState: IUserState = {};

export const userSlice = createSlice({
    name: "user",
    initialState,
    reducers: {
        emptyTutorFilter(state) {
            state.tutors = undefined;
        },
        updateErrors(state, action: PayloadAction<string[]>) {
            state.errorCodes = action.payload;
        },
        updateShortUserInfo(state, action: PayloadAction<UserShortInfoResponse>) {
            state.userShortInfo = action.payload;
            state.userShortInfoStatus = LoadingStatusEnum.Loaded;
        },
        setOpponentJoinInfo(state, action: PayloadAction<OpponentActivityMessageVM>) {
            const joined = action.payload.activityTypeId >= ActivityTypeEnum.Join;
            if (state.userShortInfo?.runningActivity) {
                state.userShortInfo.runningActivity.isOpponentJoined = joined;
            }
        },
        removeShortUserInfo(state) {
            state.userShortInfo = undefined;
        },
        updateShortUserInfoNotificationsRead(state) {
            if (!state.userShortInfo) return;
            state.userShortInfo.countOfUnreadNotifications = 0;
        },
        updateShortUserInfoNotificationsOneMoreRead(state) {
            if (!state.userShortInfo) return;
            if (!state.userShortInfo.countOfUnreadNotifications) return;
            state.userShortInfo.countOfUnreadNotifications -= 1;
        }
    },
    extraReducers: (builder) => {
        builder.addCase(updateProfileSettingsInfo.pending, (state) => {
            state.userStatus = LoadingStatusEnum.Loading;
            state.userInfoFilesStatus = LoadingStatusEnum.Loading;
            state.updateUserInfoStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(updateProfileSettingsInfo.fulfilled, (state, action) => {
            const response = action.payload as UserInfoResponse;
            state.userInfo = response;
            state.userInfoFiles = response.files;
            state.deleteUserFileStatus = LoadingStatusEnum.Loaded;
            state.updateUserInfoStatus = LoadingStatusEnum.Loaded;
            state.userStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(updateProfileSettingsInfo.rejected, (state, action) => {
            const payload = action.payload as UpdateUserInfoErrorResponseBody;
            if (payload !== undefined && payload !== null) {
                state.errorCodes = Object.keys(payload!.parameters!);
            }
            state.userStatus = LoadingStatusEnum.Loaded; // set loaded to not to showing error
            state.updateUserInfoStatus = LoadingStatusEnum.Error; // set only profileInValidationStatus since we don't want to dispay an error page
        });
        /* ----------------------------------------------- */
        builder.addCase(setProfileInValidation.pending, (state) => {
            state.userStatus = LoadingStatusEnum.Loading;
            state.setProfileInValidationStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setProfileInValidation.fulfilled, (state, action) => {
            state.userInfo = action.payload as UserInfoResponse;
            state.setProfileInValidationStatus = LoadingStatusEnum.Loaded;
            state.userStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setProfileInValidation.rejected, (state, action) => {
            const payload = action.payload as UpdateUserInfoErrorResponseBody;
            if (payload !== undefined && payload !== null) {
                state.errorCodes = Object.keys(payload!.parameters!);
            }
            state.setProfileInValidationStatus = LoadingStatusEnum.Error; // set only profileInValidationStatus since we don't want to dispay an error page
            state.userStatus = LoadingStatusEnum.Loaded; // set loaded to not to showing error
        });
        /* ----------------------------------------------- */
        builder.addCase(uploadProfilePhoto.pending, (state) => {
            state.uploadProfilePhotoStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(uploadProfilePhoto.fulfilled, (state, action) => {
            const response = action.payload as UserInfoResponse;
            state.userInfoFiles = response.files;
            state.uploadProfilePhotoStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(uploadProfilePhoto.rejected, (state) => {
            state.uploadProfilePhotoStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(uploadCertificates.pending, (state) => {
            state.uploadCertificatesStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(uploadCertificates.fulfilled, (state, action) => {
            state.userInfo = action.payload as UserInfoResponse;
            state.uploadCertificatesStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(uploadCertificates.rejected, (state) => {
            state.uploadCertificatesStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(deleteUserFile.pending, (state) => {
            state.deleteUserFileStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(deleteUserFile.fulfilled, (state, action) => {
            state.userInfo = action.payload as UserInfoResponse;
            state.deleteUserFileStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(deleteUserFile.rejected, (state) => {
            state.deleteUserFileStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setUserInfo.pending, (state) => {
            state.userStatus = LoadingStatusEnum.Loading;
            state.userInfoFilesStatus = LoadingStatusEnum.Loading;
            state.userInfoFilesStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setUserInfo.fulfilled, (state, action) => {
            state.userInfo = action.payload;
            state.userInfoFiles = action.payload.files;
            state.userStatus = LoadingStatusEnum.Loaded;
            state.userInfoFilesStatus = LoadingStatusEnum.Loaded;
            state.userInfoFilesStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setUserInfo.rejected, (state) => {
            state.userStatus = LoadingStatusEnum.Error;
            state.userInfoFilesStatus = LoadingStatusEnum.Error;
            state.userInfoFilesStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setShortUserInfo.pending, (state, action) => {
            state.userShortInfoStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setShortUserInfo.fulfilled, (state, action) => {
            state.userShortInfo = action.payload as UserShortInfoResponse;
            state.userShortInfoStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setShortUserInfo.rejected, (state, action) => {
            state.userShortInfoStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setTutorAdminInfo.pending, (state, action) => {
            state.tutorAdminInfoStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setTutorAdminInfo.fulfilled, (state, action) => {
            const response = action.payload as TutorAdminInfoResponseBody;
            state.tutorAdminInfo = response;
            state.tutorAdminInfoStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setTutorAdminInfo.rejected, (state, action) => {
            state.tutorAdminInfoStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(updateUserShortInfoStatistics.fulfilled, (state, action) => {
            const response = action.payload as UserShortInfoResponse;
            if (!state.userShortInfo) {
                state.userShortInfo = response;
                return;
            }
            state.userShortInfo!.countOfUnreadMessages = response.countOfUnreadMessages;
            state.userShortInfo!.countOfUnreadNotifications = response.countOfUnreadNotifications;
        });
        /* ----------------------------------------------- */
        builder.addCase(setTutorsFilter.pending, (state, action) => {
            state.tutorsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setTutorsFilter.fulfilled, (state, action) => {
            const payload = action.payload as PagedVMResponse<FilterTutorsResponseBody>;
            const joined = getJoinedPagedVM(state.tutors, payload.response, payload.appendType);
            state.tutors = joined;
            state.tutorsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setTutorsFilter.rejected, (state, action) => {
            state.tutors = undefined;
            state.tutorsStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setStudentsFilter.pending, (state, action) => {
            state.studentsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setStudentsFilter.fulfilled, (state, action) => {
            const payload = action.payload as PagedVMResponse<FilterStudentsResponseBody>;
            const joined = getJoinedPagedVM(state.students, payload.response, payload.appendType);
            state.students = joined;
            state.studentsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setStudentsFilter.rejected, (state, action) => {
            state.students = undefined;
            state.studentsStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setEmployeesFilter.pending, (state, action) => {
            state.employeesFilterStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setEmployeesFilter.fulfilled, (state, action) => {
            const payload = action.payload as PagedVMResponse<FilterEmployeesResponseBody>;
            const joined = getJoinedPagedVM(state.employeesFilter, payload.response, payload.appendType);
            state.employeesFilter = joined;
            state.employeesFilterStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setEmployeesFilter.rejected, (state, action) => {
            state.employeesFilter = undefined;
            state.employeesFilterStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setPublicTutorInfo.pending, (state, action) => {
            state.tutorInfo = undefined;
            state.tutorInfoStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setPublicTutorInfo.fulfilled, (state, action) => {
            state.tutorInfo = action.payload as PublicTutorInfoResponse;
            state.tutorInfoStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setPublicTutorInfo.rejected, (state, action) => {
            state.tutorInfo = undefined;
            state.tutorInfoStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setStudents.pending, (state, action) => {
            state.myStudentsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setStudents.fulfilled, (state, action) => {
            state.myStudents = action.payload as GetMyStudentsResponseBody;
            state.myStudentsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setStudents.rejected, (state, action) => {
            state.myStudentsStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setTutors.pending, (state, action) => {
            state.myTutorsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setTutors.fulfilled, (state, action) => {
            state.myTutors = action.payload as GetMyTutorsResponseBody;
            state.myTutorsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setTutors.rejected, (state, action) => {
            state.myTutorsStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(approveUser.pending, (state, action) => {
            state.approveUserStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(approveUser.fulfilled, (state, action) => {
            state.approveUserStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(approveUser.rejected, (state, action) => {
            state.approveUserStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(refuseUser.pending, (state, action) => {
            state.refuseUserStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(refuseUser.fulfilled, (state, action) => {
            state.refuseUserStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(refuseUser.rejected, (state, action) => {
            state.refuseUserStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setChangeLanguage.pending, (state, action) => {
            state.changeLanguageStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setChangeLanguage.fulfilled, (state, action) => {
            state.changeLanguageStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setChangeLanguage.rejected, (state, action) => {
            state.changeLanguageStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setChangePassword.pending, (state, action) => {
            state.changePasswordStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setChangePassword.fulfilled, (state, action) => {
            state.changePasswordStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setChangePassword.rejected, (state, action) => {
            state.changePasswordStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(setResetPassword.pending, (state, action) => {
            state.resetPasswordStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setResetPassword.fulfilled, (state, action) => {
            state.resetPasswordStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setResetPassword.rejected, (state, action) => {
            state.resetPasswordStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(deleteEmployeeAccount.pending, (state, action) => {
            state.deleteEmployeeAccountStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(deleteEmployeeAccount.fulfilled, (state, action) => {
            state.deleteEmployeeAccountStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(deleteEmployeeAccount.rejected, (state, action) => {
            state.deleteEmployeeAccountStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(updateEmployeeAccountInfo.pending, (state, action) => {
            state.editEmployeeAccountStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(updateEmployeeAccountInfo.fulfilled, (state, action) => {
            state.editEmployeeAccountStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(updateEmployeeAccountInfo.rejected, (state, action) => {
            state.editEmployeeAccountStatus = LoadingStatusEnum.Error;
        });
        /* ----------------------------------------------- */
        builder.addCase(adminEditAccountAccount.pending, (state, action) => {
            state.adminEditAccountStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(adminEditAccountAccount.fulfilled, (state, action) => {
            state.adminEditAccountStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(adminEditAccountAccount.rejected, (state, action) => {
            state.adminEditAccountStatus = LoadingStatusEnum.Error;
        });
    }
});


export const selectShortUserInfoStatus = (state: RootState) => state.user.userShortInfoStatus;
export const selectShortUserInfo = (state: RootState) => state.user.userShortInfo;

// User info
export const selectUserInfoStatus = (state: RootState) => state.user.userStatus;
export const selectUserInfo = (state: RootState) => state.user.userInfo;
export const selectUserInfoFilesStatus = (state: RootState) => state.user.userInfoFilesStatus;
export const selectUserInfoFiles = (state: RootState) => state.user.userInfoFiles;
export const selectUserInfoErrors = (state: RootState) => state.user.errorCodes;
export const selectFilteredTutors = (state: RootState) => state.user.tutors;
export const selectFilterTutorsStatus = (state: RootState) => state.user.tutorsStatus;

export const selectFilteredStudents = (state: RootState) => state.user.students;
export const selectFilterStudentsStatus = (state: RootState) => state.user.studentsStatus;

// Tutor Info Selectors
export const selectTutorInfo = (state: RootState) => state.user.tutorInfo;
export const selectTutorInfoStatus = (state: RootState) => state.user.tutorInfoStatus;

export const selectTutorAdminInfo = (state: RootState) => state.user.tutorAdminInfo;
export const selectTutorAdminInfoStatus = (state: RootState) => state.user.tutorAdminInfoStatus;

export const selectMyStudents = (state: RootState) => state.user.myStudents;
export const selectMyStudentsStatus = (state: RootState) => state.user.myStudentsStatus;

export const selectEmployeesFilter = (state: RootState) => state.user.employeesFilter;
export const selectEmployeesFilterStatus = (state: RootState) => state.user.employeesFilterStatus;

export const selectMyTutors = (state: RootState) => state.user.myTutors;
export const selectMyTutorsStatus = (state: RootState) => state.user.myTutorsStatus;
export const selectUploadProfilePhotoStatus = (state: RootState) => state.user.uploadProfilePhotoStatus;
export const selectDeleteUserFileStatus = (state: RootState) => state.user.deleteUserFileStatus;
export const selectUploadCertificatesStatus = (state: RootState) => state.user.uploadCertificatesStatus;
export const selectApproveUserStatus = (state: RootState) => state.user.approveUserStatus;
export const selectRefuseUserStatus = (state: RootState) => state.user.refuseUserStatus;
export const selectChangeLanguageStatus = (state: RootState) => state.user.changeLanguageStatus;
export const selectChangePasswordStatus = (state: RootState) => state.user.changePasswordStatus;
export const selectResetPasswordStatus = (state: RootState) => state.user.resetPasswordStatus;
export const selectUpdateUserInfoStatus = (state: RootState) => state.user.updateUserInfoStatus;
export const selectDeleteEmployeeAccountStatus = (state: RootState) => state.user.deleteEmployeeAccountStatus;
export const selectEditEmployeeAccountStatus = (state: RootState) => state.user.editEmployeeAccountStatus;
export const selectAdminEditAccountStatus = (state: RootState) => state.user.adminEditAccountStatus;

export const { emptyTutorFilter, updateErrors, setOpponentJoinInfo, updateShortUserInfo, removeShortUserInfo, updateShortUserInfoNotificationsRead, updateShortUserInfoNotificationsOneMoreRead } = userSlice.actions;
export default userSlice.reducer;