import { MasterHashResponse } from "core/redux/hashSlice";
import IndexedStorageService from "./IndexedStorageService";
import LocalStorageService from "./LocalStorageService";

export interface IObjectStoreCacheHash {
    key: string;
    value: string;
}

export interface IStorage {
    getCacheHash: (key: string) => Promise<string | undefined>,
    setCacheHash: (key: string, hash: string) => Promise<string>,
    getCache: <T = unknown>(key: string) => Promise<T | undefined>,
    removeCacheValue: (key: string) => Promise<void>,
    setCache: (key: string, data: unknown) => Promise<string>,
    getCommonValue: <T = unknown>(key: string) => Promise<T | undefined>,
    setCommonValue: (key: string, data: unknown) => Promise<string>,
    removeCommonValue: (key: string) => Promise<void>,
    clear: () => Promise<void>,
    existsCache: (key: string) => Promise<boolean>,
    update: (data: MasterHashResponse) => Promise<void>
}

export const getStorage = async (): Promise<IStorage> => {
    if (await IndexedStorageService.isSupported()) {
        return IndexedStorageService;
    }

    return LocalStorageService;
}