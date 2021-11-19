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
    [SugarTable("TB_SYS_LOG_Exception")]
    public partial class SysLogException
    {
        /// <summary>
        /// 主键 用户登录日志
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SEQ_SYS_LOG_Exception")]
        public int Exc_ID { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Exc_Requ_Param { get; set; }

        /// <summary>
        /// 异常名称
        /// </summary>
        [SugarColumn(IsNullable =true)]
        public string Exc_Name { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [SugarColumn(IsNullable =true)]
        public string Exc_Message { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        [SugarColumn(IsNullable =true)]
        public int Oper_User_ID { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [SugarColumn(IsNullable =true)]
        public string Oper_User_Name { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ControllerName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        [SugarColumn(IsNullable = true)]

        public string Oper_Method { get; set; }

        /// <summary>
        /// 请求URL
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Oper_URL { get; set; }
        /// <summary>
        /// 请求IP
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Oper_IP { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime Oper_CreateTime { get; set; } = System.DateTime.Now;

        /// <summary>
        /// 版本号
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Oper_Versions { get; set; }
    }
}
