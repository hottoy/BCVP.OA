using BCVP.OA.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BCVP.OA.Controllers
{
    /// <summary>
    /// 登录管理【无权限】
    /// </summary>
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {//取用户信息
            //var userId = User.FindFirst(ClaimTypes.Sid).Value;
            //var userName = User.Identity.Name;
            await Task.Run(() => { });
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 注销方法,退出登录
        /// </summary>
        public async Task<ActionResult> Logout()
        {
            await Task.Run(() => {
                HttpContext.Session.Clear();
                HttpContext.ChallengeAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);//若要注销当前用户并将其删除 cookie
                HttpContext.Response.Cookies.Delete($".AspNetCore.{CookieAuthenticationDefaults.AuthenticationScheme}");
                //RedirectToAction("Index", "Login");
            });
            return RedirectToAction("Index", "Login"); ;
        }
    }
}
