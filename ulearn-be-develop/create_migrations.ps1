#Remove-Item 'backend\Ulearn.Be.Data\Migrations'
#Remove-Item 'backend\Ulearn.Be.File.Data\Migrations'


dotnet ef migrations add Init --project "backend\ULearn.Be.File.Data\ULearn.Be.File.Data.csproj" -o "Migrations" -v
dotnet ef migrations add Init --project "backend\ULearn.Be.Data\ULearn.Be.Data.csproj" -o "Migrations" -v