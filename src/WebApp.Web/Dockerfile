FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-preview AS build
WORKDIR /src
COPY ["src/WebApp.Web/WebApp.Web.csproj", "WebApp.Web/"]
COPY ["src/WebApp.Domain/WebApp.Domain.csproj", "WebApp.Domain/"]
COPY ["src/WebApp.Data/WebApp.Data.csproj", "WebApp.Data/"]
COPY ["src/Force/Force.csproj", "Force/"]

RUN dotnet restore "WebApp.Web/WebApp.Web.csproj"

COPY ["src/WebApp.Web", "WebApp.Web/"]
COPY ["src/WebApp.Domain", "WebApp.Domain/"]
COPY ["src/WebApp.Data", "WebApp.Data/"]
COPY ["src/Force", "Force/"]

WORKDIR "/src/WebApp.Web"
RUN dotnet build "WebApp.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApp.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApp.Web.dll"]
