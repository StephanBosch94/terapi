using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Extensions;

namespace ULearn.Be.Common.Exceptions;

public class UnauthorizedException : ExceptionBase
{
    public UnauthorizedException() : base("Unauthorized")
    {
    }

    public UnauthorizedException(ErrorCodesEnum code) : base(code.GetDescription())
    {
    }

    public UnauthorizedException(ErrorCodesEnum errorCode, Dictionary<ErrorCodeParametersEnum, string> parameters)
        : base(errorCode.GetDescription(), parameters.ToParametersDictionary())
    {
    }
}