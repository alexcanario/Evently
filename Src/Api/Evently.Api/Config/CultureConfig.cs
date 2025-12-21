using System.Globalization;

namespace Evently.Api.Config;

internal static class CultureConfig
{
    public static IServiceCollection AddLocalization(this IServiceCollection services)
    {
        CultureInfo[] supportedCultures =
        {
            new("pt-PT"),
            new("en-US"),
            new("es-ES")
        };

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.ApplyCurrentCultureToResponseHeaders = true;
        });

        return services;
    }
}
