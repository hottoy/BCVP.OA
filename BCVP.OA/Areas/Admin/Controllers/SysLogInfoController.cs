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
using static BCVP.Model.Enums;

namespace BCVP.OA.Areas.Admin.Controllers
{
    /// <summary>
    /// 登录管理【无权限】
    /// </summary>
    [AllowAnonymous]
    [Area("Admin")]
    public class SysLogInfoController : Controller
    {
        private readonly ISysLogUserLoginServices _sysLogUserLoginServices;

        public SysLogInfoController(ISysLogUserLoginServices sysLogUserLoginServices)
        {
            this._sysLogUserLoginServices = sysLogUserLoginServices;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            await Task.Run(() => { });
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> LoginIndex()
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
        public async Task<JsonResult> GetData(int? loginType, int? loginStatus, int? loginSrc, string keyWord, int page, int limit, string field = "LOGIN_ID", string order = "DESC")
        {
            PageModel<SysLogUserLogin> entities = await _sysLogUserLoginServices.GetInfoList(loginType, loginStatus, loginSrc, keyWord, page, limit, field, order);

            if (entities.data != null && entities.data.Count != 0)
            {
                entities.data.ForEach(s => s.Login_CreateTime = (s.Login_CreateTime == null ? "" : s.Login_CreateTime.ToString()));
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

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="orgID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetData1(int? loginType, int? loginStatus, int? loginSrc, string keyWord, int page, int limit, string field = "LOGIN_ID", string order = "DESC")
        {
            JsonResponse result = new JsonResponse();
            try
            {
                var entities = await _sysLogUserLoginServices.GetInfoList(loginType, loginStatus, loginSrc, keyWord, page, limit, field, order);
                result.count = entities.dataCount;
                result.data = entities.data;
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.msg = ex.Message.ToString();
            }
            var d = Json(result);
            return d;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteByIds(string ids)
        {
            JsonResponse result = new JsonResponse();
            try
            {
                var res = await _sysLogUserLoginServices.ExecuteCommandAsync($@"DELETE FROM TB_SYS_LOG_UserLogin O WHERE O.Login_ID IN({ids})");
                if (res > 0)
                {
                    result.count = res;
                    result.msg = "删除成功";
                }
                else
                {
                    result.code = ResponseCode.Fail;
                    result.msg = "删除失败";
                }
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.msg = ex.Message.ToString();
            }
            return Json(result);
        }
    }
}
