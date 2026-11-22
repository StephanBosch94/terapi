$TRANSLATION_ENDPOINT = "http://localhost:5000/api/translation/batch"
$BEARER_TOKEN = $env:BEARER_TOKEN
$SOURCE_URL = "https://docs.google.com/spreadsheets/d/1Yd4z7inWDrPPx-02qx_vkvfGzzvXDQBVTfNvUOF7LMg/export?exportFormat=tsv"

dotnet build .\backend\ULearn.Db.Tools\ -c Development
.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe tsv-upload-translations -u $SOURCE_URL -a $TRANSLATION_ENDPOINT -t $BEARER_TOKEN