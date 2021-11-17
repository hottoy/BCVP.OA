using BCVP.IServices.IOAServices;
using BCVP.Model;
using BCVP.Model.Models.OAModel;
using BCVP.Model.ViewModels;
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
    public class SysOrgController : Controller
    {
        public readonly ISysOrgServices _sysOrgServices;
        public SysOrgController(ISysOrgServices sysOrgServices)
        {
            this._sysOrgServices = sysOrgServices;
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            await Task.Run(() => { });
            return View();
        }
        /// <summary>
        /// 获取机构信息列表
        /// </summary>
        /// <param name="KeyWords"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Data(string KeyWords)
        {
            JsonResponse result = new JsonResponse();
            try
            {
                List<SysOrg> infoData = await _sysOrgServices.SqlQueryAsync($@"SELECT O.ORGID,
                                                                                   O.PARENTID,
                                                                                   O.CODE,
                                                                                   O.NAMES,
                                                                                   O.FULLNAME,
                                                                                   O.PYCODE,
                                                                                   O.ATTRIBUT,
                                                                                   O.SCHEMAID,
                                                                                   O.ISENABLED
                                                                              FROM TB_SYS_ORG O
                                                                             WHERE O.NAMES LIKE N'%{KeyWords}%'
                                                                               AND O.CODE LIKE N'%{KeyWords}%'
                                                                             ORDER BY O.CODE ASC, O.ORDERSORT ASC");
                result.data = infoData;
                result.count = infoData.Count;
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.msg = ex.ToString();
            }
            return Json(result);
        }

        /// <summary>
        /// 用户列表左侧树显示
        /// </summary>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetOrg(string keyWords)
        {
            JsonResponse result = new JsonResponse();
            try
            {
                List<VMTree> vm = new List<VMTree>();
                var info = await _sysOrgServices.GetSysOrgs(keyWords);
                info.ForEach(s =>
                {
                    vm.Add(new VMTree { id = s.OrgID, pid = s.ParentID, title = s.Names, code = s.Code });
                });
                result.data = vm;
                result.count = vm.Count;
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.msg = ex.ToString();
            }
            return Json(result.data);
        }


        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SysOrgController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SysOrgController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SysOrgController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
