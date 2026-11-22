using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Extensions;

namespace ULearn.Be.Common.Exceptions;

public class UnprocessableEntityException : ExceptionBase
{
    public UnprocessableEntityException() : base("Unprocessable Entity")
    {
    }

    public UnprocessableEntityException(
        ErrorCodesEnum errorCode,
        Dictionary<ErrorCodeParametersEnum, string> parameters
    )
        : base(errorCode.GetDescription(), parameters.ToParametersDictionary())
    {
    }
}