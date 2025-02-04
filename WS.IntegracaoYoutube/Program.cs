//using WS.IntegracaoYoutube;

//var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();

//var host = builder.Build();
//host.Run();

using AutoMapper;
using CrossCutting.DependencyInjection;
using Domain.Security;
using Microsoft.Extensions.Options;
using Services.AutoMapper;
using WS.IntegracaoYoutube;

IHost host = Host.CreateDefaultBuilder(args)
     .UseWindowsService(config =>
     {
         config.ServiceName = "Integração Youtube";
     })
    .ConfigureServices(services =>
    {
        ConfigureRepository.ConfigureDependenciesRepository(services);
        ConfigureService.ConfigureDependenciesService(services);

        #region mapper
        var config = new MapperConfiguration(
        cfg =>
        {
            cfg.AddProfile(new ModelToDTOSetup());
        });
        IMapper mapper = config.CreateMapper();
        services.AddSingleton(mapper);
        #endregion

        #region appsettingConfig
        IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .Build();

        var confiracoesDatabase = new DatabaseConfiguration();
        new ConfigureFromConfigurationOptions<DatabaseConfiguration>(
            Configuration.GetSection("Database_Configuration"))
        .Configure(confiracoesDatabase);
        services.AddSingleton(confiracoesDatabase);

        var configuracoesYoutube = new YoutubeConfiguration();
        new ConfigureFromConfigurationOptions<YoutubeConfiguration>(
            Configuration.GetSection("Youtube_Config"))
        .Configure(configuracoesYoutube);
        services.AddSingleton(configuracoesYoutube);

        #endregion appsettingConfig 

        services.AddHostedService<Worker>();
    })
    .ConfigureHostOptions(options =>
    {
        options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
    })
    .Build();

host.Run();
