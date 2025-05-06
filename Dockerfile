# Use a imagem oficial do .NET SDK
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Adicione a imagem SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SeuProjeto/SeuProjeto.csproj", "SeuProjeto/"]
RUN dotnet restore "SeuProjeto/SeuProjeto.csproj"
COPY . .
WORKDIR "/src/SeuProjeto"
RUN dotnet build "SeuProjeto.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SeuProjeto.csproj" -c Release -o /app/publish

# Copie o build e publique a aplicação
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeuProjeto.dll"]