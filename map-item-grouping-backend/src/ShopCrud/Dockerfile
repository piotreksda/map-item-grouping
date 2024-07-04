FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ShopCrud/", "ShopCrud/"]
RUN dotnet restore "ShopCrud/ShopCrud.csproj"
WORKDIR "/src/ShopCrud"
RUN dotnet build "ShopCrud.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ENV applicationUrl http://0.0.0.0:8080
RUN dotnet publish "ShopCrud.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopCrud.dll"]