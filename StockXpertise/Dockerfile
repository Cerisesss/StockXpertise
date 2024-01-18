# Utilisez une image de base avec SDK .NET pour l'environnement de développement
FROM mcr.microsoft.com/dotnet/sdk:5.0-windowsservercore-ltsc2022 AS build-env

# Créez un répertoire de travail
WORKDIR /app

# Copiez les fichiers de projet et restaurez les dépendances
COPY *.csproj .
RUN dotnet restore

# Copiez le reste des fichiers et construisez l'application
COPY . .
RUN dotnet publish -c Release -o out

# Utilisez une image plus légère sans SDK pour l'environnement d'exécution
FROM mcr.microsoft.com/windows/servercore:ltsc2022

# Créez un répertoire pour l'application
WORKDIR /app

# Copiez les fichiers de l'environnement de développement
COPY --from=build-env /app/out .

# Commande à exécuter au démarrage du conteneur
ENTRYPOINT ["StockXpertise.exe"]
