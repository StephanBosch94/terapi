$TRANSACTION_TEMPLATE_ENDPOINT = "http://localhost:5000/api/transaction-template"
$BEARER_TOKEN = $env:BEARER_TOKEN
$GIT_KEY = $env:GIT_KEY
$STYLES_FILE = "https://gitlab.com/api/v4/projects/48038543/repository/files/styles.css?ref=main"
dotnet build .\backend\ULearn.Db.Tools\ -c Development

.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe upload-transaction-template -s "https://gitlab.com/api/v4/projects/48038543/repository/files/platforms_commission_receipt.html?ref=main" -l $STYLES_FILE -c $GIT_KEY -a $TRANSACTION_TEMPLATE_ENDPOINT -t $BEARER_TOKEN
.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe upload-transaction-template -s "https://gitlab.com/api/v4/projects/48038543/repository/files/platforms_credit_memo.html?ref=main" -l $STYLES_FILE -c $GIT_KEY -a $TRANSACTION_TEMPLATE_ENDPOINT -t $BEARER_TOKEN
.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe upload-transaction-template -s "https://gitlab.com/api/v4/projects/48038543/repository/files/platforms_withdrawal_confirmation.html?ref=main" -l $STYLES_FILE -c $GIT_KEY -a $TRANSACTION_TEMPLATE_ENDPOINT -t $BEARER_TOKEN
.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe upload-transaction-template -s "https://gitlab.com/api/v4/projects/48038543/repository/files/tutors_credit_memo.html?ref=main" -l $STYLES_FILE -c $GIT_KEY -a $TRANSACTION_TEMPLATE_ENDPOINT -t $BEARER_TOKEN
.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe upload-transaction-template -s "https://gitlab.com/api/v4/projects/48038543/repository/files/tutors_receipt_with_taxes.html?ref=main" -l $STYLES_FILE -c $GIT_KEY -a $TRANSACTION_TEMPLATE_ENDPOINT -t $BEARER_TOKEN
.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe upload-transaction-template -s "https://gitlab.com/api/v4/projects/48038543/repository/files/tutors_receipt_without_taxes.html?ref=main" -l $STYLES_FILE -c $GIT_KEY -a $TRANSACTION_TEMPLATE_ENDPOINT -t $BEARER_TOKEN
.\backend\ULearn.Db.Tools\bin\Development\net6.0\ULearn.Db.Tools.exe upload-transaction-template -s "https://gitlab.com/api/v4/projects/48038543/repository/files/money_transfer_confirmation.html?ref=main" -l $STYLES_FILE -c $GIT_KEY -a $TRANSACTION_TEMPLATE_ENDPOINT -t $BEARER_TOKEN