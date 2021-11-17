using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.OAModel
{
    public partial class SysModule
    {
        [SugarColumn(IsIgnore = true)]
        public string PNames { get; set; }
        [SugarColumn(IsIgnore = true)]
        public int? Children { get; set; }
        [SugarColumn(IsIgnore = true)]
        public int? Lengths { get; set; }

        [SugarColumn(IsIgnore = true)]
        public bool open { get; set; }
        /// <summary>
        /// 是否被选中
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int LAY_CHECKED { get; set; }
    }
}
