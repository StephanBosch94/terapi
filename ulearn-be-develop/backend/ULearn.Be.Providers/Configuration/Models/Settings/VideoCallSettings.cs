namespace ULearn.Be.Providers.Configuration.Models.Settings;

public class VideoCallSettings
{
    public string Secret { get; set; }
    public string ApiKey { get; set; }
    public List<VideoCallServer> Servers { get; set; }
    public bool Trickle { get; set; }
}