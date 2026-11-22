import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { DefaultPagePropsType, fetchDefaultPageData } from "core/services/Utils/ssrUtil";
import { useRouter } from "next/router";
import { useEffect, useState, useRef } from "react";
import AppProvider from "./AppProvider";
import { useAppDispatch } from "core/redux/reduxHooks";
import UserService from "core/services/UserService";
import { updateShortUserInfo } from "core/redux/userSlice";
import { ParsedClaimsType, getClaims } from "core/services/Utils/tokenUtil";
import { cookieConstants } from "core/constants/cookieConstants";
import Cookies from 'js-cookie';
import LinearProgressStyled from "components/styles/boxes/LinearProgressStyled";
import ErrorPage from "components/pages/ErrorPage";
import { logOut } from "core/services/Utils/authUtil";
import { isFulfilled, isRejected } from "core/services/Utils/apiUtil";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";

const fetchData = async (locale: string, claims: ParsedClaimsType) => {
    const currencyCode = Cookies.get(cookieConstants.currencyCode) ?? COMMON_CONSTANTS.DEFAULT_CURRENCY_CODE;
    const ssrDataPromise = fetchDefaultPageData(locale, currencyCode);
    const shortUserInfoPromise = claims !== undefined ? UserService.getShortUserInfo() : Promise.resolve(undefined);
    const [dataRes, shortUserInfoRes] = await Promise.allSettled([ssrDataPromise, shortUserInfoPromise]);
    const data = isFulfilled(dataRes) ? dataRes.value : undefined;
    const shortUserInfo = isFulfilled(shortUserInfoRes) ? shortUserInfoRes.value : undefined;
    const shortUserInfoErrorStatus = claims !== undefined && isRejected(shortUserInfoRes) ? shortUserInfoRes.reason?.response?.status as number | undefined : undefined;
    
    if(!data){
        throw new Error("Failed to fetch codetable data");
    }

    return {
        data,
        shortUserInfo: shortUserInfo?.data,
        shortUserInfoErrorStatus
    }
}

const AppClientSideProvider = ({ children, silent = true }: { children: JSX.Element, silent?: boolean }) => {
    const [loadingStatus, setLoadingStatus] = useState<LoadingStatusEnum | undefined>(undefined);
    const ref = useRef<DefaultPagePropsType>();
    const dispath = useAppDispatch();
    const router = useRouter();
    
    useEffect(() => {
        setLoadingStatus(LoadingStatusEnum.Loading);
        if (!router.isReady || !router.locale) {
            return;
        }

        fetchData(router.locale, getClaims(Cookies.get(cookieConstants.bearer)))
            .then((res) => {
                ref.current = res.data;
                if (res.shortUserInfo) {
                    dispath(updateShortUserInfo(res.shortUserInfo));
                }
                if (res.shortUserInfoErrorStatus === 404 || res.shortUserInfoErrorStatus === 403){
                    logOut(dispath); // mainly used in development
                }
                setLoadingStatus(LoadingStatusEnum.Loaded);
            })
            .catch((err) => {
                console.error(err);
                setLoadingStatus(LoadingStatusEnum.Error);
            })
    }, [router.isReady, dispath]);

    return (
        <>
            {loadingStatus === LoadingStatusEnum.Loading ? <LinearProgressStyled /> : <></>}
            {loadingStatus === LoadingStatusEnum.Error ? <ErrorPage /> : <></>}
            {loadingStatus === LoadingStatusEnum.Loaded ? <AppProvider data={ref.current!} serverSideInitialization={false} silent={silent}>{children}</AppProvider> : <></>}
        </>
    )
}

export const withAppClientSideProvider = <P extends object>(Component: React.ComponentType<P>, silent: boolean = true) => {
    const WithAppClientSideProvider = (props: any) => (
        <AppClientSideProvider silent={silent}>
            <Component {...props} />
        </AppClientSideProvider >
    );

    const displayName = Component.displayName || Component.name || 'Component';
    WithAppClientSideProvider.displayName = `withAppClientSideProvider(${displayName})`;

    return WithAppClientSideProvider;
};

export default AppClientSideProvider;