# Base image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy all project files
COPY . ./

# Restore dependencies
RUN dotnet restore FlightReservation.Application/FlightReservation.Application.csproj
RUN dotnet restore FlightReservation.Infrastructure/FlightReservation.Infrastructure.csproj
RUN dotnet restore FlightReservation.Presentation/FlightReservation.Presentation.csproj

# Build projects
RUN dotnet build FlightReservation.Application/FlightReservation.Application.csproj --no-restore -c Release
RUN dotnet build FlightReservation.Infrastructure/FlightReservation.Infrastructure.csproj --no-restore -c Release
RUN dotnet build FlightReservation.Presentation/FlightReservation.Presentation.csproj --no-restore -c Release

# Run tests for Application and Infrastructure
RUN dotnet test FlightReservation.Application.Test/FlightReservation.Application.Test.csproj --no-restore --no-build --logger "console;verbosity=detailed"
RUN dotnet test FlightReservation.Infrastructure.Test/FlightReservation.Infrastructure.Test.csproj --no-restore --no-build --logger "console;verbosity=detailed"

# Publish the application
RUN dotnet publish FlightReservation.Presentation/FlightReservation.Presentation.csproj -c Release -o out --no-restore

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/FlightReservation.Presentation/out ./

# Expose the port (change if your app uses another port)
EXPOSE 80

# Entry point for the application
ENTRYPOINT ["dotnet", "FlightReservation.Presentation.dll"]

