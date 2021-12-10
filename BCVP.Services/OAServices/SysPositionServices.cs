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
    /// 职位表【岗位与人对应，只能由一个人担任，一个或若干个岗位的共性体现就是职位，即职位可以由一个或多个岗位组成实现
    /// </summary>
    public class SysPositionServices:BaseServices<SysPosition>, ISysPositionServices
    {
        IBaseRepository<SysPosition> _dal;
        public SysPositionServices(IBaseRepository<SysPosition> dal)
        {
            _dal = dal;
            base.BaseDal = dal;
        }
    }
}
