namespace ULearn.Be.Common.Services.Interfaces;

public interface IGuidUtility
{
    string GenerateGuidString();
    Guid GenerateGuid();
    string GenerateCode();
    string GenerateShortCode();
}