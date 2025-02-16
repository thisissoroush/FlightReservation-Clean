using System.Text;
using FlightReservation.Application.Features.Users;
using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Application.Interfaces.Services;
using FlightReservation.Infrastructure.Email.Services;
using FlightReservation.Infrastructure.Email.Services.Models;
using FlightReservation.Infrastructure.Persistence;
using FlightReservation.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FlightReservation.Presentation.Services;

public static class ServiceInjector
{
    public static void Inject(this WebApplicationBuilder builder)
    {
        builder.RegisterDB();
        builder.RegisterEmailProvider();
        builder.RegiterJWT();

        var services = builder.Services;

        services.RegisterMediatR();
        services.RegisterHelperServices();
        services.RegisterRepositories();
        services.RegisterSwagger();
        
        
    }

    public static void RegisterDB(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    }

    public static void RegisterEmailProvider(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
    }
    public static void RegiterJWT(this WebApplicationBuilder builder)
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        builder.Services.Configure<JwtSetting>(jwt);
        
        builder.Services
            .AddAuthentication(configureOptions: x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(jwt["Key"])
                    ),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                };
            });

        builder.Services.AddAuthorization();

        
    }
    
    public static void RegisterSwagger(this IServiceCollection services)
    {
        
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                $"v1",
                new() { Title = "Flight Reservation", Version = "1" }
            );
            //uncomment for v2
            //options.SwaggerDoc("v2", new() { Title = AppConsts.ApiTitle, Version = "v2" });

            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                }
            );

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        new string[] { }
                    },
                }
            );

           
        });
    }

    public static void RegisterMediatR(this IServiceCollection services)
     {
         services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly));
     }
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFlightRepository, FlightRepository>();
        services.AddScoped<IAirportRepository, AirportRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
    }

    public static void RegisterHelperServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IJwtService, JwtService>();
    }
    
}