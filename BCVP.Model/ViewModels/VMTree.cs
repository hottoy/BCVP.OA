using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.ViewModels
{
    /// <summary>
    /// 树对象
    /// </summary>
    public class VMTree
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 父级主键ID
        /// </summary>
        public int pid { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string code { get; set; }
    }
}
