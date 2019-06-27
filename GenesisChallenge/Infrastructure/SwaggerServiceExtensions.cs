using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace GenesisChallenge.Infrastructure
{
    /// <summary>
    /// Encapsulates the configuration necessary to use Swagger
    /// </summary>
    public static class SwaggerServiceExtensions
    {
        /// <summary>
        /// Add the Swagger configuration
        /// </summary>
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "GenesisChallenge", Description = "User Management API" });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "Enter into field the word 'Bearer' followed by space and JWT",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

                var path = System.AppDomain.CurrentDomain.BaseDirectory + @"GenesisChallenge.xml";
                c.IncludeXmlComments(path);
            });

            return services;
        }

        /// <summary>
        /// Applies the Swagger configuration
        /// </summary>
        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", ""); });

            return app;
        }

    }
}
