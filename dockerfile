# Use the official .NET 6.0 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set the working directory in the image to /app
WORKDIR /app

# Copy the .csproj file(s) and restore packages
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application source
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Use the official .NET 6.0 runtime image as the base image for the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:6.0

ENV ASPNETCORE_ENVIRONMENT=Production


# Set the working directory in the image to /app
WORKDIR /app

# Copy the published files into the runtime image
COPY --from=build-env /app/out .

# Expose port 80 for the application
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Expense Tracker.dll"]
