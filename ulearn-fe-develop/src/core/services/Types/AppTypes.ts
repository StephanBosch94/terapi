import { components, paths } from "apiTypes"
import { ActivityTypeEnum } from "core/constants/enums/ActivityTypeEnum"
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum"

export interface ICodeTableObject {
    id: number,
    enumKey: string,
    nameKey: string,
    value: string | undefined,
    translation: string
}

export interface IErrorCode {
    errorCodeKey: string,
    errorCodeTranslation: string
}

export interface IJwtToken {
    USER_ID: number,
    ROLES_CLAIM: string,
    exp: number,
    iss: string,
    aud: string
}

export type AppLanguageVM = {
    code: string | null | undefined,
    name: string | null | undefined,
    isDefault?: boolean
}

export type ErrorResponse = {
    status?: number,
    message?: string,
    parameters?: { [key: string]: string | null } | null
}

export type AppUserTimeslot = {
    userId?: number,
    dayOfWeek?: {
        enumKey?: string | null,
        nameKey?: string | null,
        descriptionKey?: string | null,
        dayOfWeekId?: number
    },
    timestamp?: {
        enumKey?: string | null,
        nameKey?: string | null,
        descriptionKey?: string | null,
        timestampId?: number
    },
    startTime?: string | undefined | null,
}

export type AppUserReservation = {
    reservationId?: number;
    reservationStateId?: number,
    startTime?: string,
    endTime?: string
}

export type AppUserOverviewVM = 
    paths['/api/user/student/filter']['post']['responses']['200']['content']['application/json']['data'][0] |
    paths['/api/user/tutor/filter']['post']['responses']['200']['content']['application/json']['data'][0];

export type AppUserCardOverviewVM = paths['/api/user-recommendation/popular-tutors/filter']['post']['responses']['200']['content']['application/json']['data'][0];

export type LanguageType = paths['/api/codetable/translation-language']['get']['responses']['200']['content']['application/json'][0];
export type UserWithFilesVM = {
    files?: components["schemas"]["UserFileVM"][] | null
}

export type AppUserReview = components["schemas"]["AppUserReviewVM"];
export type AppUserCommon = paths['/api/user/short-info']['get']['responses']['200']['content']['application/json'];

export type BenefitType = {
    nameKey: string,
    descriptionKey: string,
    imgPath: string,
    altText: string
};

export type ProcessType = {
    nameKey: string,
    descriptionKey: string,
};

export type ProcessHeadingType = {
    titleKey: string,
    descriptionKey: string,
};

export type BenefitHeadingType = {
    titleKey: string,
    descriptionKey: string,
};

export type EmptyType = {
    titleKey: string,
    descriptionKey: string,
    buttonTextKey: string,
    imagePath: string
};

export type BlogPostType = {
    titleKey: string,
    descriptionKey: string,
    imagePath: string,
};
export type QuestionAndAnswersType = {
    nameKey: string,
    descriptionKey: string,
};
export type CtaType = {
    headingKey: string,
    descriptionKey: string,
    buttonTextKey: string,
};
export type HeroHeadingType = {
    headingKey: string,
    descriptionKey: string
};
export type UserSearchQueryType = {
    subjectId?: string,
    toTrialPrice?: string,
    specializationIds?: string,
    filtersApplied?: string
}

export type UserSearchQueryParsed = {
    subjectId?: number | undefined,
    toTrialPrice?: number | undefined,
    specializationIds?: number[] | undefined,
    filtersApplied?: boolean
}

export type CodeTable = {
    id: number,
    translationKey: string
}

export type AppDate = {
    year: number,
    month: number,
    date: number,
    totalMinutes: number
}

export type AppMessageAttachmentVM = NonNullable<paths["/api/message/conversation/{conversationId}/filter"]["post"]["responses"]["200"]["content"]["application/json"]["data"][0]["attachments"]>[0]
export type AppMessageVM = {
    messageId?: number,
    conversationId?: number,
    messageStateId?: 1 | 2,
    senderId?: number,
    receiverId?: number,
    textValue?: string | null,
    created?: string | undefined,
    updated?: string | undefined,
    attachments?: AppMessageAttachmentVM[] | null,
}

export type TokenVM = {
    USER_ID: string,
    USERNAME: string
}

export type AppMessageInpuVM = {
    opponentId: number,
    messageText: string,
    files: File[],
}

export type AppMessageWithConversationInpuVM = paths["/api/message"]["post"]["requestBody"]["content"]["multipart/form-data"] & { files?: File[] | undefined };

export type AppSubject = {
    enumKey?: string | null,
    nameKey?: string | null,
    descriptionKey?: string | null,
    subjectId?: number,
    iconName?: string | null
}
export type AppUserCommonWithThumbnailVM = {
    userId?: number;
    email?: string | null;
    firstName?: string | null;
    lastName?: string | null;
    userRoleId?: number | null;
    phone?: string | null;
    profileThumbnail?: string | null;
};

export type CSubjectVM = {
    enumKey?: string | null;
    nameKey?: string | null;
    descriptionKey?: string | null;
    subjectId?: number;
    iconName?: string | null;
};

export type AppReservationRoomVM = {
    roomId?: number;
    roomKey?: string | null;
};

export type AppReservationVM = NonNullable<paths["/api/user/short-info"]["get"]["responses"]["200"]["content"]["application/json"]["runningActivity"]>

export type PagedVM<T> = {
    size?: number | undefined,
    data: T[],
    status?: string | undefined | null
}

export type PagedVMResponse<T> = {
    response: T,
    appendType: AppendTypeEnum,
}

export type PagedVMRequest<T> = {
    request: T,
    appendType: AppendTypeEnum
}

export type OpponentActivityMessageVM = {
    activityTypeId: ActivityTypeEnum,
    messagePayload: string | undefined
}

export type TranslationsType = {
    [key: string]: {
        [key: string]: string;
    };
}
export type ClassDurationType = 25 | 50;
export type NumberOfLessons = 1 | 5 | 10;
export type SubjectType = paths['/api/codetable/subject']['get']['responses']['200']['content']['application/json'][0];
export type SpecializationType = paths['/api/codetable/specialization']['get']['responses']['200']['content']['application/json'][0];

export const nameof = <T>(name: Extract<keyof T, string>): string => name;