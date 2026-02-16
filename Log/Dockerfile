FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Log.Api/Log.Api.csproj", "Log.Api/"]
COPY ["Log.Application/Log.Application.csproj", "Log.Application/"]
COPY ["Log.Domain/Log.Domain.csproj", "Log.Domain/"]
COPY ["Log.Infrastructure/Log.Infrastructure.csproj", "Log.Infrastructure/"]
RUN dotnet restore "Log.Api/Log.Api.csproj"
COPY . .
WORKDIR "/src/Log.Api"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Log.Api.dll"]
