# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos del proyecto
COPY ["BazarExam.csproj", "./"]
RUN dotnet restore "./BazarExam.csproj"

# Copiar el resto y compilar
COPY . .
RUN dotnet publish "BazarExam.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Render usa el puerto 8080 por defecto
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BazarExam.dll"]
