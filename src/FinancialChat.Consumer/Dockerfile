#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /dist
COPY ["src/FinancialChat.Consumer/FinancialChat.Consumer.csproj", "src/FinancialChat.Consumer/"]
RUN dotnet restore "src/FinancialChat.Consumer/FinancialChat.Consumer.csproj"
COPY . .
WORKDIR "/dist/src/FinancialChat.Consumer"
RUN dotnet build "FinancialChat.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinancialChat.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinancialChat.Consumer.dll"]