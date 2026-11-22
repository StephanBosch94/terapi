import { createAsyncThunk, createSelector, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { paths } from "apiTypes";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import ConversationService from "core/services/ConversationService";
import { AppMessageVM, ErrorResponse } from "core/services/Types/AppTypes";
import produce from "immer";
import { RootState } from "./store";
import { selectShortUserInfo } from "core/redux/userSlice";

export type GetConversationsResponse = paths['/api/conversation']['get']['responses']['200']['content']['application/json'];
export type ConversationVM = GetConversationsResponse[0];
export type CreateConversationRequestBody = paths['/api/conversation']['post']['requestBody']['content']['application/json'];

interface IConversationsSliceState {
    selectedConversation: GetConversationsResponse[0] | undefined,
    conversations: GetConversationsResponse | undefined,
    conversationsStatus: LoadingStatusEnum | undefined,

    setReadStatus: LoadingStatusEnum | undefined,
    createStatus: LoadingStatusEnum | undefined
}

const initialState: IConversationsSliceState = {
    selectedConversation: undefined,
    conversations: undefined,
    conversationsStatus: undefined,

    setReadStatus: undefined,
    createStatus: undefined
};

export const setConversations = createAsyncThunk<GetConversationsResponse | ErrorResponse>(
    "conversations/setConversations", async (requestBody, { rejectWithValue }) => {
        try {
            const response = await ConversationService.getConversations();
            return response.data;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const create = createAsyncThunk<void | ErrorResponse, CreateConversationRequestBody>(
    "conversations/create", async (requestBody, { rejectWithValue }) => {
        try {
            await ConversationService.create(requestBody);
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setRead = createAsyncThunk<number | ErrorResponse, number>(
    "conversations/setRead", async (requestBody, { rejectWithValue }) => {
        try {
            await ConversationService.setRead(requestBody);
            return requestBody;
        } catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const conversationSlice = createSlice({
    name: "conversations",
    initialState,
    reducers: {
        addConversation(state, action: PayloadAction<ConversationVM>) {
            state.conversations = state.conversations ? [...state.conversations, action.payload] : [action.payload];
        },
        addLatestMessage(state, action: PayloadAction<{ message: AppMessageVM, currentUserId: number }>) {
            state.conversations = produce(state.conversations, draft => {
                if (!draft) return;
                const conversationIdx = draft.findIndex(conversation => conversation.conversationId === action.payload.message.conversationId);
                if (conversationIdx === -1) return;
                draft[conversationIdx].countOfUnreadMessages = action.payload.message.senderId !== action.payload.currentUserId
                    ? (draft[conversationIdx].countOfUnreadMessages ?? 0) + 1
                    : draft[conversationIdx].countOfUnreadMessages ?? 0;
                draft[conversationIdx].latestMessage = action.payload.message;
                draft[conversationIdx].updated = action.payload.message.updated;
            })
        },
        setSelectedConversation(state, action: PayloadAction<number | undefined>) {
            if (action.payload === undefined) {
                state.selectedConversation = undefined;
                return;
            }
            if (!state.conversations) return;
            const conversationIdx = state.conversations.findIndex(conversation => conversation.conversationId === action.payload);
            if (conversationIdx === -1) return;
            state.selectedConversation = state.conversations[conversationIdx];
        }
    },
    extraReducers: (builder) => {
        builder.addCase(setConversations.pending, (state) => {
            state.conversationsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setConversations.fulfilled, (state, action) => {
            state.conversations = action.payload as GetConversationsResponse;
            state.conversationsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setConversations.rejected, (state, action) => {
            state.conversationsStatus = LoadingStatusEnum.Error;
        });
        // ******************************** //
        builder.addCase(setRead.pending, (state) => {
            state.setReadStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setRead.fulfilled, (state, action) => {
            state.setReadStatus = LoadingStatusEnum.Loaded;
            state.conversations = produce(state.conversations, draft => {
                if (!draft) return;
                const index = draft.findIndex(x => x.conversationId! === action.payload as number);
                if (index === -1) return;
                draft[index].countOfUnreadMessages = 0;
            })
        });
        builder.addCase(setRead.rejected, (state, action) => {
            state.setReadStatus = LoadingStatusEnum.Error;
        });
        // ******************************** //
        builder.addCase(create.pending, (state) => {
            state.createStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(create.fulfilled, (state, action) => {
            state.createStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(create.rejected, (state, action) => {
            state.createStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectSelectedConversation = (state: RootState) => state.conversation.selectedConversation;
export const selectOpponent = () => createSelector(selectSelectedConversation, selectShortUserInfo, (conversation, user) => conversation?.initiator?.userId === user?.userId ? conversation?.opponent : conversation?.initiator);
export const selectConversations = (state: RootState) => state.conversation.conversations;
export const selectConversationsStatus = (state: RootState) => state.conversation.conversationsStatus;
export const createConversationsStatus = (state: RootState) => state.conversation.createStatus;
export const setReadStatus = (state: RootState) => state.conversation.setReadStatus;

export const { addConversation, addLatestMessage, setSelectedConversation } = conversationSlice.actions;
export default conversationSlice.reducer;