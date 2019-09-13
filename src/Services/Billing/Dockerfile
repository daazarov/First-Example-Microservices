FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Services/Billing/Billing.csproj src/Services/Billing/
COPY src/Messages/Messages.csproj src/Messages/
RUN dotnet restore src/Services/Billing/Billing.csproj
COPY . .
WORKDIR /src/src/Services/Billing
RUN dotnet build Billing.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Billing.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Billing.dll"]
