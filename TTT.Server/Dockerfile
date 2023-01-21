FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY ./TTT.Server/*.csproj ./
RUN dotnet restore

COPY ./TTT.Server ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 9050/udp
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "TTT.Server.dll" ]