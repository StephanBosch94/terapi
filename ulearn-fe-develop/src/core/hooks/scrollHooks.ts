import { useEffect, useState } from "react";

export const useWindowScroll = () => {
    const [isScrolled, setScrolled] = useState(false);

    useEffect(() => {
        const updatePosition = () => {
            const newIsScrolled = window.scrollY > 0;
            if (newIsScrolled === isScrolled) {
                return;
            }
            setScrolled(newIsScrolled);
        }

        window.addEventListener('scroll', updatePosition)
        return () => window.removeEventListener('scroll', updatePosition)
    }, [isScrolled])

    return { isScrolled };
}