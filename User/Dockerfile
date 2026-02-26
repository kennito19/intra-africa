FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["User.API/User.API.csproj", "User.API/"]
COPY ["User.Application/User.Application.csproj", "User.Application/"]
COPY ["User.Domain/User.Domain.csproj", "User.Domain/"]
COPY ["User.Infrastructure/User.Infrastructure.csproj", "User.Infrastructure/"]
RUN dotnet restore "User.API/User.API.csproj"
COPY . .
WORKDIR "/src/User.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build /src/User.API/StaticFiles ./StaticFiles
COPY entrypoint.sh /entrypoint.sh
RUN apt-get update && apt-get install -y openssh-client netcat-openbsd && rm -rf /var/lib/apt/lists/* && sed -i 's/\r//' /entrypoint.sh && chmod +x /entrypoint.sh
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["/entrypoint.sh", "dotnet", "User.API.dll"]
