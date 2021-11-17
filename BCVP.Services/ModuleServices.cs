using BCVP.IRepository.Base;
using BCVP.IServices;
using BCVP.Model.Models;
using BCVP.Services.BASE;

namespace BCVP.Services
{
    /// <summary>
    /// ModuleServices
    /// </summary>	
    public class ModuleServices : BaseServices<Modules>, IModuleServices
    {

        IBaseRepository<Modules> _dal;
        public ModuleServices(IBaseRepository<Modules> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
