FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY AuthHexagonalApi.sln ./
COPY AuthHexagonalApi.Domain/AuthHexagonalApi.Domain.csproj AuthHexagonalApi.Domain/
COPY AuthHexagonalApi.Application/AuthHexagonalApi.Application.csproj AuthHexagonalApi.Application/
COPY AuthHexagonalApi.Infrastructure/AuthHexagonalApi.Infrastructure.csproj AuthHexagonalApi.Infrastructure/
COPY AuthHexagonalApi.WebApi/AuthHexagonalApi.WebApi.csproj AuthHexagonalApi.WebApi/

RUN dotnet restore AuthHexagonalApi.WebApi/AuthHexagonalApi.WebApi.csproj

COPY . .
RUN dotnet publish AuthHexagonalApi.WebApi/AuthHexagonalApi.WebApi.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "AuthHexagonalApi.WebApi.dll"]
