#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Manage.Config.Web/Manage.Config.Web.csproj", "Manage.Config.Web/"]
COPY ["Manage.Config.Service/Manage.Config.Service.csproj", "Manage.Config.Service/"]
RUN dotnet restore "Manage.Config.Web/Manage.Config.Web.csproj"
COPY . .
WORKDIR "/src/Manage.Config.Web"
RUN dotnet build "Manage.Config.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Manage.Config.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Manage.Config.Web.dll"]