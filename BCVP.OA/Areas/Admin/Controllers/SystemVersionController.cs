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
    public class SystemVersionController : Controller
    {
        private readonly ISystemVersionServices _systemVersionServices;
        public SystemVersionController(ISystemVersionServices systemVersionServices)
        {
            _systemVersionServices = systemVersionServices;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            await Task.Run(()=>new { });
            return View();
        }

        public async Task<ActionResult> Edit()
        {
            await Task.Run(() => new { });
            return View();
        }

       /// <summary>
       /// 获取分页程序
       /// </summary>
       /// <param name="keyWord">关键词</param>
       /// <param name="page">当前页码</param>
       /// <param name="limit">每一页条数</param>
       /// <param name="field">排序字段</param>
       /// <param name="order">升序或降序</param>
       /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetData(string keyWord, int page, int limit, string field = "SYSTEMVERSIONID", string order = "DESC")
        {
            JsonResponse result = new JsonResponse();
            try
            {
                var entities = await _systemVersionServices.GetInfoList(keyWord, page, limit, field, order);
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

        [HttpPost]
        public async Task<ActionResult> SetStatus(int Id)
        {
            JsonResponse result = new JsonResponse();
            try
            {
                if (await _systemVersionServices.ExecuteCommandAsync($@"UPDATE TB_SYSTEM_VERSION    SET ISENABLED = DECODE(ISENABLED, 1, 0, 1)  WHERE SYSTEMVERSIONID = {Id}")>0)
                {
                    result.count = 1;
                }
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.msg = "设置失败！";
                result.count = 0;
                throw new Exception(ex.Message);
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetSystemVersionList()
        {
            JsonResponse result = new JsonResponse();
            try
            {
                result.data = await _systemVersionServices.QuerySql($@"SELECT SYSTEMVERSIONID,SYSTEMVERSIONNAME,SYSTEMVERSIONCODE FROM  TB_SYSTEM_VERSION  WHERE ISDELETED=0 AND ISENABLED=1 ORDER BY SYSTEMVERSIONID ASC");
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.msg = "设置失败！";
                result.count = 0;
                throw new Exception(ex.Message);
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetSystemVersionList2(int SelectId)
        {
            List<LayselectView> listModule = new List<LayselectView>();
            try
            {
                var list = await _systemVersionServices.QuerySql($@"SELECT SYSTEMVERSIONID,SYSTEMVERSIONNAME,SYSTEMVERSIONCODE FROM  TB_SYSTEM_VERSION  WHERE ISDELETED=0 AND ISENABLED=1 ORDER BY SYSTEMVERSIONID ASC");

                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        listModule.Add(new LayselectView { code = item.SystemVersionID.ToString(), codeName = item.SystemVersionName,select=(item.SystemVersionID== SelectId ? "true":"false") });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(listModule);
        }

        [HttpGet]
        public async Task<ActionResult> GetInfo(int Id)
        {
            JsonResponse result = new JsonResponse();
            try
            {
               result.data=await _systemVersionServices.QueryById(Id);
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.msg = "系统异常！";
                result.count = 0;
                throw new Exception(ex.Message);
            }
            return Json(result);
        }
        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="moduleId">主键ID</param>
        /// <param name="info">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> InsertOrEdit(SystemVersion Info)
        {
            JsonResponse result = new JsonResponse();
            try
            {
                if (await _systemVersionServices.InsertOrEdit(Info))
                {
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

        [HttpPost]
        public async Task<ActionResult> DeleteById(int Id)
        {
            JsonResponse result = new JsonResponse();
            try
            {
                if (await _systemVersionServices.DeleteById(Id))
                {
                    result.count = 1;
                }
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.msg = "删除失败！";
                result.count = 0;
                throw new Exception(ex.Message);
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteSelectIds(int[] Ids)
        {
            {
                JsonResponse result = new JsonResponse();
                try
                {
                    if (await _systemVersionServices.DeleteById(Ids))
                    {
                        result.count = 1;
                    }
                }
                catch (Exception ex)
                {
                    result.code = ResponseCode.Fail;
                    result.msg = "删除失败！";
                    result.count = 0;
                    throw new Exception(ex.Message);
                }
                return Json(result);
            }
        }
    }
}
