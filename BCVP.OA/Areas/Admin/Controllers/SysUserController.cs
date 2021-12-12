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
    public class SysUserController : Controller
    {
        private readonly ISysUserServices _sysUserServices;
        private readonly ISysDicClassServices _sysDicClassServices;
        public SysUserController(ISysUserServices sysUserServices, ISysDicClassServices sysDicClassServices)
        {
            _sysUserServices = sysUserServices;
            _sysDicClassServices = sysDicClassServices;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            await Task.Run(() => { });
            return View();
        }

        /// <summary>
        /// 添加修改
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            await Task.Run(() => { });
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> InsertOrEdit(SysUser Info)
        {
            JsonResponse result = new JsonResponse();
            try
            {
                var list = await _sysUserServices.Query(s => s.UserAccount == Info.UserAccount & s.IsDelete == 0 & s.UserID != Info.UserID);
                if (list.Count == 0)
                {
                    if (Info.UserID == 0)
                    {
                        var res = await _sysUserServices.Add(Info);
                        result.data = res;
                        result.count = res;
                    }
                    else
                    {
                        var res = await _sysUserServices.Update(Info);
                        result.data = res;
                        result.count = 1;
                    }
                }
                else
                {
                    result.count = 0;
                    result.code = ResponseCode.Fail;
                    result.msg = "账号已经存在！";
                }
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.msg = ex.Message.ToString();
                throw;
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetModuleInfo(int id)
        {
            return Json(await _sysUserServices.QueryById(id));
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="orgID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetData(string OrgCode, string userName, string userAccount, int page, int limit, string field = "USERID", string order = "DESC")
        {
            JsonResponse result = new JsonResponse();
            try
            {
                var entities = await _sysUserServices.GetInfoList(OrgCode, userName, userAccount, page, limit, field, order);
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
        [HttpGet]
        public async Task<JsonResult> GetDicTreeList(int DataType)
        {
            DTreeResponse result = new DTreeResponse();
            List<TreeEntity> tree = new List<TreeEntity>();
            var info = await _sysDicClassServices.SqlQueryAsync($@"SELECT D.DIC_ID,
       D.DATATYPE,
       D.DICNAME,
       D.DICCODE,
       D.ISENABLED,
       D.ISDELETE,
       D.ORDERSORT,
       D.CREATETIME
  FROM TB_SYS_DICCLASS D
 WHERE D.ISDELETE = 0 AN D.ISENABLED = 1
   AND D.DATATYPE = {DataType}
 ORDER BY D.DATATYPE ASC, D.ORDERSORT ASC");
            info.ForEach(s =>
            {
                tree.Add(new TreeEntity { id = s.Dic_ID, title = s.dicName, last = 0, parentId = 0, spread = true, OrgCode = s.dicCode, checkArr = "0" });
            });
            result.status = new { code = "200", message = "操作成功" };
            result.data = tree;
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetSystemDicList(int DataType, int SelectId)
        {
            List<LayselectView> listModule = new List<LayselectView>();
            try
            {
                var list = await _sysDicClassServices.QuerySql($@"SELECT D.DIC_ID,
       D.DATATYPE,
       D.DICNAME,
       D.DICCODE,
       D.ISENABLED,
       D.ISDELETE,
       D.ORDERSORT,
       D.CREATETIME
  FROM TB_SYS_DICCLASS D
 WHERE D.ISDELETE = 0 AND D.ISENABLED = 1
   AND D.DATATYPE = {DataType}
 ORDER BY D.DATATYPE ASC, D.ORDERSORT ASC");

                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        listModule.Add(new LayselectView { code = item.Dic_ID.ToString(), codeName = item.dicName, select = (item.Dic_ID == SelectId ? "true" : "false") });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(listModule);
        }
        /// <summary>
        /// 获取多选
        /// </summary>
        /// <param name="DataType"></param>
        /// <param name="SelectId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetSystemDic(int DataType, string SelectId)
        {
            List<LayselectView> listModule = new List<LayselectView>();
            try
            {
                var list = await _sysDicClassServices.QuerySql($@"SELECT D.DIC_ID,
       D.DATATYPE,
       D.DICNAME,
       D.DICCODE,
       D.ISENABLED,
       D.ISDELETE,
       D.ORDERSORT,
       D.CREATETIME
  FROM TB_SYS_DICCLASS D
 WHERE D.ISDELETE = 0 AND D.ISENABLED = 1
   AND D.DATATYPE = {DataType}
 ORDER BY D.DATATYPE ASC, D.ORDERSORT ASC");

                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        listModule.Add(new LayselectView { code = item.Dic_ID.ToString(), codeName = item.dicName, select = (SelectId.Contains(item.Dic_ID.ToString()) ? "true" : "false") });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(listModule);
        }
    }
}
