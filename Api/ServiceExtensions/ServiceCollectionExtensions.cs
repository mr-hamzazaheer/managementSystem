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

namespace Api.ServiceExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
    {

        services.Configure<Jwt>(config.GetSection("JwtSettings"));
        var jwtSettings = new Jwt();
        config.GetSection("JwtSettings").Bind(jwtSettings);
        services.AddSingleton(jwtSettings);
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        services.RegisterRepositories(Assembly.GetAssembly(typeof(IRoleRepository))!);
        services.AddIdentity<IdentityUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


        //// JWT Auth
        //var jwtSettings = config.GetSection("Jwt");
        //var secret = jwtSettings["Key"];

        //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddJwtBearer(opts =>
        //    {
        //        opts.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidIssuer = jwtSettings["Issuer"],
        //            ValidateAudience = true,
        //            ValidAudience = jwtSettings["Audience"],
        //            ValidateLifetime = true,
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        //        };
        //    });

        // Add Swagger with JWT support
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