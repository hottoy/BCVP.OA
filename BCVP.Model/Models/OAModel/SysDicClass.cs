using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.OAModel
{
    /// <summary>
    /// 系统异常信息
    /// </summary>
    [SugarTable("Tb_Sys_DicClass")]
    public class SysDicClass
    {
        /// <summary>
        /// 主键 用户登录日志
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SEQ_Sys_DicClass")]
        public int Dic_ID { get; set; }

        /// <summary>
        /// 字典分类1:兴趣爱好；2：学历
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int DataType { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string dicName { get; set; }

        /// <summary>
        /// 字典编码
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string dicCode { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDataType = "NUMBER", ColumnDescription = "1表示激活,0表示不可用", DefaultValue = "1")]
        public int IsEnabled { get; set; }

        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDataType = "NUMBER", ColumnDescription = "1表示删除；0表示未删除", DefaultValue = "0")]
        public int IsDelete { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NUMBER", ColumnDescription = "排序码")]
        public decimal OrderSort { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 返回创建时间
        /// </summary>
        [SugarColumn(IsIgnore = true, ColumnDescription = "创建日期")]
        public string CreateTimeString { get { return CreateTime.ToString("yyyy-MM-dd HH:mm"); } }
    }
}
