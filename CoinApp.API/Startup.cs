using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using AutoMapper;
using CoinApp.API.Extensions;
using CoinApp.API.Modules;
using CoinApp.Infrastructure;
using CoinApp.Infrastructure.JwtAuthentication.Extensions;
using CoinApp.Infrastructure.SeedWork;
using CommonServiceLocator;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CoinApp.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private const string CoinAppConnectionString = "CoinAppConnectionString";
        public Startup(IConfiguration configuration)
        {
            this._configuration = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .AddUserSecrets<Startup>()
                 .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            this.AddSwagger(services);

            var validationParams = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,

                ValidateAudience = true,
                ValidAudience = _configuration["Token:Audience"],

                ValidateIssuer = true,
                ValidIssuer = _configuration["Token:Issuer"],

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Token:SigningKey"])),
                ValidateIssuerSigningKey = true,

                RequireExpirationTime = true,
                ValidateLifetime = true
            };
            services.AddJwtAuthenticationForAPI(validationParams);

            services.Configure<Token>(_configuration.GetSection("Token"));

            services
                //.AddEntityFrameworkSqlServer()
                .AddDbContext<CoinAppContext>(options =>
                {
                    options
                        .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
                        .UseSqlServer(this._configuration[CoinAppConnectionString]);
                });
            
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(typeof(Startup));
            return CreateAutofacServiceProvider(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
            //Automatic database generation/update
            //using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    var context = serviceScope.ServiceProvider.GetRequiredService<CoinAppContext>();
            //    context.Database.Migrate();
            //}
            ConfigureSwagger(app);
        }
        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "CoinApp API",
                    Version = "v1",
                    Description = ".NET Core REST API with SQL",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }
        private IServiceProvider CreateAutofacServiceProvider(IServiceCollection services)
        {
            var container = new ContainerBuilder();

            container.Populate(services);

            container.RegisterModule(new InfrastructureModule(this._configuration[CoinAppConnectionString]));

            var children = this._configuration.GetSection("Caching").GetChildren();
            Dictionary<string, TimeSpan> configuration = children.ToDictionary(child => child.Key, child => TimeSpan.Parse(child.Value));
            container.RegisterModule(new CachingModule(configuration));

            var buildContainer = container.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(buildContainer));

            return new AutofacServiceProvider(buildContainer);
        }
        private static void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flutra API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
