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
    /// 组织角色关联表接口实现
    /// </summary>
    public class SysOrgRoleRelServices : BaseServices<SysOrgRoleRel>, ISysOrgRoleRelServices
    {

        IBaseRepository<SysOrgRoleRel> _dal;
        public SysOrgRoleRelServices(IBaseRepository<SysOrgRoleRel> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}
