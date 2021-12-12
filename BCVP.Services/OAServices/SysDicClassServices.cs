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
    /// 字典接口实现
    /// </summary>
    public class SysDicClassServices : BaseServices<SysDicClass>, ISysDicClassServices
    {

        IBaseRepository<SysDicClass> _dal;
        public SysDicClassServices(IBaseRepository<SysDicClass> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}
