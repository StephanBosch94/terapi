#!/bin/bash

CONNECTION_STRING="Server=localhost,5432;Database=ulearn;User Id=postgres;Password=12345"
SERVER_NAME="localhost"
DATABASE_NAME="ulearn"
USER_NAME="postgres"
PASSWORD="12345"
PORT="5432"
COMMON_SCRIPTS_FOLDER="./database/scripts/common"
LOCAL_SCRIPTS_FOLDER="./database/scripts/local"
TEST_CONTENT_FOLDER="./database/scripts/test_content"
TEST_CONTENT_PHOTOS_FOLDER="./database/scripts/test_content/profile_photos"
PROFILE_PHOTOS_UPLOAD_ENDPOINT="http://localhost:5000/api/user-file/profile-photo/admin-upload"

BEARER_TOKEN="${BEARER_TOKEN:?Set BEARER_TOKEN}"

# Build the tool project
dotnet build ./backend/ULearn.Db.Tools/ -c Development

# Drop the database
dotnet ./backend/ULearn.Db.Tools/bin/Development/net6.0/ULearn.Db.Tools.dll drop-database -s "$SERVER_NAME" -o "$PORT" -d "$DATABASE_NAME" -u "$USER_NAME" -p "$PASSWORD"

# Kill running services on ports 5000 and 5100
echo "Killing app on port 5000"
kill -9 $(lsof -ti :5000)

echo "Killing app on port 5100"
kill -9 $(lsof -ti :5100)


# Publish the projects
dotnet publish ./backend/ULearn.Db.Tools/ --no-self-contained 
dotnet publish ./backend/ULearn.Be.Api/ --no-self-contained 
dotnet publish ./backend/ULearn.Be.File.Api/ --no-self-contained 

# Create the database
dotnet ./backend/ULearn.Db.Tools/bin/Development/net6.0/ULearn.Db.Tools.dll create-database -s "$SERVER_NAME" -o "$PORT" -d "$DATABASE_NAME" -u "$USER_NAME" -p "$PASSWORD"

# Apply EF Core migrations
dotnet ef database update -v --project "./backend/ULearn.Be.Data/ULearn.Be.Data.csproj" --connection "$CONNECTION_STRING"
dotnet ef database update -v --project "./backend/ULearn.Be.File.Data/ULearn.Be.File.Data.csproj" --connection "$CONNECTION_STRING"

# Run SQL scripts
dotnet ./backend/ULearn.Db.Tools/bin/Development/net6.0/ULearn.Db.Tools.dll run-scripts -s "$SERVER_NAME" -o "$PORT" -d "$DATABASE_NAME" -u "$USER_NAME" -p "$PASSWORD" -f "$COMMON_SCRIPTS_FOLDER"

dotnet ./backend/ULearn.Db.Tools/bin/Development/net6.0/ULearn.Db.Tools.dll run-scripts -s "$SERVER_NAME" -o "$PORT" -d "$DATABASE_NAME" -u "$USER_NAME" -p "$PASSWORD" -f "$LOCAL_SCRIPTS_FOLDER"

dotnet ./backend/ULearn.Db.Tools/bin/Development/net6.0/ULearn.Db.Tools.dll run-scripts -s "$SERVER_NAME" -o "$PORT" -d "$DATABASE_NAME" -u "$USER_NAME" -p "$PASSWORD" -f "$TEST_CONTENT_FOLDER"

# Start backend services in background (assuming run_be.ps1 has Linux-friendly commands)
./run_be.sh

# Wait for services to spin up
sleep 15

# Upload profile photos
dotnet ./backend/ULearn.Db.Tools/bin/Development/net6.0/ULearn.Db.Tools.dll upload-profile-photos -f "$TEST_CONTENT_PHOTOS_FOLDER" -t "$BEARER_TOKEN" -a "$PROFILE_PHOTOS_UPLOAD_ENDPOINT"

# Run other update scripts
./update_translations.sh
./update_transaction_templates.sh