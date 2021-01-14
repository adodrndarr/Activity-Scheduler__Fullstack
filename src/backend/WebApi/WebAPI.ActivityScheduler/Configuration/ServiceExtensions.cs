﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebAPI.ActivityScheduler.DataAccess;
using WebAPI.ActivityScheduler.Entities;
using WebAPI.ActivityScheduler.EntitiesDTO;
using WebAPI.ActivityScheduler.JWTFeatures;
using WebAPI.ActivityScheduler.Services;


namespace WebAPI.ActivityScheduler.Configuration
{
    public static class ServiceExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ActivitySchedulerConnection");

            services.AddDbContext<ActivitySchedulerDbContext>(
                options => options.UseSqlServer(connectionString)
            );
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ActivitySchedulerDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            });
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<JWTAuthManager>();
            services.AddSingleton<IActivityService, ActivityService>();
            services.AddSingleton<ILoggerManager, LoggerService>();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JWTSettings");

            var secret = jwtSettings.GetSection("securityKey").Value;
            var secretBytes = Encoding.UTF8.GetBytes(secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                if (options?.Events != null)
                {
                    options.Events.OnMessageReceived = context => // --
                    {

                        if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                        {
                            context.Token = context.Request.Cookies["X-Access-Token"];
                        }

                        return Task.CompletedTask;
                    };                                            
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(secretBytes)
                };                
            });
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextExceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextExceptionFeature != null)
                    {
                        logger.LogError($"An error occured: \n{contextExceptionFeature.Error}\n\n");

                        var errorDetails = new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error"
                        };

                        await context.Response.WriteAsync(errorDetails.ToString());
                    }
                });
            });
        }
    }
}