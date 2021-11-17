using BCVP.Common;
using BCVP.Common.DB;
using BCVP.IRepository.Base;
using BCVP.IRepository.UnitOfWork;
using BCVP.Model;
using BCVP.Model.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BCVP.Repository.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private SqlSugarClient _dbBase;

        private ISqlSugarClient _db
        {
            get
            {
                /* 如果要开启多库支持，
                 * 1、在appsettings.json 中开启MutiDBEnabled节点为true，必填
                 * 2、设置一个主连接的数据库ID，节点MainDB，对应的连接字符串的Enabled也必须true，必填
                 */
                if (Appsettings.app(new string[] { "MutiDBEnabled" }).ObjToBool())
                {
                    if (typeof(TEntity).GetTypeInfo().GetCustomAttributes(typeof(SugarTable), true).FirstOrDefault((x => x.GetType() == typeof(SugarTable))) is SugarTable sugarTable && !string.IsNullOrEmpty(sugarTable.TableDescription))
                    {
                        _dbBase.ChangeDatabase(sugarTable.TableDescription.ToLower());
                    }
                    else
                    {
                        _dbBase.ChangeDatabase(MainDb.CurrentDbConnId.ToLower());
                    }
                }

                return _dbBase;
            }
        }

        public ISqlSugarClient Db
        {
            get { return _db; }
        }

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbBase = unitOfWork.GetDbClient();
        }



        public async Task<TEntity> QueryById(object objId)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().InSingle(objId));
            return await _db.Queryable<TEntity>().In(objId).SingleAsync();
        }
        /// <summary>
        /// 功能描述:根据ID查询一条数据
        /// 作　　者:BCVP
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="blnUseCache">是否使用缓存</param>
        /// <returns>数据实体</returns>
        public async Task<TEntity> QueryById(object objId, bool blnUseCache = false)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().WithCacheIF(blnUseCache).InSingle(objId));
            return await _db.Queryable<TEntity>().WithCacheIF(blnUseCache).In(objId).SingleAsync();
        }

        /// <summary>
        /// 功能描述:根据ID查询数据
        /// 作　　者:BCVP
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        public async Task<List<TEntity>> QueryByIDs(object[] lstIds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().In(lstIds).ToList());
            return await _db.Queryable<TEntity>().In(lstIds).ToListAsync();
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<int> Add(TEntity entity)
        {
            //var i = await Task.Run(() => _db.Insertable(entity).ExecuteReturnBigIdentity());
            ////返回的i是long类型,这里你可以根据你的业务需要进行处理
            //return (int)i;

            var insert = _db.Insertable(entity);
            
            //这里你可以返回TEntity，这样的话就可以获取id值，无论主键是什么类型
            //var return3 = await insert.ExecuteReturnEntityAsync();

            return await insert.ExecuteReturnIdentityAsync();
        }


        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>返回自增量列</returns>
        public async Task<int> Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            var insert = _db.Insertable(entity);
            if (insertColumns == null)
            {
                return await insert.ExecuteReturnIdentityAsync();
            }
            else
            {
                return await insert.InsertColumns(insertColumns).ExecuteReturnIdentityAsync();
            }
        }

        /// <summary>
        /// 批量插入实体(速度快)
        /// </summary>
        /// <param name="listEntity">实体集合</param>
        /// <returns>影响行数</returns>
        public async Task<int> Add(List<TEntity> listEntity)
        {
            return await _db.Insertable(listEntity.ToArray()).ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity)
        {
            ////这种方式会以主键为条件
            //var i = await Task.Run(() => _db.Updateable(entity).ExecuteCommand());
            //return i > 0;
            //这种方式会以主键为条件
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> Update(TEntity entity, string strWhere)
        {
            //return await Task.Run(() => _db.Updateable(entity).Where(strWhere).ExecuteCommand() > 0);
            return await _db.Updateable(entity).Where(strWhere).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> Update(string strSql, SugarParameter[] parameters = null)
        {
            //return await Task.Run(() => _db.Ado.ExecuteCommand(strSql, parameters) > 0);
            return await _db.Ado.ExecuteCommandAsync(strSql, parameters) > 0;
        }

        public async Task<bool> Update(object operateAnonymousObjects)
        {
            return await _db.Updateable<TEntity>(operateAnonymousObjects).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> Update(
          TEntity entity,
          List<string> lstColumns = null,
          List<string> lstIgnoreColumns = null,
          string strWhere = ""
            )
        {
            //IUpdateable<TEntity> up = await Task.Run(() => _db.Updateable(entity));
            //if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            //{
            //    up = await Task.Run(() => up.IgnoreColumns(it => lstIgnoreColumns.Contains(it)));
            //}
            //if (lstColumns != null && lstColumns.Count > 0)
            //{
            //    up = await Task.Run(() => up.UpdateColumns(it => lstColumns.Contains(it)));
            //}
            //if (!string.IsNullOrEmpty(strWhere))
            //{
            //    up = await Task.Run(() => up.Where(strWhere));
            //}
            //return await Task.Run(() => up.ExecuteCommand()) > 0;

            IUpdateable<TEntity> up = _db.Updateable(entity);
            if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            {
                up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
            }
            if (lstColumns != null && lstColumns.Count > 0)
            {
                up = up.UpdateColumns(lstColumns.ToArray());
            }
            if (!string.IsNullOrEmpty(strWhere))
            {
                up = up.Where(strWhere);
            }
            return await up.ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Delete(TEntity entity)
        {
            //var i = await Task.Run(() => _db.Deleteable(entity).ExecuteCommand());
            //return i > 0;
            return await _db.Deleteable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<bool> DeleteById(object id)
        {
            //var i = await Task.Run(() => _db.Deleteable<TEntity>(id).ExecuteCommand());
            //return i > 0;
            return await _db.Deleteable<TEntity>(id).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIds(object[] ids)
        {
            //var i = await Task.Run(() => _db.Deleteable<TEntity>().In(ids).ExecuteCommand());
            //return i > 0;
            return await _db.Deleteable<TEntity>().In(ids).ExecuteCommandHasChangeAsync();
        }



        /// <summary>
        /// 功能描述:查询所有数据
        /// 作　　者:BCVP
        /// </summary>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query()
        {
            return await _db.Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// 作　　者:BCVP
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
            return await _db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// 作　　者:BCVP
        /// </summary>
        /// <param name="whereExpression">whereExpression</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:按照特定列查询数据列表
        /// 作　　者:BCVP
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            return await _db.Queryable<TEntity>().Select(expression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:按照特定列查询数据列表带条件排序
        /// 作　　者:BCVP
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="whereExpression">过滤条件</param>
        /// <param name="expression">查询实体条件</param>
        /// <param name="strOrderByFileds">排序条件</param>
        /// <returns></returns>
        public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Select(expression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// 作　　者:BCVP
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToList());
            return await _db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).OrderByIF(strOrderByFileds != null, strOrderByFileds).ToListAsync();
        }
        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToList());
            return await _db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// 作　　者:BCVP
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
        }


        /// <summary>
        /// 功能描述:查询前N条数据
        /// 作　　者:BCVP
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int intTop,
            string strOrderByFileds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToList());
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询前N条数据
        /// 作　　者:BCVP
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            string strWhere,
            int intTop,
            string strOrderByFileds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).Take(intTop).ToList());
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).Take(intTop).ToListAsync();
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="strSql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>泛型集合</returns>
        public async Task<List<TEntity>> QuerySql(string strSql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.SqlQueryAsync<TEntity>(strSql, parameters);
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="strSql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public async Task<DataTable> QueryTable(string strSql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.GetDataTableAsync(strSql, parameters);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// 作　　者:BCVP
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int intPageIndex,
            int intPageSize,
            string strOrderByFileds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToPageList(intPageIndex, intPageSize));
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToPageListAsync(intPageIndex, intPageSize);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// 作　　者:BCVP
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
          string strWhere,
          int intPageIndex,
          int intPageSize,

          string strOrderByFileds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToPageList(intPageIndex, intPageSize));
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToPageListAsync(intPageIndex, intPageSize);
        }



        /// <summary>
        /// 分页查询[使用版本，其他分页未测试]
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns></returns>
        public async Task<PageModel<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null)
        {

            RefAsync<int> totalCount = 0;
            var list = await _db.Queryable<TEntity>()
             .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
             .WhereIF(whereExpression != null, whereExpression)
             .ToPageListAsync(intPageIndex, intPageSize, totalCount);

            int pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / intPageSize.ObjToDecimal())).ObjToInt();
            return new PageModel<TEntity>() { dataCount = totalCount, pageCount = pageCount, page = intPageIndex, PageSize = intPageSize, data = list };
        }


        /// <summary> 
        ///查询-多表查询
        /// </summary> 
        /// <typeparam name="T">实体1</typeparam> 
        /// <typeparam name="T2">实体2</typeparam> 
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param> 
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param> 
        /// <returns>值</returns>
        public async Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null)
            {
                return await _db.Queryable(joinExpression).Select(selectExpression).ToListAsync();
            }
            return await _db.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).ToListAsync();
        }


        /// <summary>
        /// 两表联合查询-分页
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体1</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式</param>
        /// <param name="selectExpression">返回表达式</param>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="intPageIndex">页码</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="strOrderByFileds">排序字段</param>
        /// <returns></returns>
        public async Task<PageModel<TResult>> QueryTabsPage<T, T2, TResult>(
            Expression<Func<T, T2, object[]>> joinExpression,
            Expression<Func<T, T2, TResult>> selectExpression,
            Expression<Func<TResult, bool>> whereExpression,
            int intPageIndex = 1,
            int intPageSize = 20,
            string strOrderByFileds = null)
        {

            RefAsync<int> totalCount = 0;
            var list = await _db.Queryable<T, T2>(joinExpression)
             .Select(selectExpression)
             .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
             .WhereIF(whereExpression != null, whereExpression)
             .ToPageListAsync(intPageIndex, intPageSize, totalCount);
            int pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / intPageSize.ObjToDecimal())).ObjToInt();
            return new PageModel<TResult>() { dataCount = totalCount, pageCount = pageCount, page = intPageIndex, PageSize = intPageSize, data = list };
        }

        /// <summary>
        /// 两表联合查询-分页-分组
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体1</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式</param>
        /// <param name="selectExpression">返回表达式</param>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="intPageIndex">页码</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="strOrderByFileds">排序字段</param>
        /// <returns></returns>
        public async Task<PageModel<TResult>> QueryTabsPage<T, T2, TResult>(
            Expression<Func<T, T2, object[]>> joinExpression,
            Expression<Func<T, T2, TResult>> selectExpression,
            Expression<Func<TResult, bool>> whereExpression,
            Expression<Func<T, object>> groupExpression,
            int intPageIndex = 1,
            int intPageSize = 20,
            string strOrderByFileds = null)
        {

            RefAsync<int> totalCount = 0;
            var list = await _db.Queryable<T, T2>(joinExpression).GroupBy(groupExpression)
             .Select(selectExpression)
             .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
             .WhereIF(whereExpression != null, whereExpression)
             .ToPageListAsync(intPageIndex, intPageSize, totalCount);
            int pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / intPageSize.ObjToDecimal())).ObjToInt();
            return new PageModel<TResult>() { dataCount = totalCount, pageCount = pageCount, page = intPageIndex, PageSize = intPageSize, data = list };
        }

        //var exp = Expressionable.Create<ProjectToUser>()
        //        .And(s => s.tdIsDelete != true)
        //        .And(p => p.IsDeleted != true)
        //        .And(p => p.pmId != null)
        //        .AndIF(!string.IsNullOrEmpty(model.paramCode1), (s) => s.uID == model.paramCode1.ObjToInt())
        //                .AndIF(!string.IsNullOrEmpty(model.searchText), (s) => (s.groupName != null && s.groupName.Contains(model.searchText))
        //                        || (s.jobName != null && s.jobName.Contains(model.searchText))
        //                        || (s.uRealName != null && s.uRealName.Contains(model.searchText)))
        //                .ToExpression();//拼接表达式
        //var data = await _projectMemberServices.QueryTabsPage<sysUserInfo, ProjectMember, ProjectToUser>(
        //    (s, p) => new object[] { JoinType.Left, s.uID == p.uId },
        //    (s, p) => new ProjectToUser
        //    {
        //        uID = s.uID,
        //        uRealName = s.uRealName,
        //        groupName = s.groupName,
        //        jobName = s.jobName
        //    }, exp, s => new { s.uID, s.uRealName, s.groupName, s.jobName }, model.currentPage, model.pageSize, model.orderField + " " + model.orderType);
        #region 扩展ExecuteCommand返回受影响行数，一般用于增删改SQL 

        //public DataTable QueryTable(string strSql, SugarParameter[] parameters = null)
        //{
        //    //            var t12 = _db.SqlQueryable<dynamic>("select * from sysUserInfo").ToPageList(1, 2);
        //    //            //var t = _db.Ado.GetDataTable("select * from sysUserInfo where uStatus=0 ", new { uStatus = 0});
        //    //            var dt = _db.Ado.GetDataTable("select * from sysUserInfo where uStatus=@uStatus", new List<SugarParameter>(){
        //    //  new SugarParameter("@uStatus",0)
        //    //});
        //    return _db.Ado.GetDataTable(strSql, parameters);
        //}

        /// <summary>
        /// 根据字典创建sqlSugar参数集合
        /// </summary>
        /// <param name="dicPara"></param>
        /// <param name="isReturnNull"></param>
        /// <returns></returns>
        public static List<SugarParameter> CreateSugarParameterList(Dictionary<string, object> dicPara, bool isReturnNull = true)
        {
            if (dicPara == null || dicPara.Count <= 0) return null;
            return dicPara.Select(r => new SugarParameter(r.Key, r.Value)).ToList();
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="strSql">完整的sql语句</param>
        /// <param name="parameters">SugarParameter参数</param>
        /// <returns></returns>
        public DataTable QueryTable(string strSql, Dictionary<string, object> dicParameters = null)
        {
            if (dicParameters == null || dicParameters.Count <= 0)
            {
                return _db.Ado.GetDataTable(strSql);
            }
            else
            {
                var paraList = CreateSugarParameterList(dicParameters);
                return _db.Ado.GetDataTable(strSql, paraList);
            }
        }

        /// <summary>
        /// 单条返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteCommand(string sql, object parameters)
        {
            return _db.Ado.ExecuteCommand(sql, parameters);
        }

        /// <summary>
        /// 单条返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteCommand(string sql, SugarParameter[] parameters = null)
        {
            return _db.Ado.ExecuteCommand(sql, parameters);
        }

        public bool ExecuteCommandTransaction(Dictionary<string, SugarParameter[]> dictionary)
        {
            var flg = false;
            try
            {

                if (_db.Ado.Transaction == null)
                {
                    _db.Ado.BeginTran(IsolationLevel.ReadCommitted);
                }
                foreach (var item in dictionary)
                {
                    _db.Ado.ExecuteCommand(item.Key.ToLower(), item.Value);
                    flg = true;
                }
                _db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                flg = false;
                _db.Ado.RollbackTran();
                throw new Exception(ex.Message);
            }
            finally
            {
                _db.Ado.Dispose();
                _db.Dispose();
            }
            return flg;
        }



        /// <summary>
        /// 批量返回受影响行数，增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteCommand(string sql, List<SugarParameter> parameters)
        {
            return _db.Ado.ExecuteCommand(sql, parameters);
        }

        /// <summary>
        /// Task 单条返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteCommandAsync(string sql, object parameters)
        {
            return await _db.Ado.ExecuteCommandAsync(sql, parameters);
        }

        /// <summary>
        /// Task 单条返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteCommandAsync(string sql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.ExecuteCommandAsync(sql, parameters);
        }

        /// <summary>
        /// Task 批量返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteCommandAsync(string sql, List<SugarParameter> parameters)
        {
            return await _db.Ado.ExecuteCommandAsync(sql, parameters);
        }
        #endregion

        #region SQL 查询

        /// <summary>
        /// 获取首行首列string
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetString(string sql, object parameters)
        {
            return _db.Ado.GetString(sql, parameters);
        }

        /// <summary>
        /// 获取首行首列string
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetString(string sql, SugarParameter[] parameters = null)
        {
            return _db.Ado.GetString(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列string
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetString(string sql, List<SugarParameter> parameters)
        {
            return _db.Ado.GetString(sql, parameters);
        }

        /// <summary>
        /// 获取首行首列Asyncstring
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(string sql, object parameters)
        {
            return await _db.Ado.GetStringAsync(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列Asyncstring
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(string sql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.GetStringAsync(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列Asyncstring
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(string sql, List<SugarParameter> parameters)
        {
            return await _db.Ado.GetStringAsync(sql, parameters);
        }

        /// <summary>
        /// 获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int GetInt(string sql, object parameters)
        {
            return _db.Ado.GetInt(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public int GetInt(string sql, SugarParameter[] parameters = null)
        {
            return _db.Ado.GetInt(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public int GetInt(string sql, List<SugarParameter> parameters)
        {
            return _db.Ado.GetInt(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public async Task<int> GetIntAsync(string sql, object parameters)
        {
            return await _db.Ado.GetIntAsync(sql, parameters);
        }
        /// <summary>
        /// Async获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public async Task<int> GetIntAsync(string sql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.GetIntAsync(sql, parameters);
        }
        /// <summary>
        /// Async获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public async Task<int> GetIntAsync(string sql, List<SugarParameter> parameters)
        {
            return await _db.Ado.GetIntAsync(sql, parameters);
        }

        /// <summary>
        /// 获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Double GetDouble(string sql, object parameters)
        {
            return _db.Ado.GetDouble(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public Double GetDouble(string sql, SugarParameter[] parameters = null)
        {
            return _db.Ado.GetDouble(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public Double GetDouble(string sql, List<SugarParameter> parameters)
        {
            return _db.Ado.GetDouble(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<Double> GetDoubleAsync(string sql, object parameters)
        {
            return await _db.Ado.GetDoubleAsync(sql, parameters);
        }
        /// <summary>
        /// Async获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public async Task<Double> GetDoubleAsync(string sql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.GetDoubleAsync(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public async Task<Double> GetDoubleAsync(string sql, List<SugarParameter> parameters)
        {
            return await _db.Ado.GetDoubleAsync(sql, parameters);
        }

        /// <summary>
        /// 获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Decimal GetDecimal(string sql, object parameters)
        {
            return _db.Ado.GetDecimal(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Decimal GetDecimal(string sql, SugarParameter[] parameters = null)
        {
            return _db.Ado.GetDecimal(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Decimal GetDecimal(string sql, List<SugarParameter> parameters)
        {
            return _db.Ado.GetDecimal(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<Decimal> GetDecimalAsync(string sql, object parameters)
        {
            return await _db.Ado.GetDecimalAsync(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<Decimal> GetDecimalAsync(string sql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.GetDecimalAsync(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<Decimal> GetDecimalAsync(string sql, List<SugarParameter> parameters)
        {
            return await _db.Ado.GetDecimalAsync(sql, parameters);
        }

        /// <summary>
        /// 获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string sql, object parameters)
        {
            return _db.Ado.GetDateTime(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string sql, SugarParameter[] parameters = null)
        {
            return _db.Ado.GetDateTime(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string sql, List<SugarParameter> parameters)
        {
            return _db.Ado.GetDateTime(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DateTime> GetDateTimeAsync(string sql, object parameters)
        {
            return await _db.Ado.GetDateTimeAsync(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DateTime> GetDateTimeAsync(string sql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.GetDateTimeAsync(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DateTime> GetDateTimeAsync(string sql, List<SugarParameter> parameters)
        {
            return await _db.Ado.GetDateTimeAsync(sql, parameters);
        }

        /// <summary>
        /// 查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<TEntity> SqlQuery(string sql, object parameters)
        {
            return _db.Ado.SqlQuery<TEntity>(sql, parameters);
        }
        /// <summary>
        /// 查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<TEntity> SqlQuery(string sql, SugarParameter[] parameters = null)
        {
            return _db.Ado.SqlQuery<TEntity>(sql, parameters);
        }

        /// <summary>
        /// 查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<TEntity> SqlQuery(string sql, List<SugarParameter> parameters)
        {
            return _db.Ado.SqlQuery<TEntity>(sql, parameters);
        }


        /// <summary>
        /// Async查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> SqlQueryAsync(string sql, object parameters)
        {
            return await _db.Ado.SqlQueryAsync<TEntity>(sql, parameters);
        }
        /// <summary>
        /// Async查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> SqlQueryAsync(string sql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.SqlQueryAsync<TEntity>(sql, parameters);
        }
        /// <summary>
        /// Async查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> SqlQueryAsync(string sql, List<SugarParameter> parameters)
        {
            return await _db.Ado.SqlQueryAsync<TEntity>(sql, parameters);
        }

        /// <summary>
        /// 查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public TEntity SqlQuerySingle(string sql, object parameters)
        {
            return _db.Ado.SqlQuerySingle<TEntity>(sql, parameters);
        }
        /// <summary>
        /// 查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public TEntity SqlQuerySingle(string sql, SugarParameter[] parameters = null)
        {
            return _db.Ado.SqlQuerySingle<TEntity>(sql, parameters);
        }
        /// <summary>
        /// 查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public TEntity SqlQuerySingle(string sql, List<SugarParameter> parameters)
        {
            return _db.Ado.SqlQuerySingle<TEntity>(sql, parameters);
        }
        /// <summary>
        /// Async查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TEntity> SqlQuerySingleAsync(string sql, object parameters)
        {
            return await _db.Ado.SqlQuerySingleAsync<TEntity>(sql, parameters);
        }

        /// <summary>
        /// Async查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TEntity> SqlQuerySingleAsync(string sql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.SqlQuerySingleAsync<TEntity>(sql, parameters);
        }
        /// <summary>
        /// Async查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TEntity> SqlQuerySingleAsync(string sql, List<SugarParameter> parameters)
        {
            return await _db.Ado.SqlQuerySingleAsync<TEntity>(sql, parameters);
        }

        #region 存储过程
        /// <summary>
        /// 存储过程获取一个集合
        /// </summary>
        /// <param name="sp_sql">sp_sql</param>
        /// <param name="parameters">SugarParameter参数集合</param>
        /// <returns></returns>
        public int ExecuteProcedure(string sp_sql, SugarParameter[] parameters)
        {
            var t = " ";
            foreach (var item in parameters)
            {
                t = t + item.ParameterName.ToString() + ",";
            }
            //sp_sql= sp_sql+t.Substring(0,t.Length-1);
            return _db.Ado.ExecuteCommand("exec " + sp_sql + t.TrimEnd(',') + " ", parameters);
        }
        #endregion

        #endregion
    }

}
