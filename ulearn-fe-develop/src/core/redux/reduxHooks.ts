import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux'
import type { RootState, AppDispatch } from './store'
import { Context, useContext } from 'react'
import { useEffect, useRef, useState } from 'react';
import { LoadingStatusEnum } from 'core/constants/enums/LoadingStatusEnum';
import { AxiosResponse } from 'axios';

// Use throughout your app instead of plain `useDispatch` and `useSelector`
export const useAppDispatch: () => AppDispatch = useDispatch
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector

/* eslint-disable no-console */
export const contextFactoryCreate = <T>(name: string, context: Context<T>) => () => {
    const ctx = useContext(context)

    if (ctx === undefined) {
        throw new Error(`use ${name}Context must be used withing a ${name}ContextProvider.`)
    }
    return ctx as NonNullable<T>
}
/* eslint-enable no-console */

export const useUpdateEffect = (
    effect: () => void | (() => void),
    deps?: ReadonlyArray<any>,
): void => {
    const isFirstRender = useRef(true);

    useEffect(() => {
        if (isFirstRender.current) {
            isFirstRender.current = false;
            return;
        }

        return effect();
    }, deps);
};

export const useApiCall = <T>(callback: () => Promise<AxiosResponse<T>>, deps?: ReadonlyArray<any>) => {
    const [state, setState] = useState(LoadingStatusEnum.Loading);
    const [error, setError] = useState<any | undefined>();
    const [response, setResponse] = useState<T | undefined>();
    useEffect(() => {
        callback()
            .then(res => {
                setResponse(res?.data);
                setState(LoadingStatusEnum.Loaded);
            })
            .catch(err => {
                setError(err);
                setState(LoadingStatusEnum.Error);
            })
    }, [deps]);

    return { state, error, response }
}

export const useIsSsr = () => {
    // we always start off in "SSR mode", to ensure our initial browser render
    // matches the SSR render
    const [isSsr, setIsSsr] = useState(true);

    useEffect(() => {
        // `useEffect` never runs on the server, so we must be on the client if
        // we hit this block
        setIsSsr(false);
    }, []);

    return isSsr;
}