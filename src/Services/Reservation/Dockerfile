FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Services/Reservation/Reservation.csproj src/Services/Reservation/
COPY src/Messages/Messages.csproj src/Messages/
RUN dotnet restore src/Services/Reservation/Reservation.csproj
COPY . .
WORKDIR /src/src/Services/Reservation
RUN dotnet build Reservation.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Reservation.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Reservation.dll"]
