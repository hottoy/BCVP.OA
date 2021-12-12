using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.ViewModels
{
    public class DtreeDataModel
    {
        ///// <summary>
        ///// 节点ID。树绑定的元素ID，唯一值，后面的事件监听也需要绑定ID，后续所有基于树的操作都是根据这个ID来绑定关联的。
        ///// </summary>
        //public string id { get; set; }
        ///// <summary>
        ///// 节点名称
        ///// </summary>
        //public string title { get; set; }
        ///// <summary>
        ///// 复选框列表（开启复选框必填，默认是json数组。）"checkArr": [{"type": "0", "checked": "0"}],
        ///// {"id":"001005","title": "郴州市","checkArr": "0","iconClass": "dtree-icon-caidan_xunzhang","parentId": "001"}
        ///// </summary>
        //public string checkArr { get; set; }
        ///// <summary>
        ///// 上级节点ID
        ///// </summary>
        //public string parentId { get; set; }

        ///// <summary>
        ///// 是否展开节点
        ///// </summary>
        //public bool spread { get; set; }
        ///// <summary>
        ///// 是否最后一级节点（true：是，false：否，布尔值，非必填）
        ///// </summary>
        //public bool last { get; set; }

        ///// <summary>
        ///// 自定义图标class【"iconClass": "dtree-icon-caidan_xunzhang"】
        ///// </summary>
        //public string iconClass { get; set; }

        /////// <summary>
        /////// 复选框是否选中
        /////// </summary>
        ////public string isChecked { get; set; }

        ///// <summary>
        ///// type: "type", //复选框标记（开启复选框，从0开始。非必填）
        ///// </summary>
        //public string type { get; set; }

        ///// <summary>
        ///// 节点是否初始为选中状态【是否选中（开启复选框，0-未选中，1-选中，2-半选。非必填）】
        ///// </summary>
        //public bool @checked { get; set; }

        ///// <summary>
        ///// hide: "hide", //节点隐藏状态（v2.5.0版本新增。true：隐藏，false：不隐藏，布尔值，非必填）
        ///// </summary>
        //public bool hide { get; set; }

        ///// <summary>
        ///// disabled: "disabled", //节点禁用状态（v2.5.0版本新增。true：禁用，false：不禁用，布尔值，非必填）
        ///// </summary>
        //public bool disabled { get; set; }

        // {"id":"001","title": "湖南省","checkArr": "0","parentId": "0"},
        public string id { get; set; }
        public string title { get; set; }
        public string checkArr { get; set; }
        public string parentId { get; set; }

        public bool spread { get; set; }
    }
}
