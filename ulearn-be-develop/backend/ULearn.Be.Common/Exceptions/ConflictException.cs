using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Extensions;

namespace ULearn.Be.Common.Exceptions;

public class ConflictException : ExceptionBase
{
    public ConflictException() : base("Conflict")
    {
    }

    public ConflictException(string message) : base(message)
    {
    }

    public ConflictException(ErrorCodesEnum errorCode) : base(errorCode.GetDescription())
    {
    }

    public ConflictException(ErrorCodesEnum errorCode, Dictionary<ErrorCodeParametersEnum, string> parameters)
        : base(errorCode.GetDescription(), parameters.ToParametersDictionary())
    {
    }

    public ConflictException(ErrorCodesEnum errorCode, Dictionary<string, string> parameters)
        : base(errorCode.GetDescription(), parameters)
    {
    }
}