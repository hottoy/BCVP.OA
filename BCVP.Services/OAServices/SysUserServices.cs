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
    /// <summary>
    /// 系统用户接口实现
    /// </summary>
    public class SysUserServices : BaseServices<SysUser>, ISysUserServices
    {
        IBaseRepository<SysUser> _dal;
        public SysUserServices(IBaseRepository<SysUser> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        /// <summary>
        /// 根据机构ID获取对应的角色
        /// </summary>
        /// <param name="OrgID">机构OrgID</param>
        /// <returns></returns>
        public async Task<PageModel<SysUser>> GetInfoList(int OrgID, string userName, string userAccount, int? page, int? limit, string field, string order)
        {
            try
            {
                var sql = $@"SELECT SU.USERID,
       SU.ORGID,
       O.NAMES ORGNAME,
       SU.USERNAME,
       SU.USERACCOUNT,
       SU.MOBILE,
       SU.TELEPHONE,
       SU.EMAIL,
       DECODE(SU.SEX, 0, '女', 1, '男') SexName,
       SU.CREATETIME,
       SU.REMARK,
       E.EDUCATIONNAME,
       P.PROFESSIONANAME,
       (SELECT WM_CONCAT(R.NAMES) ne
          FROM TB_SYS_USERROLE UR
          LEFT JOIN TB_SYS_ROLE R
            ON R.ROLEID = UR.ROLEID
         WHERE UR.USERID = SU.USERID) ROLENAME
  FROM TB_SYS_USER SU
  LEFT JOIN TB_SYS_PROFESSIONA P
    ON P.PROFESSIONAID = SU.PROFESSIONALID
  LEFT JOIN TB_SYS_EDUCATION E
    ON E.EDUCATIONID = SU.EDUCATIONID
  LEFT JOIN TB_SYS_ORG O
    ON O.ORGID = SU.ORGID
 WHERE SU.ISDELETE = 0 AND SU.ORGID={OrgID} AND SU.USERNAME LIKE'%{userName}%' AND SU.USERACCOUNT LIKE'%{userAccount}%'
ORDER BY {field} {order}";
                var list = await _dal.SqlQueryAsync(Common.Helper.DBHelper.getDataPage(sql, page, limit));
                int dataCount = await _dal.GetIntAsync(Common.Helper.DBHelper.getCountSql(sql));
                return new PageModel<SysUser>()
                {
                    data = list,
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

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<bool> insertOrUpdate(SysUser info)
        {
            var flg = false;
            try
            {
                if (info != null)
                {
                    if (info.UserID == 0)
                    {
                        var res = await _dal.GetIntAsync($@"SELECT COUNT(*) FROM TB_SYS_USER U WHERE U.ISDELETE=0 AND U.USERACCOUNT='{info.UserAccount}'");
                        if (res == 0)
                        {
                            flg = await _dal.Add(info) > 0;
                        }
                    }
                    else
                    {
                        var res = await _dal.GetIntAsync($@"SELECT COUNT(*) FROM TB_SYS_USER U WHERE U.ISDELETE=0 AND U.USERACCOUNT='{info.UserAccount}'  AND U.USERID<>{info.UserID}");
                        if (res == 0)
                        {
                            flg = await _dal.Update(info);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return flg;
        }
    }
}
