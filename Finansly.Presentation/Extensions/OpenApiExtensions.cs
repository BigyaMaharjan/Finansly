using Microsoft.OpenApi;

namespace Finansly.Presentation.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApiWithJwtAuth(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

                document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Enter your JWT token below. Example: eyJhbGci..."
                };

                return Task.CompletedTask;
            });
        });

        return services;
    }
}
