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

namespace BCVP.Services.OAServices
{
    public class SysLogExceptionServices:BaseServices<SysLogException>, ISysLogExceptionServices
    {
        IBaseRepository<SysLogException> _dal;

        public SysLogExceptionServices(IBaseRepository<SysLogException> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

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
        public async Task<PageModel<SysLogException>> GetInfoList(int? loginSrc, string keyWord, int? page, int? limit, string field, string order)
        {
            try
            {
                string sqlWhere = "";
                if (loginSrc != null)
                {
                    sqlWhere = sqlWhere + " AND LOGIN_SRC =" + loginSrc + " ";
                }

                var sql = $@"SELECT EXC_ID,
       EXC_REQU_PARAM,
       EXC_NAME,
       EXC_MESSAGE,
       OPER_USER_ID,
       OPER_USER_NAME,
       CONTROLLERNAME,
       OPER_METHOD,
       OPER_URL,
       OPER_IP,
       OPER_CREATETIME,
       OPER_VERSIONS,
       LOGIN_SRC
  FROM TB_SYS_LOG_EXCEPTION 
 WHERE (EXC_NAME LIKE '%{keyWord}%'
    OR EXC_MESSAGE LIKE '%{keyWord}%'
    OR OPER_USER_NAME LIKE '%{keyWord}%'
    OR CONTROLLERNAME LIKE '%{keyWord}%'
    OR OPER_METHOD LIKE '%{keyWord}%')
   {sqlWhere} ORDER BY {field} {order}";
                var list = await _dal.SqlQueryAsync(Common.Helper.DBHelper.getDataPage(sql, page, limit));
                int dataCount = await _dal.GetIntAsync(Common.Helper.DBHelper.getCountSql(sql));
                return new PageModel<SysLogException>()
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
