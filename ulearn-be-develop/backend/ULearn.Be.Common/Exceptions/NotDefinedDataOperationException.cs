using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Extensions;

namespace ULearn.Be.Common.Exceptions;

public class NotDefinedDataOperationException : ExceptionBase
{
    public NotDefinedDataOperationException() : base("DataOperation is not defined")
    {
    }

    public NotDefinedDataOperationException(
        ErrorCodesEnum errorCode,
        Dictionary<ErrorCodeParametersEnum, string> parameters
    )
        : base(errorCode.GetDescription(), parameters.ToParametersDictionary())
    {
    }
}