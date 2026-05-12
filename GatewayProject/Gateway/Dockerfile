FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia apenas o csproj e restaura para aproveitar cache
COPY Gateway/Gateway.csproj Gateway/
RUN dotnet restore Gateway/Gateway.csproj

# Copia todo o código e publica
COPY . .
RUN dotnet publish Gateway/Gateway.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5170
EXPOSE 5170
ENTRYPOINT ["dotnet", "Gateway.dll"]
