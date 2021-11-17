using BCVP.IRepository.Base;
using BCVP.IServices;
using BCVP.Model.Models;
using BCVP.Services.BASE;

namespace BCVP.Services
{
    public partial class PasswordLibServices : BaseServices<PasswordLib>, IPasswordLibServices
    {
        IBaseRepository<PasswordLib> _dal;
        public PasswordLibServices(IBaseRepository<PasswordLib> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

    }
}
