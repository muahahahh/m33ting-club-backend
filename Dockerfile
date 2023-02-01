FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/API/M33tingClub.Web", "M33tingClub.Web"]
COPY ["src/API/M33tingClub.Infrastructure", "M33tingClub.Infrastructure"]
COPY ["src/API/M33tingClub.Application", "M33tingClub.Application"]
COPY ["src/API/M33tingClub.Domain", "M33tingClub.Domain"]
RUN dotnet restore "M33tingClub.Web/M33tingClub.Web.csproj"
COPY . .
WORKDIR "/src/M33tingClub.Web"
RUN dotnet build "M33tingClub.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "M33tingClub.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY appspec.yml .
COPY codedeploy .
ENTRYPOINT ["dotnet", "M33tingClub.Web.dll"]