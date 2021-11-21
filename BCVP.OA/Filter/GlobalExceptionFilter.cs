using BCVP.Common.Helper;
using BCVP.Common.LogHelper;
using BCVP.Hubs;
using BCVP.IServices.IOAServices;
using BCVP.Model;
using BCVP.Model.Models.OAModel;
using BCVP.Model.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using System;
using System.Linq;
using System.Text;
using Nancy.Json;
using System.IO;

namespace BCVP.OA.Filter
{
    /// <summary>
    /// 全局异常错误日志
    /// </summary>
    public class GlobalExceptionsFilter : IExceptionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<GlobalExceptionsFilter> _loggerHelper;
        private readonly ISysLogExceptionServices _sysLogExceptionServices;
        public GlobalExceptionsFilter(IWebHostEnvironment env, ILogger<GlobalExceptionsFilter> loggerHelper, IHubContext<ChatHub> hubContext, IHttpContextAccessor httpContextAccessor, ISysLogExceptionServices sysLogExceptionServices)
        {
            _env = env;
            _loggerHelper = loggerHelper;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
            _sysLogExceptionServices = sysLogExceptionServices;
        }

        public void OnException(ExceptionContext context)
        {
            var json = new MessageModel<string>();
            json.msg = context.Exception.Message;//错误信息
            json.status = 500;//500异常 
            var errorAudit = "Unable to resolve service for";
            if (!string.IsNullOrEmpty(json.msg) && json.msg.Contains(errorAudit))
            {
                json.msg = json.msg.Replace(errorAudit, $"（若新添加服务，需要重新编译项目）{errorAudit}");
            }

            if (_env.EnvironmentName.ObjToString().Equals("Development"))
            {
                json.msgDev = context.Exception.StackTrace;//堆栈信息
            }
            var res = new ContentResult();
            res.Content = JsonHelper.GetJSON<MessageModel<string>>(json);

            context.Result = res;

            MiniProfiler.Current.CustomTiming("Errors：", json.msg);

            #region 将异常日志写入数据表

            var UserSessionSessionContext = context.HttpContext.GetSession();//获取当前登录用户Session信息
            //var userInfo = new JsonSerializer().Deserialize<LoginInfoViewModels>(Encoding.UTF8.GetString(_session.Get("MyLoginInfo")));

            if (context != null && context.HttpContext != null && context.Exception != null)
            {
                var UserSession = context.HttpContext.Session.GetString("MyLoginInfo");
                if (UserSession != null && UserSession != "" && UserSession != System.String.Empty && UserSession.Length > 0)
                {
                    JavaScriptSerializer jsonReader = new JavaScriptSerializer();
                    var userInfo = (LoginInfoViewModels)jsonReader.Deserialize<LoginInfoViewModels>(context.HttpContext.Session.GetString("MyLoginInfo"));
                    var p = "";
                    foreach (var item in context.ActionDescriptor.Parameters.ToList())
                    {
                        p = p + item.Name + ",";
                    }
                    SysLogException m = new SysLogException();
                    m.ControllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName.ToString();
                    m.Oper_IP = userInfo.IP;
                    m.Oper_User_Name = userInfo.uLoginName;
                    m.Oper_Versions = DateTime.Now.ToShortDateString();
                    m.Oper_User_ID = userInfo.uLoginUserId;
                    m.Exc_Message = context.Exception.ToString();
                    m.Oper_CreateTime = DateTime.Now;
                    m.Oper_Method = context.ActionDescriptor.DisplayName.ToString();
                    m.Exc_Requ_Param = p;
                    m.Exc_Name = context.Exception.Message.ToString();
                    _sysLogExceptionServices.Add(m);
                }
                // 此处进行异常记录，可以记录到数据库或文本，也可以使用其他日志记录组件。
                // 通过filterContext.Exception来获取这个异常。
                string filePath = @"D:\OA系统异常日志\"+DateTime.Now.ToString("yyyy-MM-dd")+"\\";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string errorMsg = $"------------------------开始时间："+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "------------------------\r\n" + context.Exception.ToString() + " \r\n------------------------结束时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "------------------------\r\n";
                FileHelper.FileAdd(filePath + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", errorMsg);
            }

            #endregion

            //采用log4net 进行错误日志记录
            _loggerHelper.LogError(json.msg + WriteLog(json.msg, context.Exception));
            _hubContext.Clients.All.SendAsync("ReceiveUpdate", LogLock.GetLogData()).Wait();
            //表示异常已经处理
            context.ExceptionHandled = true;

        }

        /// <summary>
        /// 自定义返回格式
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public string WriteLog(string throwMsg, Exception ex)
        {
            return string.Format("\r\n【自定义错误】：{0} \r\n【异常类型】：{1} \r\n【异常信息】：{2} \r\n【堆栈调用】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
        }

    }
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
    //返回错误信息
    public class JsonErrorResponse
    {
        /// <summary>
        /// 生产环境的消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 开发环境的消息
        /// </summary>
        public string DevelopmentMessage { get; set; }
    }
}
