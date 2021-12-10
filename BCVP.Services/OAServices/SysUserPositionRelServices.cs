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
    /// 用户职位关联表接口实现
    /// </summary>
    public class SysUserPositionRelServices: BaseServices<SysUserPositionRel>, ISysUserPositionRelServices
    {
        IBaseRepository<SysUserPositionRel> _dal;
        public SysUserPositionRelServices(IBaseRepository<SysUserPositionRel> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}
