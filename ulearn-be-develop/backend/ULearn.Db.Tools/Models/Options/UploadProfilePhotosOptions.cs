using CommandLine;

namespace ULearn.Db.Tools.Models.Options;

[Verb(
    "upload-profile-photos",
    HelpText = @"Uploads profile photos from the specified folder"
)]
public class UploadProfilePhotosOptions
{
    [Option(
        'f',
        "folder",
        Required = true,
        HelpText = "Path to the folder with profile photos where each photo is labeled with email"
    )]
    public string PathToFolder { get; set; }
    
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