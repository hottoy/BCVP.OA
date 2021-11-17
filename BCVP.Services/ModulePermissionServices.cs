using BCVP.Services.BASE;
using BCVP.Model.Models;
using BCVP.IRepository;
using BCVP.IServices;
using BCVP.IRepository.Base;

namespace BCVP.Services
{	
	/// <summary>
	/// ModulePermissionServices
	/// </summary>	
	public class ModulePermissionServices : BaseServices<ModulePermission>, IModulePermissionServices
    {

        IBaseRepository<ModulePermission> _dal;
        public ModulePermissionServices(IBaseRepository<ModulePermission> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
