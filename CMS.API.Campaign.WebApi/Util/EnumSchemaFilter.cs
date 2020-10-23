using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace CMS.API.Campaign.WebApi.Util
{
    /// <summary>
    /// https://stackoverflow.com/questions/36452468/swagger-ui-web-api-documentation-present-enums-as-strings
    /// </summary>
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                model.Enum.Clear();
                foreach (var name in Enum.GetNames(context.Type))
                {
                    model.Enum.Add(new OpenApiString($"{name}"));
                }
            }
        }
    }
}