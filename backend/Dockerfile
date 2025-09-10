# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/LearningCenter.API/LearningCenter.API.csproj", "src/LearningCenter.API/"]
COPY ["src/LearningCenter.Application/LearningCenter.Application.csproj", "src/LearningCenter.Application/"]
COPY ["src/LearningCenter.Domain/LearningCenter.Domain.csproj", "src/LearningCenter.Domain/"]
COPY ["src/LearningCenter.Infrastructure/LearningCenter.Infrastructure.csproj", "src/LearningCenter.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/LearningCenter.API/LearningCenter.API.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/src/LearningCenter.API"
RUN dotnet build "LearningCenter.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "LearningCenter.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install PostgreSQL client
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=publish /app/publish .

# Create logs directory
RUN mkdir -p /app/logs

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Start the application
ENTRYPOINT ["dotnet", "LearningCenter.API.dll"]
