using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pokemons.API.Handlers;
using Pokemons.API.Jwt;
using Pokemons.API.Middlewares;
using Pokemons.Core.ApiHandlers;
using Pokemons.Core.BackgroundServices.BotRequestsListener;
using Pokemons.Core.BackgroundServices.CacheCollector;
using Pokemons.Core.BackgroundServices.LeagueUpdater;
using Pokemons.Core.BackgroundServices.NotificationCreator;
using Pokemons.Core.BackgroundServices.RabbitMqListener;
using Pokemons.Core.BackgroundServices.RabbitMqNotificationSender;
using Pokemons.Core.Helpers;
using Pokemons.Core.MapperProfiles;
using Pokemons.Core.Providers.TimeProvider;
using Pokemons.Core.Services.BattleService;
using Pokemons.Core.Services.GuildService;
using Pokemons.Core.Services.MarketService;
using Pokemons.Core.Services.MissionService;
using Pokemons.Core.Services.NotificationService;
using Pokemons.Core.Services.PlayerService;
using Pokemons.Core.Services.RatingService;
using Pokemons.Core.Services.ReferralService;
using Pokemons.DataLayer.Cache.Repository;
using Pokemons.DataLayer.Database;
using Pokemons.DataLayer.Database.Repositories.BattleRepos;
using Pokemons.DataLayer.Database.Repositories.GuildRepos;
using Pokemons.DataLayer.Database.Repositories.MarketRepos;
using Pokemons.DataLayer.Database.Repositories.MissionRepos;
using Pokemons.DataLayer.Database.Repositories.PlayerRepos;
using Pokemons.DataLayer.Database.Repositories.RatingRepos;
using Pokemons.DataLayer.Database.Repositories.ReferralRepos;
using Pokemons.DataLayer.Database.Repositories.UnitOfWork;
using Pokemons.DataLayer.MasterRepositories.BattleRepository;
using Pokemons.DataLayer.MasterRepositories.CommonRepository;
using Pokemons.DataLayer.MasterRepositories.GuildRepository;
using Pokemons.DataLayer.MasterRepositories.MarketRepository;
using Pokemons.DataLayer.MasterRepositories.MissionRepository;
using Pokemons.DataLayer.MasterRepositories.NotificationRepository;
using Pokemons.DataLayer.MasterRepositories.PlayerRepository;
using Pokemons.DataLayer.MasterRepositories.RatingRepository;
using Pokemons.DataLayer.MasterRepositories.ReferralNodeRepository;
using RabbitMQ.Client;
using StackExchange.Redis;
using TimeProvider = Pokemons.Core.Providers.TimeProvider.TimeProvider;

namespace Pokemons.Core.Extensions;

public static class WebApplicationBuilderExtenstion
{
    public static void Configure(this WebApplicationBuilder builder)
    {
        ConfigureMapper(builder);
        ConfigureServices(builder);
        ConfigurePipeline(builder);
        ConfigureDbContext(builder);
        ConfigureApiHandlers(builder);
        ConfigureRepositories(builder);
        ConfigureTimeProvider(builder);
        ConfigureCacheRepository(builder);
        ConfigureRabbitMqConnection(builder);
        ConfigureBackgroundServices(builder);
        ConfigureDatabaseRepositories(builder);
        ConfigureAuthorization(builder);
    }

    private static void ConfigureMapper(IHostApplicationBuilder builder)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new BattleProfile());
            mc.AddProfile(new MarketProfile());
            mc.AddProfile(new PlayerProfile());
        });
        
        var mapper = mapperConfig.CreateMapper();
        builder.Services.AddSingleton(mapper);
    }

    private static void ConfigureApiHandlers(IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthHandler, AuthHandler>();
        builder.Services.AddScoped<IGuildHandler, GuildHandler>();
        builder.Services.AddScoped<IBattleHandler, BattleHandler>();
        builder.Services.AddScoped<IRatingHandler, RatingHandler>();
        builder.Services.AddScoped<ICryptoHandler, CryptoHandler>();
        builder.Services.AddScoped<IMarketHandler, MarketHandler>();
        builder.Services.AddScoped<IMissionHandler, MissionHandler>();
        builder.Services.AddScoped<IReferralHandler, ReferralHandler>();
        builder.Services.AddScoped<INotificationHandler, NotificationHandler>();
    }

    private static void ConfigureDatabaseRepositories(IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        builder.Services.AddScoped<IGuildDatabaseRepository, GuildDatabaseRepository>();
        builder.Services.AddScoped<IPlayerDatabaseRepository, PlayerDatabaseRepository>();
        builder.Services.AddScoped<IBattleDatabaseRepository, BattleDatabaseRepository>();
        builder.Services.AddScoped<IMarketDatabaseRepository, MarketDatabaseRepository>();
        builder.Services.AddScoped<IRatingDatabaseRepository, RatingDatabaseRepository>();
        builder.Services.AddScoped<IMissionDatabaseRepository, MissionDatabaseRepository>();
        builder.Services.AddScoped<IReferralNodeDatabaseRepository, ReferralNodeDatabaseRepository>();
    }

    private static void ConfigureRepositories(IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IGuildRepository, GuildRepository>();
        builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
        builder.Services.AddScoped<IBattleRepository, BattleRepository>();
        builder.Services.AddScoped<IMarketRepository, MarketRepository>();
        builder.Services.AddScoped<IRatingRepository, RatingRepository>();
        builder.Services.AddScoped<ICommonRepository, CommonRepository>();
        builder.Services.AddScoped<IMissionRepository, MissionRepository>();
        builder.Services.AddScoped<IReferralNodeRepository, ReferralNodeRepository>();
        builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
    }

    private static void ConfigureServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IGuildService, GuildService>();
        builder.Services.AddScoped<IPlayerService, PlayerService>();
        builder.Services.AddScoped<IBattleService, BattleService>();
        builder.Services.AddScoped<IMarketService, MarketService>();
        builder.Services.AddScoped<IRatingService, RatingService>();
        builder.Services.AddScoped<IMissionService, MissionService>();
        builder.Services.AddScoped<IReferralService, ReferralService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
    }
    
    private static void ConfigureTimeProvider(IHostApplicationBuilder builder)
    {
        if (builder.Environment.EnvironmentName == Environments.Production)
            builder.Services.AddSingleton<ITimeProvider, TimeProvider>();
        else
            builder.Services.AddSingleton<ITimeProvider, DevelopmentTimeProvider>();
    }

    private static void ConfigureCacheRepository(IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis") ?? 
                                          throw new ArgumentNullException()));

        builder.Services.AddSingleton<ICacheRepository, CacheRepository>();
    }

    private static void ConfigurePipeline(IHostApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            c.AddSecurityDefinition("UserId", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "UserId",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "CustomHeaderScheme"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "UserId"
                        }
                    },
                    new string[] {}
                }
            });
        });

        if (builder.Environment.EnvironmentName == Environments.Development)
            builder.Configuration.AddUserSecrets<Program>();

        builder.Services.AddSingleton<JwtHandler>();
        builder.Services.AddScoped<AuthMiddleware>();
        builder.Services.Configure<ConnectionFactory>(builder.Configuration.GetSection("RabbitMqConnectionFactory") 
                                                      ?? throw new NullReferenceException(
                                                          "Rabbit connection not found"));
    }

    private static void ConfigureAuthorization(IHostApplicationBuilder builder)
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] 
                                          ?? throw new ArgumentException("jwt key not found"));
        
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
    }

    private static void ConfigureRabbitMqConnection(IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IConnection>(provider =>
        {
            var logger = provider.GetService<ILogger<RabbitMqListener>>()!;
            var factory = new ConnectionFactory();
            builder.Configuration.GetSection("RabbitMqConnectionFactory").Bind(factory);
            
            var isConnected = false;
            while (!isConnected)
            {
                try
                {
                    var connection = factory.CreateConnectionAsync().Result;
                    isConnected = true;
                    return connection;
                }
                catch (Exception e)
                {
                    logger.LogError(e?.Message);
                    Task.Delay(1000).Wait();
                }
            }

            throw new ArgumentException("Message broker is invalid");
        });
    }
    
    private static void ConfigureDbContext(IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContextFactory<AppDbContext>(optionsAction =>
        {
            optionsAction.UseNpgsql(builder.Configuration.GetConnectionString("Database") 
                                    ?? throw new ArgumentNullException());
            optionsAction.EnableSensitiveDataLogging();
        });
    }

    private static void ConfigureBackgroundServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddHostedService<LeagueUpdaterService>();
        builder.Services.AddHostedService<CacheCollectorService>();
        builder.Services.AddHostedService<RabbitMqListener>();
        builder.Services.AddHostedService<RabbitMqNotificationSender>();
        builder.Services.AddHostedService<NotificationCreator>();
        builder.Services.AddHostedService<BotRequestsListener>();
    }
}