using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.SwaggerGen;
using DpControl.Domain.IRepository;
using DpControl.Domain.Repository;
using DpControl.Domain.EFContext;
using DpControl.Utility.ExceptionHandler;
using DpControl.Utility.Middlewares;
using Microsoft.AspNet.Identity.EntityFramework;
using DpControl.Domain.Models;
using DpControl.Utility.Authentication;
using Serilog.Core;
using Serilog;
using Serilog.Events;

namespace DpControl
{
    public class Startup
    {
        private string pathToDoc ;
        public static IConfigurationRoot Configuration { get; set; }

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

            //pathToDoc = env.MapPath("../../../artifacts/bin/DpControl/Debug/dnx451/DpControl.xml");
            //使用MapPath或者Combine，Migration数据库的时候会报错？
            pathToDoc = env.MapPath("../DpControl.xml");
            builder.AddEnvironmentVariables();
            Configuration = builder.Build().ReloadOnChanged("appsettings.json");
        }


        //private string pathToDoc = "../../../artifacts/bin/DpControl/Debug/dnx451/DpControl.xml";


        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddEntityFramework()
                .AddSqlServer();

            var mvcBuilder = services.AddMvc(config =>
            {
               // config.Filters.Add(new DigestAuthorizationAttribute());
            });
            #region 增加支持XML Formatter
            //mvcBuilder.AddXmlDataContractSerializerFormatters();

            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new GlobalExceptionFilter());

            //});
            #endregion
            #region Cache
            //Add MemoryCache
            services.AddCaching();
            //Add SqlServerCache
            services.AddSqlServerCache(options =>
             {
                 options.ConnectionString = Configuration["SqlsServerCache:ConnectionString"];
                 options.SchemaName = Configuration["SqlsServerCache:SchemaName"];
                 options.TableName = Configuration["SqlsServerCache:TableName"];
             }
            );
            #endregion
            #region  Add Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ShadingContext>()
            .AddDefaultTokenProviders();
            #endregion
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
            #region Register Dependency Injection
            services.AddSingleton<ShadingContext, ShadingContext>();
            services.AddScoped<AbstractAuthentication, BasicAuthentication>();
            services.AddSingleton<IProjectRepository, CustomerRepository>();
            #endregion
        }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            #region Serilog Logging
            var logWarning = new Serilog.LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.RollingFile(
                pathFormat: env.MapPath("Warning/Exception.log"),
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}{NewLine}{NewLine}"
                ).CreateLogger();
            
            loggerFactory.AddSerilog(logWarning);
            #endregion

            app.UseIISPlatformHandler();

            app.UseApplicationInsightsRequestTelemetry();

            // Add Application Insights exceptions handling to the request pipeline.
            app.UseApplicationInsightsExceptionTelemetry();

            //捕获全局异常消息
            app.UseExceptionHandler(errorApp =>GlobalExceptionBuilder.ExceptionBuilder(errorApp));

            //Add API Authentication Middleware
            app.UseMiddleware<APIAuthenticationMiddleware>(
                new AuthenticationOptions()
                {
                    Path = "/v1"  //只对API进行身份验证
                }
             );
            //X-HTTP-Method-Override
            app.UseMiddleware<XHttpHeaderOverrideMiddleware>();

            app.UseStaticFiles();

            //Identity
            app.UseIdentity();

            //app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwaggerGen();

            app.UseSwaggerUi();

            

            DbInitialization.Initialize(app.ApplicationServices);
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
