using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Extensions;

namespace ULearn.Be.Common.Exceptions;

public class NotFoundException : ExceptionBase
{
    public NotFoundException() : base("Not Found")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(ErrorCodesEnum errorCode, Dictionary<ErrorCodeParametersEnum, string> parameters)
        : base(errorCode.GetDescription(), parameters.ToParametersDictionary())
    {
    }
}