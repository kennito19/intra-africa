FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["API-Gateway.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build /src/StaticFiles ./StaticFiles
COPY --from=build /src/Resources ./Resources
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "API-Gateway.dll"]
