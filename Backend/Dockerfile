# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar el proyecto y restaurar dependencias
COPY ["IguanaSV.Api/IguanaSV.Api.csproj", "IguanaSV.Api/"]
RUN dotnet restore "IguanaSV.Api/IguanaSV.Api.csproj"

# Copiar el resto del código fuente
COPY . .
WORKDIR "/src/IguanaSV.Api"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Etapa final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IguanaSV.Api.dll"]