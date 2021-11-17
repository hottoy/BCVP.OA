using BCVP.IRepository.Base;
using BCVP.IServices.IOAServices;
using BCVP.Model.Models.OAModel;
using BCVP.Services.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Services.OAServices
{
    /// <summary>
    /// TB_Sys_OrgServices
    /// </summary>	
    public class SysOrgServices : BaseServices<SysOrg>, ISysOrgServices
    {

        IBaseRepository<SysOrg> _dal;
        public SysOrgServices(IBaseRepository<SysOrg> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<List<SysOrg>> GetSysOrgs(string keyWords)
        {
            return await _dal.SqlQueryAsync($@"SELECT O.ORGID,
       O.PARENTID,
       O.NAMES,
       O.FULLNAME,
       O.CODE,
       O.IsEnabled,
       O.REMARK,
       O.CREATEID
  FROM TB_SYS_ORG O
 WHERE O.ISDELETE = 0 AND (O.NAMES like'%{keyWords}%' OR O.CODE LIKE'%{keyWords}%')
 ORDER BY O.CODE ASC, O.ORDERSORT ASC");
        }

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="info"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public async Task<int> insertOrUpdate(SysOrg info, int typeId)
        {
            //判断是否已经存在
            info.IsEnabled = info.IsEnabled == null ? 0 : info.IsEnabled;
            string strSQL = string.Format($@"SELECT COUNT(*) FROM TB_SYS_ORG M WHERE M.NAMES=N'{info.Names}'  AND M.CODE LIKE'{info.Code}____'");

            string strSQL2 = string.Format($@"select COUNT(*) from TB_Sys_Org m where m.Names=N'{info.Names}'   AND M.CODE LIKE'{info.Code}____'  and m.OrgID!={info.OrgID} ");

            var res = await _dal.GetIntAsync(typeId == 0 ? strSQL : strSQL2);
            if (res == 0)
            {
                SysOrg model = await base.QueryById(info.OrgID);
                if (typeId == 0)
                {
                    //添加
                    model.ParentID = info.OrgID;
                    model.Code = getModuleCode(info.OrgID, info.ParentID, info.Code);
                    model.Names = info.Names;
                    model.FullName = model.FullName + "\\" + info.Names;
                    model.PyCode = info.Names;
                    model.IsDelete = 0;
                    model.OrderSort = await _dal.GetIntAsync($@"SELECT NVL(MAX(O.ORDERSORT),0)+1 FROM TB_SYS_ORG O WHERE O.PARENTID={info.OrgID}");
                    model.Remark = info.Remark;
                    model.Attribut = info.Attribut;
                    model.SchemaID = info.SchemaID;
                    model.CreateId = 1;
                    model.CreateBy = "新增";
                    model.ModifyId = 2;
                    model.ModifyBy = "修改";
                    model.CreateTime = DateTime.Now;
                    model.ModifyTime = DateTime.Now;
                    model.PyCode = info.Names.ToLower();
                    return await base.Add(model);
                }
                else
                {
                    //修改
                    model.Names = info.Names;
                    model.PyCode = info.Names;
                    model.Attribut = info.Attribut;
                    model.SchemaID = info.SchemaID;
                    model.IsEnabled = info.IsEnabled;
                    model.Remark = info.Remark;
                    if (await _dal.Update(model))
                    {
                        return 1;
                    }
                }
            }
            return 0;

        }
        /// <summary>
        /// 计算树形编码
        /// </summary>
        /// <param name="moduleId">当前节点ID，新增是为0</param>
        /// <param name="parentId">当前父级节点，新增顶级时为0</param>
        /// <param name="code">当前节点编码</param>
        /// <returns>返回新增编码</returns>
        public string getModuleCode(int moduleId, int? parentId, string code)
        {
            if (moduleId == 0 && parentId == 0)
            {
                //新增根节点编码【按照编码位数查询】SQL数据库的写法
                //return _dal.GetString(string.Format($@"select RIGHT('0000'+CAST(max(m.Code)+1 as varchar(10)),4) from Tb_Sys_Module m where m.ParentId={parentId}"));
                //Oracle数据库的写法
                return _dal.GetString($@"SELECT NVL(LPAD(NVL(MAX(M.CODE),0)+1,4,'0'),'0000') FROM TB_Sys_Org M WHERE M.PARENTID=0");
            }
            else
            {
                int len = code.Length + 4;

                //ORACLE数据库的写法
                var res = _dal.GetString(string.Format($@"SELECT NVL(LPAD(NVL(MAX(M.CODE),0)+1,{len},'0'),'0000') FROM TB_Sys_Org M WHERE M.CODE LIKE'{code}%' AND M.PARENTID={moduleId}"));
                if (res != null && res != "" && res.TrimStart('0') == "1")
                {
                    return code + "0001";
                }
                return res;
            }
        }

        /// <summary>
        /// 根据主键获取对象实体
        /// </summary>
        /// <param name="OrgID"></param>
        /// <returns></returns>
        public async Task<SysOrg> GetSysOrg(int OrgID)
        {
            return await _dal.SqlQuerySingleAsync($@"SELECT O.*,P.Names ParentName,O.IsEnabled FROM TB_SYS_ORG O
 LEFT JOIN TB_SYS_ORG P ON P.ORGID=O.PARENTID
 WHERE O.ORGID={OrgID}");
        }

        /// <summary>
        /// 根据编码删除机构
        /// </summary>
        /// <param name="code">机构编码</param>
        /// <returns></returns>
        public async Task<int> deleteSysOrgByCode(string code)
        {
            var res = 0;
            try
            {
                res = await _dal.ExecuteCommandAsync($@"DELETE FROM TB_SYS_ORG O WHERE O.CODE LIKE'{code}%'");
            }
            catch (Exception)
            {
                res = 0;
            }
            return res;
        }

        /// <summary>
        /// 根据主键ID修改启用状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public async Task<int> saveEnabled(int id, int enabled)
        {
            return await _dal.ExecuteCommandAsync($@"UPDATE TB_SYS_ORG O SET O.IsEnabled={enabled} WHERE O.OrgID IN ({id})");
        }

        /// <summary>
        /// 获取全部有效的机构
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysOrg>> GetAllSysOrg(int orgID, int roleID)
        {
            string sql = $@"SELECT O.ORGID,
        O.PARENTID,
        O.CODE,
        O.NAMES,
        O.FULLNAME,
        O.PYCODE,
        O.ATTRIBUT,
        O.SCHEMAID,
        O.ISENABLED,
        O.ISDELETE,
        O.ORDERSORT,
        O.REMARK,
        O.CREATEID,
        O.CREATEBY,
        O.CREATETIME,
        O.MODIFYID,
        O.MODIFYBY,
        O.MODIFYTIME,
        DP.DATAID,
        'true' AS open,
        DECODE(O.ORGID,DP.DATAID,1,0) AS LAY_CHECKED
        FROM TB_SYS_ORG O
 LEFT JOIN  TB_SYS_DATAPERMISSION DP ON DP.DATAID=O.ORGID AND DP.DATATYPEID=1 AND DP.ORGID={orgID} AND DP.ROLEID={roleID} AND DP.ISENABLED=1 AND DP.ISDELETE=0
 WHERE O.ISENABLED=1 AND O.ISDELETE=0
 ORDER BY O.CODE ASC,O.ORGID ASC";
            return await _dal.SqlQueryAsync(sql);

        }

        /// <summary>
        /// 获取全部有效的机构
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns></returns>
        public async Task<List<SysOrg>> UserGetAllSysOrg(int userID)
        {
            string sql = $@" SELECT O.ORGID,
        O.PARENTID,
        O.CODE,
        O.NAMES,
        O.FULLNAME,
        O.PYCODE,
        O.ATTRIBUT,
        O.SCHEMAID,
        O.MODIFYID,
        NVL(U.DATAID, 0) DATAID,
        'true' AS OPEN,
        DECODE(O.ORGID, U.DATAID, 1, 0) AS LAY_CHECKED
   FROM TB_SYS_ORG O
   LEFT JOIN TB_SYS_USERPERMISSION U
     ON U.DATAID = O.ORGID
    AND U.DATATYPE = 2
    AND U.USERID = {userID}
  WHERE O.ISENABLED = 1
    AND O.ISDELETE = 0
  ORDER BY O.CODE ASC";
            return await _dal.SqlQueryAsync(sql);

        }
    }
}
