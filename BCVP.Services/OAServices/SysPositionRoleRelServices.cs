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
    /// 身份-角色关联表接口实现
    /// </summary>
    public class SysPositionRoleRelServices: BaseServices<SysPositionRoleRel>, ISysPositionRoleRelServices
    {
        IBaseRepository<SysPositionRoleRel> _dal;
        public SysPositionRoleRelServices(IBaseRepository<SysPositionRoleRel> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}
