using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.SwaggerGen;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;

namespace DpControl
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build().ReloadOnChanged("appsettings.json");
        }

        public IConfigurationRoot Configuration { get; set; }

        private string pathToDoc = "../../../artifacts/bin/DpControl/Debug/dnx451/DpControl.xml";

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ShadingContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddMvc();

            #region  swagger
        services.AddSwaggerGen();
            services.ConfigureSwaggerDocument(options =>
            {

                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "DpControl WebApi v1",
                    Description = "A WebApi Test and Document For DpControl",
                    TermsOfService = "None"
                });
                options.OperationFilter(new Swashbuckle.SwaggerGen.XmlComments.ApplyXmlActionComments(pathToDoc));
            });

            services.ConfigureSwaggerSchema(options =>
            {
                options.DescribeAllEnumsAsStrings = true;
                options.ModelFilter(new Swashbuckle.SwaggerGen.XmlComments.ApplyXmlTypeComments(pathToDoc));
            });

            #endregion
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseMvc();

            app.UseSwaggerGen();

            app.UseSwaggerUi();
            DbInitialization.Initialize(app.ApplicationServices);
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
