import { AxiosResponse } from "axios";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { getStorage } from "../Core/StorageService";
import { ErrorResponse } from "../Types/AppTypes";
import { PayloadAction } from "@reduxjs/toolkit";

export const getLoadingStatus = (statuses: (LoadingStatusEnum | undefined)[]): LoadingStatusEnum | undefined => {
    if (statuses.some(x => x === LoadingStatusEnum.Error)) {
        return LoadingStatusEnum.Error;
    }
    if (statuses.some(x => x === LoadingStatusEnum.Loading)) {
        return LoadingStatusEnum.Loading;
    }
    if (statuses.some(x => x === undefined)) {
        return undefined;
    }
    return LoadingStatusEnum.Loaded;
}

export const customSleep = (delay: number) => {
    return new Promise(res => setTimeout(res, delay));
}

export const customSleepWithCallback = async (delay: number, callback: () => void) => {
    await customSleep(delay);
    callback();
}

export const apiHasMoreItems = (total: number, showed: number) => {
    return total > showed;
}

export const statusOk = (status: number) => {
    return status >= 200 && status < 300;
}

export const cachedRequest = async <T>(requestCallback: () => Promise<AxiosResponse<T>>, cacheKey: string): Promise<T | ErrorResponse> => {
    const storage = await getStorage();
    const cachedResult = await storage.getCache(cacheKey);
    if (cachedResult) {
        return cachedResult as T;
    }

    const response = await requestCallback();
    if (!statusOk(response.status)) {
        return response.data as ErrorResponse;
    }
    await storage.setCache(cacheKey, response.data);
    return response.data as T;
}

export const isFulfilled = <T,>(p: PromiseSettledResult<T>): p is PromiseFulfilledResult<T> => p.status === 'fulfilled';
export const isRejected = (p: PromiseSettledResult<any>): p is PromiseRejectedResult => p.status === 'rejected';
export const isDispatchFulfilled = <T,>(p: PayloadAction<T | ErrorResponse | any>): p is PayloadAction<T> => p.type.endsWith("fulfilled");
export const isDispatchRejected = <T,>(p: PayloadAction<T | ErrorResponse | any>): p is PayloadAction<ErrorResponse> => p.type.endsWith("rejected");