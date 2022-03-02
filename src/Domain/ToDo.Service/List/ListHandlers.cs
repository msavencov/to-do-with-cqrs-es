using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Queries;
using MediatR;
using ToDo.Api.Contract.List.Models;
using ToDo.Api.Contract.List.Operations;
using ToDo.Api.Contract.Tasks.Models;
using ToDo.Core.List;
using ToDo.Core.List.Commands;
using ToDo.ReadStore.ToDo;
using ToDo.ReadStore.ToDo.Queries;
using ToDoList = ToDo.Api.Contract.List.Models.ToDoList;

namespace ToDo.Service.List
{
    public class ListHandlers : 
        IRequestHandler<CreateList, ToDoList>,
        IRequestHandler<FindLists, ToDoListCollection>,
        IRequestHandler<GetList, ToDoList>,
        IRequestHandler<GetTasks, ToDoItemCollection>,
        IRequestHandler<RenameList, Unit>
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;

        public ListHandlers(ICommandBus commandBus, IQueryProcessor queryProcessor)
        {
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
        }

        public async Task<ToDoList> Handle(CreateList request, CancellationToken cancellationToken)
        {
            var todo = new CreateToDoList(request.Name);
            var list = await _commandBus.PublishAsync(todo, default);
            var created = await _queryProcessor.ProcessAsync(new ReadModelByIdQuery<ToDoListReadModel>(todo.AggregateId), default);

            return new ToDoList
            {
                Id = created.Id,
                Name = created.Name,
                CreatedAt = created.CreatedAt, 
                CreatedBy = created.CreatedBy,
                TaskCount =  created.TaskCount,
                ActiveTaskCount = created.TaskCount - created.CompletedTaskCount,
            };
        }

        public async Task<ToDoListCollection> Handle(FindLists request, CancellationToken cancellationToken)
        {
            var query = await _queryProcessor.ProcessAsync(new GetAllListsQuery(), default);
            var items = query.Select(t => new ToDoList
            {
                Id = t.Id,
                Name = t.Name,
                CreatedAt = t.CreatedAt,
                CreatedBy = t.CreatedBy, 
                TaskCount = t.TaskCount,
                ActiveTaskCount = t.TaskCount - t.CompletedTaskCount,
            });

            return new ToDoListCollection(items);
        }

        public async Task<ToDoList> Handle(GetList request, CancellationToken cancellationToken)
        {
            var query = await _queryProcessor.ProcessAsync(new GetAllListsQuery(), default);

            return query.Where(t => t.Id == request.ListId)
                        .Select(t => new ToDoList
                        {
                            Id = t.Id,
                            Name = t.Name,
                            CreatedAt = t.CreatedAt,
                            CreatedBy = t.CreatedBy,
                            TaskCount = t.TaskCount,
                            ActiveTaskCount = t.TaskCount - t.CompletedTaskCount,
                        })
                        .FirstOrDefault();
        }

        public async Task<ToDoItemCollection> Handle(GetTasks request, CancellationToken cancellationToken)
        {
            var query = new GetTasksQuery(ToDoListId.With(request.ListId));
            var result = await _queryProcessor.ProcessAsync(query, CancellationToken.None);
            var tasks = result.Select(t => new ToDoItem
            {
                Id = t.Id, 
                Task = t.Description, 
                CreatedAt = t.CreatedAt, 
                CreatedBy = t.CreatedBy,
                ListId = t.ListId, 
                IsCompleted = t.IsCompleted
            });
            return new ToDoItemCollection(tasks);
        }

        public async Task<Unit> Handle(RenameList request, CancellationToken cancellationToken)
        {
            var id = ToDoListId.With(request.Id);
            var cmd = new ChangeListName(id, request.Name);
            var result = await _commandBus.PublishAsync(cmd, default);

            return Unit.Value;
        }
    }
}