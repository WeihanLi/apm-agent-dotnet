using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CMS.API.Campaign.WebApi.Util
{
    /// <summary>
    /// https://stackoverflow.com/questions/36452468/swagger-ui-web-api-documentation-present-enums-as-strings
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                foreach (var name in Enum.GetNames(context.Type))
                {
                    schema.Enum.Add(new OpenApiString($"{name}"));
                }
            }
        }
    }
}