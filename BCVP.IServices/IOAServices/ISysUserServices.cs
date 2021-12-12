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
    /// 系统用户信息表接口
    /// </summary>
    public interface ISysUserServices : IBaseServices<SysUser>
    {
        /// <summary>
        /// 根据机构ID获取对应的用户信息
        /// </summary>
        /// <param name="OrgCode">机构编码</param>
        /// <param name="userName">用户名</param>
        /// <param name="userAccount">登录账号</param>
        /// <param name="page">每页显示条数</param>
        /// <param name="limit">页面</param>
        /// <param name="field">排序字段</param>
        /// <param name="order">desc , asc</param>
        /// <returns>Task<PageModel<SysUser>></returns>
        Task<PageModel<SysUser>> GetInfoList(string OrgCode, string userName, string userAccount, int? page, int? limit, string field, string order);

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<bool> insertOrUpdate(SysUser info);
    }
}
