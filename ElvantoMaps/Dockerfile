FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ElvantoMaps/ElvantoMaps.csproj", "ElvantoMaps/"]
RUN dotnet restore "ElvantoMaps/ElvantoMaps.csproj"
COPY . .
WORKDIR "/src/ElvantoMaps"
RUN dotnet build "ElvantoMaps.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ElvantoMaps.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ElvantoMaps.dll"]