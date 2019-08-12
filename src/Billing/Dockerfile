FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Billing/Billing.csproj src/Billing/
COPY src/Messages/Messages.csproj src/Messages/
RUN dotnet restore src/Billing/Billing.csproj
COPY . .
WORKDIR /src/src/Billing
RUN dotnet build Billing.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Billing.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Billing.dll"]
