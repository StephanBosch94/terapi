using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Extensions;

namespace ULearn.Be.Common.Exceptions;

public class MissingRequiredPropertyException : ExceptionBase
{
    public MissingRequiredPropertyException(string propertyName) : base("Missing " + propertyName)
    {
    }

    public MissingRequiredPropertyException(
        ErrorCodesEnum errorCode,
        Dictionary<ErrorCodeParametersEnum, string> parameters
    )
        : base(errorCode.GetDescription(), parameters.ToParametersDictionary())
    {
    }
}