using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Http;
using ToDo.Api.Contract.ToDo;
using ToDo.Api.Contract.ToDo.Models;
using ToDo.Core.Item;
using ToDo.Core.Item.Commands;
using ToDo.Core.List;
using ToDo.Core.List.Commands;
using ToDo.ReadStore.ToDo;
using ToDo.ReadStore.ToDo.Queries;
using ToDoItem = ToDo.Api.Contract.ToDo.Models.ToDoItem;
using ToDoList = ToDo.Api.Contract.ToDo.Models.ToDoList;

namespace ToDo.Api.Host.Services
{
    public class ToDo : IToDo
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IHttpContextAccessor _contextAccessor;

        public ToDo(ICommandBus commandBus, IQueryProcessor queryProcessor, IHttpContextAccessor contextAccessor)
        {
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
            _contextAccessor = contextAccessor;
        }
        
        public async Task<ToDoListCollection> GetAllLists()
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

        public async Task<ToDoList> GetList(string id)
        {
            var query = await _queryProcessor.ProcessAsync(new GetAllListsQuery(), default);

            return query.Where(t => t.Id == id)
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

        public async Task<ToDoList> NewList(string name)
        {
            var todo = new CreateToDoList(name);
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

        public async Task<ToDoItem> NewTask(string listId, string task)
        {
            var item = new CreateToDoItem(new ToDoListId(listId), task);
            var result = await _commandBus.PublishAsync(item, default);

            return new ToDoItem
            {
                Id = item.AggregateId.Value,
                ListId = listId,
                Task = task,
            };
        }

        public async Task<ToDoItemCollection> GetTasks(string listId)
        {
            var query = new GetTasksQuery(ToDoListId.With(listId));
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

        public async Task UpdateListName(string id, string name)
        {
            var cmd = new ChangeListName(id, name);
            var result = await _commandBus.PublishAsync(cmd, default);
        }

        public async Task TaskDone(string id)
        {
            var cmd = new CompleteToDoItem(ToDoItemId.With(id));
            var result = await _commandBus.PublishAsync(cmd, default);
        }

        public async Task DeleteTask(string id)
        {
            var cmd = new DeleteToDoItem(new ToDoItemId(id));
            var result = await _commandBus.PublishAsync(cmd, default);
        }
    }
}