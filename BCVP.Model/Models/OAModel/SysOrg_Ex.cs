using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.OAModel
{
    public partial class SysOrg
    {
        [SugarColumn(IsIgnore = true)]
        public string ParentName { get; set; }

        /// <summary>
        /// 是否打开
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool open { get; set; }

        [SugarColumn(IsIgnore = true)]
        public bool LAY_CHECKED { get; set; }

    }
}
