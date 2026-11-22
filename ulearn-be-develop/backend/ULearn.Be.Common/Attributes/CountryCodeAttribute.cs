namespace ULearn.Be.Common.Attributes;

public class CountryCodeAttribute : Attribute
{
    public CountryCodeAttribute(string countryCode)
    {
        CountryCode = countryCode;
    }
    
    public string CountryCode { get; set; }
}