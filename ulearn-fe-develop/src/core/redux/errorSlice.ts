import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RootState } from "./store";
import { updateProfileSettingsInfo, UpdateUserInfoErrorResponseBody } from "./userSlice";

interface IErrorState {
    errorMessage: string | undefined
}

const initialState: IErrorState = {
    errorMessage: undefined
};

export const errorSlice = createSlice({
    name: "error",
    initialState,
    reducers: {
        setError(state, action: PayloadAction<string | undefined>) {
            state.errorMessage = action.payload;
        }
    },
    extraReducers: (builder) => {
        builder.addCase(updateProfileSettingsInfo.pending, (state, action) => {
            state.errorMessage = undefined;
        });
        builder.addCase(updateProfileSettingsInfo.fulfilled, (state, action) => {
            state.errorMessage = undefined;
        });
        builder.addCase(updateProfileSettingsInfo.rejected, (state, action) => {
            const payload = action.payload as UpdateUserInfoErrorResponseBody;
            if (payload.message === undefined) {
                return;
            }
            if (payload.message === null) {
                return;
            }
            state.errorMessage = payload.message;
        });
    }
});

export const selectErrorMessage = (state: RootState) => state.error.errorMessage;
export const { setError } = errorSlice.actions;
export default errorSlice.reducer;