using BCVP.Common.Helper;
using BCVP.IServices.IOAServices;
using BCVP.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static BCVP.Model.Enums;

namespace BCVP.OA.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISysUserServices _sysUserServices;
        private readonly ISysLogUserLoginServices _sysLogUserLoginServices;
        private IHttpContextAccessor _accessor;
        public LoginController(ISysUserServices sysUserServices, IHttpContextAccessor accessor, ISysLogUserLoginServices sysLogUserLoginServices)
        {
            this._sysUserServices = sysUserServices;
            this._accessor = accessor;
            this._sysLogUserLoginServices = sysLogUserLoginServices;
        }

        public async Task<ActionResult> Index()
        {
            await Task.Run(() => { }).ConfigureAwait(false);
            return View();
        }

        public async Task<JsonResult> CheckLogin(string account, string password)
        {
     
            string ipd = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            JsonResponse result = new JsonResponse();
            string strHostName = System.Net.Dns.GetHostName();
            //string clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            string IPOK = IpHelper.GetCurrentIp(strHostName);//System.Net.Dns.GetHostAddresses(strHostName)[2].ToString();
            result.ip = IPOK;
            try
            {
                // 获取客户端的IP
                string ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                //string ip = _accessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                password = MD5Helper.MD5Encrypt32(password);
                var userInfo = await _sysUserServices.Query(u => u.UserAccount == account && u.UserPassword == password && u.IsDelete == 0);
                if (userInfo != null && userInfo.Count != 0)
                {
                    //一定要声明AuthenticationScheme
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.DateOfBirth, DateTime.Now.ToShortDateString()));
                    identity.AddClaim(new Claim(ClaimTypes.MobilePhone, userInfo.FirstOrDefault().Mobile));
                    identity.AddClaim(new Claim(ClaimTypes.PrimarySid, userInfo.FirstOrDefault().UserID.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Sid, $"{account}"));
                    identity.AddClaim(new Claim(ClaimTypes.Name, userInfo.FirstOrDefault().UserName));
                    identity.AddClaim(new Claim(ClaimTypes.Email, userInfo.FirstOrDefault().Email));
                    identity.AddClaim(new Claim(ClaimTypes.Role, "助理,组长,销售,办公室,经理"));
                    identity.AddClaim(new Claim("IP", IPOK));
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userInfo.FirstOrDefault().UserAccount));
                    //ClaimsPrincipal（身份人，当事人，身份所有人）
                    await HttpContext.SignInAsync(identity.AuthenticationType,
                                                  new ClaimsPrincipal(identity),
                                                  new AuthenticationProperties
                                                  {
                                                      //ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                                                      IsPersistent = false,
                                                      AllowRefresh = false,
                                                      RedirectUri = "/Home/Index",
                                                  });
                    result.LogID = userInfo.FirstOrDefault().UserID;
                    result.msg = "登录成功！";
                    HttpContext.Session.SetString("Ip", IPOK);
                    //取值
                    ViewBag.Code = HttpContext.Session.GetString("Ip");
                }
                else
                {
                    result.code = ResponseCode.Fail;
                    result.msg = "登录失败,账号或密码错误！";
                }
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.count = 0;
                result.msg = "登录失败！系统异常：" + ex.Message;
                await _sysLogUserLoginServices.WriteSystemLog(result);
            }
            await _sysLogUserLoginServices.WriteSystemLog(result);
            return Json(result);
        }
    }
}
