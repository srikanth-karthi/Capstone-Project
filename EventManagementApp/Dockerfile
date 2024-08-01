FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5500

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EventManagementApp.csproj", "."]
RUN dotnet restore "./EventManagementApp.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "./EventManagementApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./EventManagementApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY wwwroot ./wwwroot
ENTRYPOINT ["dotnet", "EventManagementApp.dll"]

