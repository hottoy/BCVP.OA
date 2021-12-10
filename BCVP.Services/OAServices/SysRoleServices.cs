using BCVP.IRepository.Base;
using BCVP.IServices.IOAServices;
using BCVP.Model.Models.OAModel;
using BCVP.Repository.Base;
using BCVP.Services.BASE;
using Fireasy.Common.Extensions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Services.OAServices
{
    /// <summary>
    /// 系统角色业务层
    /// </summary>
    public class SysRoleServices : BaseServices<SysRole>, ISysRoleServices
    {
        IBaseRepository<SysRole> _dal;
        public SysRoleServices(IBaseRepository<SysRole> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="moduleId">主键ID</param>
        /// <param name="info">对象实体</param>
        /// <returns></returns>
        public async Task<bool> InsertOrUpdate(int Id, SysRole Info, int TypeId)
        {
            string strSQL = string.Format($@"select COUNT(*) from TB_SYS_Role m where m.Role_Name=N'{Info.Role_Name}'  and M.Parent_ID={Info.Parent_ID}");
            string strSQL2 = string.Format($@"select COUNT(*) from TB_SYS_Role m where m.Role_Name=N'{Info.Role_Name}'  and M.Parent_ID={Info.Parent_ID} and m.Role_ID!={Info.Role_ID} ");
            var res = await _dal.GetIntAsync(TypeId == 0 ? strSQL : strSQL2);
            if (res == 0)
            {
                Info.PyCode = Info.Role_Name.ToPinyin();
                if (TypeId == 0)
                {
                    string strSQL3 = string.Format($@"SELECT COUNT(*)+1 FROM TB_SYS_Role M WHERE M.Parent_ID={Info.Parent_ID}");
                    Info.Parent_ID = Info.Parent_ID;
                    Info.Role_Code = getModuleCode(Info.Role_ID, Info.Parent_ID, Info.Role_Code);
                    Info.OrderSort = await _dal.GetIntAsync(strSQL3);
                    return await _dal.Add(Info) > 0;
                }
                else
                {
                    var m = await _dal.QueryById(Id);
                    m.IsVersions = Info.IsVersions + 1;
                    m.ModifyTime = DateTime.Now;
                    m.PyCode = Info.Role_Name.ToPinyin();
                    return await _dal.Update(Info);
                }
            }
            else
            { return false; }
        }

        /// <summary>
        /// 计算树形编码
        /// </summary>
        /// <param name="moduleId">当前节点ID，新增是为0</param>
        /// <param name="parentId">当前父级节点，新增顶级时为0</param>
        /// <param name="code">当前节点编码</param>
        /// <returns>返回新增编码</returns>
        public string getModuleCode(int id, int? parentId, string code)
        {
            if (id == 0 && parentId == 0)
            { 
                return _dal.GetString($@"SELECT NVL(LPAD(NVL(MAX(M.Role_Code),0)+1,4,'0'),'0000') FROM TB_SYS_Role M WHERE M.Parent_ID=0");
            }
            else
            {
                int len = code.Length + 4;
                var res = _dal.GetString(string.Format($@"SELECT NVL(LPAD(NVL(MAX(M.Role_Code),0)+1,{len},'0'),'0000') FROM TB_SYS_Role M WHERE M.Role_Code LIKE'{code}%' AND M.Parent_ID={parentId}"));
                if (res != null && res != "" && res.TrimStart('0') == "1")
                {
                    return code + "0001";
                }
                return res;
            }
        }
    }
}
