using DockerApps.Configurations;
using DockerApps.Domain;
using DockerApps.Integrations;
using DockerApps.Interfaces;
using DockerApps.Middleware;
using DockerApps.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DockerApps
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DB");
            var simpleProperty = Configuration.GetValue<string>("SimpleProperty");
            var nestedProp = Configuration.GetValue<string>("Inventory:NestedProperty");

            Log.ForContext("ConnectionString", connectionString)
                .ForContext("SimpleProperty", simpleProperty)
                .ForContext("Inventory:NestedProperty", nestedProp)
                .Information("Configuração carregada", connectionString);

            var dbView = (Configuration as IConfigurationRoot).GetDebugView();
            Log.ForContext("ConfigurationDebuView", dbView).Information("Configurations dumb.");


            services.AddScoped<IProductLogic, ProductLogic>();
            services.AddScoped<IQuickOrderLogic, QuickOrderLogic>();
            services.AddSingleton<IOrderProcessingNotification, OrderProcessingNotification>();


            services.AddScoped<IDbConnection>(d => new SqlConnection(connectionString));
            services.AddScoped<ICarvedRockRepository, CarvedRockRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DockerApps", Version = "v1" });
            });
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DockerApps v1"));
            }

            app.UseCustomRequestLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<CustomExceptionHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
