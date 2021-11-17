using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.OAModel
{
    public partial class SysLogUserLogin
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string UserName { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string UserAccount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
