# First run
```
.\init_local_db.ps1
```

# Reset db
```
.\reset_local_db.ps1
```

# Update be
- If BE is running, you need to close it first
- Then run the following commands
```
git pull
.\reset_local_db.ps1 
```

# Build
```
docker build -t ulearn -f Dockerfile .
``` 

# Run
```
docker run -p 8000:80  -it ulearn
```

# Create a migration
```
dotnet ef migrations add Init  --project "ULearn.Be.Data\ULearn.Be.Data.csproj" -o "Migrations" -v
dotnet ef migrations add Init  --project "ULearn.Be.File.Data\ULearn.Be.File.Data.csproj" -o "Migrations" -v
```

# Apply migration
```
dotnet ef migrations script --project "ULearn.Be.Data\ULearn.Be.Data.csproj"
dotnet ef database update -v --project "ULearn.Be.Data\ULearn.Be.Data.csproj" --connection "<connection_string>"
```
