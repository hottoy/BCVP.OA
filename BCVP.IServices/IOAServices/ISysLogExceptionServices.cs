using BCVP.IServices.BASE;
using BCVP.Model;
using BCVP.Model.Models.OAModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.IServices.IOAServices
{
    /// <summary>
    /// 系统异常日志
    /// </summary>
    public interface ISysLogExceptionServices:IBaseServices<SysLogException>
    {
        /// <summary>
        /// 获取日志信息列表
        /// </summary>
        /// <param name="loginSrc">登录来源（0:PC 1:webapp 2:App 3:微信 4:IOS）</param>
        /// <param name="keyWord">关键词查询</param>
        /// <param name="page">当前页面</param>
        /// <param name="limit">每页多少天</param>
        /// <param name="field">排序字段</param>
        /// <param name="order">升序或者降序</param>
        /// <returns>Task<PageModel<SysLogUserLogin>></returns>
        Task<PageModel<SysLogException>> GetInfoList(int? loginSrc, string keyWord, int? page, int? limit, string field, string order);
    }
}
