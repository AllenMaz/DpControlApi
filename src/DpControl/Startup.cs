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
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Cors;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;

namespace DpControl
{
    public class Startup
    {
        private string _pathToDoc;
        private string _apiPath;
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
            _pathToDoc = "../wwwroot/DpControl.xml";
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

            //Corss Origin Resource Sharing
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin",
                    builder => builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                );
                options.AddPolicy("RefuseAllOrigin",
                    builder => builder.WithOrigins()
                              .DisallowCredentials()
                );
            });
            
            services.AddMvc()
            .AddJsonOptions(options =>
            {

                var settings = options.SerializerSettings;
                settings.Formatting = Formatting.Indented; //pretty-print
                //settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                //settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                //settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //settings.NullValueHandling = NullValueHandling.Ignore;
            });

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

            _apiPath = Configuration["APISettings:Path"];
            services.Configure<IdentityOptions>(options =>
            {
                options.Cookies.ApplicationCookie.LoginPath = new PathString("/Account/Login");
                options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = ctx =>
                    {
                        int httpStatusCode = ctx.Response.StatusCode;
                        if (ctx.Request.Path.StartsWithSegments(_apiPath) &&
                        (httpStatusCode == (int)HttpStatusCode.OK //使用Identity Authoriz授权失败时httpStatusCode == (int)HttpStatusCode.OK
                        || httpStatusCode == (int)HttpStatusCode.Unauthorized
                        || httpStatusCode == (int)HttpStatusCode.MethodNotAllowed))
                        {
                            ctx.Response.StatusCode = 401;
                            ctx.Response.WriteAsync("UnAuthorized");
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
                
                options.OperationFilter(new Swashbuckle.SwaggerGen.XmlComments.ApplyXmlActionComments(_pathToDoc));
                //options.GroupActionsBy(apiDesc => apiDesc.HttpMethod.ToString());


            });
            
            services.ConfigureSwaggerSchema(options =>
            {
                options.DescribeAllEnumsAsStrings = true;
                options.ModelFilter(new Swashbuckle.SwaggerGen.XmlComments.ApplyXmlTypeComments(_pathToDoc));
                
               
            });
            

            #endregion
            #region Register Dependency Injection
            services.AddTransient<ShadingContext, ShadingContext>();
            services.AddScoped<AbstractAuthentication, BasicAuthentication>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ILoginUserRepository, UserInfoManager>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<ISceneRepository, SceneRepository>();
            services.AddScoped<ISceneSegmentRepository, SceneSegmentRepository>();
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
            app.SeedData();

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

            //before UseMvc
            app.UseCors("AllowAllOrigin");


            #region OAuth2.0 Token授权
            #region IdentityServer3
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); 
            //app.UseJwtBearerAuthentication(options =>
            //{
            //    options.Authority = "http://localhost:30466";
            //    options.Audience = "http://localhost:30466/resources";
            //    options.RequireHttpsMetadata = false;
            //    options.AutomaticAuthenticate = true;
            //});
            //app.UseMiddleware<RequiredScopesMiddleware>(new List<string> { "dpcontrolapiscope" });
            #endregion
            #region IdentityServer4
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseIdentityServerAuthentication(options =>
            {
                options.Authority = Configuration["IdentityServer:AuthorizationServerBaseAddress"];
                options.ScopeName = Configuration["IdentityServer:APIScopeName"];
                options.ScopeSecret = Configuration["IdentityServer:APIScopeSecret"];
                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = true;
            });
            #endregion
            #endregion

            //Response Compression:ZGip, before any other middlewares,
            app.UseMiddleware<CompressionMiddleware>(new MiddlewareOptions() {
                Path = _apiPath //for api
            });
            //捕获全局异常消息
            app.UseExceptionHandler(errorApp => new GlobalExceptionBuilder(loggerFactory).ExceptionBuilder(errorApp));
            //X-HTTP-Method-Override
            app.UseMiddleware<XHttpHeaderOverrideMiddleware>(new MiddlewareOptions()
            {
                Path = _apiPath //for api
            });

            //Identity
            app.UseIdentity();
            
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
