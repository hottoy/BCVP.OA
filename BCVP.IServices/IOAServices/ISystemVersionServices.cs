using BCVP.IServices.BASE;
using BCVP.Model;
using BCVP.Model.Models.OAModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.IServices.IOAServices
{
    /// <summary>
    /// 系统版本
    /// </summary>
    public interface ISystemVersionServices:IBaseServices<SystemVersion>
    {
        /// <summary>
        /// 获取系统版本分页
        /// </summary>
        /// <param name="keyWord">关键词</param>
        /// <param name="page">当前页面</param>
        /// <param name="limit">每页默认显示的数量</param>
        /// <param name="field">排序字段</param>
        /// <param name="order">排序desc asc</param>
        /// <returns>Task<PageModel<SystemVersion>></returns>
        Task<PageModel<SystemVersion>> GetInfoList(string keyWord, int? page, int? limit, string field, string order);

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="Info">实体对象</param>
        /// <returns></returns>
        Task<bool> InsertOrEdit(SystemVersion Info);
    }
}
