export const ENV_CONFIGURATION_KEYS = {
    AUTH: {
        REDIRECT_URL: "be.module.registration.settings.RedirectUrl",
        GOOGLE: {
            API_URL: "be.module.google_api.settings.OauthUrl",
            CLIENT_ID: "be.module.google_api.settings.ClientId",
            TUTOR: {
                REDIRECT_URL: "be.module.google_api.settings.TutorRedirectUrl",
                SCOPES: "be.module.google_api.settings.TutorScopes"
            },
            STUDENT: {
                REDIRECT_URL: "be.module.google_api.settings.StudentRedirectUrl",
                SCOPES: "be.module.google_api.settings.StudentScopes"
            },
            COMPANY: {
                REDIRECT_URL: "be.module.google_api.settings.CompanyRedirectUrl",
                SCOPES: "be.module.google_api.settings.CompanyScopes"
            }
        },
        FACEBOOK: {
            API_URL: "be.module.facebook_api.settings.OauthUrl",
            CLIENT_ID: "be.module.facebook_api.settings.ClientId",
            TUTOR_REDIRECT_URL: "be.module.facebook_api.settings.TutorRedirectUrl",
            STUDENT_REDIRECT_URL: "be.module.facebook_api.settings.StudentRedirectUrl",
            COMPANY_REDIRECT_URL: "be.module.facebook_api.settings.CompanyRedirectUrl",
            SCOPES: "be.module.facebook_api.settings.Scopes"
        },
        SEZNAM: {
            API_URL: "be.module.seznam_api.settings.OauthUrl",
            CLIENT_ID: "be.module.seznam_api.settings.ClientId",
            TUTOR_REDIRECT_URL: "be.module.seznam_api.settings.TutorRedirectUrl",
            STUDENT_REDIRECT_URL: "be.module.seznam_api.settings.StudentRedirectUrl",
            COMPANY_REDIRECT_URL: "be.module.seznam_api.settings.CompanyRedirectUrl",
            SCOPES: "be.module.seznam_api.settings.Scopes"
        }
    },
    VIDEO_CALL: {
        SERVERS: "be.videocall.settings.Servers",
        TRICKLE: "be.videocall.settings.Trickle"
    }
}