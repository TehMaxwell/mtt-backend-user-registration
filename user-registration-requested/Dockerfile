# DOCKERFILE
# A base docker build file for all MTT consumer/producer containers.

# ----- BUILD SEQUENCE -----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS buildImage
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# ----- FINAL IMAGE -----
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=buildImage /app/out .
ENTRYPOINT [ "./user-registration-requested" ]
