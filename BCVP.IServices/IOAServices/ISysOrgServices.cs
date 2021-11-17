using BCVP.IServices.BASE;
using BCVP.Model.Models.OAModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.IServices.IOAServices
{
	public interface ISysOrgServices : IBaseServices<SysOrg>
	{
		/// <summary>
		/// 加载
		/// </summary>
		/// <returns></returns>
		Task<List<SysOrg>> GetSysOrgs(string keyWords);

		/// <summary>
		/// 获取全部机构接口
		/// </summary>
		/// <returns></returns>
		Task<List<SysOrg>> GetAllSysOrg(int orgID, int roleID);

		/// <summary>
		/// 用户获取全部机构接口
		/// </summary>
		/// <param name="userID">用户ID</param>
		/// <returns></returns>
		Task<List<SysOrg>> UserGetAllSysOrg(int userID);

		/// <summary>
		/// 添加或修改
		/// </summary>
		/// <param name="info"></param>
		/// <param name="typeId"></param>
		/// <returns></returns>
		Task<int> insertOrUpdate(SysOrg info, int typeId);

		/// <summary>
		/// 根据主键获取对象实体
		/// </summary>
		/// <param name="OrgID">主键</param>
		/// <returns></returns>
		Task<SysOrg> GetSysOrg(int OrgID);

		/// <summary>
		/// 删除机构
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		Task<int> deleteSysOrgByCode(string code);

		/// <summary>
		/// 修改状态
		/// </summary>
		/// <param name="id"></param>
		/// <param name="enabled"></param>
		/// <returns></returns>
		Task<int> saveEnabled(int id, int enabled);
	}
}
