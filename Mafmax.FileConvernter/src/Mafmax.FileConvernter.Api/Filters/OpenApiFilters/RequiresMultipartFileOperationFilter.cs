using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mafmax.FileConvernter.Api.Filters.OpenApiFilters;

/// <summary>
/// Filter, which add multipart file requirement to OpenAPI documentation.
/// </summary>
public class RequiresMultipartFileOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var isRequiresMultipartFile = context.MethodInfo.GetCustomAttribute<RequiresMultipartFileAttribute>() != null;

        if (!isRequiresMultipartFile) return;

        var properties = InitializeProperties(operation);

        properties.Add("file", new OpenApiSchema
        {
            Type = "string",
            Format = "binary"
        });
    }

    private static IDictionary<string, OpenApiSchema> InitializeProperties(OpenApiOperation operation)
    {
        var requestBody = operation.RequestBody ??= new OpenApiRequestBody();

        var content = requestBody.Content ??= new Dictionary<string, OpenApiMediaType>();

        const string multipartDataType = "multipart/form-data";
        if (!content.ContainsKey(multipartDataType))
        {
            content[multipartDataType] = new OpenApiMediaType();
        }

        var schema = content[multipartDataType].Schema ??= new OpenApiSchema()
        {
            Type = "object",
        };

        var properties = schema.Properties ??= new Dictionary<string, OpenApiSchema>();

        return properties;
    }

}