FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Sancus-Discord.NET/Sancus-Discord.NET.csproj", "Sancus-Discord.NET/"]
COPY ["LoggerService/LoggerService.csproj", "LoggerService/"]
COPY ["MongoDbService/MongoDbService.csproj", "MongoDbService/"]
RUN dotnet restore "Sancus-Discord.NET/Sancus-Discord.NET.csproj"
COPY . .
WORKDIR "/src/Sancus-Discord.NET"
RUN dotnet build "Sancus-Discord.NET.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sancus-Discord.NET.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sancus-Discord.NET.dll"]
