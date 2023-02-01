FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY ./src/Database ./

WORKDIR /app/M33tingClub.DatabaseMigrator
RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=build-env /app/M33tingClub.DatabaseMigrator/out .
COPY src/Database/M33tingClub.Database/Migrations ./Migrations
RUN ls
ENTRYPOINT ["dotnet", "M33tingClub.DatabaseMigrator.dll"]
CMD [ "arg0", "arg1" ]