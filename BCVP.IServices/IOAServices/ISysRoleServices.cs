using BCVP.IServices.BASE;
using BCVP.Model.Models.OAModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.IServices.IOAServices
{
    /// <summary>
    /// 系统角色
    /// </summary>
    public interface ISysRoleServices : IBaseServices<SysRole>
    {
        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="Id">主键ID</param>
        /// <param name="Info">实体对象</param>
        /// <param name="TypeId">0添加</param>
        /// <returns></returns>
        Task<bool> InsertOrUpdate(int Id, SysRole Info, int TypeId);
    }
}
