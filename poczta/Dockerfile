FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["poczta/poczta.csproj", "poczta/"]
RUN dotnet restore "poczta/poczta.csproj"
COPY . .
WORKDIR "/src/poczta"
RUN dotnet build "poczta.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "poczta.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "poczta.dll"]
