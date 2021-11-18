using BCVP.Common.Helper;
using BCVP.Common.LogHelper;
using BCVP.Hubs;
using BCVP.IServices.IOAServices;
using BCVP.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        public GlobalExceptionsFilter(IWebHostEnvironment env, ILogger<GlobalExceptionsFilter> loggerHelper, IHubContext<ChatHub> hubContext, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _loggerHelper = loggerHelper;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnException(ExceptionContext context)
        {
            //string ip = _session.GetString("Ip");
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


            //采用log4net 进行错误日志记录
            _loggerHelper.LogError(json.msg + WriteLog(json.msg, context.Exception));

            _hubContext.Clients.All.SendAsync("ReceiveUpdate", LogLock.GetLogData()).Wait();

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
