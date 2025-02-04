using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces.Access;
using Services.Services.Access;

namespace CrossCutting.DependencyInjection
{
    public class ConfigureServiceAPI
    {
        public static void ConfigureDependenciesServiceAPI(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAuthorizations, AuthorizationService>();
        }
    }
}
