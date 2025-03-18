using Api.Rest;
using Api.Rest.Extensions;
using Application;
using Application.Models;
using Infrastructure.Postgres;
using Microsoft.Extensions.Options;
using NSwag.Generation;
using Scalar.AspNetCore;
using Startup.Documentation;
using Startup.Proxy;

namespace Startup;

public class Program
{
    
 public static async Task Main()
    {
        var builder = WebApplication.CreateBuilder();
        builder.AddSuperAwesomeLoggingConfig();
        ConfigureServices(builder.Services, builder.Configuration);
        var app = builder.Build();
        await ConfigureMiddleware(app);
        await app.RunAsync();
    }
    
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var appOptions = services.AddAppOptions(configuration);
    
        // Register services (only once)
        services.RegisterApplicationServices();
        services.AddDataSourceAndRepositories();
    
        // Add authentication with custom JWT validator
        services.AddCustomJwtAuthentication(
            services.BuildServiceProvider().GetRequiredService<IOptionsMonitor<AppOptions>>());
        
        services.AddAuthorization();
    
        services.AddControllers();
    
        // Register API services
        services.RegisterRestApiServices();
        
        //services.AddWebsocketInfrastructure();
        /*if(!string.IsNullOrEmpty(appOptions.MQTT_BROKER_HOST)) {services.RegisterMqttInfrastructure();}
        else
        {
            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
            logger.LogInformation("No MQTT_BROKER_HOST provided, skipping MQTT configuration (you're probably not doing IoT stuff)");
        }*/
        
        //services.RegisterWebsocketApiServices();
        
        // Add OpenAPI document (only once with explicit document name)
        services.AddOpenApiDocument(conf =>
        {
            // Set explicit document name
            conf.DocumentName = "v1";
            conf.Title = "Your API";
            conf.Version = "v1";
            
            // Processors
            conf.DocumentProcessors.Add(new AddAllDerivedTypesProcessor());
            conf.DocumentProcessors.Add(new AddStringConstantsProcessor());
    
            // Add JWT authentication support to Swagger
            conf.AddSecurity("JWT", new NSwag.OpenApiSecurityScheme
            {
                Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                Description = "Enter your JWT token in the format: Bearer {your_token}"
            });
    
            conf.OperationProcessors.Add(new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });
        
        services.AddSingleton<IProxyConfig, ProxyConfig>();
    }

    public static async Task ConfigureMiddleware(WebApplication app)
    {
        var appOptions = app.Services.GetRequiredService<IOptionsMonitor<AppOptions>>().CurrentValue;

       /* using (var scope = app.Services.CreateScope())
        {
            if (appOptions.Seed)
                await scope.ServiceProvider.GetRequiredService<Seeder>().Seed();
        }*/

        app.Urls.Clear();
        app.Urls.Add($"http://0.0.0.0:{appOptions.REST_PORT}");
        app.Services.GetRequiredService<IProxyConfig>()
            .StartProxyServer(appOptions.PORT, appOptions.REST_PORT, appOptions.WS_PORT, appOptions.MQTT_PORT);

        app.UseAuthentication();
        app.UseAuthorization();
        app.ConfigureRestApi();
        /* await app.ConfigureWebsocketApi(appOptions.WS_PORT);
         if(!string.IsNullOrEmpty(appOptions.MQTT_BROKER_HOST)) {await app.ConfigureMqtt(appOptions.MQTT_PORT);}
         else
         {
             app.Logger.LogInformation("No MQTT_BROKER_HOST provided, skipping MQTT configuration (you're probably not doing IoT stuff)");
         }*/

        app.MapGet("Acceptance", () => "Accepted");

        app.UseOpenApi(conf =>
        {
            conf.Path = "openapi/v1.json";
        });
        app.UseSwaggerUi(conf =>
        {
            conf.Path = "/swagger";  // Access UI at http://localhost:8080/swagger
            conf.DocumentPath = "/openapi/v1.json";
        });
        
        app.MapScalarApiReference();

        var document = await app.Services.GetRequiredService<IOpenApiDocumentGenerator>().GenerateAsync("v1");
        var json = document.ToJson();
        await File.WriteAllTextAsync("openapi.json", json);

       // app.GenerateTypeScriptClient("/../../client/src/generated-client.ts").GetAwaiter().GetResult();
    }
}