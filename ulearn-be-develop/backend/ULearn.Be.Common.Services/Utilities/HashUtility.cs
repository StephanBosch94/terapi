using ULearn.Be.Common.Services.Interfaces;

namespace ULearn.Be.Common.Services.Utilities;

public class HashUtility: IHashUtility
{
    public Guid GenerateGuid()
    {
        return Guid.NewGuid();
    }
}