import { selectIsAuthorized, updateClaims } from "core/redux/authSlice";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { selectShortUserInfo, setShortUserInfo } from "core/redux/userSlice";
import { DefaultPagePropsType } from "core/services/Utils/ssrUtil";
import { createContext, useEffect, useState } from "react";
import TranslationsProvider from "../TranslationProvider";
import Seo from "../Seo";
import UIContextProvider from "../UIContextProvider";
import ChatSocketConnector from "../ChatSocketConnector";
import { paths } from "apiTypes";
import { useRouter } from "next/router";
import LinearProgressStyled from "components/styles/boxes/LinearProgressStyled";
import { cookieConstants } from "core/constants/cookieConstants";
import { getClaims } from "core/services/Utils/tokenUtil";
import Cookies from 'js-cookie';
import CookiesModal from "components/modals/CookiesModal";
import HubspotChat from "../HubspotChat";
import Trustpilot from "../TrustPilot";
import LessonDiscountModal from "components/modals/LessonDiscountModal";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";

type AppDataProvierContextType = DefaultPagePropsType & {
    currentCurrency: paths['/api/codetable/currency']['get']['responses']['200']['content']['application/json'][0],
    currentLanguage: paths['/api/codetable/translation-language']['get']['responses']['200']['content']['application/json'][0],
    changeCurrency: (newCurrencyCode: string) => void
}

const AppDataContext = createContext<AppDataProvierContextType | undefined>(undefined);
export const useAppDataContext = contextFactoryCreate('AppData', AppDataContext);

const RouterProgressBar = () => {
    const router = useRouter();
    const [showProgress, setShowProgress] = useState(false);

    useEffect(() => {
        router.events.on("routeChangeStart", () => {
            setShowProgress(true);
        })
        router.events.on("routeChangeComplete", () => {
            setShowProgress(false);
        })
    }, []);

    return showProgress ? <LinearProgressStyled /> : <></>
}

const AppProvider = ({ children, data, serverSideInitialization = true, silent = true }: { children: JSX.Element, data: DefaultPagePropsType, serverSideInitialization?: boolean, silent?: boolean }) => {
    const dispatch = useAppDispatch();
    const [currencyCode, setCurrencyCode] = useState(data?.currencyCode);
    const isUserAuthorized = useAppSelector(selectIsAuthorized);
    const currentLanguage = data.languages.find(x => x.languageCode === data.locale)!;
    const currentCurrency = data.currencies.find(x => x.enumKey === currencyCode)
        ?? data.currencies.find(x => x.enumKey === COMMON_CONSTANTS.DEFAULT_CURRENCY_CODE)!;
    const shortUserInfo = useAppSelector(selectShortUserInfo);

    useEffect(() => {
        const bearer = Cookies.get(cookieConstants.bearer);
        dispatch(updateClaims(getClaims(bearer)));
    }, [shortUserInfo]);

    useEffect(() => {
        if (serverSideInitialization && isUserAuthorized) {
            // All requests that requere authorization
            dispatch(setShortUserInfo());
        }
    }, [serverSideInitialization, isUserAuthorized]);

    useEffect(() => {
        const cookie = Cookies.get(cookieConstants.currencyCode);
        if (!cookie) {
            Cookies.set(cookieConstants.currencyCode, currentCurrency.enumKey!);
        }
    }, [currentCurrency]);

    useEffect(() => {
        setCurrencyCode(data.currencyCode);
    }, [data]);

    const handleChangeCurrency = (newCurrencyCode: string) => {
        setCurrencyCode(newCurrencyCode);
    }

    return (
        <TranslationsProvider
            locale={data.locale}
            translations={data.translations}
        >
            <AppDataContext.Provider value={{
                ...data,
                currentLanguage,
                currentCurrency,
                currencyCode,
                changeCurrency: handleChangeCurrency
            }}>
                <UIContextProvider userAgentScreenType={data.screenType}>
                    <RouterProgressBar />
                    <HubspotChat />
                    <LessonDiscountModal />
                    <Trustpilot />
                    <Seo />
                    <ChatSocketConnector silent={silent} />
                    <CookiesModal />
                    {children}
                </UIContextProvider>
            </AppDataContext.Provider>
        </TranslationsProvider>
    )
}

export const withAppProvider = <P extends DefaultPagePropsType>(
    Component: React.ComponentType<P>
) => {
    const WithAppProvider = (input: P) => (
        <AppProvider data={input}>
            <Component {...input} />
        </AppProvider>
    );

    WithAppProvider.displayName = `WithAppProvider(${Component.displayName || Component.name || 'Component'})`;

    return WithAppProvider;
};

export default AppProvider;