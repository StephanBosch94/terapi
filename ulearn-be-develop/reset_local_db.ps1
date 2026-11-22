$CONNECTION_STRING = "Server=localhost,5432;Database=ulearn;User Id=postgres;Password=12345"
$SERVER_NAME = "localhost"
$DATABASE_NAME = "ulearn"
$USER_NAME = "postgres"
$PASSWORD = "12345"
$PORT="5432"
$COMMON_SCRIPTS_FOLDER = "./database/scripts/common"
$LOCAL_SCRIPTS_FOLDER = "./database/scripts/local"
$TEST_CONTENT_FOLDER = "./database/scripts/test_content"
$TEST_CONTENT_PHOTOS_FOLDER = "./database/scripts/test_content/profile_photos"
$PROFILE_PHOTOS_UPLOAD_ENDPOINT = "http://localhost:5000/api/user-file/profile-photo/admin-upload"

$BEARER_TOKEN = $env:BEARER_TOKEN

dotnet build .\backend\ULearn.Db.Tools\ -c Development

.\backend\Ulearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe drop-database -s  $SERVER_NAME -o $PORT -d $DATABASE_NAME -u $USER_NAME -p $PASSWORD

npx kill-port 5000
npx kill-port 5100

dotnet publish .\backend\ULearn.Db.Tools\ --no-self-contained 
dotnet publish .\backend\ULearn.Be.Api\ --no-self-contained 
dotnet publish .\backend\ULearn.Be.File.Api\  --no-self-contained 

.\backend\Ulearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe create-database -s $SERVER_NAME -o $PORT -d $DATABASE_NAME -u $USER_NAME -p $PASSWORD

dotnet ef database update -v --project ".\backend\ULearn.Be.Data\ULearn.Be.Data.csproj" --connection $CONNECTION_STRING
dotnet ef database update -v --project ".\backend\ULearn.Be.File.Data\ULearn.Be.File.Data.csproj" --connection $CONNECTION_STRING

.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe run-scripts -s $SERVER_NAME -o $PORT -d $DATABASE_NAME -u $USER_NAME -p $PASSWORD -f $COMMON_SCRIPTS_FOLDER

.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe run-scripts -s $SERVER_NAME -o $PORT -d $DATABASE_NAME -u $USER_NAME -p $PASSWORD -f $LOCAL_SCRIPTS_FOLDER

.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe run-scripts -s $SERVER_NAME -o $PORT -d $DATABASE_NAME -u $USER_NAME -p $PASSWORD -f $TEST_CONTENT_FOLDER

Start-Process PowerShell -Argument ".\run_be.ps1"

Start-Sleep -Seconds 15

.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe upload-profile-photos -f $TEST_CONTENT_PHOTOS_FOLDER -t $BEARER_TOKEN -a $PROFILE_PHOTOS_UPLOAD_ENDPOINT
.\update_translations.ps1
.\update_transaction_templates.ps1