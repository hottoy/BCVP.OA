using BCVP.IServices.BASE;
using BCVP.Model.Models.OAModel;
using BCVP.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.IServices.IOAServices
{
    /// <summary>
    /// Sys_ModuleServices
    /// </summary>	
    public interface ISysModuleServices : IBaseServices<SysModule>
    {
        /// <summary>
        /// 获取首页权限树OK
        /// </summary>
        /// <returns></returns>
        List<treeView> GetMenuInfo();

        /// <summary>
        /// 异步获取首页权限树OK
        /// </summary>
        /// <returns></returns>
        Task<List<treeView>> GetMenuInfoAsync();

        /// <summary>
        /// 获取菜单列表OK
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        Task<List<SysModule>> GetModulesList(string keyWord);

        /// <summary>
        /// 计算树形编码
        /// </summary>
        /// <param name="moduleId">当前节点ID，新增是为0</param>
        /// <param name="parentId">当前父级节点，新增顶级时为0</param>
        /// <param name="code">当前节点编码</param>
        /// <returns>返回新增编码</returns>
        string getModuleCode(int moduleId, int? parentId, string code);


        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="moduleId">主键ID</param>
        /// <param name="info">实体对象</param>
        /// <param name="typeId">0添加</param>
        /// <returns></returns>
        Task<int> insertOrUpdate(int moduleId, SysModule info, int typeId);

        /// <summary>
        /// 读取首页菜单
        /// </summary>
        /// <returns></returns>
        Task<List<SysModule>> GetModules();

        /// <summary>
        /// 获取全部菜单
        /// </summary>
        /// <returns></returns>
        Task<List<SysModule>> GetModulesSchema();

        /// <summary>
        /// 根据模块编码异步删除
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<int> asyDeleteModuleByCode(string[] code);

        /// <summary>
        /// 根据模块编码异步删除
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<int> deleteModuleByCode(string[] code);
    }
}
