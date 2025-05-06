# Use a imagem oficial do .NET SDK
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080  # Alterar para a porta 8080

# Adicione a imagem SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Thoughts.csproj", "/app/"]
RUN dotnet restore "Thoughts.csproj"
COPY . . 
WORKDIR "/src"
RUN dotnet build "Thoughts.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Thoughts.csproj" -c Release -o /app/publish

# Copie o build e publique a aplicação
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "Thoughts.dll"]
