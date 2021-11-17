using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.ViewModels
{
    public class treeView
    {
        /// <summary>
        /// 模块ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父级模块。ModuleID
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 树节点名称。如：组件管理
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 菜单链接地址。如：page/icon.html
        /// </summary>
        public string href { get; set; }

        /// <summary>
        /// 树节点显示图标。如：fa fa-dot-circle-o
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 打开方式：
        /// "_blank":在新窗口中浏览新的页面。
        /// "_self":在同一个窗口打开新的页面。
        /// "_parent":在父窗口中打开新的页面。（页面中使用框架才有用）
        /// "_top" :以整个浏览器作为窗口显示新页面。（突破了页面框架的限制）
        /// </summary>
        public string target { get; set; }

        /// <summary>
        /// 孩子
        /// </summary>
        public object child { get; set; }
        //public IList<treeView> child = new List<treeView>();
        //public void Addchildren(treeView node)
        //{
        //    this.child.Add(node);
        //}
    }
}
