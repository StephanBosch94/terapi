import {createAsyncThunk, createSelector, createSlice, PayloadAction} from "@reduxjs/toolkit";
import {LoadingStatusEnum} from "../constants/enums/LoadingStatusEnum";
import {paths} from "../../apiTypes";
import {AppDispatch, RootState} from "./store";
import { getStorage } from "../services/Core/StorageService";
import HashService from "../services/HashService";


export type MasterHashResponse = paths['/api/codetable/master-cache']['get']['responses']['200']['content']['application/json'];

interface IHashState{
    hashes: MasterHashResponse,
    status: LoadingStatusEnum | undefined
}

export const setHashesAsync = createAsyncThunk<MasterHashResponse, {} | null | undefined, { dispatch: AppDispatch, state: RootState }>(
    "hash/setHash",
    async () => {
        const response = await HashService.getHashes();
        await (await getStorage()).update(response.data);
        return response.data;
    }
);

const initialState: IHashState = {
    hashes: [],
    status: undefined
};

export const hashSlice = createSlice({
    name: "hash",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(setHashesAsync.pending, (state) => {
            state.status = LoadingStatusEnum.Loading;
        });
        builder.addCase(
            setHashesAsync.fulfilled,
            (state, action) => {
                state.hashes = action.payload;
                state.status = LoadingStatusEnum.Loaded;
            }
        );
        builder.addCase(
            setHashesAsync.rejected,
            (state, action) => {
                state.status = LoadingStatusEnum.Error;
            }
        );
    }
});

type Selector<S> = (state: RootState) => S;

export const selectHashStatus = (state: RootState) => state.hash.status
export const selectHashes = (state: RootState) => state.hash.hashes;
export const selectHashByKey = (key: string): Selector<(string|null|undefined)> => createSelector(selectHashes, hashes => hashes?.find(x => x.tableName === key)?.hash);
export default hashSlice.reducer;