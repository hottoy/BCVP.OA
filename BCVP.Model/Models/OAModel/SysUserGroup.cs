using BCVP.Model.Models.RootTkey;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.OAModel
{
    /// <summary>
    /// 用户组表
    /// </summary>
    [SugarTable("Tb_Sys_UserGroup")]
    public class SysUserGroup: RootEntitySys
    {
        /// <summary>
        /// 主键 用户组表
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SEQ_Sys_UserGroup")]
        public int UserGroupID { get; set; }

        /// <summary>
        /// 用户组表父级ID
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public int ParentID { get; set; }

        /// <summary>
        /// 用户组名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string UserGroupName { get; set; }

        /// <summary>
        /// 用户组编码
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string UserGroupCode { get; set; }

    }
}
