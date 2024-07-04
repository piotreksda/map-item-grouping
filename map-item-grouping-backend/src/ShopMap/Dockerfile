FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ShopMap/", "ShopMap/"]
RUN dotnet restore "ShopMap/ShopMap.csproj"
WORKDIR "/src/ShopMap"
RUN dotnet build "ShopMap.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ENV applicationUrl http://0.0.0.0:8080
RUN dotnet publish "ShopMap.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopMap.dll"]


#FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
#USER $APP_UID
#WORKDIR /app
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["ShopMap/ShopMap.csproj", "ShopMap/"]
#RUN dotnet restore "ShopMap/ShopMap.csproj"
#COPY . .
#WORKDIR "/src/ShopMap"
#RUN dotnet build "ShopMap.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "ShopMap.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "ShopMap.dll"]
