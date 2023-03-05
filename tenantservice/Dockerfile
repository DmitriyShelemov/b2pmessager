#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["tenantservice.csproj", "."]

RUN dotnet nuget add source http://BagetIp:80/v3/index.json -n LocalNuget

RUN dotnet restore "./tenantservice.csproj"

COPY . .
WORKDIR "/src/."
RUN dotnet build "tenantservice.csproj" -c Release -o /app/buildcd 

FROM build AS publish
RUN dotnet publish "tenantservice.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "tenantservice.dll"]




