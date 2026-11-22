using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ULearn.Be.Common.Attributes;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;

public class NotificationTemplateConverter : JsonConverter
{
    private static readonly JsonSerializer Serializer = new();

    private static readonly Dictionary<string, Type> TypeMapper = new()
    {
        { MapType(NotificationTemplateTypeEnum.NewReservation), typeof(NewReservationNotificationVm) },
        { MapType(NotificationTemplateTypeEnum.NewPackageReservation), typeof(NewPkgReservationNotificationVm) },
        {
            MapType(NotificationTemplateTypeEnum.ReservationCancellation), typeof(ReservationCancellationNotificationVM)
        },
        { MapType(NotificationTemplateTypeEnum.PackageCancellation), typeof(PackageCancellationNotificationVm) },
        { MapType(NotificationTemplateTypeEnum.ReservationReschedule), typeof(ReservationRescheduleNotificationVm) },
        { MapType(NotificationTemplateTypeEnum.RefundRequested), typeof(RefundRequestedNotificationVM) },
        { MapType(NotificationTemplateTypeEnum.NewReview), typeof(NewReviewNotificationVm) },
        { MapType(NotificationTemplateTypeEnum.WithdrawalApproved), typeof(WithdrawalApprovedNotificationVm) },
        { MapType(NotificationTemplateTypeEnum.RefundApproved), typeof(RefundApprovedNotificationVM) },
        { MapType(NotificationTemplateTypeEnum.RefundDeclined), typeof(RefundDeclinedNotificationVM) },
        { MapType(NotificationTemplateTypeEnum.ReservationReminder), typeof(ReservationReminderNotificationVM) },
        { MapType(NotificationTemplateTypeEnum.TutorProfileVisited), typeof(TutorProfileVisitedNotificationVm) }
    };

    public override bool CanWrite => false;

    private static string MapType(NotificationTemplateTypeEnum type)
    {
        return type.GetAttribute<ObjectNameAttribute>().Name;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
    }

    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer
    )
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        var jObject = JObject.Load(reader);
        var type = GetObjectType(jObject);

        return type != null
            ? Serializer.Deserialize(jObject.CreateReader(), type)
            : null;
    }

    private static Type GetObjectType(JToken jObject)
    {
        var objectTypeStr = jObject?.Value<string>("type");
        if (string.IsNullOrEmpty(objectTypeStr))
        {
            return null;
        }

        return TypeMapper.ContainsKey(objectTypeStr) ? TypeMapper[objectTypeStr] : null;
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(NotificationBaseVM).IsAssignableFrom(objectType) ||
               typeof(NotificationBaseVM) == objectType;
    }
}