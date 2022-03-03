namespace Pidp.Infrastructure;

using FluentValidation;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

public class FluentValidationClientErrorOperationFilter : IOperationFilter
{
    private static readonly IEnumerable<Type> TypesWithValidators = Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => t.BaseType != null
            && t.BaseType.IsGenericType
            && t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
        .Select(t => t.BaseType!.GenericTypeArguments.Single());

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var paramType in context.MethodInfo.GetParameters().Select(p => p.ParameterType))
        {
            if (TypesWithValidators.Contains(paramType))
            {
                operation.Responses.TryAdd("400", new OpenApiResponse { Description = "Bad Request" });
                return;
            }
        }
    }
}
