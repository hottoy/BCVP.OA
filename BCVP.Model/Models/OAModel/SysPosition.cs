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
    /// 职位表【岗位与人对应，只能由一个人担任，一个或若干个岗位的共性体现就是职位，即职位可以由一个或多个岗位组成】
    /// </summary>
    [SugarTable("Tb_Sys_Position")]
    public class SysPosition : RootEntitySys
    {
        /// <summary>
        /// 主键 职位表
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SEQ_Sys_Position")]
        public int PositionID { get; set; }

        /// <summary>
        /// 父级ID 上级职位
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int Parent_ID { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string PositionName { get; set; }

        /// <summary>
        /// 职位编码
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string PositionCode { get; set; }

        /// <summary>
        /// 组织机构ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int OrgID { get; set; }

    }
}
