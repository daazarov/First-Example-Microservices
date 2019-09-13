FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Services/Hotel/Hotel.csproj src/Services/Hotel/
COPY src/Messages/Messages.csproj src/Messages/
RUN dotnet restore src/Services/Hotel/Hotel.csproj
COPY . .
WORKDIR /src/src/Services/Hotel
RUN dotnet build Hotel.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Hotel.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Hotel.dll"]
