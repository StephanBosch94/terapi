using ULearn.Be.Data.Enums;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IMasterHashService
{
    Task<DateTime> GetLastUpdated(CMasterHashEnum masterHash);
}