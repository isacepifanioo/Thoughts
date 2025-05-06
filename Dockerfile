# Use a imagem oficial do .NET SDK
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Adicione a imagem SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copie o arquivo .csproj e restaure as dependências
COPY ["Thoughts.csproj", "./"]
RUN dotnet restore "Thoughts.csproj"

# Copie todos os arquivos do projeto e faça o build
COPY . .
RUN dotnet build "Thoughts.csproj" -c Release -o /app/build

# Publique a aplicação
FROM build AS publish
RUN dotnet publish "Thoughts.csproj" -c Release -o /app/publish

# Copie o build e publique a aplicação
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Thoughts.dll"]