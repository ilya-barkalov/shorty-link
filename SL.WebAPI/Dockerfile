FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5432

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SL.WebAPI/SL.WebAPI.csproj", "SL.WebAPI/"]
COPY ["SL.Infrastructure/SL.Infrastructure.csproj", "SL.Infrastructure/"]
COPY ["SL.Domain/SL.Domain.csproj", "SL.Domain/"]
COPY ["SL.Application/SL.Application.csproj", "SL.Application/"]
RUN dotnet restore "SL.WebAPI/SL.WebAPI.csproj"
COPY . .
WORKDIR "/src/SL.WebAPI"
RUN dotnet build "SL.WebAPI.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "SL.WebAPI.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SL.WebAPI.dll"]
