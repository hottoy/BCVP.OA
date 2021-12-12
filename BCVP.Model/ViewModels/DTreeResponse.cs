using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BCVP.Model.Enums;

namespace BCVP.Model.ViewModels
{
    /// <summary>
    /// response返回类
    /// </summary>
    public class DTreeResponse
    {
        /// <summary>
        /// 状态码 200操作成功
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 信息标识
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 数据集合
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object status { get; set; }
    }
}
