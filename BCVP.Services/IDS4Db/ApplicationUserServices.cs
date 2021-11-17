using BCVP.IRepository.Base;
using BCVP.Model.IDS4DbModels;
using BCVP.Services.BASE;

namespace BCVP.IServices
{
    public class ApplicationUserServices : BaseServices<ApplicationUser>, IApplicationUserServices
    {

        IBaseRepository<ApplicationUser> _dal;
        public ApplicationUserServices(IBaseRepository<ApplicationUser> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

    }
}