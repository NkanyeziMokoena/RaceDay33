FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["raceday_mvc/raceday_mvc.csproj", "raceday_mvc/"]
RUN dotnet restore "raceday_mvc/raceday_mvc.csproj"
COPY . .
WORKDIR "/src/raceday_mvc"
RUN dotnet publish "raceday_mvc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "raceday_mvc.dll"]
