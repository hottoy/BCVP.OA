using Autofac;
using BCVP.Common;
using BCVP.Common.LogHelper;
using BCVP.Extensions;
using BCVP.Hubs;
using BCVP.IServices;
using BCVP.Middlewares;
using BCVP.Model.Seed;
using BCVP.OA.Filter;
using BCVP.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace BCVP.OA
{
    public class Startup
    {

        private IServiceCollection _services;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 2021-11-14自己扩展的
            services.AddMvc();
            //把控制器作为服务注册，然后使用它内置的ioc来替换原来的控制器的创建器，这样就可以使用IOC来依赖注入和控制反转创建对应的控制器 
            services.AddControllersWithViews().AddControllersAsServices();
            //Controller 默认是不会通过自带容器来 Resolve&Activate 的，是通过MVC自身管理的。可以通过调用 AddControllersAsServices()方法来让 Controller 使用自带容器。
            services.AddControllers().AddControllersAsServices();
            //services.AddTransient<ITestA,TestA>();
            //services.AddTransient<ITestB,TestB>();

            services.AddRazorPages();
            /*
             AddTransient瞬时模式：每次请求，都获取一个新的实例。即使同一个请求获取多次也会是不同的实例
             AddScoped：每次请求，都获取一个新的实例。同一个请求获取多次会得到相同的实例
             AddSingleton单例模式：每次都获取同一个实例 
             */
            #region 添加cookie认证

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;//使用默认的Scheme认证授权Scheme

            }).AddCookie(o =>
            {
                //关于Cookie的配置信息请参考：https://www.cnblogs.com/sheldon-lou/p/9545726.html
                //o.Cookie.Domain = ".contoso.com";//设置Cookie的作用域：他的作用域就包括contoso.com,www.contoso.com
                o.LoginPath = "/Login/Index"; //在身份验证的时候判断为“未登录”则跳转到这个页面
                o.LogoutPath = "/ErrorHandler/Index";//如果要退出登录则跳转到这个页面
                //o.AccessDeniedPath = "/Account/AccessDenied"; //如果已经通过身份验证，但是没有权限访问则跳转到这个页面
                o.Cookie.HttpOnly = true;//设置 cookie 是否是只能被服务器访问，默认 true,为true时通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，窃取cookie内容，这样就增加了cookie的安全性
                o.SlidingExpiration = true;//设置Cookie过期时间为相对时间；也就是说在Cookie设定过期的这个时间内用户没有访问服务器，那么cookie就会过期，若有访问服务器，那么cookie期限将从新设为这个时间
                o.ExpireTimeSpan = TimeSpan.FromDays(1); //设置Cookie过期时间为1天
                o.ClaimsIssuer = "Cookie";//获取或设置应用于创建的任何声明的颁发者
                //o.Cookie.Path = "/app1"; //用来隔离同一个服务器下面的不同站点。比如站点是运行在/app1下面，设置这个属性为/app1，那么这个 cookie 就只在 app1下有效。
            });

            #endregion

            //基于内存的Session【分布式内存缓存】
            services.AddDistributedMemoryCache();

            #region 注册Session【Session的性质.配置session超时时间60分钟。也可以不指定时间】
            services.AddSession(options =>
            {
                options.Cookie.Name = ".Market.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(60);
                //options.Cookie.HttpOnly = true; //设置在浏览器不能通过js获得该cookie的值防止xss攻击
                //options.Cookie.IsEssential = true;//Cookie是必须的(默认是false),可以覆盖上面的cookie策略
            });

            #endregion


            //利用ASP.NET Core提供的IHttpContextAccessor来获取HttpContext
            //services.AddHttpContextAccessor();
            //services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            #endregion


            // 以下code可能与文章中不一样,对代码做了封装,具体查看右侧 Extensions 文件夹.
            services.AddSingleton(new Appsettings(Configuration));
            services.AddSingleton(new LogLock(Env.ContentRootPath));



            //Permissions.IsUseIds4 = Appsettings.app(new string[] { "Startup", "IdentityServer4", "Enabled" }).ObjToBool();

            // 确保从认证中心返回的ClaimType不被更改，不使用Map映射
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddMemoryCacheSetup();
            services.AddRedisCacheSetup();

            services.AddSqlsugarSetup();
            services.AddDbSetup();
            services.AddAutoMapperSetup();
            services.AddCorsSetup();
            services.AddMiniProfilerSetup();
            services.AddSwaggerSetup();
            services.AddJobSetup();
            services.AddHttpContextSetup();
            services.AddAppConfigSetup(Env);
            services.AddHttpApi();
            services.AddRedisInitMqSetup();

            services.AddRabbitMQSetup();
            services.AddKafkaSetup(Configuration);
            services.AddEventBusSetup();

            services.AddNacosSetup(Configuration);

            // 授权+认证 (jwt or ids4)
            services.AddAuthorizationSetup();
            //if (Permissions.IsUseIds4)
            //{
            //    services.AddAuthentication_Ids4Setup();
            //}
            //else
            //{
            //    services.AddAuthentication_JWTSetup();
            //}

            services.AddIpPolicyRateLimitSetup(Configuration);

            services.AddSignalR().AddNewtonsoftJsonProtocol();

            services.AddScoped<UseServiceDIAttribute>();

            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                    .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            #region 防止页面切换后Session ID改变，Seesion失效
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;//在使用Session和Cookie时true必须改为false
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            #endregion

            services.AddControllers(o =>
            {
                // 全局异常过滤
                o.Filters.Add(typeof(GlobalExceptionsFilter));
                // 全局路由权限公约
                //o.Conventions.Insert(0, new GlobalRouteAuthorizeConvention());
                // 全局路由前缀，统一修改路由
                //o.Conventions.Insert(0, new GlobalRoutePrefixFilter(new RouteAttribute(RoutePrefix.Name)));
            })
            // 这种写法也可以
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //})
            //全局配置Json序列化处理
            .AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //设置时间格式
                //options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                //忽略Model中为null的属性
                //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                //设置本地时间而非UTC时间
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            });

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            _services = services;
            //支持编码大全 例如:支持 System.Text.Encoding.GetEncoding("GB2312")  System.Text.Encoding.GetEncoding("GB18030") 
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        // 注意在Program.CreateHostBuilder，添加Autofac服务工厂
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(HttpContextAccessor)).As(typeof(IHttpContextAccessor)).SingleInstance();
            builder.RegisterModule(new AutofacModuleRegister());
            builder.RegisterModule<AutofacPropertityModuleReg>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MyContext myContext, ITasksQzServices tasksQzServices, ISchedulerCenter schedulerCenter, IHostApplicationLifetime lifetime)
        {
            // Ip限流,尽量放管道外层
            //app.UseIpLimitMildd();
            //// 记录请求与返回数据 
            //app.UseReuestResponseLog();
            // 用户访问记录(必须放到外层，不然如果遇到异常，会报错，因为不能返回流)
            //app.UseRecordAccessLogsMildd();
            //// signalr 
            //app.UseSignalRSendMildd();
            // 记录ip请求
            //app.UseIPLogMildd();
            // 查看注入的所有服务
            app.UseAllServicesMildd(_services);

            if (env.IsDevelopment())
            {
                // 在开发环境中，使用异常页面，这样可以暴露错误堆栈信息，所以不要放在生产环境。
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // 在非开发环境中，使用HTTP严格安全传输(or HSTS) 对于保护web安全是非常重要的。
                // 强制实施 HTTPS 在 ASP.NET Core，配合 app.UseHttpsRedirection
                //app.UseHsts();
            }

            // 封装Swagger展示
            //app.UseSwaggerMildd(() => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("BCVP.Api.index.html"));

            // ↓↓↓↓↓↓ 注意下边这些中间件的顺序，很重要 ↓↓↓↓↓↓

            // CORS跨域
            //app.UseCors(Appsettings.app(new string[] { "Startup", "Cors", "PolicyName" }));
            // 跳转https
            //app.UseHttpsRedirection();
            // 使用静态文件
            app.UseStaticFiles();
            // 使用cookie
            app.UseCookiePolicy();
            //进行注入session中间件
            app.UseSession();
            // 返回错误码
            app.UseStatusCodePages();
            // Routing
            app.UseRouting();
            // 这种自定义授权中间件，可以尝试，但不推荐
            // app.UseJwtTokenAuth();

            // 测试用户，用来通过鉴权
            if (Configuration.GetValue<bool>("AppSettings:UseLoadTest"))
            {
                app.UseMiddleware<ByPassAuthMidd>();
            }
            // 先开启认证
            app.UseAuthentication();
            // 然后是授权中间件
            app.UseAuthorization();
            //开启性能分析
            app.UseMiniProfilerMildd();
            // 开启异常中间件，要放到最后
            app.UseExceptionHandlerMidd();


            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapHub<ChatHub>("/api2/chatHub");
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });

            // 生成种子数据
            //app.UseSeedDataMildd(myContext, Env.WebRootPath);
            // 开启QuartzNetJob调度服务
            //app.UseQuartzJobMildd(tasksQzServices, schedulerCenter);
            // 服务注册
            app.UseConsulMildd(Configuration, lifetime);
            // 事件总线，订阅服务
            app.ConfigureEventBus();

        }

    }
}
