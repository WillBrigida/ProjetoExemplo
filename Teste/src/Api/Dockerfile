FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

Label version="1.0"

COPY dist /app

WORKDIR /app

EXPOSE 8080

EXPOSE 8081

ENTRYPOINT ["dotnet", "Api.dll"]
