using BCVP.IServices.IOAServices;
using BCVP.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BCVP.Model.Enums;

namespace BCVP.OA.Areas.Admin.Controllers
{
    /// <summary>
    /// 登录管理【无权限】
    /// </summary>
    [AllowAnonymous]
    [Area("Admin")]
    public class SysUserController : Controller
    {
        private readonly ISysUserServices _sysUserServices;
        public SysUserController(ISysUserServices sysUserServices)
        {
            _sysUserServices = sysUserServices;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            await Task.Run(() => { });
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            await Task.Run(() => { });
            return View();
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="orgID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetData(int orgID, string userName, string userAccount, int page, int limit, string field = "USERID", string order = "DESC")
        {
            JsonResponse result = new JsonResponse();
            try
            {
                var entities = await _sysUserServices.GetInfoList(orgID, userName, userAccount, page, limit, field, order);
                result.count = entities.dataCount;
                result.data = entities.data;
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.msg = ex.Message.ToString();
            }
            return Json(result);
        }

        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}
