# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Esta fase é usada para compilar o projeto de serviço
#define que a imagem base para construção é o SDK do .NET 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#define que o projeto sera construido em modo Release
ARG BUILD_CONFIGURATION=Release 

# Define o diretório de trabalho dentro do container para /src
WORKDIR /src
#    ["pasta/arquivo .csproj", "pasta de destino (docker)/"]
COPY ["umfgcloud.loja.webapi/umfgcloud.loja.webapi.csproj", "marscloud.atlas.presentation/"]
COPY ["umfgcloud.loja.aplicacao.service/umfgcloud.loja.aplicacao.service.csproj", "umfgcloud.loja.aplicacao.service/"]
COPY ["umfgcloud.loja.dominio.service/umfgcloud.loja.dominio.service.csproj", "umfgcloud.loja.dominio.service/"]
COPY ["umfgcloud.infraestrutura.service/umfgcloud.infraestrutura.service.csproj", "umfgcloud.infraestrutura.service/"]

# Restaura as dependências NuGet do projeto antes de copiar o restante do código
# para aproveitar o cache de camadas do Docker e acelerar o build.
RUN dotnet restore "./umfgcloud.loja.webapi/umfgcloud.loja.webapi.csproj"
COPY . .

# Define a pasta de trabalho do container e compila o projeto no modo configurado, 
# gerando os binários na pasta /app/build.
WORKDIR "/src/umfgcloud.loja.webapi"
RUN dotnet build "./umfgcloud.loja.webapi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./umfgcloud.loja.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal 
# (padrão quando não está usando a configuração de Depuração)
# a porta da url deve ser dinamica pois sera definida pelo Heroku
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet umfgcloud.loja.dll