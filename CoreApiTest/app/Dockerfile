FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
ARG mongo_url=mongodb://username:password@cluster0-lozjx.gcp.mongodb.net/sampledb
ENV ControllerSettings__DbConfig__DbConnectionString=$mongo_url

COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "app.dll"]
