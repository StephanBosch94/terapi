using CommandLine;

namespace ULearn.Db.Tools.Models.Options;

public class ConnectionStringBase
{
    [Option(
        's',
        "server",
        Required = true,
        HelpText = "Server name"
    )]
    public string ServerName { get; set; }
    
    [Option(
        'o',
        "Port",
        Required = true,
        HelpText = "Port"
    )]
    public string Port { get; set; }
    
    [Option(
        'd',
        "database",
        Required = true,
        HelpText = "Database name"
    )]
    public string DatabaseName { get; set; }
    
    [Option(
        'u',
        "username",
        Required = true,
        HelpText = "User"
    )]
    public string UserName { get; set; }
    
    [Option(
        'p',
        "password",
        Required = true,
        HelpText = "Password"
    )]
    public string Password { get; set; }
}