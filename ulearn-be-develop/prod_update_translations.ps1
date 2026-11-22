$SOURCE_URL = "https://docs.google.com/spreadsheets/d/1Yd4z7inWDrPPx-02qx_vkvfGzzvXDQBVTfNvUOF7LMg/export?exportFormat=tsv"
$TRANSLATION_ENDPOINT = "https://prod.beapi.yaspeak.com/api/translation/batch"
$BEARER_TOKEN = $env:BEARER_TOKEN

dotnet build .\backend\ULearn.Db.Tools\ -c Development
.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe tsv-upload-translations -u $SOURCE_URL -a $TRANSLATION_ENDPOINT -t $BEARER_TOKEN

curl --request GET --url 'https://www.yaspeak.com/api/revalidate?secret=07ZXeF8RnLOR'