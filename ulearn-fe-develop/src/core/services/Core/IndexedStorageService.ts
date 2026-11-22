import { DBSchema, deleteDB, IDBPDatabase, openDB } from 'idb';
import Logger from "../Utils/Logger";
import { MasterHashResponse } from "../../redux/hashSlice";
import { IObjectStoreCacheHash, IStorage } from './StorageService';

const PRIMARY_DATABASE_NAME = 'uLearnDB';
const PRIMARY_DATABASE_VERSION = 4;
const OBJECT_STORE_CACHE = 'cache';
const OBJECT_STORE_CACHE_HASH = 'cache-hash';
const OBJECT_STORE_COMMON = 'common';


interface StorageSchema extends DBSchema {
  [OBJECT_STORE_CACHE]: {
    key: string;
    value: unknown;
  };
  [OBJECT_STORE_CACHE_HASH]: IObjectStoreCacheHash;
  [OBJECT_STORE_COMMON]: {
    key: string;
    value: unknown;
  };
}

class IndexedStorageService implements IStorage {
  database?: IDBPDatabase<StorageSchema>;

  async isSupported() {
    try {
      await this.getStorage();
      return true;
    }
    catch (e) {
      return false;
    }
  }

  async getStorage() {
    if (!this.database) {
      this.database = await openDB<StorageSchema>(PRIMARY_DATABASE_NAME, PRIMARY_DATABASE_VERSION, {
        upgrade(database) {
          if (!database.objectStoreNames.contains(OBJECT_STORE_CACHE)) {
            database.createObjectStore(OBJECT_STORE_CACHE);
          }
          if (!database.objectStoreNames.contains(OBJECT_STORE_CACHE_HASH)) {
            database.createObjectStore(OBJECT_STORE_CACHE_HASH);
          }
          if (!database.objectStoreNames.contains(OBJECT_STORE_COMMON)) {
            database.createObjectStore(OBJECT_STORE_COMMON);
          }
        },
        blocked() {
        },
        blocking() {
        },
        terminated() {
        }
      });
    }

    return this.database;
  }

  async getCacheHash(key: string) {
    return (await this.getStorage()).get(OBJECT_STORE_CACHE_HASH, key);
  }

  async setCacheHash(key: string, hash: string) {
    return (await this.getStorage()).put(OBJECT_STORE_CACHE_HASH, hash, key);
  }

  async getCache<T = unknown>(key: string): Promise<T | undefined> {
    try {
      return ((await this.getStorage()).get(OBJECT_STORE_CACHE, key)) as Promise<T>;
    }
    catch (error) {
      Logger.error(error);
      return undefined;
    }
  }

  async removeCacheValue(key: string) {
    return (await this.getStorage()).delete(OBJECT_STORE_CACHE, key);
  }

  async setCache(key: string, data: unknown) {
    return (await this.getStorage()).put(OBJECT_STORE_CACHE, data, key);
  }

  async getCommonValue<T = unknown>(key: string): Promise<T | undefined> {
    return (await this.getStorage()).get(OBJECT_STORE_COMMON, key) as Promise<T | undefined>;
  }

  async setCommonValue(key: string, data: unknown) {
    return (await this.getStorage()).put(OBJECT_STORE_COMMON, data, key);
  }

  async removeCommonValue(key: string) {
    return (await this.getStorage()).delete(OBJECT_STORE_COMMON, key);
  }

  async clear() {
    (await this.getStorage()).close();
    await deleteDB(PRIMARY_DATABASE_NAME);
  }

  async existsCache(key: string) {
    const storage = await this.getStorage();
    const value = await storage.get(OBJECT_STORE_CACHE, key);
    return !!value;
  }

  async update(data: MasterHashResponse) {
    const storage = await this.getStorage();
    const transation = storage.transaction([OBJECT_STORE_CACHE, OBJECT_STORE_CACHE_HASH], "readwrite");
    const cacheStore = transation.objectStore(OBJECT_STORE_CACHE);
    const hashStore = transation.objectStore(OBJECT_STORE_CACHE_HASH);

    for (const x of data) {
      const currenValue = await hashStore.get(x.enumKey!);
      const hashesEquals = x.hash! === currenValue;
      if (!hashesEquals) {
        await hashStore.put(x.hash!, x.enumKey!);
        await cacheStore.delete(x.enumKey!);
      }
    }

    await transation.done;
  }
}

export default new IndexedStorageService();
