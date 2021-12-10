using BCVP.Model.Models.RootTkey;
using SqlSugar;
using static BCVP.Model.Enums;

namespace BCVP.Model.Models.OAModel
{
	/// <summary>
	/// 系统角色表
	/// </summary>
    [SugarTable("TB_SYS_Role")]
    public partial class SysRole: RootEntitySys
	{
		/// <summary>
		/// 主键自增非空
		/// </summary>
		[SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SEQ_SYS_Role", ColumnDescription = "主键")]
		public int Role_ID { get; set; }

		/// <summary>
		/// 父级ID
		/// </summary>
		[SugarColumn(IsNullable = false)]
		public int Parent_ID { get; set; }

		/// <summary>
		/// 角色编码
		/// </summary>
		[SugarColumn(ColumnDataType = "NVARCHAR2", Length = 128, IsNullable = false)]
		public string Role_Code { get; set; }

		/// <summary>
		/// 角色名称
		/// </summary>
		[SugarColumn(ColumnDataType = "NVARCHAR2", Length = 64, IsNullable = false)]
		public string Role_Name { get; set; }

		/// <summary>
		/// 属性标记
		/// </summary>
		[SugarColumn(IsNullable = false, ColumnDataType = "NUMBER(1)", ColumnDescription = "1表示可用,0表示不可用", DefaultValue = "1")]
		public int Role_Attribute { get; set; }

		/// <summary>
		/// 系统架构编码【2021-12-3新加】
		/// </summary>
		[SugarColumn(IsNullable = false)]
		public int SystemVersionID { get; set; }

	}
}
