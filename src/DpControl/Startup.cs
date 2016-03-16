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
using DpControl.Domain.Entities;
using DpControl.Utility.Authentication;
using Serilog.Core;
using Serilog;
using Serilog.Events;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Http;
using System.Threading.Tasks;
using System.Net;
using DpControl.Utility.Authorization;
using DpControl.Utility;

namespace DpControl
{
    public class Startup
    {
        private string pathToDoc;
        public static IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                //when publish ,filename mu be certain name
                //.AddJsonFile($"appsettings.Development.json", optional: true);
            
            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            
            //使用MapPath或者Combine，Migration数据库的时候会报错？
            //pathToDoc = env.MapPath("DpControl.xml");
            pathToDoc = "../wwwroot/DpControl.xml";
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        
        
        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddEntityFramework()
                .AddSqlServer();

            services.AddMvc();

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
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonLetterOrDigit = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ShadingContext>()
            .AddDefaultTokenProviders();
            
            services.Configure<IdentityOptions>(options =>
            {
                options.Cookies.ApplicationCookie.LoginPath = new PathString("/Account/Login");
                options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = ctx =>
                    {
                        int httpStatusCode = ctx.Response.StatusCode;
                        if (ctx.Request.Path.StartsWithSegments("/v1") &&
                        (httpStatusCode == (int)HttpStatusCode.OK //使用Identity Authoriz授权失败时httpStatusCode == (int)HttpStatusCode.OK
                        || httpStatusCode == (int)HttpStatusCode.Unauthorized
                        || httpStatusCode == (int)HttpStatusCode.MethodNotAllowed))
                        {

                            return Task.FromResult<object>(null);
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                            return Task.FromResult<object>(null);
                        }
                    }
                };
            });
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
            services.AddTransient<ShadingContext, ShadingContext>();
            services.AddScoped<AbstractAuthentication, BasicAuthentication>();
            services.AddScoped<IUserInfoRepository, UserInfoManager>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<ISceneRepository, SceneRepository>();
            services.AddScoped<ISceneSegmentRepository, SceneSegmentRepository>();
            services.AddScoped<IGroupLocationRepository, GroupLocationRepository>();
            services.AddScoped<IUserGroupRepository, UserGroupRepository>();
            services.AddScoped<IUserLocationRepository, UserLocationRepository>();
            services.AddScoped<IAlarmRepository, AlarmRepository>();
            services.AddScoped<IAlarmMessageRepository, AlarmMessageRepository>();
            services.AddScoped<IHolidayRepository, HolidayRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<ILogDescriptionRepository, LogDescriptionRepository>();
            #endregion
        }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
            
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            #region Serilog Logging
            var logWarning = new Serilog.LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.RollingFile(
                pathFormat: env.MapPath("Error/Exception.log"),
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}{NewLine}{NewLine}"
                ).CreateLogger();
            
            loggerFactory.AddSerilog(logWarning);
            #endregion

            app.UseIISPlatformHandler();

            app.UseApplicationInsightsRequestTelemetry();

            // Add Application Insights exceptions handling to the request pipeline.
            app.UseApplicationInsightsExceptionTelemetry();

            //捕获全局异常消息
            app.UseExceptionHandler(errorApp => GlobalExceptionBuilder.ExceptionBuilder(errorApp));

            //Identity
            app.UseIdentity();
            

            //X-HTTP-Method-Override
            app.UseMiddleware<XHttpHeaderOverrideMiddleware>();

            app.UseStaticFiles();

            //app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Default}/{id?}");
            });
            
            app.UseSwaggerGen();

            app.UseSwaggerUi();

            

            DbInitialization.Initialize(app.ApplicationServices);
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }

}
