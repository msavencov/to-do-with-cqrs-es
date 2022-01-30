using System;
using System.Linq;
using System.Reflection;
using EventFlow.Aggregates;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using ToDo.Core.Item.Events;
using ToDo.Core.List.Events;

namespace ToDo.Api.Host.Doc
{
    public class EventsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var query = from t in AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.DefinedTypes)
                        where t.BaseType?.IsGenericType == true
                        where t.BaseType.GetGenericTypeDefinition() == typeof(AggregateEvent<,>)
                        select t;
            
            foreach (var type in query.OrderBy(t => t.FullName))
            {
                var schemaId = type.FullName?.Replace(".", "/");
                var schema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);

                context.SchemaRepository.AddDefinition(schemaId, schema);
            }
        }
    }
}