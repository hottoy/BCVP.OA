using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.OAModel
{
    public partial class SysRole
    {
        /// <summary>
        /// 父级名称
        /// </summary>
        [SugarColumn(IsIgnore =true)]
        public string ParentName { get; set; }

        /// <summary>
        /// 父级编码
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string ParentCode { get; set; }

        /// <summary>
        /// 系统编码
        /// </summary>
        [SugarColumn(IsIgnore =true)]
        public string SystemVersionName { get; set; }
    }
}
