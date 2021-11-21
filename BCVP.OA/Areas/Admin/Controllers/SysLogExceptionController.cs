using BCVP.IServices.IOAServices;
using BCVP.Model;
using BCVP.Model.Models.OAModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCVP.OA.Areas.Admin.Controllers
{
    /// <summary>
    /// 登录管理【无权限】
    /// </summary>
    [AllowAnonymous]
    [Area("Admin")]
    public class SysLogExceptionController : Controller
    {
        private readonly ISysLogExceptionServices _sysLogExceptionServices;

        public SysLogExceptionController(ISysLogExceptionServices sysLogExceptionServices)
        {
            this._sysLogExceptionServices = sysLogExceptionServices;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
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
        public async Task<JsonResult> GetData(int? loginSrc, string keyWord, int page, int limit, string field = "EXC_ID", string order = "DESC")
        {
            PageModel<SysLogException> entities = await _sysLogExceptionServices.GetInfoList(loginSrc, keyWord, page, limit, field, order);

            if (entities.data != null && entities.data.Count != 0)
            {
                entities.data.ForEach(s => s.Oper_CreateTime = (s.Oper_CreateTime == null ? "" : s.Oper_CreateTime.ToString()));
            }
            Hashtable table = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = entities.dataCount,//总条数
                ["data"] = entities.data//分页数据
            };
            //return Json(table);
            return Json(new { code = 0, count = entities.dataCount, data = entities.data, msg = "" });
        }

    }
}
