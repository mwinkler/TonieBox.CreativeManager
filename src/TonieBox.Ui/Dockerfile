#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
ARG HTTPS_PROXY

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/TonieBox.Ui/TonieBox.Ui.csproj", "src/TonieBox.Ui/"]
RUN dotnet restore "src/TonieBox.Ui/TonieBox.Ui.csproj"
COPY . .
WORKDIR "/src/src/TonieBox.Ui"
RUN dotnet build "TonieBox.Ui.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TonieBox.Ui.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TonieBox.Ui.dll"]