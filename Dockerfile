#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["QRCodeTracker.sln", "./"]
COPY ["src/QRCodeTracker.csproj", "./src/"]
COPY ["tests/QRCodeTrackerTests.csproj", "./tests/"]
RUN dotnet restore
COPY . .
WORKDIR "/src/."
RUN dotnet build -c Release -o /app/build

from build AS test
RUN dotnet test

FROM build AS publish
RUN dotnet publish "./src/QRCodeTracker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QRCodeTracker.dll"]