FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app

# Render uses PORT environment variable (default: 10000)
ENV PORT=10000
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
EXPOSE ${PORT}

FROM base AS build
WORKDIR /src
COPY ["backend/ULearn.Be.File.Api/ULearn.Be.File.Api.csproj", "backend/ULearn.Be.File.Api/"]
COPY ["backend/ULearn.Be.File.Service/ULearn.Be.File.Service.csproj", "backend/ULearn.Be.File.Service/"]
COPY ["backend/ULearn.Be.File.Data/ULearn.Be.File.Data.csproj", "backend/ULearn.Be.File.Data/"]
COPY ["backend/ULearn.Be.Api.Common/ULearn.Be.Api.Common.csproj", "backend/ULearn.Be.Api.Common/"]
COPY ["backend/ULearn.Be.Common/ULearn.Be.Common.csproj", "backend/ULearn.Be.Common/"]
COPY ["backend/ULearn.Be.Common.Services/ULearn.Be.Common.Services.csproj", "backend/ULearn.Be.Common.Services/"]
COPY ["backend/ULearn.Be.Providers/ULearn.Be.Providers.csproj", "backend/ULearn.Be.Providers/"]

RUN dotnet restore "backend/ULearn.Be.File.Api/ULearn.Be.File.Api.csproj"
COPY backend ./backend
WORKDIR "/src/backend/ULearn.Be.File.Api"
RUN dotnet build "ULearn.Be.File.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ULearn.Be.File.Api.csproj" -c Release -o /app


FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# Create directory for persistent file storage
RUN mkdir -p /data/files && chmod 755 /data/files

# Declare volume for persistent file storage
VOLUME ["/data/files"]

ENTRYPOINT ["dotnet", "ULearn.Be.File.Api.dll"]

