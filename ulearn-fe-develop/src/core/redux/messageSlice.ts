import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { paths } from "apiTypes";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import MessageService from "core/services/MessageService";
import { AppMessageVM, AppMessageWithConversationInpuVM, ErrorResponse } from "core/services/Types/AppTypes";
import { RootState } from "./store";

export type FilterMessagesResponse = paths['/api/message/conversation/{conversationId}/filter']['post']['responses']['200']['content']['application/json'];
export type FilterMessageRequest = paths['/api/message/conversation/{conversationId}/filter']['post']['requestBody']['content']['application/json'];
type FilterReponseWithOptions = {addToThePrevious: boolean, response: FilterMessagesResponse};

interface IMessagesSliceState{
    messages: FilterMessagesResponse | undefined,
    messagesStatus: LoadingStatusEnum | undefined,
    createStatus: LoadingStatusEnum | undefined
}

const initialState: IMessagesSliceState = {
    messages: undefined,
    messagesStatus: undefined,
    createStatus: undefined
};

export const setMessagesFilter = createAsyncThunk<FilterReponseWithOptions | ErrorResponse, {conversationId: number, addToThePrevious: boolean, filter: FilterMessageRequest}>(
    "messages/setMessagesFilter", async (requestBody, {rejectWithValue}) => {
        try{
            const response = await MessageService.filter(requestBody.conversationId, requestBody.filter);
            return {
                addToThePrevious: requestBody.addToThePrevious,
                response: response.data
            };
        } catch (e: any){
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const createMessageWithAttachment = createAsyncThunk<void | ErrorResponse, AppMessageWithConversationInpuVM>(
    "messages/createWithAttachment", async (requestBody, {rejectWithValue}) => {
        try{
            await MessageService.create(requestBody);
        } catch (e: any){
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const messageSlice = createSlice({
    name: "messages",
    initialState,
    reducers: {
        addMessage(state, action: PayloadAction<AppMessageVM>){
            if (!state.messages) return;
            const newMessageConversationId = action.payload.conversationId!;
            if (state.messages.data.length > 0 && state.messages.data[0].conversationId! !== newMessageConversationId) return;  
            state.messages = {
                size: (state?.messages?.size ?? 0) + 1,
                data: [action.payload, ...(state?.messages?.data ?? [])]
            } 
        },
        refreshMessages(state){
            state.messages = undefined;
        },
        resetCreateStatus(state){
            state.createStatus = undefined;
        }
    },
    extraReducers: (builder) => {
        builder.addCase(setMessagesFilter.pending, (state) => {
            state.messagesStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setMessagesFilter.fulfilled, (state, action) => {
            const currentPayload = action.payload as FilterReponseWithOptions;
            state.messages = !state.messages 
                ? currentPayload.response
                : {
                    size: currentPayload.response.size,
                    status: currentPayload.response.status,
                    data: currentPayload.addToThePrevious 
                        ? [...state.messages.data, ...currentPayload.response.data]
                        : currentPayload.response.data
                }
            state.messagesStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setMessagesFilter.rejected, (state, action) => {
            state.messagesStatus = LoadingStatusEnum.Error;
        });
        // ******************************** //
         builder.addCase(createMessageWithAttachment.pending, (state) => {
            state.createStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(createMessageWithAttachment.fulfilled, (state, action) => {
            state.createStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(createMessageWithAttachment.rejected, (state, action) => {
            state.createStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectMessages = (state: RootState) => state.message.messages;
export const selectMessagesStatus = (state: RootState) => state.message.messagesStatus;
export const selectCreateStatus = (state: RootState) => state.message.createStatus;
export const selectHasMoreMessages = (state: RootState) => !state.message.messages || state.message.messages.data.length < (state.message.messages.size ?? 0);

export const { addMessage, refreshMessages, resetCreateStatus } = messageSlice.actions;
export default messageSlice.reducer;