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
        /// <param name="OrgID">机构OrgID</param>
        /// <returns></returns>
        Task<PageModel<SysUser>> GetInfoList(int OrgID, string userName, string userAccount, int? page, int? limit, string field, string order);

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<bool> insertOrUpdate(SysUser info);
    }
}
