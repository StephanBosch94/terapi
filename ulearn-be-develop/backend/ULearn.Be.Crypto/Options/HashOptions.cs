using CommandLine;

namespace ULearn.Be.Crypto.Options;

[Verb(
    "hash",
    HelpText = @"Hash passwords. Use format 'username:password' or 'password'.
For more info, use 'crypto.exe hash help'.

Usage with inline data: crypto.exe hash -i <username:password> [<username:password> <username:password> ...]
Usage with file: crypto.exe hash -f data.txt

Returns JSON:
    [ {""username"": ""xxx"", ""password"": ""test123"" } ]

Input file format (each user:password on new line):
user1:password1
user2:password2
user3:password3
"
)]
public class HashOptions
{
    [Option(
        'i',
        "inline",
        Required = true,
        HelpText = "Mutual exclusive with -f. Use format 'username:password' or 'password'.",
        SetName = "inline"
    )]
    public IEnumerable<string> Passwords { get; set; }

    [Option(
        'f',
        "filename",
        Required = true,
        HelpText = "Mutual exclusive with -i. One password per line. Use format 'username:password' or 'password'.",
        SetName = "file"
    )]
    public string FileName { get; set; }
}