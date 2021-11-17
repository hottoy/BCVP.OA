using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BCVP.Model.Enums;

namespace BCVP.Model.Models.OAModel
{
    /// <summary>
    /// 用户登录日志
    /// </summary>
    [SugarTable("TB_SYS_LOG_UserLogin")]
    public partial class SysLogUserLogin
    {
        /// <summary>
        /// 主键 用户登录日志
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SEQ_SYS_LOG_UserLogin")]
        public int Login_ID { get; set; }

        /// <summary>
        /// 类型（0注册，1登录,2重置密码,3禁用）
        /// </summary>
        [SugarColumn(IsNullable = true)]
        [Required]
        public LoginType Login_Type { get; set; }

        /// <summary>
        /// 登录人
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int Login_UserID { get; set; }

        /// <summary>
        /// 登录状态（0失败，1成功）
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public LoginStatus Login_Status { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 64, IsNullable = true)]
        public string Login_IP { get; set; }

        /// <summary>
        /// 登录来源（0:PC 1:webapp 2:App 3:微信 4:IOS）
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public LoginSrc Login_Src { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 256, IsNullable = true)]
        public string Login_Message { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public object Login_CreateTime { get; set; }
    }
}
