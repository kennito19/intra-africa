FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Catalogue.API/Catalogue.API.csproj", "Catalogue.API/"]
COPY ["Catalogue.Application/Catalogue.Application.csproj", "Catalogue.Application/"]
COPY ["Catalogue.Domain/Catalogue.Domain.csproj", "Catalogue.Domain/"]
COPY ["Catalogue.Infrastructure/Catalogue.Infrastructure.csproj", "Catalogue.Infrastructure/"]
RUN dotnet restore "Catalogue.API/Catalogue.API.csproj"
COPY . .
WORKDIR "/src/Catalogue.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Catalogue.API.dll"]
