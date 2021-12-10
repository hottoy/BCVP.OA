using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.OAModel
{
    /// <summary>
    /// 用户组——角色关联表
    /// </summary>
    [SugarTable("Tb_Sys_UserGroupRole_Rel")]
    public class SysUserGroupRoleRel
    {
        /// <summary>
        /// 主键ID 
        /// </summary>
        [SugarColumn(IsPrimaryKey =true,OracleSequenceName = "SEQ_Sys_UserGroupRole_Rel",ColumnDescription = "用户组——角色关联表ID")]
        public int UserGroupRoleID { get; set; }

        /// <summary>
        /// 用户组主键ID
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDescription = "用户组主键ID")]
        public int UserGroupID { get; set; }

        /// <summary>
        /// 角色主键ID
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDescription = "角色主键ID")]
        public int RoleID { get; set; }

        /// <summary>
        /// 数据权限ID
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDescription = "数据权限ID")]
        public int DataID { get; set; }


        /// <summary>
        /// 系统架构编码ID
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDataType = "NUMBER", ColumnDescription = "系统架构编码ID")]
        public int SystemVersionID { get; set; }


        /// <summary>
        /// 1表示删除；0表示未删除
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDataType = "NUMBER(1)", ColumnDescription = "1表示删除；0表示未删除", DefaultValue = "0")]
        public int IsDeleted { get; set; } = 0;

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 8, ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = System.DateTime.Now;

        /// <summary>
        /// 返回创建时间
        /// </summary>
        [SugarColumn(IsIgnore = true, ColumnDescription = "创建日期")]
        public string CreateTimeString { get { return CreateTime.ToString("yyyy-MM-dd HH:mm"); } }
    }
}
