FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copiar archivos de proyecto
COPY . .

# Restaurar dependencias
RUN dotnet restore

# Compilar
RUN dotnet build -c Release -o /app/build

# Publicar
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "NotificationAPI.API.dll"]
