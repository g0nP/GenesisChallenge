using GenesisChallenge.DataAccess;
using GenesisChallenge.Domain;
using GenesisChallenge.Domain.Repositories;
using GenesisChallenge.Domain.Services;
using GenesisChallenge.Infrastructure;
using GenesisChallenge.Persistence;
using GenesisChallenge.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GenesisChallenge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new CustomAuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //TODO: move to appsettings
            var connection = @"Server=(localdb)\mssqllocaldb;Database=GenesisChallenge.Db;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<RepositoryContext> (options => options.UseSqlServer(connection));

            services.AddSwaggerConfiguration();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                };
            });

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerConfiguration();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        } 
    }
}
