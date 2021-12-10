using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.OAModel
{
    /// <summary>
    /// 用户组织关联表
    /// </summary>
    [SugarTable("Tb_Sys_UserOrg_Rel")]
    public partial class SysUserOrgRel
    {
        /// <summary>
        /// 主键 用户登录日志
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SEQ_Sys_UserOrg_Rel")]
        public int UserOrgRelID { get; set; }

        /// <summary>
        /// 用户主键ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int UserID { get; set; }

        /// <summary>
        /// 组织机构主键ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int OrgID { get; set; }


        /// <summary>
        /// 组织类型
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int OrgType { get; set; }

        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDataType = "NUMBER", ColumnDescription = "1表示删除；0表示未删除", DefaultValue = "0")]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public object CreateTime { get; set; } = System.DateTime.Now;

    }
}
