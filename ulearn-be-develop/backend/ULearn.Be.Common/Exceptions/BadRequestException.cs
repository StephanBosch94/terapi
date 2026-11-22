using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Extensions;

namespace ULearn.Be.Common.Exceptions;

public class BadRequestException : ExceptionBase
{
    public BadRequestException() : base("Bad request")
    {
    }

    public BadRequestException(ErrorCodesEnum text) : base(text.GetDescription())
    {
    }

    public BadRequestException(ErrorCodesEnum text, Dictionary<string, string> parameters)
        : base(text.GetDescription(), parameters)
    {
    }
}