$BASE_ENDPOINT = "https://beapi.int.yaspeak.com"
$BEARER_TOKEN = $env:BEARER_TOKEN

dotnet build .\backend\ULearn.Db.Tools\ -c Development
.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe batch-delete-accounts -a $BASE_ENDPOINT -t $BEARER_TOKEN -e "yaspeakczechia@gmail.com" "ulearn.cz@gmail.com" "yaspeak.com@gmail.com" "stefanobos1994@gmail.com" "stephanbosch94@gmail.com" "ulearn.cz@seznam.cz"
