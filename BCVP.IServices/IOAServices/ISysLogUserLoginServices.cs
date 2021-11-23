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
    /// 用户登录日志表接口
    /// </summary>
    public interface ISysLogUserLoginServices : IBaseServices<SysLogUserLogin>
    {
        /// <summary>
        /// 获取日志信息列表
        /// </summary>
        /// <param name="loginType">类型（0注册，1登录,2重置密码,3禁用）</param>
        /// <param name="loginStatus">登录状态（0失败，1成功）</param>
        /// <param name="loginSrc">登录来源（0:PC 1:webapp 2:App 3:微信 4:IOS）</param>
        /// <param name="keyWord">关键词查询</param>
        /// <param name="page">当前页面</param>
        /// <param name="limit">每页多少天</param>
        /// <param name="field">排序字段</param>
        /// <param name="order">升序或者降序</param>
        /// <returns>Task<PageModel<SysLogUserLogin>></returns>
        Task<PageModel<SysLogUserLogin>> GetInfoList(int? loginType, int? loginStatus, int? loginSrc, string keyWord, int? page, int? limit, string field, string order);
    }
}
