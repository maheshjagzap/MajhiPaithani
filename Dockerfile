# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["MajhiPaithani.API/MajhiPaithani.API.csproj", "MajhiPaithani.API/"]
COPY ["MajhiPaithani.Application/MajhiPaithani.Application.csproj", "MajhiPaithani.Application/"]
COPY ["MajhiPaithani.Domain/MajhiPaithani.Domain.csproj", "MajhiPaithani.Domain/"]
COPY ["MajhiPaithani.Infrastructure/MajhiPaithani.Infrastructure.csproj", "MajhiPaithani.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "MajhiPaithani.API/MajhiPaithani.API.csproj"

# Copy the rest of the source
COPY . .

# Build the project
WORKDIR "/src/MajhiPaithani.API"
RUN dotnet build "MajhiPaithani.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MajhiPaithani.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "MajhiPaithani.API.dll"]