FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia apenas o csproj e restaura para aproveitar cache
COPY Services/apiProdutos/apiProdutos.csproj Services/apiProdutos/
RUN dotnet restore Services/apiProdutos/apiProdutos.csproj

# Copia todo o código e publica
COPY . .
RUN dotnet publish Services/apiProdutos/apiProdutos.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5004
EXPOSE 5004
ENTRYPOINT ["dotnet", "apiProdutos.dll"]
