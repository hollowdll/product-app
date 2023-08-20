# Use this Dockerfile to build an image of the backend app and run it as container using Docker

# Use the official .NET SDK to build the application
# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ./*.csproj .
RUN dotnet restore --use-current-runtime

# copy everything else and build app
COPY . .
RUN dotnet publish --use-current-runtime --self-contained false --no-restore -o /app

# Use the official ASP.NET Core runtime as a base image
# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "productapi.dll"]
