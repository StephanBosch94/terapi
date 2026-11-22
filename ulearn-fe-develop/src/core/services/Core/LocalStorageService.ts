import { MasterHashResponse } from "core/redux/hashSlice";
import { IStorage } from "./StorageService";

export const USER_TOKEN_STORAGE_KEY = 'PORTAL_TOKEN';
const OBJECT_STORE_CACHE = 'cache';
const OBJECT_STORE_CACHE_HASH = 'cache-hash';
const OBJECT_STORE_COMMON = 'common';
const DEFAULT_VALUE = 'undefined';
declare global {
    interface Window {
        storage: Record<string, any>;
    }
}

const isStorageSupported = (): boolean => {
    try {
        localStorage.setItem('test', 'test');
        localStorage.removeItem('test');

        return true;
    } catch (error) {
        return false;
    }
};

class LocalStorageService implements IStorage {
    hasStorageSupport = isStorageSupported();

    getStorage() {
        if (this.hasStorageSupport) {
            return localStorage;
        }

        if (!window.hasOwnProperty('storage')) {
            window.storage = {};
        }

        return window.storage;
    }

    get(name: string): string | undefined {
        const data = this.getStorage()[name];
        return data !== DEFAULT_VALUE
            ? data
            : undefined;
    }

    set(name: string, item: any): void {
        this.getStorage()[name] = item;
    }

    removeItem(name: string) {
        if (this.hasStorageSupport) {
            localStorage.removeItem(name);
        } else {
            delete this.getStorage()[name];
        }
    }

    getWithHashStoragePrefix(key: string) {
        return `${OBJECT_STORE_CACHE_HASH}_${key}`;
    }

    getWithCacheStoragePrefix(key: string) {
        return `${OBJECT_STORE_CACHE}_${key}`;
    }

    getWithCommonStoragePrefix(key: string) {
        return `${OBJECT_STORE_COMMON}_${key}`;
    }

    clear() {
        return new Promise<void>(() => {
            if (this.hasStorageSupport) {
                localStorage.clear();
            } else {
                window.storage = {};
            }
        })
    }

    async getCacheHash(key: string) {
        return this.get(this.getWithHashStoragePrefix(key));
    }

    async setCacheHash(key: string, hash: string) {
        this.set(this.getWithHashStoragePrefix(key), hash);
        return key;
    }

    async getCache<T = unknown>(key: string) {
        const strValue = this.get(this.getWithCacheStoragePrefix(key))
        return strValue !== undefined
            ? JSON.parse(strValue) as T
            : undefined;
    }

    async removeCacheValue(key: string) {
        this.set(this.getWithCacheStoragePrefix(key), undefined)
    }

    async setCache(key: string, data: unknown) {
        const str = JSON.stringify(data);
        this.set(this.getWithCacheStoragePrefix(key), str)
        return key;
    }

    async getCommonValue<T = unknown>(key: string) {
        const strValue = this.get(this.getWithCommonStoragePrefix(key))
        return strValue !== undefined
            ? JSON.parse(strValue) as T
            : undefined;
    }

    async setCommonValue(key: string, data: unknown) {
        const str = JSON.stringify(data);
        this.set(this.getWithCommonStoragePrefix(key), str)
        return key;
    }

    async removeCommonValue(key: string) {
        this.set(this.getWithCommonStoragePrefix(key), undefined)
    }

    async existsCache(key: string) {
        return this.get(this.getWithCacheStoragePrefix(key)) !== undefined
    }

    async update(data: MasterHashResponse) {
        for (const x of data) {
            const currenValue = await this.getCacheHash(x.enumKey!);
            const hashesEquals = x.hash! === currenValue;
            if (!hashesEquals) {
                await this.setCacheHash(x.enumKey!, x.hash!);
                await this.removeCacheValue(x.enumKey!);
            }
        }

    }
}

export default new LocalStorageService();
