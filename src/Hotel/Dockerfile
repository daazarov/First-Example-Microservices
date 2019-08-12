FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Hotel/Hotel.csproj src/Hotel/
COPY src/Messages/Messages.csproj src/Messages/
RUN dotnet restore src/Hotel/Hotel.csproj
COPY . .
WORKDIR /src/src/Hotel
RUN dotnet build Hotel.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Hotel.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Hotel.dll"]
