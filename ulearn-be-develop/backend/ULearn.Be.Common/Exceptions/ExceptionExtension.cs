using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Extensions;

namespace ULearn.Be.Common.Exceptions;

public static class ExceptionExtension
{
    public static Dictionary<string, string> ToParametersDictionary(
        this Dictionary<ErrorCodeParametersEnum, string> parameters
    )
    {
        return parameters.ToDictionary(
            x => x.Key.GetDescription(),
            x => x.Value
        );
    }
}