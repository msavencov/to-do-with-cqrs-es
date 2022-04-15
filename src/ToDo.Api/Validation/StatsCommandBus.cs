using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlow.Core;

namespace ToDo.Api.Validation;

internal class StatsCommandBus : ICommandBus
{
    private readonly ICommandBus _inner;

    public StatsCommandBus(ICommandBus commandBus)
    {
        _inner = commandBus;
    }

    public Task<TExecutionResult> PublishAsync<TAggregate, TIdentity, TExecutionResult>(ICommand<TAggregate, TIdentity, TExecutionResult> command, CancellationToken cancellationToken) where TAggregate : IAggregateRoot<TIdentity> where TIdentity : IIdentity where TExecutionResult : IExecutionResult
    {
        return _inner.PublishAsync(command, cancellationToken);
    }
}