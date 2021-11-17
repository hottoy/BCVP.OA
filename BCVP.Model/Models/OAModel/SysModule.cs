using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.OAModel
{
    ///<summary>
    ///系统菜单
    ///</summary>
    [SugarTable("Tb_Sys_Module")]
    public partial class SysModule
    {

        /// <summary>
        /// 主键 如果是SQL数据库直接是自增类型
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SEQ_Sys_Module")]
        public int ModuleID { get; set; }

        [Required]
        public int? ParentId { get; set; }


        [SugarColumn(ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
        public string Names { get; set; }


        [SugarColumn(ColumnDataType = "nvarchar", Length = 128, IsNullable = true)]
        public string Code { get; set; }


        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string PyCode { get; set; }


        [SugarColumn(ColumnDataType = "nvarchar", Length = 256, IsNullable = true)]
        public string LinkUrl { get; set; }


        [SugarColumn(ColumnDataType = "nvarchar", Length = 128, IsNullable = true)]
        public string Area { get; set; }


        [SugarColumn(ColumnDataType = "nvarchar", Length = 128, IsNullable = true)]
        public string Controller { get; set; }


        [SugarColumn(ColumnDataType = "nvarchar", Length = 128, IsNullable = true)]
        public string Actions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? IsMenu { get; set; }


        [SugarColumn(IsNullable = true)]
        public int? IsEnabled { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Arguments { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string Icon1 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string Icon2 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string Icon3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? IsDelete { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? OrderSort { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? CreateId { get; set; }


        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string CreateBy { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; }


        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? ModifyId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ModifyBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? ModifyTime { get; set; }

        [SugarColumn(IsNullable = true)]
        public string IsTarget { get; set; }
    }
}
