using BCVP.IRepository.Base;
using BCVP.IServices;
using BCVP.Model.Models;
using BCVP.Services.BASE;

namespace BCVP.Services
{
    /// <summary>
    /// PermissionServices
    /// </summary>	
    public class PermissionServices : BaseServices<Permission>, IPermissionServices
    {

        IBaseRepository<Permission> _dal;
        public PermissionServices(IBaseRepository<Permission> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
