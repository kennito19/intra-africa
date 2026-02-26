FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["JWTTokenProvider.API/JWTTokenProvider.API.csproj", "JWTTokenProvider.API/"]
COPY ["IDServer.Domain/IDServer.Domain.csproj", "IDServer.Domain/"]
COPY ["IDServer.Infrastructure/IDServer.Infrastructure.csproj", "IDServer.Infrastructure/"]
COPY ["IDServerApplication/IDServerApplication.csproj", "IDServerApplication/"]
RUN dotnet restore "JWTTokenProvider.API/JWTTokenProvider.API.csproj"
COPY . .
WORKDIR "/src/JWTTokenProvider.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build /src/JWTTokenProvider.API/StaticFiles ./StaticFiles
COPY entrypoint.sh /entrypoint.sh
RUN apt-get update && apt-get install -y openssh-client netcat-openbsd && rm -rf /var/lib/apt/lists/* && sed -i 's/\r//' /entrypoint.sh && chmod +x /entrypoint.sh
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["/entrypoint.sh", "dotnet", "JWTTokenProvider.API.dll"]
