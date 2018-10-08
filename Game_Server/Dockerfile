FROM microsoft/dotnet:2.0-runtime-nanoserver-1709 AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY Game_Server/Game_Server.csproj Game_Server/
RUN dotnet restore Game_Server/Game_Server.csproj
COPY . .
WORKDIR /src/Game_Server
RUN dotnet build Game_Server.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Game_Server.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Game_Server.dll"]
