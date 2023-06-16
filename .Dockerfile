#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Rideshare.WebApi/Rideshare.WebApi.csproj", "Rideshare.WebApi/"]
COPY ["Rideshare.Application/Rideshare.Application.csproj", "Rideshare.Application/"]
COPY ["Rideshare.Domain/Rideshare.Domain.csproj", "Rideshare.Domain/"]
COPY ["Rideshare.Persistence/Rideshare.Persistence.csproj", "Rideshare.Persistence/"]
RUN dotnet restore "Rideshare.WebApi/Rideshare.WebApi.csproj"
COPY . .
WORKDIR "/src/Rideshare.WebApi"
RUN dotnet build "Rideshare.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rideshare.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rideshare.WebApi.dll"]