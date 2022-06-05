#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80/tcp
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /dist
COPY ["src/FinancialChat.Web/FinancialChat.Web.csproj", "src/FinancialChat.Web/"]
RUN dotnet restore "src/FinancialChat.Web/FinancialChat.Web.csproj"
COPY . .
WORKDIR "/dist/src/FinancialChat.Web"
RUN dotnet build "FinancialChat.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinancialChat.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinancialChat.Web.dll"]

