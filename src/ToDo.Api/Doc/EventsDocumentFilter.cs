using System;
using System.Linq;
using EventFlow.Aggregates;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ToDo.Api.Doc
{
    internal class EventsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var query = from a in AppDomain.CurrentDomain.GetAssemblies()
                        where a.IsDynamic == false
                        from t in a.DefinedTypes
                        where t.BaseType?.IsGenericType == true
                        where t.BaseType?.GetGenericTypeDefinition() == typeof(AggregateEvent<,>)
                        select t;
            
            foreach (var type in query.OrderBy(t => t.FullName))
            {
                context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);
            }
        }
    }
}