#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Core/Core.csproj", "Core/"]
COPY ["Core.DbModels/Core.DbModels.csproj", "Core.DbModels/"]
COPY ["Core.IdentityService/Core.IdentityService.csproj", "Core.IdentityService/"]
COPY ["Core.CryptService/Core.CryptService.csproj", "Core.CryptService/"]
COPY ["Core.Utils/Core.Utils.csproj", "Core.Utils/"]
COPY ["Core.Repositories/Core.Repositories.csproj", "Core.Repositories/"]
COPY ["Core.ApiResponses/Core.ApiResponses.csproj", "Core.ApiResponses/"]
COPY ["Core.ApiRequests/Core.ApiRequests.csproj", "Core.ApiRequests/"]
COPY ["Core.CacheServices/Core.CacheServices.csproj", "Core.CacheServices/"]
RUN dotnet restore "Core/Core.csproj"
COPY . .
WORKDIR "/src/Core"
RUN dotnet build "Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core.dll"]
