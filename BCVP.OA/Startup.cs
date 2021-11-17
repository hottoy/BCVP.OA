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
            #region 2021-11-14�Լ���չ��
            services.AddMvc();
            //�ѿ�������Ϊ����ע�ᣬȻ��ʹ�������õ�ioc���滻ԭ���Ŀ������Ĵ������������Ϳ���ʹ��IOC������ע��Ϳ��Ʒ�ת������Ӧ�Ŀ����� 
            services.AddControllersWithViews().AddControllersAsServices();
            //Controller Ĭ���ǲ���ͨ���Դ������� Resolve&Activate �ģ���ͨ��MVC�������ġ�����ͨ������ AddControllersAsServices()�������� Controller ʹ���Դ�������
            services.AddControllers().AddControllersAsServices();
            //services.AddTransient<ITestA,TestA>();
            //services.AddTransient<ITestB,TestB>();

            services.AddRazorPages();
            /*
             AddTransient˲ʱģʽ��ÿ�����󣬶���ȡһ���µ�ʵ������ʹͬһ�������ȡ���Ҳ���ǲ�ͬ��ʵ��
             AddScoped��ÿ�����󣬶���ȡһ���µ�ʵ����ͬһ�������ȡ��λ�õ���ͬ��ʵ��
             AddSingleton����ģʽ��ÿ�ζ���ȡͬһ��ʵ�� 
             */

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.Cookie.Name = "_AdminTicketCookie";
                o.LoginPath = new PathString("/Login/Index");//��¼ҳ���url
                o.AccessDeniedPath = new PathString("/Account/Login");//û����Ȩ��ת��ҳ��
                o.LogoutPath = new PathString("/Login/Index");
                o.AccessDeniedPath = new PathString("/ErrorHandler/Index");//û����Ȩ��ת��ҳ��,��cookie�ķ�ʽ��֤��˳���ʼ����¼��ַ
                //o.ExpireTimeSpan = TimeSpan.FromHours(0.5); // cookies�Ĺ���ʱ��
            });

            //�����ڴ��Session
            services.AddDistributedMemoryCache();

            //Session������.����session��ʱʱ��60���ӡ�Ҳ���Բ�ָ��ʱ��
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60);
                //options.Cookie.HttpOnly = true; //���������������ͨ��js��ø�cookie��ֵ
                //options.Cookie.IsEssential = true;//Cookie�Ǳ����(Ĭ����false),���Ը��������cookie����
            });
            //����ASP.NET Core�ṩ��IHttpContextAccessor����ȡHttpContext
            //services.AddHttpContextAccessor();
            //services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            #endregion


            // ����code�����������в�һ��,�Դ������˷�װ,����鿴�Ҳ� Extensions �ļ���.
            services.AddSingleton(new Appsettings(Configuration));
            services.AddSingleton(new LogLock(Env.ContentRootPath));


            //Permissions.IsUseIds4 = Appsettings.app(new string[] { "Startup", "IdentityServer4", "Enabled" }).ObjToBool();

            // ȷ������֤���ķ��ص�ClaimType�������ģ���ʹ��Mapӳ��
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

            // ��Ȩ+��֤ (jwt or ids4)
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

            services.AddControllers(o =>
            {
                // ȫ���쳣����
                o.Filters.Add(typeof(GlobalExceptionsFilter));
                // ȫ��·��Ȩ�޹�Լ
                //o.Conventions.Insert(0, new GlobalRouteAuthorizeConvention());
                // ȫ��·��ǰ׺��ͳһ�޸�·��
                //o.Conventions.Insert(0, new GlobalRoutePrefixFilter(new RouteAttribute(RoutePrefix.Name)));
            })
            // ����д��Ҳ����
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //})
            //ȫ������Json���л�����
            .AddNewtonsoftJson(options =>
            {
                //����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //��ʹ���շ���ʽ��key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //����ʱ���ʽ
                //options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                //����Model��Ϊnull������
                //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                //���ñ���ʱ�����UTCʱ��
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            });

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            _services = services;
            //֧�ֱ����ȫ ����:֧�� System.Text.Encoding.GetEncoding("GB2312")  System.Text.Encoding.GetEncoding("GB18030") 
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        // ע����Program.CreateHostBuilder�����Autofac���񹤳�
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(HttpContextAccessor)).As(typeof(IHttpContextAccessor)).SingleInstance();
            builder.RegisterModule(new AutofacModuleRegister());
            builder.RegisterModule<AutofacPropertityModuleReg>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MyContext myContext, ITasksQzServices tasksQzServices, ISchedulerCenter schedulerCenter, IHostApplicationLifetime lifetime)
        {
            // Ip����,�����Źܵ����
            //app.UseIpLimitMildd();
            //// ��¼�����뷵������ 
            //app.UseReuestResponseLog();
            // �û����ʼ�¼(����ŵ���㣬��Ȼ��������쳣���ᱨ����Ϊ���ܷ�����)
            //app.UseRecordAccessLogsMildd();
            //// signalr 
            //app.UseSignalRSendMildd();
            // ��¼ip����
            //app.UseIPLogMildd();
            // �鿴ע������з���
            app.UseAllServicesMildd(_services);

            if (env.IsDevelopment())
            {
                // �ڿ��������У�ʹ���쳣ҳ�棬�������Ա�¶�����ջ��Ϣ�����Բ�Ҫ��������������
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // �ڷǿ��������У�ʹ��HTTP�ϸ�ȫ����(or HSTS) ���ڱ���web��ȫ�Ƿǳ���Ҫ�ġ�
                // ǿ��ʵʩ HTTPS �� ASP.NET Core����� app.UseHttpsRedirection
                //app.UseHsts();
            }

            // ��װSwaggerչʾ
            //app.UseSwaggerMildd(() => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("BCVP.Api.index.html"));

            // ������������ ע���±���Щ�м����˳�򣬺���Ҫ ������������

            // CORS����
            //app.UseCors(Appsettings.app(new string[] { "Startup", "Cors", "PolicyName" }));
            // ��תhttps
            //app.UseHttpsRedirection();
            // ʹ�þ�̬�ļ�
            app.UseStaticFiles();
            // ʹ��cookie
            app.UseCookiePolicy();
            //����ע��session�м��
            app.UseSession();
            // ���ش�����
            app.UseStatusCodePages();
            // Routing
            app.UseRouting();
            // �����Զ�����Ȩ�м�������Գ��ԣ������Ƽ�
            // app.UseJwtTokenAuth();

            // �����û�������ͨ����Ȩ
            if (Configuration.GetValue<bool>("AppSettings:UseLoadTest"))
            {
                app.UseMiddleware<ByPassAuthMidd>();
            }
            // �ȿ�����֤
            app.UseAuthentication();
            // Ȼ������Ȩ�м��
            app.UseAuthorization();
            //�������ܷ���
            app.UseMiniProfilerMildd();
            // �����쳣�м����Ҫ�ŵ����
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

            // ������������
            //app.UseSeedDataMildd(myContext, Env.WebRootPath);
            // ����QuartzNetJob���ȷ���
            //app.UseQuartzJobMildd(tasksQzServices, schedulerCenter);
            // ����ע��
            app.UseConsulMildd(Configuration, lifetime);
            // �¼����ߣ����ķ���
            app.ConfigureEventBus();

        }

    }
}
