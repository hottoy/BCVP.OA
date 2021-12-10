using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.OAModel
{
    /// <summary>
    /// 用户与用户组关联表
    /// </summary>
    [SugarTable("Tb_Sys_UserGroup_Rel")]
    public class SysUserGroupRel
    {
        /// <summary>
        /// 用户与用户组关联表主键ID
        /// </summary>
        [SugarColumn(IsNullable =false,IsPrimaryKey =true,OracleSequenceName = "SEQ_Sys_UserGroup_Rel")]
        public int UserGroupRelID { get; set; }

        /// <summary>
        /// 用户表ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int UserID { get; set; }

        /// <summary>
        /// 用户组表ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int UserGroupID { get; set; }

        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDataType = "NUMBER(1)", ColumnDescription = "1表示删除；0表示未删除", DefaultValue = "0")]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDescription = "创建日期")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
