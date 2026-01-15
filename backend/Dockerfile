# ---------- BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copiamos solution y csproj (para cache de restore)
COPY *.sln .
COPY src/Fundo.Applications.WebApi/*.csproj src/Fundo.Applications.WebApi/

# Navegamos a la carpeta del proyecto ANTES de restaurar
WORKDIR /src/src/Fundo.Applications.WebApi
RUN dotnet restore

# Volvemos a la raíz para copiar todo
WORKDIR /src
COPY . .

# Publicamos desde la carpeta del proyecto
WORKDIR /src/src/Fundo.Applications.WebApi
RUN dotnet publish -c Release -o /app/publish

# ---------- RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Fundo.Applications.WebApi.dll"]
