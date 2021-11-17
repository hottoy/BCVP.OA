using BCVP.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model
{
    /// <summary>
    /// 菜单结果对象
    /// </summary>
    public class MenusInfoResultDTO
    {
        /// <summary>
        /// Home
        /// </summary>
        public HomeInfo homeInfo { get; set; }
        /// <summary>
        /// logo
        /// </summary>
        public LogoInfo logoInfo { get; set; }
        /// <summary>
        /// 权限菜单树
        /// </summary>
        public List<treeView> menuInfo { get; set; }

    }

    public class LogoInfo
    {
        public string title { get; set; } = "LAYUI MINI";
        public string image { get; set; } = "../images/logo.png";
        public string href { get; set; } = "http://layuimini.99php.cn/docs/init/netcore.html";
    }

    public class HomeInfo
    {
        public string title { get; set; } = "首页";
        public string href { get; set; } = "../page/welcome-1.html?t=1";

    }
}
