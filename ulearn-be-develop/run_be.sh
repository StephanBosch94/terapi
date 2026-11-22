#!/bin/bash

# Start ULearn.Be.Api
osascript -e 'tell application "Terminal" to do script "cd '"$PWD"' && dotnet run --project ./backend/ULearn.Be.Api/ULearn.Be.Api.csproj"'
sleep 10

# Start ULearn.Be.File.Api
osascript -e 'tell application "Terminal" to do script "cd '"$PWD"' && dotnet run --project ./backend/ULearn.Be.File.Api/ULearn.Be.File.Api.csproj"'
sleep 10

# Start Yaspeak.Be.Scheduler
osascript -e 'tell application "Terminal" to do script "cd '"$PWD"' && dotnet run --project ./backend/Yaspeak.Be.Scheduler/Yaspeak.Be.Scheduler.csproj"'