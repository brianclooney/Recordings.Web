FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY API/*.csproj ./API/
COPY Shared/*.csproj ./Shared/
RUN dotnet restore ./API/API.csproj

# Copy everything else and build
COPY API/ ./API/
COPY Shared/ ./Shared/
RUN dotnet publish ./API/API.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /src/out .
RUN rm appsettings.Development.json
ENTRYPOINT ["dotnet", "API.dll"]