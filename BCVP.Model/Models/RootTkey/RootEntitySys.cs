using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.RootTkey
{
    public class RootEntitySys
    {
        public RootEntitySys()
        {
            IsVersions = 1;
            OrderSort = 1;
            IsEnabled = 1;
            IsDeleted = 0;
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
        }
        /// <summary>
        /// 名称拼音字母
        /// </summary>
        [SugarColumn(ColumnDataType = "NVARCHAR2", Length = 128, IsNullable = true)]
        public string PyCode { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDataType = "NUMBER",Length =10, ColumnDescription = "版本号", DefaultValue = "1")]
        public int IsVersions { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NUMBER", ColumnDescription = "排序码")]
        public decimal OrderSort { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDataType = "NUMBER", ColumnDescription = "1表示激活,0表示不可用", DefaultValue = "1")]
        public int IsEnabled { get; set; }

        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDataType = "NUMBER", ColumnDescription = "1表示删除；0表示未删除", DefaultValue = "0")]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDescription = "创建日期")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "修改时间")]
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true,ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(IsIgnore = true, ColumnDescription = "创建日期")]
        public string CreateTimeString { get { return CreateTime.ToString("yyyy-MM-dd HH:mm"); } }

        [SugarColumn(IsIgnore = true, ColumnDescription = "修改时间")]
        public string ModifyTimeString { get { return ModifyTime.HasValue ? ModifyTime.Value.ToString("yyyy-MM-dd HH:mm") : ""; } }
    }
}
