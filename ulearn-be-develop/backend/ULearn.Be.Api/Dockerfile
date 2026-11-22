FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app

# Render uses PORT environment variable (default: 10000)
ENV PORT=10000
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
EXPOSE ${PORT}

FROM base AS build
WORKDIR /src
COPY ["backend/ULearn.Be.Api/ULearn.Be.Api.csproj", "backend/ULearn.Be.Api/"]
COPY ["backend/ULearn.Be.Api.Common/ULearn.Be.Api.Common.csproj", "backend/ULearn.Be.Api.Common/"]
COPY ["backend/ULearn.Be.Common/ULearn.Be.Common.csproj", "backend/ULearn.Be.Common/"]
COPY ["backend/ULearn.Be.Common.Services/ULearn.Be.Common.Services.csproj", "backend/ULearn.Be.Common.Services/"]
COPY ["backend/ULearn.Be.Data/ULearn.Be.Data.csproj", "backend/ULearn.Be.Data/"]
COPY ["backend/ULearn.Be.Models/ULearn.Be.Models.csproj", "backend/ULearn.Be.Models/"]
COPY ["backend/ULearn.Be.Providers/ULearn.Be.Providers.csproj", "backend/ULearn.Be.Providers/"]
COPY ["backend/ULearn.Be.Repositories/ULearn.Be.Repositories.csproj", "backend/ULearn.Be.Repositories/"]
COPY ["backend/ULearn.Be.Services/ULearn.Be.Services.csproj", "backend/ULearn.Be.Services/"]
COPY ["backend/ULearn.Be.Shared.Services/ULearn.Be.Shared.Services.csproj", "backend/ULearn.Be.Shared.Services/"]
COPY ["backend/ULearn.Be.Websocket.Services/ULearn.Be.Websocket.Services.csproj", "backend/ULearn.Be.Websocket.Services/"]

RUN dotnet restore "backend/ULearn.Be.Api/ULearn.Be.Api.csproj"
COPY backend ./backend
WORKDIR "/src/backend/ULearn.Be.Api"
RUN dotnet build "ULearn.Be.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ULearn.Be.Api.csproj" -c Release -o /app


FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ULearn.Be.Api.dll"]

