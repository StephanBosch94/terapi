using System.ComponentModel;

namespace ULearn.Be.Common.Enums;

public enum ErrorCodesEnum
{
    [Description("ALREADY_ACTIVATED")] AlreadyActivated,

    [Description("EXISTS_NEWER")] ExistsNewer,

    [Description("ACCOUNT_IS_NOT_ACTIVATED")]
    AccountIsNotActivated,

    [Description("EXISTS")] Exists,

    [Description("MISSING_LANGUAGES")] MissingLanguages,

    [Description("MISSING_KEYS")] MissingKeys,

    [Description("EMAIL_EXISTS")] EmailExists,

    [Description("fe.error.missing_first_name")]
    MissingFirstName,

    [Description("fe.error.missing_last_name")]
    MissingLastName,

    [Description("fe.error.missing_email")]
    MissingEmail,

    [Description("fe.error.missing_country_of_origin")]
    MissingCountryOfOrigin,

    [Description("fe.error.missing_spoken_language")]
    MissingSpokenLanguage,

    [Description("fe.error.missing_subject")]
    MissingSubject,

    [Description("fe.error.missing_profile_photo")]
    MissingProfilePhoto,

    [Description("fe.error.description_too_short")]
    DescriptionTooShort,

    [Description("fe.error.missing_specializations")]
    MissingSpecializations,

    [Description("fe.error.missing_hour_rate")]
    MissingHourRate,

    [Description("fe.error.invalid_hour_rate")]
    InvalidHourRate,

    [Description("fe.error.missing_subject_level")]
    MissingSubjectLevel,

    [Description("fe.error.missing_phone_number")]
    MissingPhoneNumber,

    [Description("fe.error.missing_teaching_experience")]
    MissingTeachingExperience,

    [Description("fe.error.some_fields_are_empty")]
    SomeFieldsAreEmpty,

    [Description("ROLE_MISMATCH")] RoleMismatch,

    [Description("NO_FILES")] NoFiles,

    [Description("SESSION_EXPIRED")] SessionExpired,

    [Description("USER_IS_NOT_A_TUTOR")] UserIsNotATutor,

    [Description("CODE_DOES_NOT_EXISTS_OR_INVALID")]
    CodeDoesNotExistsOrInvalid,

    [Description("fe.error.invalid_previous_password")]
    InvalidPreviousPassword,

    [Description("fe.error.missing_interests")]
    MissingInterests,

    [Description("fe.error.invalid_refund_percentage")]
    InvalidRefundPercentage,

    [Description("fe.error.invalid_student_refund_percentage")]
    InvalidStudentRefundPercentage,

    [Description("fe.error.insufficient_cancellation_amount")]
    InsufficientCancellationAmount,

    [Description("fe.error.missing_ico")] MissingIco,

    [Description("fe.error.missing_place_of_business")]
    MissingPlaceOfBusiness,

    [Description("fe.error.missing_bank_account_number")]
    MissingBankAccountNumber,

    [Description("fe.error.missing_bank_code")]
    MissingBankCode,

    [Description("fe.error.missing_file")] MissingFile,

    [Description("fe.error.not_valid_transaction_type")]
    NotValidTransactionType,

    [Description("fe.error.not_enoght_money")]
    NotEnoughMoney,

    [Description("fe.error.invalid_employee_id")]
    InvalidEmployeeId,

    [Description("fe.error.cannot_change_role_ones_already_confirmed")]
    CannotChangeRoleOnesAlreadyConfirmed
}