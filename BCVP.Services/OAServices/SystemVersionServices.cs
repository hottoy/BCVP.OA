using BCVP.IRepository.Base;
using BCVP.IServices.IOAServices;
using BCVP.Model;
using BCVP.Model.Models.OAModel;
using BCVP.Services.BASE;
using Fireasy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Services.OAServices
{
    /// <summary>
    /// 系统版本
    /// </summary>
    public class SystemVersionServices:BaseServices<SystemVersion>, ISystemVersionServices
    {
        IBaseRepository<SystemVersion> _dal;
        public SystemVersionServices(IBaseRepository<SystemVersion> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        /// <summary>
        /// 获取系统版本分页
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="keyWord">关键词</param>
        /// <param name="page">当前页面</param>
        /// <param name="limit">每页默认显示的数量</param>
        /// <param name="field">排序字段</param>
        /// <param name="order">排序desc asc</param>
        /// <returns>Task<PageModel<SystemVersion>></returns>
        public async Task<PageModel<SystemVersion>> GetInfoList(string keyWord, int? page, int? limit, string field, string order)
        {
            try
            {
                var sql = $@"SELECT SYSTEMVERSIONID,
                               SYSTEMVERSIONNAME,
                               SYSTEMVERSIONCODE,
                               ISVERSIONS,
                               ORDERSORT,
                               ISENABLED,
                               ISDELETED,
                               CREATETIME,
                               MODIFYTIME,
                               NVL(REMARK,' ') REMARK
                          FROM TB_SYSTEM_VERSION V
                         WHERE ISDELETED = 0
                           AND SYSTEMVERSIONNAME LIKE '%{keyWord}%'
                            OR SYSTEMVERSIONCODE LIKE '%{keyWord}%'
                        ORDER BY {field} {order}";
                var list = await _dal.SqlQueryAsync(Common.Helper.DBHelper.getDataPage(sql, page, limit));
                int dataCount = await _dal.GetIntAsync(Common.Helper.DBHelper.getCountSql(sql));
                return new PageModel<SystemVersion>()
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
        /// 添加或者修改
        /// </summary>
        /// <param name="Info">实体对象</param>
        /// <returns></returns>
        public async Task<bool> InsertOrEdit(SystemVersion Info)
        {
            if (Info!=null)
            {
                var list = await _dal.Query(s =>s.SystemVersionCode == Info.SystemVersionCode&&s.SystemVersionID!=Info.SystemVersionID&&s.IsDeleted==0);
                if (list.Count==0)
                {
                    Info.PyCode = Info.SystemVersionName.ToPinyin();
                    if (Info.SystemVersionID > 0)
                    {
                        return await _dal.Update(Info);
                    }
                    else
                    {
                        return await _dal.Add(Info) > 0;
                    }
                }
            }
            return false;
        }
    }
}
