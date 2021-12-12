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


        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            await Task.Run(() => { });
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> Test()
        {
            await Task.Run(() => { });
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetDataTreeList(int selectOrgID)
        {
            DTreeResponse result = new DTreeResponse();
            List<TreeEntity> tree = new List<TreeEntity>();
            var info = await _sysOrgServices.SqlQueryAsync($@"SELECT O.ORGID,
                                                                           O.PARENTID,
                                                                           O.CODE,
                                                                           O.NAMES,
                                                                           O.FULLNAME,
                                                                           O.PYCODE,
                                                                           O.ATTRIBUT,
                                                                           O.SCHEMAID,
                                                                           O.ISENABLED,
                                                                           O.ISDELETE,
                                                                           O.ORDERSORT,
                                                                           O.REMARK,
                                                                           O.CREATEID,
                                                                           O.CREATEBY,
                                                                           O.CREATETIME,
                                                                           O.MODIFYID,
                                                                           O.MODIFYBY,
                                                                           O.MODIFYTIME
                                                                      FROM TB_SYS_ORG O
                                                                     WHERE O.ISENABLED = 1
                                                                       AND O.ISDELETE = 0
                                                                     ORDER BY O.CODE ASC, O.ORDERSORT ASC");
            info.ForEach(s =>
            {
                tree.Add(new TreeEntity { id = s.OrgID, title = s.Names, last = 0, parentId = s.ParentID, spread = (s.Code.Length == 4 ? true : false), OrgCode = s.Code, checkArr = "0",@checked=(s.OrgID==selectOrgID?true:false) });
            });
            result.status = new { code = "200", message = "操作成功" };
            result.data = tree;
            return Json(result);
        }
    }
}
