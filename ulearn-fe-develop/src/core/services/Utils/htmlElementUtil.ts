export const getOffsetTopRecursively = (element: HTMLElement | null) => {
    let offsetTop = 0;
    let currentElement = element;
    while (currentElement) {
        offsetTop += currentElement.offsetTop;
        currentElement = currentElement.offsetParent as HTMLElement;
    }
    return offsetTop;
}