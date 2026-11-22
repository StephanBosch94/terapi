export const COMMON_CONSTANTS = {
    YOUTUBE_BASE_URL: 'https://www.youtube.com/embed',
    YOUTUBE_THUMBNAIL_URL: 'https://img.youtube.com/vi',
    CALENDAR: {
        MIN_POSSIBLE_HOUR: 0,  //default timestamps until more timezones are supported (than change to TimestampEnum.TimeStamp00_00)
        MAX_POSSIBLE_HOUR: 24, //default timestamps until more timezones are supported (than change to TimestampEnum.TimeStamp24_00),
        DAYS_TO_SHOW: 7
    },
    PACKAGES: {
        ONE_LESSON_SALE: 0,
        FIVE_LESSONS_SALE: 0.05,
        TEN_LESSONS_SALE: 0.1
    },
    TIMESTAMP_DURATION_MIN: 30,
    LP_CALCULATOR: {
        COMMISSION_RATE: 0.21,
        DEFAULT_HOUR_RATE: 500,
        DEFAULT_NUMBER_OF_HOURS: 100,
        MIN_NUMBER_OF_HOURS: 0,
        MAX_NUMBER_OF_HOURS: 160,
        DEFAULT_CURRENCY: 'Kč'
    },
    EMAIL: {
        RESEND_EMAIL_AWAITING_TIME: 60
    },
    MAX_NUMBER_OF_MATCHING_TUTORS: 10,
    DEFAULT_USER_NAME: "Yaspeaker",
    DEFAULT_NUMBER_OF_ITEMS_TO_SHOW: 50,
    WITHDRAW_COMMON_RATE: 0.03,
    DEFAULT_CURRENCY_CODE: "eur",
    INTERNAL_CURRENCY_CODE: "czk",
    REGISTRATION: {
        TUTOR: {
            MAX_NUMBER_OF_INTERESTS: 10,
            MAX_PERCENTAGE_OF_SPECIALIZATIONS: 60, // cannot be an exact number since number of specializations depends on the subject   
        }
    }
}

