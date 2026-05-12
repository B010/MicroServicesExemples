FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia apenas o csproj e restaura para aproveitar cache
COPY Services/apiPedidos/apiPedidos.csproj Services/apiPedidos/
RUN dotnet restore Services/apiPedidos/apiPedidos.csproj

# Copia todo o código e publica
COPY . .
RUN dotnet publish Services/apiPedidos/apiPedidos.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5026
EXPOSE 5026
ENTRYPOINT ["dotnet", "apiPedidos.dll"]
