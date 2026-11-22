$SOURCE_URL = "https://docs.google.com/spreadsheets/d/1Yd4z7inWDrPPx-02qx_vkvfGzzvXDQBVTfNvUOF7LMg/export?exportFormat=tsv"
$TRANSLATION_ENDPOINT = "https://beapi.int.yaspeak.com/api/translation/batch"
$BEARER_TOKEN = $env:BEARER_TOKEN

Write-Output "1. Updating translations in DB"
dotnet build .\backend\ULearn.Db.Tools\ -c Development
.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe tsv-upload-translations -u $SOURCE_URL -a $TRANSLATION_ENDPOINT -t $BEARER_TOKEN

Write-Output "2. Revalidating pages"
curl --request GET --url 'https://int.yaspeak.com/api/revalidate?secret=07ZXeF8RnLOR'