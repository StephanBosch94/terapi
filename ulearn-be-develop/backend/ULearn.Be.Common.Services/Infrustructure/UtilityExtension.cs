using Microsoft.Extensions.DependencyInjection;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;

namespace ULearn.Be.Common.Services.Infrustructure;

public static class UtilityExtension
{
    public static void AddUtilities(this IServiceCollection services)
    {
        services.AddSingleton<IHashUtility, HashUtility>();
    }
}