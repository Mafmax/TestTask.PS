namespace Mafmax.FileConvernter.Api.Filters.OpenApiFilters;

/// <summary>
/// Marks a method as required to contain multipart file. Used for OpenAPI filters.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RequiresMultipartFileAttribute : Attribute
{

}