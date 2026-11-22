using CommandLine;

namespace ULearn.Be.Crypto.Options;

[Verb(
    "decrypt",
    HelpText = @"Decrypt secrets. You can specify multiple secrets.
For more info, use 'crypto.exe decrypt help'.

Usage: crypto.exe decrypt -i <cipher> [<cipher> <cipher> ...] [-p <MasterPassword>]"
)]
public class DecryptOptions
{
    [Option('i', "inline", Required = true, HelpText = "Secrets to be decrypted.", SetName = "inline")]
    public IEnumerable<string> EncryptedSecrets { get; set; }

    [Option('p', "master-password", HelpText = "Master password to be used for decrypting.")]
    public string MasterPassword { get; set; }
}