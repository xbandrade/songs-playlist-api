FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
EXPOSE 80
COPY . ./webapp/
WORKDIR /source/webapp
RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
EXPOSE 80
EXPOSE 5000
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "GlobalPlaylistApi.dll"]
