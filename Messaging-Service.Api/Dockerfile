FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80

COPY Messaging-Service.Api/Messaging-Service.Api.csproj Messaging-Service.Api/
RUN dotnet restore Messaging-Service.Api/Messaging-Service.Api.csproj
COPY . .
RUN dotnet publish Messaging-Service.Api/Messaging-Service.Api.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","Messaging-Service.Api.dll"]