﻿using api.Contracts.BL;
using api.Contracts.BL.CISSA;
using api.Contracts.BL.UBK;
using api.Contracts.BL.Verifiers;
using api.Contracts.Helpers;
using api.Infrastructure.HttpClients;
using api.Infrastructure.Swagger;
using api.Services.BL;
using api.Services.BL.CISSA;
using api.Services.BL.UBK;
using api.Services.BL.Verifiers;
using api.Services.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Polly;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAndConfigLocalization(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        var supportedCultures = new List<CultureInfo> { new("ru"), new("en"), new("fa") };
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("ru");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        return services;
    }

    public static IServiceCollection AddAndConfigApiVersioning(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

        services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                })
            .AddApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds
                    // IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                })
            // this enables binding ApiVersion as a endpoint callback parameter. if you don't use
            // it, then you should remove this configuration.
            .EnableApiVersionBinding();

        return services;
    }

    public static IServiceCollection AddAndConfigSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options =>
        {
            // add a custom operation filter which sets default values
            options.OperationFilter<SwaggerDefaultValues>();
            options.OperationFilter<SwaggerLanguageHeader>();

            // JWT Bearer Authorization
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    public static IServiceCollection AddAndConfigWeatherHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var weatherSettings = new WeatherSettings();
        configuration.GetSection("WeatherSettings").Bind(weatherSettings);
        services.AddSingleton(weatherSettings);

        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10);

        services.AddHttpClient<IWeatherHttpClient, WeatherHttpClient>()
            .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)))
            .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(6, TimeSpan.FromSeconds(5)))
            .AddPolicyHandler(request =>
            {
                if (request.Method == HttpMethod.Get)
                    return timeoutPolicy;

                return Policy.NoOpAsync<HttpResponseMessage>();
            });

        return services;
    }

    public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<ICissaRefService, CissaRefServiceImpl>();
        services.AddScoped<IDataService, DataServiceImpl>();
        services.AddScoped<ISocialAppsService, SocialAppsServiceImpl>();
        services.AddScoped<ILogicVerifier, LogicVerifierImpl>();
        services.AddScoped<IInputJsonParser, InputJsonParserImpl>();
        services.AddScoped<IPersonalIdentityVerifier, PersonalIdentityVerifierImpl>();
        services.AddScoped<IPropertyVerifier, PropertyVerifierImpl>();
        services.AddScoped<IPassportDataVerifier, PassportDataVerifierImpl>();
        services.AddScoped<IPersonDataVerifier, PersonDataVerifierImpl>();
        services.AddScoped<IPinVerifier, PinVerifierImpl>();
        services.AddScoped<IDataHelper, DataHelperImpl>();
        services.AddScoped<ICissaDataProvider, CissaDataProviderImpl>();
        services.AddScoped<IAddressApiHelper, AddressApiHelperImpl>();
        services.AddScoped<IHttpService, HttpServiceImpl>();

    }
}