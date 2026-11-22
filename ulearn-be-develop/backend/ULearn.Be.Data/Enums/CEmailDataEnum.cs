using System.ComponentModel;

namespace ULearn.Be.Data.Enums;

public enum CEmailDataEnum
{
    [Description("action_url")] ActionUrl,
    [Description("fe_link")] FeLink,
    [Description("username")] Username,
    [Description("user_photo_url")] UserPhotoUrl,
    [Description("from")] From,
    [Description("to")] To,
    [Description("number_of_minutes")] NumberOfMinutes,
    [Description("number_of_messages")] NumberOfUnreadMessages,
    [Description("previous_from")] PreviousFrom,
    [Description("previous_to")] PreviousTo,
    [Description("number_of_lessons")] NumberOfLessons,
    [Description("issue_description")] IssueDescription,
    [Description("issue_type")] IssueType,
    [Description("tutor")] Tutor,
    [Description("student")] Student,
    [Description("refund_ammout")] RefundAmount,

    [Description("previous_amount_to_pay")]
    PreviousAmountToPay,
    [Description("amount_to_pay")] AmountToPay,
    [Description("admin_comment")] AdminComment,
    [Description("admin_email")] AdminEmail,
    [Description("code")] Code,

    [Description("notification_settings_url")]
    NotificationSettingsUrl,
    [Description("valid_to")] ValidTo,
    [Description("expires_in_days")] ExpiresInDays,
    [Description("discount_percentage")] DiscountPercentage
}