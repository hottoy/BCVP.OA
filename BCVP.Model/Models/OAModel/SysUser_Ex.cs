using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.OAModel
{
    public partial class SysUser
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string RoleName { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string OrgName { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string EducationName { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string ProfessionaName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string SexName { get; set; }
    }
}
