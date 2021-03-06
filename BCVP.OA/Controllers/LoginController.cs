using BCVP.Common.Helper;
using BCVP.IServices.IOAServices;
using BCVP.Model;
using BCVP.Model.Models.OAModel;
using BCVP.Model.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp.Serialization.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
            SysLogUserLogin loginModel = new SysLogUserLogin();
            string ipd = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            JsonResponse result = new JsonResponse();
            string strHostName = System.Net.Dns.GetHostName();
            //string clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            string IPOK = IpHelper.GetCurrentIp(strHostName);//System.Net.Dns.GetHostAddresses(strHostName)[2].ToString();
            loginModel.Login_IP = IPOK;
            loginModel.Login_Src = LoginSrc.PC;
            loginModel.Login_Status = LoginStatus.Success;
            loginModel.Login_CreateTime = DateTime.Now;
            loginModel.Login_Type = LoginType.Login;
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
                    //创建身份证这个证件的携带者：我们叫这个证件携带者为“证件当事人”
                    var principal = new ClaimsPrincipal(identity);

                    Task.Run(async () =>
                    {
                        //生成 Cookie 并保存到硬盘中，指定 Cookie 的过期时间
                        await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                            AllowRefresh = true
                        });
                    }).Wait();

                    LoginInfoViewModels loginInfo = new LoginInfoViewModels();
                    result.msg = "登录成功！";
                    loginInfo.uLoginUserId = userInfo.FirstOrDefault().UserID;
                    loginInfo.uLoginUserAccount = account;
                    loginInfo.uLoginName = userInfo.FirstOrDefault().UserName;
                    loginInfo.IP = IPOK;

                    var json = new JsonSerializer().Serialize(loginInfo);
                    HttpContext.Session.Set("MyLoginInfo", Encoding.UTF8.GetBytes(json));

                    SessionContext session = new SessionContext();
                    session.uLoginUserId = userInfo.FirstOrDefault().UserID;
                    session.uLoginUserAccount = account;
                    session.uLoginName = userInfo.FirstOrDefault().UserName;
                    session.IP = IPOK;

                    HttpContext.SetSession(session);
                    //var session2 = HttpContext.GetSession();
                    loginModel.Login_UserID = userInfo.FirstOrDefault().UserID;
                    loginModel.Login_Message = "登录成功";

                }
                else
                {
                    result.code = ResponseCode.Fail;
                    result.msg = "登录失败,账号或密码错误！";
                    loginModel.Login_Message = result.msg;
                    loginModel.Login_Status = LoginStatus.Fail;
                }
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.count = 0;
                result.msg = "登录失败！系统异常：" + ex.Message;
                loginModel.Login_Message = result.msg;
                loginModel.Login_Status = LoginStatus.Fail;
                await _sysLogUserLoginServices.Add(loginModel);
            }
            await _sysLogUserLoginServices.Add(loginModel);
            return Json(result);
        }
    }
}
