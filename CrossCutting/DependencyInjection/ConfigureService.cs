using Domain.Interfaces.Utils;
using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces.RDManipulacao;
using Services.Interfaces.Requests;
using Services.Interfaces.Youtube;
using Services.Services.RDManipulacao;
using Services.Services.Requests;
using Services.Services.Youtube;

namespace CrossCutting.DependencyInjection
{
    public class ConfigureService
    {
        public static void ConfigureDependenciesService(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRestSharp, Services.Utils.RestSharp>();
            serviceCollection.AddTransient<IRequestManager, RequestManagerService>();
            serviceCollection.AddTransient<IYoutube, YoutubeService>();
            serviceCollection.AddTransient<IRDManipulacao, RDManipulacaoService>();
        }
    }
}
