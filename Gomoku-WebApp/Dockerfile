FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Gomoku-WebApp/Gomoku-WebApp.csproj", "Gomoku-WebApp/"]
RUN dotnet restore "Gomoku-WebApp/Gomoku-WebApp.csproj"
COPY . .
WORKDIR "/src/Gomoku-WebApp"
RUN dotnet build "Gomoku-WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gomoku-WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gomoku-WebApp.dll"]
