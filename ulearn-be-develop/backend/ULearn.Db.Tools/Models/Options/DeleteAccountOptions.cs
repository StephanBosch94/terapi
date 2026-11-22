using CommandLine;

namespace ULearn.Db.Tools.Models.Options;

[Verb(
    "batch-delete-accounts",
    HelpText = @"Hard deletes an account"
)]
public class DeleteAccountOptions
{
    [Option(
        'e', 
        "emails",
        Required = true,
        HelpText = "Email accounts to delete",
        Separator = ',')]
    public IEnumerable<string> Emails { get; set; }
    
    [Option(
        't',
        "token",
        Required = true,
        HelpText = "Bearer admin token to BE.API"
    )]
    public string BearerToken { get; set; }
    
    
    [Option(
        'a',
        "api",
        Required = true,
        HelpText = "BE.API URL"
    )]
    public string BeApiUrl { get; set; }
}