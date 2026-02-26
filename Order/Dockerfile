FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Order.API/Order.API.csproj", "Order.API/"]
COPY ["Order.Application/Order.Application.csproj", "Order.Application/"]
COPY ["Order.Domain/Order.Domain.csproj", "Order.Domain/"]
COPY ["Order.Infrastructure/Order.Infrastructure.csproj", "Order.Infrastructure/"]
RUN dotnet restore "Order.API/Order.API.csproj"
COPY . .
WORKDIR "/src/Order.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build /src/Order.API/StaticFiles ./StaticFiles
COPY entrypoint.sh /entrypoint.sh
RUN apt-get update && apt-get install -y openssh-client netcat-openbsd && rm -rf /var/lib/apt/lists/* && sed -i 's/\r//' /entrypoint.sh && chmod +x /entrypoint.sh
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["/entrypoint.sh", "dotnet", "Order.API.dll"]
