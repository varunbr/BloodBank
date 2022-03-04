FROM node:14.18.1-alpine AS node
WORKDIR /src
COPY client/package.json .
RUN npm install
COPY client .
RUN npm run build -- --configuration="staging"

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY API/API.csproj .
RUN dotnet restore API.csproj
COPY API .
COPY --from=node /src/dist/client /src/wwwroot
RUN dotnet build "API.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]