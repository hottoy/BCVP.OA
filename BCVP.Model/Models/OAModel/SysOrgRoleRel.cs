using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.OAModel
{
    /// <summary>
    /// 组织角色关联表
    /// </summary>
    [SugarTable("Tb_Sys_OrgRole_Rel")]
    public partial class SysOrgRoleRel
    {
        /// <summary>
        /// 主键 用户登录日志
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SEQ_Sys_OrgRole_Rel",ColumnDescription = "用户登录日志")]
        public int OrgRoleRelID { get; set; }

        /// <summary>
        /// 组织机构主键ID
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public int OrgID { get; set; }

        /// <summary>
        /// 角色主键ID
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public int RoleID { get; set; }

        /// <summary>
        /// 数据权限ID
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public int DataID { get; set; }
        
        /// <summary>
        /// 系统架构编码ID
        /// </summary>
        [SugarColumn(IsNullable =false)]
        public int SystemVersionID { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [SugarColumn(IsNullable = false,DefaultValue ="0")]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime CreateTime { get; set; } = System.DateTime.Now;

        /// <summary>
        /// 返回创建时间
        /// </summary>
        [SugarColumn(IsIgnore = true, ColumnDescription = "创建日期")]
        public string CreateTimeString { get { return CreateTime.ToString("yyyy-MM-dd HH:mm"); } }
    }
}
