namespace ULearn.Be.Common.Exceptions;

public abstract class ExceptionBase : Exception
{
    public readonly Dictionary<string, string> Parameters;

    public ExceptionBase(string message) : base(message)
    {
        Parameters = new Dictionary<string, string>();
    }

    public ExceptionBase(string message, Dictionary<string, string> parameters) : base(message)
    {
        Parameters = parameters;
    }
}