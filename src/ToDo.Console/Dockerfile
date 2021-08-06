FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ToDo.Console/ToDo.Console.csproj", "ToDo.Console/"]
RUN dotnet restore "ToDo.Console/ToDo.Console.csproj"
COPY . .
WORKDIR "/src/ToDo.Console"
RUN dotnet build "ToDo.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToDo.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDo.Console.dll"]
