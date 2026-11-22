

export const getShortenText = (text: string | null | undefined, maxLength: number) => {
    return (text ?? "").length > maxLength
        ? `${(text ?? "").slice(0, maxLength)}...`
        : text ?? "";
}