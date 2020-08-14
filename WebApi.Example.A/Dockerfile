#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WebApi.Example.A/WebApi.Example.A.csproj", "WebApi.Example.A/"]
RUN dotnet restore "WebApi.Example.A/WebApi.Example.A.csproj"
COPY . .
WORKDIR "/src/WebApi.Example.A"
RUN dotnet build "WebApi.Example.A.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.Example.A.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.Example.A.dll"]