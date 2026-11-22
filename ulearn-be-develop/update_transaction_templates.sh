#!/bin/bash

TRANSACTION_TEMPLATE_ENDPOINT="http://localhost:5000/api/transaction-template"
BEARER_TOKEN="${BEARER_TOKEN:?Set BEARER_TOKEN}"
GIT_KEY="${GIT_KEY:?Set GIT_KEY}"
STYLES_FILE="https://gitlab.com/api/v4/projects/48038543/repository/files/styles.css?ref=main"

# Build the tools project
dotnet build ./backend/ULearn.Db.Tools/ -c Development

# Define path to the tool
TOOL="./backend/ULearn.Db.Tools/bin/Development/net6.0/ULearn.Db.Tools.exe"

# Array of HTML templates
TEMPLATES=(
  "platforms_commission_receipt.html"
  "platforms_credit_memo.html"
  "platforms_withdrawal_confirmation.html"
  "tutors_credit_memo.html"
  "tutors_receipt_with_taxes.html"
  "tutors_receipt_without_taxes.html"
  "money_transfer_confirmation.html"
)

# Base URL
BASE_URL="https://gitlab.com/api/v4/projects/48038543/repository/files"

# Upload each template
for TEMPLATE in "${TEMPLATES[@]}"; do
  SOURCE_URL="$BASE_URL/$TEMPLATE?ref=main"
  "$TOOL" upload-transaction-template -s "$SOURCE_URL" -l "$STYLES_FILE" -c "$GIT_KEY" -a "$TRANSACTION_TEMPLATE_ENDPOINT" -t "$BEARER_TOKEN"
done
