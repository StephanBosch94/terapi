import { useRouter } from "next/router";
import Script from "next/script";
import { useEffect } from "react";
import { getFromPathString } from "routes/routes";

const HubspotChat = () => {
    const router = useRouter();

    useEffect(() => {
        if (!router.isReady) return;
        const path = getFromPathString(router.pathname);
        const shouldShowChat = path?.showChat ?? false;

        if (shouldShowChat) {
            window.HubSpotConversations?.widget?.load();
        }
        else {
            window.HubSpotConversations?.widget?.remove();
        }
    }, [router.isReady]);

    return (
        <>
            <Script id="hs-script-loader" src="//js.hs-scripts.com/27238454.js" strategy="lazyOnload" />
        </>
    )
}

export default HubspotChat;