FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Presentation/ToDo.App/ToDo.App.csproj", "ToDo.App/"]
RUN dotnet restore "Presentation/ToDo.App/ToDo.App.csproj"
COPY . .
WORKDIR "/src/ToDo.App"
RUN dotnet build "ToDo.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToDo.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDo.App.dll"]
