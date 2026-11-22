$CONNECTION_STRING = "Server=ep-sweet-bonus-agxqjuik-pooler.c-2.eu-central-1.aws.neon.tech;Port=5432;Database=neondb;User Id=neondb_owner;Password=npg_gOeRJvVbUE58"
$SERVER_NAME = "ep-sweet-bonus-agxqjuik-pooler.c-2.eu-central-1.aws.neon.tech"
$PORT="5432"
$DATABASE_NAME = "neondb"
$USER_NAME = "technical_admin"
$PASSWORD = "npg_gOeRJvVbUE58"
$COMMON_SCRIPTS_FOLDER = "./database/scripts/common"
$TEST_CONTENT_FOLDER = "./database/scripts/test_content"
$TEST_CONTENT_PHOTOS_FOLDER = "./database/scripts/test_content/profile_photos"
$DEV_CONFIG_FOLDER = "./database/scripts/dev"
$PROFILE_PHOTOS_UPLOAD_ENDPOINT = "https://beapi.int.yaspeak.com/api/user-file/profile-photo/admin-upload"
$BEARER_TOKEN = $env:BEARER_TOKEN

Write-Output "1. Building db tools"
dotnet build .\backend\ULearn.Db.Tools\ -c Development

Write-Output "2. Drop the database"
.\backend\Ulearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe reset-database -s $SERVER_NAME -o $PORT -d $DATABASE_NAME -u $USER_NAME -p $PASSWORD 

Write-Output "3. Seeding database"
dotnet ef database update -v --project ".\backend\ULearn.Be.Data\ULearn.Be.Data.csproj" --connection $CONNECTION_STRING
dotnet ef database update -v --project ".\backend\ULearn.Be.File.Data\ULearn.Be.File.Data.csproj" --connection $CONNECTION_STRING

Write-Output "4. Uploading common scripts"
.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe run-scripts -s $SERVER_NAME -o $PORT -d $DATABASE_NAME -u $USER_NAME -p $PASSWORD -f $COMMON_SCRIPTS_FOLDER

Write-Output "5. Uploading env-specific scripts"
.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe run-scripts -s $SERVER_NAME -o $PORT -d $DATABASE_NAME -u $USER_NAME -p $PASSWORD -f $DEV_CONFIG_FOLDER

Write-Output "6. Uploading test content"
.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe run-scripts -s $SERVER_NAME -o $PORT -d $DATABASE_NAME -u $USER_NAME -p $PASSWORD -f $TEST_CONTENT_FOLDER

curl --request POST "https://gitlab.com/api/v4/projects/38748396/trigger/pipeline?token=glptt-3e001fc3e472acc63e56d23feb8db5ca7def0b55&ref=develop"
Write-Output "7. Restarting BE.API and FileService.API ... waiting 30 seconds ..."
Start-Sleep -Seconds 30

Write-Output "8. Uploading photos"
.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe upload-profile-photos -f $TEST_CONTENT_PHOTOS_FOLDER -t $BEARER_TOKEN -a $PROFILE_PHOTOS_UPLOAD_ENDPOINT

Write-Output "9. Uploading transaction templates"
.\dev_update_transaction_templates.ps1

Write-Output "10. Uploading translations"
.\dev_update_translations.ps1