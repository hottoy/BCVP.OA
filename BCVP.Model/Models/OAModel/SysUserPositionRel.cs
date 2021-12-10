using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.OAModel
{
    /// <summary>
    /// 用户职位关联表
    /// </summary>
    [SugarTable("Tb_Sys_UserPosition_Rel")]
    public class SysUserPositionRel
    {
        /// <summary>
        /// 主键 用户职位关联表
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SEQ_Sys_UserPosition_Rel")]
        public int UserPositionRelID { get; set; }

        /// <summary>
        /// 用户表ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int UserID { get; set; }

        /// <summary>
        /// 职位ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int PositionID { get; set; }

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
