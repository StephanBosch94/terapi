import { COMMON_CONSTANTS } from "core/constants/commonConstants";

export const getEmbededId = (youtubeUrl: string) => {
    const baseUrl = 'https://www.youtube.com/watch';
    const mobileUrl = 'https://youtu.be/';

    if (youtubeUrl.includes(baseUrl)) {
        const embedIdParam = 'v';
        const replaced = youtubeUrl.replace(baseUrl, '');
        const urlParameters = new URLSearchParams(replaced);

        return urlParameters.get(embedIdParam)
    }

    if (youtubeUrl.includes(mobileUrl)) {
        const replaced = youtubeUrl.replace(mobileUrl, '');
        return replaced;
    }

    return youtubeUrl;
}


export const getYoutubeVideoUrlFromQuery = (query: string) => {
    return `${COMMON_CONSTANTS.YOUTUBE_BASE_URL}/${query}?autoplay=1`;
}


export const getYoutubeImageUrlFromQuery = (query: string) => {
    return `${COMMON_CONSTANTS.YOUTUBE_THUMBNAIL_URL}/${query}/sddefault.jpg`;
}

