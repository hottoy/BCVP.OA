using BCVP.IServices.IOAServices;
using BCVP.Model;
using BCVP.Model.Models.OAModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static BCVP.Model.Enums;

namespace BCVP.OA.Areas.Admin.Controllers
{
    /// <summary>
    /// 登录管理【无权限】
    /// </summary>
    [AllowAnonymous]
    [Area("Admin")]
    public class SysModuleController : Controller
    {
        private readonly ISysModuleServices _sysModuleServices;

        public SysModuleController(ISysModuleServices sysModuleServices)
        {
            this._sysModuleServices = sysModuleServices;
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            await Task.Run(() => { });
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> IconBrowser()
        {
            await Task.Run(() => { });
            return View();
        }

        /// <summary>
        /// 获取权限名称
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> getMenu()
        {
            JsonResponse result = new JsonResponse();
            try
            {
                MenusInfoResultDTO m = new MenusInfoResultDTO();
                m.homeInfo = new HomeInfo { title = "首页", href = "../views/home/console.html?t=1" };
                m.logoInfo = new LogoInfo { title = "LAYUI MINI", image = "../images/logo.png", href = "http://layuimini.99php.cn/docs/init/netcore.html" };
                m.menuInfo = await _sysModuleServices.GetMenuInfoAsync();
                result.data = m;
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.msg = "获取菜单失败，" + ex.ToString();
                //await _logServices.WriteExceptionLog(LoginUser.Id, "获取权限", "编辑菜单:" + ex.ToString());
            }
            return Json(result.data);
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> ModuleData()
        {
            JsonResponse result = new JsonResponse();
            result.code = 0;
            result.data = await _sysModuleServices.GetModulesSchema();
            result.msg = "操作成功";
            return Json(result);
        }

        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <param name="KeyWord">菜单名称或者编码名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Data(string keyWord = "")
        {
            JsonResponse result = new();
            try
            {
                var list = await _sysModuleServices.GetModulesList(keyWord);
                result.count = list.Count;
                result.data = list;
            }
            catch (Exception ex)
            {
                //await _logServices.WriteExceptionLog(LoginUser.Id, "保存权限", ex.ToString());
                result.code = ResponseCode.Fail;
                result.msg = "读取数据失败," + ex.ToString();
                return Json(result);
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            SysModule model = new SysModule();
            model.Icon1 = "layui-icon layui-icon-group"; //设置默认图片
            model.IsEnabled = 1;
            model.IsMenu = 1;
            model.IsTarget = "_self";
            if (id > 0 && id != null)
            {
                model = await _sysModuleServices.QueryById(id);
            }
            return View(model);
        }

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="moduleId">主键ID</param>
        /// <param name="info">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> InsertOrEdit(int moduleId, SysModule info, int typeId)
        {
            JsonResponse result = new JsonResponse();
            try
            {
                result.count = await _sysModuleServices.insertOrUpdate(moduleId, info, typeId);
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Fail;
                result.msg = "保存失败," + ex.ToString();
                return Json(result);
            }
            return Json(result);

        }

        [HttpPost]
        public IActionResult ImgUpload([FromServices] IWebHostEnvironment env)
        {
            var imgFile = Request.Form.Files[0];
            if (imgFile != null && !string.IsNullOrEmpty(imgFile.FileName))
            {
                var filename = ContentDispositionHeaderValue
                                .Parse(imgFile.ContentDisposition)
                                .FileName
                                .Trim('"');

                var extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf(".")); //扩展名，如.jpg
                                                                                                                          //取扩展名
                                                                                                                          // var ext = imgFile.FileName.Substring(imgFile.FileName.LastIndexOf("."));
                                                                                                                          //新文件以guid+扩展名
                                                                                                                          //var fileName = Guid.NewGuid().ToString() + ext;

                #region 判断后缀
                if (!extname.ToLower().Contains("jpg") && !extname.ToLower().Contains("png") && !extname.ToLower().Contains("gif"))
                {
                    return Json(new { code = 1, msg = "只允许上传jpg,png,gif格式的图片.", });
                }
                #endregion

                #region 判断大小
                long mb = imgFile.Length / 1024 / 1024; // MB
                if (mb > 5)
                {
                    return Json(new { code = 1, msg = "只允许上传小于 5MB 的图片.", });
                }
                #endregion

                var newFileName = System.Guid.NewGuid().ToString().Substring(0, 6) + extname;

                string dir = DateTime.Now.ToString("yyyyMMdd");
                //var path = hostingEnv.WebRootPath; //网站静态文件目录  wwwroot 要把IHostingEnvironment注入进来
                //存放到 wwwroot 目录下
                var path = Path.Combine(env.WebRootPath, "Files\\upload\\") + dir;
                //var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "upload");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //完整的路径
                var filePath = Path.Combine(path, newFileName);
                //保存文件
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    imgFile.CopyTo(fileStream);
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream.Close();
                    }
                }

                //存放到库里的路径
                var virPath = Path.Combine("Files\\upload\\", filename).Replace("\\", "/");

                //完整物理路径，注意要用平台无关目录分隔符
                //string wuli_path = path + $"{Path.DirectorySeparatorChar}upload{Path.DirectorySeparatorChar}{dir}{Path.DirectorySeparatorChar}";

                //if (!System.IO.Directory.Exists(wuli_path))
                //{
                //    System.IO.Directory.CreateDirectory(wuli_path);
                //}
                //filename = wuli_path + filename1;
                //size += imgFile.Length;
                //using (FileStream fs = System.IO.File.Create(filename))
                //{
                //    imgFile.CopyTo(fs);
                //    fs.Flush();
                //}
                return Json(new { code = 0, msg = "上传成功", data = new { src = $"/Files/upload/{dir}/{newFileName}", imgName = newFileName } });
            }
            return Json(new { code = 1, msg = "上传失败", });
        }

        [HttpPost]
        public async Task<ActionResult> deleteModuleByCode(string[] code)
        {
            var res = await _sysModuleServices.deleteModuleByCode(code);
            return Json(new JsonResponse
            {
                code = ResponseCode.Success,
                msg = "成功",
                count = res
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetInfo(int id)
        {
            return Json(await _sysModuleServices.QueryById(id));
        }


    }
}
