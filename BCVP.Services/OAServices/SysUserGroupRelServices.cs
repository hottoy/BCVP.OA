using BCVP.IRepository.Base;
using BCVP.IServices.IOAServices;
using BCVP.Model.Models.OAModel;
using BCVP.Services.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Services.OAServices
{
    /// <summary>
    /// 用户与用户组关联表 接口实现
    /// </summary>
    public class SysUserGroupRelServices:BaseServices<SysUserGroupRel>, ISysUserGroupRelServices
    {
        IBaseRepository<SysUserGroupRel> _dal;
        public SysUserGroupRelServices(IBaseRepository<SysUserGroupRel> dal)
        {
            _dal = dal;
            base.BaseDal = dal;
        }
    }
}
