using CommandLine;

namespace ULearn.Be.Crypto.Options;

[Verb(
    "encrypt",
    HelpText = @"Encrypt values. You can specify multiple values.
For more info, use 'crypto.exe encrypt help'.

Usage with inline data: crypto.exe encrypt -i <text> [<text> <text> ...] [-p <MasterPassword>]
Usage with inline file: crypto.exe encrypt -f <filename> [-p <MasterPassword>]

Structure of the input JSON file:
  [ {""Configuration_Key"": ""be.module.password"",
        ""Comment"": ""Comment of the configuration key"",
        ""Clear"": ""Clear Password""} ]

Return of json file (same path as input file with *.out extension):
    [ {""Configuration_Key"": ""be.module.password"",
            ""Comment"": ""Comment of the configuration key"",
            ""Clear"": ""Clear Password"",
            ""Encrypted"": ""Encrypted Password"",
            ""SqlUpdate"": ""UPDATE ENV_Configuration ..."",
            ""SqlProcedureExec"": ""EXEC [dbo].[p_ENV_Configure] ...""} ]
"
)]
public class EncryptOptions : IMasterPassword
{
    [Option(
        'i',
        "inline",
        Required = true,
        HelpText = "Mutual exclusive with -f. Text to be encrypted.",
        SetName = "inline"
    )]
    public IEnumerable<string> PlaintextSecrets { get; set; }

    [Option(
        'f',
        "filename",
        Required = true,
        HelpText = "Mutual exclusive with -i. Path to the JSON file.",
        SetName = "file"
    )]
    public string FileName { get; set; }

    [Option('p', "master-password", HelpText = "Master password to be used for decrypting.")]
    public string MasterPassword { get; set; }
}