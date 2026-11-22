namespace ULearn.Be.Models.ViewModels.Email;

public class EmailDataInputFullVm
{
    public string From { get; set; }
    public string To { get; set; }
    public string TemplateAlias { get; set; }
    public bool TrackOpens { get; set; } = true;
    public Dictionary<string, object> TemplateModel { get; set; }
}