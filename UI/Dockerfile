FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY UI/*.csproj ./UI/
COPY Shared/*.csproj ./Shared/
RUN dotnet restore ./UI/UI.csproj

# Copy everything else and build
COPY UI/ ./UI/
COPY Shared/ ./Shared/
RUN dotnet publish ./UI/UI.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /src/out .
RUN rm appsettings.Development.json
ENTRYPOINT ["dotnet", "UI.dll"]