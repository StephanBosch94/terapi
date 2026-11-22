using System.ComponentModel;
using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CMasterHashEnum
{
    [Description("96b64cfc-2429-4a8a-b084-503cb66c659a")] [EnumKey("c_age_range")]
    CAgeRange = 1,

    [Description("0862bdaf-6cf5-43cd-8571-02cbb81ea596")] [EnumKey("c_category")]
    CCategory = 2,

    [Description("9b70284a-6a9b-4c62-a8cf-895f3a6e00ec")] [EnumKey("c_country")]
    CCountry = 3,

    [Description("5bbdab1e-2413-4a4a-b64c-84f11f019e5e")] [EnumKey("c_currency")]
    CCurrency = 4,

    [Description("9f6add02-b02a-471f-929e-a516c027decc")] [EnumKey("c_current_situation")]
    CCurrentSituation = 5,

    [Description("167c046a-bf9e-4890-9f67-d2e8ff7d8b5c")] [EnumKey("c_day_of_week")]
    CDayOfWeek = 6,

    [Description("4ffc2109-71f5-4aff-9bf2-3f1fc594277c")] [EnumKey("c_file_type")]
    CFileType = 7,

    [Description("4562a911-d774-4a67-9b5b-04f41912874f")] [EnumKey("c_language")]
    CLanguage = 8,

    [Description("855a9ae4-f22e-44d8-82bc-69edbbba6601")] [EnumKey("c_language_level")]
    CLanguageLevel = 9,

    [Description("523861e8-4b39-4cb4-a9bb-877d75eeb722")] [EnumKey("c_master_hash")]
    CMasterHash = 10,

    [Description("f6dccfb0-0719-44a0-9a11-d97d98c1eb90")] [EnumKey("c_payment_service_provider")]
    CPaymentServiceProvider = 11,

    [Description("7282c9ac-fa8d-4bc4-8b06-b46277561206")] [EnumKey("c_reservation_state")]
    CReservationState = 12,

    [Description("6a7aae1c-acc5-4ebe-aded-928700ff0f3f")] [EnumKey("c_subject")]
    CSubject = 13,

    [Description("a308fe31-27b0-4082-8800-f95f34c7adde")] [EnumKey("c_subject_level")]
    CSubjectLevel = 14,

    [Description("794f86db-28fe-4b1d-b554-33f836d6f419")] [EnumKey("c_timestamp")]
    CTimestamp = 15,

    [Description("c0010df7-f357-4ea4-8a6e-ed260dc70192")] [EnumKey("c_timezone")]
    CTimezone = 16,

    [Description("cffa85d3-4bd9-4cd1-9569-3425c549d770")] [EnumKey("c_token_provider")]
    CTokenProvider = 17,

    [Description("b2e83f0b-26e9-4f30-8228-452bee08e15e")] [EnumKey("c_token_type")]
    CTokenType = 18,

    [Description("8afe9882-8ab7-4b36-aeb3-9cf1fa781c54")] [EnumKey("c_transaction_state")]
    CTransactionState = 19,

    [Description("3dd2cbae-9130-4885-b8f5-a5187a7cba06")] [EnumKey("c_transaction_type")]
    CTransactionType = 20,

    [Description("f5e2a33b-4dd3-4650-90ac-ed249c259cae")] [EnumKey("c_user_attribute_type")]
    CUserAttributeType = 21,

    [Description("8e60da47-86ac-4851-8b61-5e23562eb8e7")] [EnumKey("c_user_role")]
    CUserRole = 22,

    [Description("7c18b78f-d28a-4ccc-8682-de67a0f1e5a9")] [EnumKey("c_user_state")]
    CUserState = 23,

    [Description("4ed00f01-6b16-43af-bb52-b2c0d0540c75")] [EnumKey("env_configuration")]
    CEnvConfiguration = 24,

    [Description("40a2def3-f242-4c39-bd72-a63f7602044e")] [EnumKey("c_translation_language")]
    CTranslationLanguage = 25,

    [Description("8442c6a5-0772-4ee6-9bc1-8bd3d1a78a88")] [EnumKey("c_translation")]
    CTranslation = 26,

    [Description("e2c7611a-00c9-4482-b44a-820e0e6eb9bc")] [EnumKey("c_specialization")]
    CSpecialization = 27,
}