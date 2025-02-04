using AutoMapper;
using CrossCutting.DependencyInjection;
using Domain.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Services.AutoMapper;
using Services.Interfaces.Access;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Text;
using Services.DTOs.Access;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Services.DTOs;
using Services.Interfaces.RDManipulacao;
using System;
using System.Runtime.InteropServices;
using Services.DTOs.Youtube;

namespace API.YoutubeService
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private IAuthorizations _authorization;
        private IRDManipulacao _rdManipulacaoService;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddEndpointsApiExplorer();

            ConfigureService.ConfigureDependenciesService(services);
            ConfigureRepository.ConfigureDependenciesRepository(services);
            ConfigureServiceAPI.ConfigureDependenciesServiceAPI(services);

            #region appsettingConfig

            var confiracoesDatabase = new DatabaseConfiguration();
            new ConfigureFromConfigurationOptions<DatabaseConfiguration>(
                _config.GetSection("Database_Configuration"))
                     .Configure(confiracoesDatabase);
            services.AddSingleton(confiracoesDatabase);

            var configuracoesYoutube = new YoutubeConfiguration();
            new ConfigureFromConfigurationOptions<YoutubeConfiguration>(
                _config.GetSection("Youtube_Config"))
            .Configure(configuracoesYoutube);
            services.AddSingleton(configuracoesYoutube);

            #endregion appsettingConfig         

            #region MapperConfig
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ModelToDTOSetup());
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            #endregion MapperConfig

            #region SwaggerConfig
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Youtube Service - Teste",
                    Version = "v1.0.0.0",
                    Description = "API responsável por consultar informações do youtube",
                    Contact = new OpenApiContact()
                    {
                        Email = "duarthe0@hotmail.com",
                        Name = "Francimário Duarte Costa"
                    }
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Auth Header(geração de token)"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[]{}
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Entre com o Token JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
                c.EnableAnnotations();
            });
            #endregion SwaggerConfig

            #region Autenticacao JWT
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = "api_youtube";
                paramsValidation.ValidIssuer = "sistema_api";
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            services.AddAuthorization(auth =>
            {

                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });
            #endregion Atenticacao JWT
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAuthorizations authorization, IRDManipulacao rdManipulacaoService
        )
        {
            this._authorization = authorization;
            this._rdManipulacaoService = rdManipulacaoService;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/Authorization", async (HttpContext context) =>
                {
                    try
                    {
                        var authHeader = context.Request.Headers["Authorization"].ToString();
                        var authHeaderRegex = new Regex(@"Basic (.*)");

                        var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authHeader, "$1")));
                        var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
                        var authUsername = authSplit[0];
                        var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

                        UsuarioDTO objUsuario = new UsuarioDTO()
                        { usuario = authUsername, senha = authPassword };

                        var retorno = _authorization.CheckAccess(objUsuario).Result;

                        if (retorno == null)
                        {
                            return Results.Unauthorized();
                        }
                        else
                        {
                            return Results.Ok(retorno);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(ex.Message);
                    }
                })
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces<object>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("Authorization")
                .WithTags("Acesso")
                .WithMetadata(new SwaggerOperationAttribute("Acesso", "Responsável por gerar o token de acesso aos demais serviços."));

                endpoints.MapPost("/UploadVideo", async (
                    [FromQuery, Required] string q,
                    [FromQuery, Required] DateTime publishedAfter,
                    [FromQuery, Required] DateTime publishedBefore,
                    [FromQuery, Required] string regionCode
                    ) =>
                {
                    try
                    {
                        var retorno = await _rdManipulacaoService.InsertYoutubeVideos(q, publishedAfter.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T"), publishedBefore.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T"), regionCode);

                        if (retorno != null)
                        {
                            return Results.Ok(retorno);
                        }
                        else
                        {
                            return Results.NotFound();
                        }

                        return Results.Ok();
                    }
                    catch (ArgumentException ex)
                    {
                        return Results.BadRequest(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(ex.Message);
                    }
                })
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces<IDictionary<string, Boolean>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("UploadVideos")
                .WithTags("Upload de videos")
                .RequireAuthorization("Bearer")
                .WithMetadata(new SwaggerOperationAttribute("Endpoint upload de dados do youtube", "Responsável por receber as requisições contendo os dados do youtube."))
                .AddEndpointFilter(async (context, next) =>
                {
                    string authHeader = (string)context.HttpContext.Request.Headers.Authorization.ToString();
                    authHeader = authHeader.Replace("Bearer ", "");

                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(authHeader);
                    var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                    var perfil = tokenS.Claims.Where(claim => claim.Type == "perfilAcesso").FirstOrDefault();
                    string perfilAcesso = perfil.Value;

                    if (!perfilAcesso.Equals("Admin"))
                    {
                        return Results.BadRequest("Perfil sem premissão");
                    }
                    return await next(context);
                });

                endpoints.MapPut("/UpdateVideo", async ([FromBody, Required] YoutubeVideosDTO objYoutubeVideos) =>
                {
                    try
                    {
                        return Results.Ok();
                    }
                    catch (ArgumentException ex)
                    {
                        return Results.BadRequest(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(ex.Message);
                    }
                })
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("UpdateVideos")
                .WithTags("Update de videos")
                .RequireAuthorization("Bearer")
                .WithMetadata(new SwaggerOperationAttribute("Endpoint update de dados do youtube", "Responsável por atualizar o contendo dos dados do youtube."))
                .AddEndpointFilter(async (context, next) =>
                {
                    string authHeader = (string)context.HttpContext.Request.Headers.Authorization.ToString();
                    authHeader = authHeader.Replace("Bearer ", "");

                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(authHeader);
                    var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                    var perfil = tokenS.Claims.Where(claim => claim.Type == "perfilAcesso").FirstOrDefault();
                    string perfilAcesso = perfil.Value;

                    if (!perfilAcesso.Equals("Admin"))
                    {
                        return Results.BadRequest("Perfil sem premissão");
                    }
                    return await next(context);
                });

                endpoints.MapDelete("/DeleteVideo", async ([FromQuery, Required] int id) =>
                {
                    try
                    {
                        if (!id.Equals(0))
                        {
                            var retorno = await _rdManipulacaoService.DeleteYoutubeVideos(id);

                            if (retorno != null)
                            {
                                return Results.Ok(retorno);
                            }
                            else
                            {
                                return Results.NotFound();
                            }
                        }
                        else
                        {
                            return Results.BadRequest("Sem dados para processar");
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        return Results.BadRequest(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(ex.Message);
                    }
                })
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces<IDictionary<string, Boolean>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("DeleteVideos")
                .WithTags("Exclusão de video")
                .RequireAuthorization("Bearer")
                .WithMetadata(new SwaggerOperationAttribute("Endpoint de exclusão", "Responsável por excluir videos."))
                .AddEndpointFilter(async (context, next) =>
                {
                    string authHeader = (string)context.HttpContext.Request.Headers.Authorization.ToString();
                    authHeader = authHeader.Replace("Bearer ", "");

                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(authHeader);
                    var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                    var perfil = tokenS.Claims.Where(claim => claim.Type == "perfilAcesso").FirstOrDefault();
                    string perfilAcesso = perfil.Value;

                    if (!perfilAcesso.Equals("Admin"))
                    {
                        return Results.BadRequest("Perfil sem premissão");
                    }
                    return await next(context);
                });


                endpoints.MapGet("/GetVideos", async ([FromQuery, Optional] string? titulo,
                    [FromQuery, Optional] string? duracao,
                    [FromQuery, Optional] string? autor,
                    [FromQuery, Optional] DateTime? dataInicio,
                    [FromQuery, Optional] DateTime? dataFim,
                    [FromQuery, Optional] string? q) =>
                {
                    try
                    {
                        var retornos = await _rdManipulacaoService.GetYoutubeVideos(titulo, duracao, autor, dataInicio, dataFim, q);

                        if (retornos.Any())
                        {
                            return Results.Ok(retornos);
                        }
                        else
                        {
                            return Results.NotFound();
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        return Results.BadRequest(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(ex.Message);
                    }
                })
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces<List<RetornoYoutubeVideosDTO>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("GetVideos")
                .WithTags("Consulta de videos")
                .RequireAuthorization("Bearer")
                .WithMetadata(new SwaggerOperationAttribute("Endpoint de consulta ", "Responsável por consultar dados do youtube."))
                .AddEndpointFilter(async (context, next) =>
                {
                    string authHeader = (string)context.HttpContext.Request.Headers.Authorization.ToString();
                    authHeader = authHeader.Replace("Bearer ", "");

                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(authHeader);
                    var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                    var perfil = tokenS.Claims.Where(claim => claim.Type == "perfilAcesso").FirstOrDefault();
                    string perfilAcesso = perfil.Value;

                    if (!perfilAcesso.Equals("Admin"))
                    {
                        return Results.BadRequest("Perfil sem premissão");
                    }
                    return await next(context);
                });

            });
        }
    }
}
