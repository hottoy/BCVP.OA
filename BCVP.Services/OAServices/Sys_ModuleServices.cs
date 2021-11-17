using BCVP.IRepository.Base;
using BCVP.IServices.IOAServices;
using BCVP.Model.Models.OAModel;
using BCVP.Model.ViewModels;
using BCVP.Services.BASE;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Services.OAServices
{
    /// <summary>
    /// Sys_ModuleServices
    /// </summary>	
    public class Sys_ModuleServices : BaseServices<SysModule>, ISysModuleServices
    {

        IBaseRepository<SysModule> _dal;
        public Sys_ModuleServices(IBaseRepository<SysModule> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
        /// <summary>
        /// 模块管理数据列表
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public async Task<List<SysModule>> GetModulesList(string keyWord)
        {
            string sql = $@"SELECT
                       M.CODE,
                       M.NAMES,
                       M.MODULEID,
                       M.PARENTID,
                       NVL((SELECT PM.NAMES
                             FROM TB_SYS_MODULE PM
                            WHERE PM.MODULEID = M.PARENTID),
                           '顶级') PNAMES,
                       M.LINKURL,
                       M.ISMENU,
                       M.ISENABLED,
                       M.AREA,
                       M.CONTROLLER,
                       M.ACTIONS,
                       M.ICON1,
                       M.ICON2,
                       M.ISTARGET
                  FROM TB_SYS_MODULE M
                 WHERE M.ISDELETE = 0
                 AND (M.NAMES LIKE N'%{keyWord}%' OR M.CODE LIKE N'{keyWord}%')
                 ORDER BY M.CODE ASC,M.ORDERSORT ASC";
            return await _dal.SqlQueryAsync(sql);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<int> deleteModuleByCode(string[] code)
        {
            var res = 0;
            try
            {
                for (int i = 0; i < code.Length; i++)
                {
                    res = await _dal.ExecuteCommandAsync(string.Format($@"DELETE FROM Tb_Sys_Module m WHERE m.Code like'{code[i]}%'"));
                }

            }
            catch (Exception)
            {
                res = 0;
            }
            return res;
        }

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="moduleId">主键ID</param>
        /// <param name="info">对象实体</param>
        /// <returns></returns>
        public async Task<int> insertOrUpdate(int moduleId, SysModule info, int typeId)
        {
            //info.IsMenu = info.IsMenu == null ? 0 : info.IsMenu;
            //info.IsEnabled = info.IsEnabled == null ? 0 : info.IsEnabled;
            string strSQL = string.Format($@"select COUNT(*) from Tb_Sys_Module m where m.Names=N'{info.Names}'  and M.ParentId={info.ParentId}");

            string strSQL2 = string.Format($@"select COUNT(*) from Tb_Sys_Module m where m.Names=N'{info.Names}'  and M.ParentId={info.ParentId} and m.ModuleID!={info.ModuleID} ");

            var res = await _dal.GetIntAsync(typeId == 0 ? strSQL : strSQL2);
            if (res == 0)
            {
                if (typeId == 0)
                {
                    string strSQL3 = string.Format($@"SELECT COUNT(*)+1 FROM TB_SYS_MODULE M WHERE M.PARENTID={info.ParentId}");
                    info.ParentId = info.ParentId;
                    info.Code = getModuleCode(info.ModuleID, info.ParentId, info.Code);
                    info.CreateId = 1;
                    info.CreateBy = "新增";
                    info.ModifyId = 2;
                    info.ModifyBy = "修改";
                    info.IsDelete = 0;
                    info.OrderSort = await _dal.GetIntAsync(strSQL3);
                    //info.CreateTime = DateTime.Now;
                    //info.ModifyTime = DateTime.Now;
                    await _dal.Add(info);
                    return 1;
                }
                else
                {
                    await _dal.Update(new
                    {
                        Names = info.Names,
                        LinkUrl = info.LinkUrl,
                        Area = info.Area,
                        Actions = info.Actions,
                        Arguments = info.Arguments,
                        Icon1 = info.Icon1,
                        IsMenu = info.IsMenu,
                        IsEnabled = info.IsEnabled,
                        IsTarget = info.IsTarget,
                        Remark = info.Remark,
                        ModifyTime = DateTime.Now,
                        ModuleID = moduleId
                    });
                }
                return (int)moduleId;
            }
            else { return 0; }
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
            {  //Oracle数据库的写法
                return _dal.GetString($@"SELECT NVL(LPAD(NVL(MAX(M.CODE),0)+1,4,'0'),'0000') FROM TB_SYS_MODULE M WHERE M.PARENTID=0");
            }
            else
            {
                int len = code.Length + 4;
                SugarParameter[] configParms = new SugarParameter[]{
                                new SugarParameter("@Code","%"+code),
                                new SugarParameter("@Len",len)
                            };

                //string bu = 0.ToString().PadLeft(len, '0');
                //var likeMatch = new string('0', len);
                //var res = _dal.GetString("select RIGHT('"+ likeMatch+ "'+CAST(max(m.Code)+1 as varchar(128)),"+len+")  from Tb_Sys_Module m where m.Code like '%"+ code + "____'");
                //var res = _dal.GetString("SELECT left(max(m.Code),len(max(m.Code))-4)+ right('0000'+ltrim(cast(RIGHT(max(m.Code),4) as int)+1),4) from Tb_Sys_Module m where m.Code like '@Code____' and len(m.Code)=@Len", configParms);
                //第一种写法
                //var sql3 = string.Format($@"SELECT left(max(m.Code), len(max(m.Code)) - 4) + right('0000' + ltrim(cast(RIGHT(max(m.Code), 4) as int) + 1), 4) from Tb_Sys_Module m where m.Code like '%{code}____' and len(m.Code) ={len}");
                //SQL数据库写法
                //var res = _dal.GetString(string.Format($@"SELECT left(max(m.Code), len(max(m.Code)) - 4) + right('0000' + ltrim(cast(RIGHT(max(m.Code), 4) as int) + 1), 4) from Tb_Sys_Module m where m.Code like '%{code}____' and len(m.Code) ={len}"));

                //ORACLE数据库的写法
                var res = _dal.GetString(string.Format($@"SELECT NVL(LPAD(NVL(MAX(M.CODE),0)+1,{len},'0'),'0000') FROM TB_SYS_MODULE M WHERE M.CODE LIKE'{code}%' AND M.PARENTID={parentId}"));
                if (res != null && res != "" && res.TrimStart('0') == "1")
                {
                    return code + "0001";
                }
                return res;
            }
        }

        /// <summary>
        /// 读取首页菜单
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysModule>> GetModules()
        {
            //SQL数据库的写法
            //string sql = $@"SELECT
            //(SELECT COUNT(*) FROM TB_SYS_MODULE S WHERE LEN(S.CODE)=(LEN(M.CODE)+4) AND S.CODE LIKE '%' + M.CODE + '____') CHILDREN,
            //LEN(M.CODE) LENGTHS,
            //M.CODE,M.NAMES,M.MODULEID,M.PARENTID,M.LINKURL,M.AREA,M.CONTROLLER,M.ACTIONS,M.ICON1,M.ICON2,M.ISTARGET
            //FROM TB_SYS_MODULE M WHERE M.ISDELETE=0 AND M.ISMENU=1 AND M.ISENABLED=1 ORDER BY M.CODE ASC";

            //ORACLE数据库的写法
            string sql = $@"SELECT (SELECT COUNT(*)
          FROM TB_SYS_MODULE p
         WHERE p.parentid = m.moduleid
           and p.ismenu = 1
           and p.isenabled = 1
           and p.isdelete = 0) CHILDREN,
       length(M.CODE) LENGTHS,
       M.CODE,
       M.NAMES,
       M.MODULEID,
       M.PARENTID,
       M.LINKURL,
       M.AREA,
       M.CONTROLLER,
       M.ACTIONS,
       M.ICON1,
       M.ICON2,
       M.ISTARGET
  FROM TB_SYS_MODULE M
 WHERE M.ISDELETE = 0
   AND M.ISMENU = 1
   AND M.ISENABLED = 1
 ORDER BY M.CODE ASC,M.ORDERSORT ASC
";
            return await _dal.SqlQueryAsync(sql);
        }

        /// <summary>
        /// 获取全部角色OK
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysModule>> GetModulesSchema()
        {
            string sql = $@" SELECT M.MODULEID,
        M.PARENTID,
        M.NAMES,
        M.CODE,
        M.PYCODE,
        M.LINKURL,
        M.AREA,
        M.CONTROLLER,
        M.ACTIONS,
        M.ISMENU,
        M.ISENABLED,
        M.ARGUMENTS,
        M.ICON1,
        M.ICON2,
        M.ICON3,
        M.ISDELETE,
        M.REMARK,
        M.ORDERSORT,
        M.CREATEID,
        M.CREATEBY,
        M.CREATETIME,
        M.MODIFYID,
        M.MODIFYBY,
        M.MODIFYTIME,
        M.ISTARGET,
        'true' AS open
   FROM TB_SYS_MODULE M
  WHERE M.ISENABLED = 1
    AND M.ISDELETE = 0
  ORDER BY M.CODE ASC, M.ORDERSORT ASC";
            return await _dal.SqlQueryAsync(sql);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<int> asyDeleteModuleByCode(string[] code)
        {
            var res = 0;
            try
            {
                for (int i = 0; i < code.Length; i++)
                {
                    res = await _dal.ExecuteCommandAsync(string.Format($@"DELETE FROM Tb_Sys_Module m WHERE m.Code like'{code[i]}%'"));
                }

            }
            catch (Exception)
            {
                res = 0;
            }
            return res;
            //return await _dal.ExecuteCommandAsync(string.Format($@"DELETE m FROM Tb_Sys_Module m WHERE m.Code like'{code}%'"));
        }

        public async Task<List<treeView>> GetMenuInfoAsync()
        {
            string sql = $@"SELECT (SELECT COUNT(*)
                          FROM TB_SYS_MODULE p
                         WHERE p.parentid = m.moduleid
                           and p.ismenu = 1
                           and p.isenabled = 1
                           and p.isdelete = 0) CHILDREN,
                       length(M.CODE) LENGTHS,
                       M.CODE,
                       M.NAMES,
                       M.MODULEID,
                       M.PARENTID,
                       M.LINKURL,
                       M.AREA,
                       M.CONTROLLER,
                       M.ACTIONS,
                       M.ICON1,
                       M.ICON2,
                       M.ISTARGET
                  FROM TB_SYS_MODULE M
                 WHERE M.ISDELETE = 0
                   AND M.ISMENU = 1
                   AND M.ISENABLED = 1
                 ORDER BY M.CODE ASC,M.ORDERSORT ASC
                ";
            var allMenuList = await _dal.SqlQueryAsync(sql);
            List<treeView> rootNodeList = new List<treeView>();
            foreach (var parentNodeList in allMenuList.Where(t => t.ParentId == 0))
            {
                treeView menuNode = new treeView();
                menuNode.Id = parentNodeList.ModuleID;
                menuNode.ParentId = parentNodeList.ParentId;
                menuNode.title = parentNodeList.Names;
                menuNode.code = parentNodeList.Code;
                menuNode.href = parentNodeList.LinkUrl;
                menuNode.icon = parentNodeList.Icon1;
                menuNode.target = parentNodeList.IsTarget;
                menuNode.child = CreateChildTree(allMenuList, menuNode);
                rootNodeList.Add(menuNode);
            }
            return rootNodeList;
        }

        /// <summary>
        /// 获取菜单树结构
        /// </summary>
        /// <returns></returns>
        public List<treeView> GetMenuInfo()
        {
            string sql = $@"SELECT (SELECT COUNT(*)
                          FROM TB_SYS_MODULE p
                         WHERE p.parentid = m.moduleid
                           and p.ismenu = 1
                           and p.isenabled = 1
                           and p.isdelete = 0) CHILDREN,
                       length(M.CODE) LENGTHS,
                       M.CODE,
                       M.NAMES,
                       M.MODULEID,
                       M.PARENTID,
                       M.LINKURL,
                       M.AREA,
                       M.CONTROLLER,
                       M.ACTIONS,
                       M.ICON1,
                       M.ICON2,
                       M.ISTARGET
                  FROM TB_SYS_MODULE M
                 WHERE M.ISDELETE = 0
                   AND M.ISMENU = 1
                   AND M.ISENABLED = 1
                 ORDER BY M.CODE ASC,M.ORDERSORT ASC
                ";
            var allMenuList = _dal.SqlQuery(sql);
            List<treeView> rootNodeList = new List<treeView>();
            foreach (var parentNodeList in allMenuList.Where(t => t.ParentId == 0))
            {
                treeView menuNode = new treeView();
                menuNode.Id = parentNodeList.ModuleID;
                menuNode.ParentId = parentNodeList.ParentId;
                menuNode.title = parentNodeList.Names;
                menuNode.code = parentNodeList.Code;
                menuNode.href = parentNodeList.LinkUrl;
                menuNode.icon = parentNodeList.Icon1;
                menuNode.target = parentNodeList.IsTarget;
                menuNode.child = CreateChildTree(allMenuList, menuNode);
                rootNodeList.Add(menuNode);
            }
            return rootNodeList;
        }

        /// <summary>
        /// 递归生成子树
        /// </summary>
        /// <param name="AllMenuList"></param>
        /// <param name="vmMenu"></param>
        /// <returns></returns>
        private List<treeView> CreateChildTree(List<SysModule> AllMenuList, treeView vmMenu)
        {
            int parentMenuID = vmMenu.Id;//根节点ID
            List<treeView> nodeList = new List<treeView>();
            var children = AllMenuList.Where(t => t.ParentId == parentMenuID);
            foreach (var chl in children)
            {
                treeView node = new treeView();
                node.Id = chl.ModuleID;
                node.ParentId = chl.ParentId;
                node.code = chl.Code;
                node.title = chl.Names;
                node.href = chl.LinkUrl;
                node.icon = chl.Icon1;
                node.target = chl.IsTarget;
                node.child = CreateChildTree(AllMenuList, node);
                nodeList.Add(node);
            }
            return nodeList;
        }
    }
}
