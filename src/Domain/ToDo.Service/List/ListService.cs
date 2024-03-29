﻿using System.Linq;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Queries;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ToDo.Api.Contract.Lists;
using ToDo.Api.Contract.Shared;
using ToDo.Core.List;
using ToDo.Core.List.Commands;
using ToDo.ReadStore.Lists;
using ToDo.ReadStore.Lists.Queries;
using ToDo.ReadStore.Shared;
using ToDo.Service.Helpers;

namespace ToDo.Api.Services.List
{
    public class ListService : Api.Contract.Lists.ListService.ListServiceBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;

        public ListService(ICommandBus commandBus, IQueryProcessor queryProcessor)
        {
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
        }

        public override async Task<AddRequest.Types.Response> Add(AddRequest request, ServerCallContext context)
        {
            var todo = new CreateToDoList(request.Name);
            var result = await _commandBus.PublishAsync(todo, default);

            return new AddRequest.Types.Response
            {
                Id = todo.AggregateId.Value,
            };
        }

        public override async Task<FindRequest.Types.Response> Find(FindRequest request, ServerCallContext context)
        {
            var query = new FilterListsQuery();

            if (request.Paging is { Page: > 0, Size: > 0 } paging)
            {
                query.Paging = new PagingSettings
                {
                    Page = (ushort)paging.Page,
                    Rows = (ushort)paging.Size
                };
            }

            if (request.Sorting is {} sorting)
            {
                if (sorting.CreatedAt is {})
                {
                    query.Sorting.AddSorting(sorting.CreatedAt, model => model.CreatedAt);
                }

                if (sorting.Name is {})
                {
                    query.Sorting.AddSorting(sorting.Name, t => t.Name);
                }
            }

            if (request.Filter is {} filter)
            {
                if (filter.CreatedAt is {})
                {
                    query.Criteria.AddCriteria(t => t.CreatedAt, filter.CreatedAt);
                }

                if (filter.Name is {})
                {
                    query.Criteria.AddCriteria(model => model.Name, filter.Name);
                }
            }

            var result = await _queryProcessor.ProcessAsync(query, default);
            var items = result.Result.Select(t => new ListItem
            {
                Id = t.Id,
                Name = t.Name,
                CreatedAt = t.CreatedAt.ToTimestamp(),
                CreatedBy = t.CreatedBy,
                CompletedCount = t.CompletedTaskCount,
                ActiveCount = t.TaskCount - t.CompletedTaskCount
            });

            return new FindRequest.Types.Response
            {
                Page = new Paged
                {
                    TotalRows = result.TotalRows
                },
                Result = { items }
            };
        }

        public override async Task<GetRequest.Types.Response> Get(GetRequest request, ServerCallContext context)
        {
            var query = await _queryProcessor.ProcessAsync(new GetAllListsQuery(), default);
            var list = query.Where(t => t.Id == request.Id)
                .Select(t => new ListItem
                {
                    Id = t.Id,
                    Name = t.Name,
                    CreatedAt = t.CreatedAt.ToTimestamp(),
                    CreatedBy = t.CreatedBy,
                    ActiveCount = t.TaskCount - t.CompletedTaskCount,
                    CompletedCount = t.CompletedTaskCount,
                })
                .FirstOrDefault();

            if (list == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"The list with id '{request.Id}' not found."));
            }

            return new GetRequest.Types.Response
            {
                List = list
            };
        }

        public override async Task<Empty> Rename(RenameRequest request, ServerCallContext context)
        {
            var id = ToDoListId.With(request.Id);
            var cmd = new ChangeListName(id, request.Name);
            var result = await _commandBus.PublishAsync(cmd, default);

            return new Empty();
        }
    }
}