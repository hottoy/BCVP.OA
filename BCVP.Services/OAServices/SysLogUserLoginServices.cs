using BCVP.IRepository.Base;
using BCVP.IServices.IOAServices;
using BCVP.Model;
using BCVP.Model.Models.OAModel;
using BCVP.Services.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BCVP.Model.Enums;

namespace BCVP.Services.OAServices
{
    /// <summary>
    /// 用户登录日志表接口实现
    /// </summary>
    public class SysLogUserLoginServices : BaseServices<SysLogUserLogin>, ISysLogUserLoginServices
    {

        IBaseRepository<SysLogUserLogin> _dal;
        public SysLogUserLoginServices(IBaseRepository<SysLogUserLogin> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

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
        public async Task<PageModel<SysLogUserLogin>> GetInfoList(int? loginType, int? loginStatus, int? loginSrc, string keyWord, int? page, int? limit, string field, string order)
        {
            try
            {
                string sqlWhere = "";
                if (loginType != null)
                {
                    sqlWhere = "AND L.LOGIN_TYPE =" + loginType + "";
                }
                if (loginStatus != null)
                {
                    sqlWhere = sqlWhere + " AND L.LOGIN_STATUS = " + loginStatus + " ";
                }
                if (loginSrc != null)
                {
                    sqlWhere = sqlWhere + " AND L.LOGIN_SRC =" + loginSrc + " ";
                }

                var sql = $@"SELECT L.LOGIN_ID,
       L.LOGIN_TYPE,
       L.LOGIN_USERID,
       L.LOGIN_STATUS,
       L.LOGIN_IP,
       L.LOGIN_SRC,
       L.LOGIN_MESSAGE,
       L.LOGIN_CREATETIME,
       U.CREATETIME,
       U.USERNAME,
       U.USERACCOUNT
  FROM TB_SYS_LOG_USERLOGIN L
  LEFT JOIN TB_SYS_USER U
    ON U.USERID = L.LOGIN_USERID
   {sqlWhere}
 WHERE (L.LOGIN_MESSAGE LIKE '%{keyWord}%' OR U.USERNAME LIKE '%{keyWord}%' OR U.USERACCOUNT LIKE '%{keyWord}%')
   {sqlWhere} ORDER BY {field} {order}";
                var list = await _dal.SqlQueryAsync(Common.Helper.DBHelper.getDataPage(sql, page, limit));
                int dataCount = await _dal.GetIntAsync(Common.Helper.DBHelper.getCountSql(sql));
                return new PageModel<SysLogUserLogin>()
                {
                    data = (list.Count == 0 ? null : list),
                    dataCount = dataCount,
                    page = Convert.ToInt32(page),
                    PageSize = Convert.ToInt32(limit)
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

    }
}
