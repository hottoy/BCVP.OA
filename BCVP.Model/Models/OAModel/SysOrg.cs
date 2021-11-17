using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BCVP.Model.Enums;

namespace BCVP.Model.Models.OAModel
{
	[SugarTable("TB_Sys_Org")]
	public partial class SysOrg
	{

		/// <summary>
		/// 主键自增非空
		/// </summary>
		[SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SEQ_Sys_Org")]
		public int OrgID { get; set; }

		[SugarColumn(IsNullable = false)]
		public int ParentID { get; set; }

		/// <summary>
		/// 编码
		/// </summary>
		[SugarColumn(ColumnDataType = "nvarchar", Length = 128, IsNullable = false)]
		public string Code { get; set; }

		/// <summary>
		/// 机构名称
		/// </summary>
		[SugarColumn(ColumnDataType = "nvarchar", Length = 128, IsNullable = false)]
		public string Names { get; set; }

		/// <summary>
		/// 机构全称。比如：有限公司/销售部/
		/// </summary>
		[SugarColumn(ColumnDataType = "nvarchar", Length = 128, IsNullable = false)]
		public string FullName { get; set; }

		/// <summary>
		/// 编码
		/// </summary>
		[SugarColumn(ColumnDataType = "nvarchar", Length = 128, IsNullable = false)]
		public string PyCode { get; set; }

		/// <summary>
		/// 机构属性
		/// </summary>
		[SugarColumn(IsNullable = false)]
		public OrgAttribute Attribut { get; set; }
		public int? SchemaID { get; set; }

		[SugarColumn(IsNullable = true)]
		public int? IsEnabled { get; set; }

		[SugarColumn(IsNullable = true)]
		public int? IsDelete { get; set; }

		[SugarColumn(IsNullable = true)]
		public string Remark { get; set; }

		[SugarColumn(IsNullable = true)]
		public decimal? OrderSort { get; set; }

		/// <summary>
		/// 修改人
		/// </summary>
		[SugarColumn(IsNullable = true)]
		public int? CreateId { get; set; }

		/// <summary>
		/// 修改时间
		/// </summary>
		[SugarColumn(IsNullable = true)]
		public string CreateBy { get; set; }

		/// <summary>
		/// 修改时间
		/// </summary>
		[SugarColumn(IsNullable = true)]
		public DateTime? CreateTime { get; set; } = DateTime.Now;

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
		public DateTime? ModifyTime { get; set; } = DateTime.Now;

	}
}
