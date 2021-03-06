# TODO App

The main objective of this project is R&D in DDD of applications using CQRS+ES architecture.

## Technologies & Frameworks used
* [EventFlow](https://github.com/eventflow/EventFlow) CQRS+ES framework
* [MediatR](https://github.com/jbogard/MediatR) translate API operations into domain commands
* [RabbitMQ](https://www.rabbitmq.com/) event broker for integrations
* [Entity Framework Core](https://docs.microsoft.com/ef/core/) ORM for read models
* [Radzen Blazor](https://blazor.radzen.com/) component library for UI
* [ASP.NET Core](https://docs.microsoft.com/aspnet/core) web UI and API service
* [ASP.NET Core Blazor](https://docs.microsoft.com/aspnet/core/blazor/) UI framework

## Data stores
* [EventStoreDB](https://www.eventstore.com/) aggregates events persistence 
* [Sql Server 2019](https://www.microsoft.com/sql-server/sql-server-downloads) read model persistence

## To run the app in development env.

* clone project
* build solutions
```shell
cd ./src
dotnet build todo.sln
```
* run service on which app depends
```shell
cd ./docker
docker-compose -f docker-compose.services.yml up 
```
* configure `Authority` and `Audience` under `Auth:OIDC` for OpenID Connect authentication in `./src/Presentation/ToDo.Api.Host/appsettings.json`
* run API service
```shell
cd ./src/Presentation/ToDo.Api.Host
dotnet run ToDo.Api.Host.csproj
```
* run UI web service
```shell
cd ./src/Presentation/ToDo.App
dotnet run ToDo.App.csproj
```