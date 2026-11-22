import { createContext, useEffect, useState, useRef } from "react";

export type ViewScreenType = "mobile" | "desktop"

type UIContextType = {
    screenType: ViewScreenType
}

export const UIContext = createContext<UIContextType>({
    screenType: "desktop"
});

const getScreenType = (width: number): ViewScreenType => {
    return width > 940 ? "desktop" : "mobile";
}

const UIContextProvider = ({ children, userAgentScreenType = "desktop" }: { children: any, userAgentScreenType?: ViewScreenType }) => {
    const [screenType, setScreenType] = useState<ViewScreenType>(userAgentScreenType);
    const throttleInProgress = useRef<boolean>(false);

    useEffect(() => {
        setScreenType(userAgentScreenType);
    }, [userAgentScreenType]);

    const handleWidthChange = () => {
        const newScreenType = getScreenType(window.innerWidth);
        if (newScreenType === screenType) {
            return;
        }
        setScreenType(newScreenType);
    }

    const handleWidthChangeThrottle = () => {
        if (throttleInProgress.current) { return; }
        throttleInProgress.current = true;
        setTimeout(() => {
            handleWidthChange();
            throttleInProgress.current = false;
        }, 100);
    }

    useEffect(() => {
        handleWidthChange();
    }, []);

    useEffect(() => {
        if (!window) return
        if (typeof window === "undefined") return;
        window.addEventListener("resize", handleWidthChangeThrottle);

        return () => {
            window.removeEventListener("resize", handleWidthChangeThrottle);
        }
    }, [screenType, throttleInProgress]);

    return (
        <UIContext.Provider value={{
            screenType: screenType
        }}>
            {children}
        </UIContext.Provider>
    )
};

export default UIContextProvider;