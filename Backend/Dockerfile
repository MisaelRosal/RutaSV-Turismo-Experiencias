# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["IguanaSV.Api.sln", "./"]
COPY ["IguanaSV.Api/IguanaSV.Api.csproj", "IguanaSV.Api/"]
RUN dotnet restore "IguanaSV.Api/IguanaSV.Api.csproj"

COPY . .
WORKDIR "/src/IguanaSV.Api"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Etapa final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IguanaSV.Api.dll"]