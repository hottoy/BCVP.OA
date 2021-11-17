using BCVP.IRepository.Base;
using BCVP.IServices;
using BCVP.Model.Models;
using BCVP.Services.BASE;

namespace BCVP.Services
{
    public partial class TasksQzServices : BaseServices<TasksQz>, ITasksQzServices
    {
        IBaseRepository<TasksQz> _dal;
        public TasksQzServices(IBaseRepository<TasksQz> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

    }
}
                    