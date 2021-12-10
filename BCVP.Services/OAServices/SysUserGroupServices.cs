using BCVP.IRepository.Base;
using BCVP.IServices.IOAServices;
using BCVP.Model.Models.OAModel;
using BCVP.Model.ViewModels;
using BCVP.Services.BASE;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Services.OAServices
{
    /// <summary>
    /// 用户组表 接口实现
    /// </summary>
    public class SysUserGroupServices:BaseServices<SysUserGroup>, ISysUserGroupServices
    {
        IBaseRepository<SysUserGroup> _dal;
        public SysUserGroupServices(IBaseRepository<SysUserGroup> dal)
        {
            _dal = dal;
            base.BaseDal = dal;
        }
    }
}
