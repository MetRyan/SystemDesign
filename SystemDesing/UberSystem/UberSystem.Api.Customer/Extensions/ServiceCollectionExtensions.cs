﻿using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using UberSystem.Infrastructure;

namespace UberSystem.Api.Customer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Register(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "UberSystem.Api.Customer",
                    Description = "An ASP.NET Core Web API for managing customers",
                    TermsOfService = new Uri("https://lms-hcmuni.fpt.edu.vn/course/view.php?id=2110"),
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Contact",
                        Url = new Uri("https://lms-hcmuni.fpt.edu.vn/course/view.php?id=2110")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense
                    {
                        Name = "License",
                        Url = new Uri("https://lms-hcmuni.fpt.edu.vn/course/view.php?id=2110")
                    }
                });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            });
            // Register database
            var connectionString = configuration.GetConnectionString("Default") ?? string.Empty;
            services.AddDatabase(connectionString);


            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string connection)
        {
            services.AddDbContext<UberSystemDbContext>(opt =>
            {
                opt.UseSqlServer(connection, sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.CommandTimeout(120);
                });
            });
            return services;
        }
    }
}
