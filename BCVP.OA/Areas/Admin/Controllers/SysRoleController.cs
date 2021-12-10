using BCVP.IServices.IOAServices;
using BCVP.Model;
using BCVP.Model.Models.OAModel;
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
    public class SysRoleController : Controller
    {
        private readonly ISysRoleServices _sysRoleServices;

        public SysRoleController(ISysRoleServices sysRoleServices)
        {
            _sysRoleServices = sysRoleServices;
        }

        /// <summary>
        /// 角色管理主页
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            await Task.Run(()=>new { });
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
                List<SysRole> infoData = await _sysRoleServices.SqlQueryAsync($@"SELECT R.ROLE_ID,
                                                                                R.PARENT_ID,
                                                                                R.ROLE_CODE,
                                                                                R.ROLE_NAME,
                                                                                R.PYCODE,
                                                                                R.ROLE_ATTRIBUTE,
                                                                                R.ISVERSIONS,
                                                                                R.ORDERSORT,
                                                                                R.ISENABLED,
                                                                                R.ISDELETED,
                                                                                R.CREATETIME,
                                                                                R.MODIFYTIME,
                                                                                R.REMARK,
                                                                                R.SYSTEMVERSIONID,
                                                                                NVL(V.SYSTEMVERSIONNAME,'未设置') SYSTEMVERSIONNAME,
                                                                                DECODE(F.ROLE_ID,NULL,'顶级',F.ROLE_NAME)  PARENTNAME,
                                                                                DECODE(F.ROLE_ID,NULL,'0000',F.ROLE_CODE)  PARENTCODE
                                                                            FROM TB_SYS_ROLE R
                                                                            LEFT JOIN TB_SYS_ROLE F ON F.ROLE_ID=R.PARENT_ID
                                                                            LEFT JOIN TB_SYSTEM_VERSION V ON V.SYSTEMVERSIONID=R.SYSTEMVERSIONID
                                                                            WHERE R.ISDELETED = 0
                                                                            AND (R.ROLE_CODE LIKE '%{KeyWords}%' OR R.ROLE_NAME LIKE '%{KeyWords}%' OR
                                                                                R.PYCODE LIKE '%{KeyWords}%')
                                                                            ORDER BY R.ROLE_CODE ASC,R.ORDERSORT DESC");
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
        /// 修改
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int?Id)
        {
            SysRole m = new SysRole();
            if (Id != null&& Id > 0)
            {
                m = await _sysRoleServices.QueryById(Id);
            }
            return View(m);
        }

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="moduleId">主键ID</param>
        /// <param name="info">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> InsertOrEdit(int Id, SysRole info, int typeId)
        {
            JsonResponse result = new JsonResponse();
            try
            {
                if (await _sysRoleServices.InsertOrUpdate(Id, info, typeId))
                {
                    result.msg = "保存成功！";
                    result.count = 1;
                }
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.msg = "保存失败！";
                result.count = 0;
                throw new Exception(ex.Message);
            }
            return Json(result);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="Code">编码</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> DeleteModuleByCode(string Code)
        {
            JsonResponse result = new JsonResponse();
            try
            {
                if (Code!=null&&Code.Length>0)
                {
                    if (await _sysRoleServices.ExecuteCommandAsync($@"DELETE FROM TB_SYS_ROLE R WHERE R.ROLE_CODE LIKE'{Code}%'") < 1)
                    {
                        result.code = ResponseCode.Fail;
                        result.msg = "删除失败！";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Json(result);
        }
    }
}
