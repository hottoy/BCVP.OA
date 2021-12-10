using BCVP.Model.Models.RootTkey;
using SqlSugar;

namespace BCVP.Model.Models.OAModel
{
    /// <summary>
    ///系统表
    /// </summary>
    [SugarTable("Tb_System_Version")]
    public partial class SystemVersion: RootEntitySys
    {
        /// <summary>
        /// 主键自增非空
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SEQ_System_Version", ColumnDescription = "主键")]
        public int SystemVersionID { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        [SugarColumn(ColumnDataType = "NVARCHAR2", Length = 64, IsNullable = false)]
        public string SystemVersionName { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        [SugarColumn(ColumnDataType = "NVARCHAR2", Length = 64, IsNullable = false)]
        public string SystemVersionCode { get; set; }
    }
}
