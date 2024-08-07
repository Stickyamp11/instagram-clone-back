#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["1-Presentation/Instagram_Api.Presentation.csproj", "1-Presentation/"]
COPY ["1.5- Contracts/Instagram_Api.Contracts.csproj", "1.5- Contracts/"]
COPY ["2-Application/Instagram_Api.Application.csproj", "2-Application/"]
COPY ["3-Domain/Instagram_Api.Domain.csproj", "3-Domain/"]
COPY ["4-Infrastructure/Instagram_Api.Infrastructure.csproj", "4-Infrastructure/"]
RUN dotnet restore "./1-Presentation/./Instagram_Api.Presentation.csproj"
COPY . .
WORKDIR "/src/1-Presentation"
RUN dotnet build "./Instagram_Api.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Instagram_Api.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Instagram_Api.Presentation.dll"]