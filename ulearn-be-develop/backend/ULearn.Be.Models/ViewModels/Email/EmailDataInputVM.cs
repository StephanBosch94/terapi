using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.ViewModels.Email;

public class EmailDataInputVM
{
    public string To { get; set; }
    public string LangCode { get; set; }
    public CNotificationTypeEnum EmailTemplateEnum { get; set; }
    public Dictionary<string, object> Params { get; set; } = new();
}