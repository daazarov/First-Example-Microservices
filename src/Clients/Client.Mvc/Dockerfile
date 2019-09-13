FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Clients/Client.Mvc/Client.Mvc.csproj", "src/Clients/Client.Mvc/"]
COPY ["src/Messages/Messages.csproj", "src/Messages/"]
RUN dotnet restore "src/Clients/Client.Mvc/Client.Mvc.csproj"
COPY . .
WORKDIR "/src/src/Clients/Client.Mvc"
RUN dotnet build "Client.Mvc.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Client.Mvc.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Client.Mvc.dll"]