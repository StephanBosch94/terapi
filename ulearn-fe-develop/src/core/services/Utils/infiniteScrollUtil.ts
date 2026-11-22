import { RefObject } from "react";

export const isTimeToFeatchMore = (ref: RefObject<Element> ) => {
    const divHeight = ref.current?.clientHeight ?? 0;
    const scrolled = ref.current?.scrollTop ?? 0;
    const height = ref.current?.scrollHeight ?? 0
    
    return scrolled + divHeight > height - divHeight;
}