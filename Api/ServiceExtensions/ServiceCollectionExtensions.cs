using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; 
using System.Text; 
using Infrastructure;
using Services.UnitOfWork.IUnitOfWork; 
using Services.Logger;
using Services.Logger.ILogger;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Entities;
using System.Reflection;
using Services.Service.IService;
using Services.Service;
using Services.Repository.IRepository;
using Shared.Common;
using Serilog;

namespace Api.ServiceExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
    {
    //Get App settings
        services.Configure<Jwt>(config.GetSection("Jwt"));
        var jwtSettings = new Jwt();
        config.GetSection("Jwt").Bind(jwtSettings);
        services.AddSingleton(jwtSettings);
    //Conection String
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
    //Identity Setup
        services.RegisterRepositories(Assembly.GetAssembly(typeof(IRoleRepository))!);
        services.AddIdentity<IdentityUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
    //Loging 
        Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(config).MinimumLevel.Error()
        .WriteTo.File(
            "logs/app-log-.txt",rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30, // 👈 keep last 30 days of logs, older files auto-deleted
            outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
        .Enrich.FromLogContext().CreateLogger();
        // JWT Auth
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });
        //Add Swagger with JWT support
        services.AddSwaggerDocumentation(); 
        return services;
    }
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MyProject API",
                Version = "v1"
            });

            // JWT Authentication support in Swagger
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            c.AddSecurityDefinition("Bearer", securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { securityScheme, new[] { "Bearer" } }
        });
        });

        return services;
    }
    public static void RegisterRepositories(this IServiceCollection services, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();

            var implementations = types.Where(t =>
                t.IsClass && !t.IsAbstract &&
                (t.Name.EndsWith("Repository") || t.Name.EndsWith("Service")))
                .ToList();

            foreach (var implementation in implementations)
            {
                var interfaces = implementation.GetInterfaces()
                    .Where(i =>
                        i.Name.EndsWith("Repository") || i.Name.EndsWith("Service"))
                    .ToList();

                foreach (var iface in interfaces)
                {
                    services.AddScoped(iface, implementation);
                }
            }
        } 
        services.RegisterCustomRepositories();
    }

    public static void RegisterCustomRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IActivityLogger, ActivityLogger>();
        //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }
}