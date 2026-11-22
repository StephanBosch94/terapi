import { Provider } from 'react-redux';
import type { AppProps } from 'next/app';
import { store } from "core/redux/store";
import "../../styles/globals.css";
import { appTheme } from "../themes/Theme";
import { ThemeProvider } from "@mui/material/styles";
import React, { useEffect, useState } from "react";
import Snackbar from 'components/core/Snackbar';
import CssBaseline from '@mui/material/CssBaseline';
import { Inter } from "next/font/google";
import TagManager from "react-gtm-module";
import { isProd } from 'core/services/Utils/urlUtil';

const inter = Inter({
    subsets: ['latin', 'cyrillic'],
    weight: ['400', '500', '600', '700', '800'],
    style: ['normal']
})

const MyApp = ({ Component, pageProps, router }: AppProps) => {
    const [isMounted, setIsMounted] = useState(false);

    useEffect(() => {
        setIsMounted(true);
        document.body.className = inter.className; // workaround for modals
    }, []);

    useEffect(() => {
        if (isProd()) {
            TagManager.initialize({ gtmId: 'GTM-TT7CXS4' });
        }
    }, []);

    useEffect(() => {
        if (!isMounted) return;

        // Hide the initial HTML loading screen once the app is ready
        const hideInitialLoading = () => {
            const initialLoadingScreen = document.getElementById('initial-loading-screen');
            if (initialLoadingScreen) {
                // Small delay to ensure smooth transition
                setTimeout(() => {
                    initialLoadingScreen.style.display = 'none';
                }, 300);
            }
        };

        // Check if page is already loaded
        if (document.readyState === 'complete') {
            hideInitialLoading();
        } else {
            window.addEventListener('load', hideInitialLoading);
        }

        // Also hide when router is ready (for client-side navigation)
        if (router.isReady) {
            hideInitialLoading();
        } else {
            router.events.on('routeChangeComplete', hideInitialLoading);
        }

        return () => {
            window.removeEventListener('load', hideInitialLoading);
            router.events.off('routeChangeComplete', hideInitialLoading);
        };
    }, [router, isMounted]);

    return (
        <main className={inter.className}>
            <ThemeProvider theme={appTheme}>
                <Provider store={store}>
                    <Snackbar>
                        <CssBaseline enableColorScheme />
                        <Component {...pageProps} />
                    </Snackbar>
                </Provider>
            </ThemeProvider>
        </main>
    );
}

export default MyApp;