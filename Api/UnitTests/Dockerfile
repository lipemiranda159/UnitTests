FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /src
COPY ["UnitTests.Facades/UnitTests.Facades.csproj", "UnitTests.Facades/"]
COPY ["UnitTests.Models/UnitTests.Models.csproj", "UnitTests.Models/"]
COPY ["UnitTests.Services/UnitTests.Services.csproj", "UnitTests.Services/"]
COPY ["UnitTests/UnitTests.csproj", "UnitTests/"]

RUN dotnet restore "UnitTests/UnitTests.csproj"
COPY . .
WORKDIR "/src/UnitTests"
RUN dotnet build "UnitTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UnitTests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UnitTests.dll"]