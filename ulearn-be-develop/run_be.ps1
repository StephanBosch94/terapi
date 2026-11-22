Start-Process PowerShell -Argument "dotnet run --project  .\backend\ULearn.Be.Api\ULearn.Be.Api.csproj"
Start-Sleep -Seconds 10
Start-Process PowerShell -Argument "dotnet run --project  .\backend\ULearn.Be.File.Api\ULearn.Be.File.Api.csproj"
Start-Sleep -Seconds 10
Start-Process PowerShell -Argument "dotnet run --project  .\backend\Yaspeak.Be.Scheduler\Yaspeak.Be.Scheduler.csproj"