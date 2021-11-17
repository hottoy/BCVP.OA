using BCVP.IRepository.Base;
using BCVP.IServices;
using BCVP.Model.Models;
using BCVP.Services.BASE;

namespace BCVP.Services
{
    public partial class OperateLogServices : BaseServices<OperateLog>, IOperateLogServices
    {
        IBaseRepository<OperateLog> _dal;
        public OperateLogServices(IBaseRepository<OperateLog> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

    }
}
