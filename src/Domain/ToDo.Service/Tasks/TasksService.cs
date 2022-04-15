using System;
using System.Linq;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Queries;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ToDo.Api.Contract.Shared;
using ToDo.Api.Contract.Tasks;
using ToDo.Core.Item;
using ToDo.Core.Item.Commands;
using ToDo.Core.List;
using ToDo.ReadStore.Items.Queries;

namespace ToDo.Api.Services.Tasks
{
    public sealed class TasksService : Api.Contract.Tasks.TasksService.TasksServiceBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;
        
        public TasksService(ICommandBus commandBus, IQueryProcessor queryProcessor)
        {
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
        }
        
        public override async Task<AddRequest.Types.Response> Add(AddRequest request, ServerCallContext context)
        {
            var listId = ToDoListId.With(request.ListId);
            var item = new CreateToDoItem(listId, request.Title);
            var result = await _commandBus.PublishAsync(item, context.CancellationToken);
            
            return new AddRequest.Types.Response
            {
                TaskId = item.AggregateId.Value
            };
        }

        public override async Task<Empty> Complete(CompleteRequest request, ServerCallContext context)
        {
            var itemId = ToDoItemId.With(request.Id);
            var cmd = new CompleteToDoItem(itemId);
            var result = await _commandBus.PublishAsync(cmd, context.CancellationToken);

            return new Empty();
        }

        public override async Task<Empty> Delete(DeleteRequest request, ServerCallContext context)
        {
            var itemId = ToDoItemId.With(request.Id);
            var cmd = new DeleteToDoItem(itemId);
            var result = await _commandBus.PublishAsync(cmd, context.CancellationToken);

            return new Empty();
        }

        public override async Task<Empty> Rename(RenameRequest request, ServerCallContext context)
        {
            var itemId = ToDoItemId.With(request.Id);
            var cmd = new RenameToDoItem(itemId, request.Title);
            var result = await _commandBus.PublishAsync(cmd, context.CancellationToken);

            return new Empty();
        }
        
        public override async Task<FindRequest.Types.Response> Find(FindRequest request, ServerCallContext context)
        {
            var query = new GetTasksQuery
            {
                ListId = ToDoListId.With(request.ListId),
                IncludeDeleted = request.ShowDeleted
            };
            var result = await _queryProcessor.ProcessAsync(query, context.CancellationToken);
            var tasks = result.Select(t => new TaskItem
            {
                Id = t.Id,
                Title = t.Description, 
                CreatedAt = t.CreatedAt.ToTimestamp(), 
                CreatedBy = t.CreatedBy,
                ListId = t.ListId, 
                IsCompleted = t.IsCompleted, 
                IsDeleted = t.IsDeleted, 
            });

            return new FindRequest.Types.Response
            {
                Page = new Paged
                {
                    TotalRows = result.Count()
                },
                Items = {tasks}
            };
        }

        public override async Task<GetRequest.Types.Response> Get(GetRequest request, ServerCallContext context)
        {
            var query = new GetTasksQuery
            {
                Id = new ToDoItemId(request.Id),
            };
            var result = await _queryProcessor.ProcessAsync(query, context.CancellationToken);
            var items = result.Select(z => new TaskItem
            {
                Id = z.Id, Title = z.Description, CreatedAt = z.CreatedAt.ToTimestamp(),
                CreatedBy = z.CreatedBy,
                IsCompleted = z.IsCompleted,
                ListId = z.ListId,
            });
            
            try
            {
                return new GetRequest.Types.Response
                {
                    Task = items.Single()
                };
            }
            catch (InvalidOperationException)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"The task with id '{request.Id}' not found."));
            }
        }
    }
}