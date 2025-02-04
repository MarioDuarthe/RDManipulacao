using Data.DapperConfig;
using Data.Repositories;
using Data.Repositories.CreateTables;
using Domain.Interfaces.Data.DapperConfig;
using Domain.Interfaces.Data.Repositories;
using Domain.Interfaces.Data.Repositories.CreateTables;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.DependencyInjection
{
    public class ConfigureRepository
    {
        public static void ConfigureDependenciesRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IConnection, Connection>();
            serviceCollection.AddTransient<IScripts, Scripts>();
            serviceCollection.AddTransient<IYoutubeRepository, YoutubeRepository>();
            serviceCollection.AddTransient<IAccessRepository,  AccessRepository>();
        }
    }
}
