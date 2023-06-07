FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ShopOfThings.sln .
COPY Data/Data.csproj ./Data/
COPY Business/Business.csproj ./Business/
COPY ShopOfThings.Tests/ShopOfThings.Tests.csproj ./ShopOfThings.Tests/
COPY WebApi/WebApi.csproj ./WebApi/

RUN dotnet restore

COPY . .

RUN dotnet build -c Release -o /app/build/WebApi

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish/WebApi --no-restore

FROM base AS final
EXPOSE 5000
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish/WebApi .

# Set the entry point
ENTRYPOINT ["dotnet", "WebApi.dll"]