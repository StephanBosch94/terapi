using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Extensions;

namespace ULearn.Be.Common.Exceptions;

public class ForbiddenException : ExceptionBase
{
    public ForbiddenException() : base("Forbidden")
    {
    }

    public ForbiddenException(ErrorCodesEnum errorCode) : base(errorCode.GetDescription())
    {
    }

    public ForbiddenException(ErrorCodesEnum errorCode, Dictionary<ErrorCodeParametersEnum, string> parameters)
        : base(errorCode.GetDescription(), parameters.ToParametersDictionary())
    {
    }
}