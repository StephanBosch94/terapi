using Newtonsoft.Json;
using ULearn.Be.Common.JsonConverters;

namespace ULearn.Be.Common.Responses;

public class ErrorResponse
{
    public ErrorResponse()
    {
        Parameters = new Dictionary<string, string>();
    }

    public int Status { get; set; }
    public string Message { get; set; }
    public Dictionary<string, string> Parameters { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(
            this,
            new JsonSerializerSettings
            {
                ContractResolver = new LowercaseContractResolver()
            }
        );
    }
}