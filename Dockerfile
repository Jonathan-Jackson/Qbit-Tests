#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["qbit-tests/qbit-tests.csproj", "qbit-tests/"]
RUN dotnet restore "qbit-tests.csproj"
COPY . .
WORKDIR "/src/qbit-tests"
RUN dotnet build "qbit-tests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "qbit-tests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "qbit-tests.dll"]