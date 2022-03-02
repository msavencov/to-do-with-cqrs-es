using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using MediatR;
using ToDo.Api.Contract.Tasks.Operations;
using ToDo.Core.Item;
using ToDo.Core.Item.Commands;
using ToDo.Core.List;

namespace ToDo.Service.Tasks
{
    internal class TasksHandlers :
        IRequestHandler<AddTask, ToDo.Api.Contract.Tasks.Models.ToDoItem>,
        IRequestHandler<CompleteTask, Unit>,
        IRequestHandler<DeleteTask, Unit>,
        IRequestHandler<RenameTask, Unit>
    {
        private readonly ICommandBus _commandBus;

        public TasksHandlers(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public async Task<ToDo.Api.Contract.Tasks.Models.ToDoItem> Handle(AddTask request, CancellationToken cancellationToken)
        {
            var listId = ToDoListId.With(request.ListId);
            var item = new CreateToDoItem(listId, request.Task);
            var result = await _commandBus.PublishAsync(item, default);

            return new ToDo.Api.Contract.Tasks.Models.ToDoItem
            {
                Id = item.AggregateId.Value,
                ListId = request.ListId,
                Task = request.Task,
            };
        }

        public async Task<Unit> Handle(CompleteTask request, CancellationToken cancellationToken)
        {
            var itemId = ToDoItemId.With(request.TaskId);
            var cmd = new CompleteToDoItem(itemId);
            var result = await _commandBus.PublishAsync(cmd, default);
            
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteTask request, CancellationToken cancellationToken)
        {
            var itemId = ToDoItemId.With(request.TaskId);
            var cmd = new DeleteToDoItem(itemId);
            var result = await _commandBus.PublishAsync(cmd, default);
            
            return Unit.Value;
        }
        
        public async Task<Unit> Handle(RenameTask request, CancellationToken cancellationToken)
        {
            var itemId = ToDoItemId.With(request.TaskId);
            var cmd = new Core.Item.Commands.RenameToDoItem(itemId, request.NewName);
            var result = await _commandBus.PublishAsync(cmd, default);
            
            return Unit.Value;
        }
    }
}