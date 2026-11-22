using ULearn.Be.Common.Services.Interfaces;

namespace ULearn.Be.Common.Services.Utilities.TimeZone;

public class UtcTimeZoneProvider : ITimeZoneProvider
{
    public string GetTimeZone()
    {
        return "Etc/UTC";
    }
}
