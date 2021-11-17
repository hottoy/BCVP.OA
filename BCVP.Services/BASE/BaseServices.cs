using BCVP.IRepository.Base;
using BCVP.IServices.BASE;
using BCVP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BCVP.Services.BASE
{
    public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : class, new()
    {
        //public IBaseRepository<TEntity> baseDal = new BaseRepository<TEntity>();
        public IBaseRepository<TEntity> BaseDal { get; set; }//通过在子类的构造函数中注入，这里是基类，不用构造函数

        public async Task<TEntity> QueryById(object objId)
        {
            return await BaseDal.QueryById(objId);
        }
        /// <summary>
        /// 功能描述:根据ID查询一条数据
        /// 作　　者:AZLinli.BCVP
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="blnUseCache">是否使用缓存</param>
        /// <returns>数据实体</returns>
        public async Task<TEntity> QueryById(object objId, bool blnUseCache = false)
        {
            return await BaseDal.QueryById(objId, blnUseCache);
        }

        /// <summary>
        /// 功能描述:根据ID查询数据
        /// 作　　者:AZLinli.BCVP
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        public async Task<List<TEntity>> QueryByIDs(object[] lstIds)
        {
            return await BaseDal.QueryByIDs(lstIds);
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<int> Add(TEntity entity)
        {
            return await BaseDal.Add(entity);
        }

        /// <summary>
        /// 批量插入实体(速度快)
        /// </summary>
        /// <param name="listEntity">实体集合</param>
        /// <returns>影响行数</returns>
        public async Task<int> Add(List<TEntity> listEntity)
        {
            return await BaseDal.Add(listEntity);
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity)
        {
            return await BaseDal.Update(entity);
        }
        public async Task<bool> Update(TEntity entity, string strWhere)
        {
            return await BaseDal.Update(entity, strWhere);
        }
        public async Task<bool> Update(object operateAnonymousObjects)
        {
            return await BaseDal.Update(operateAnonymousObjects);
        }

        public async Task<bool> Update(
         TEntity entity,
         List<string> lstColumns = null,
         List<string> lstIgnoreColumns = null,
         string strWhere = ""
            )
        {
            return await BaseDal.Update(entity, lstColumns, lstIgnoreColumns, strWhere);
        }


        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Delete(TEntity entity)
        {
            return await BaseDal.Delete(entity);
        }

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<bool> DeleteById(object id)
        {
            return await BaseDal.DeleteById(id);
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIds(object[] ids)
        {
            return await BaseDal.DeleteByIds(ids);
        }



        /// <summary>
        /// 功能描述:查询所有数据
        /// 作　　者:AZLinli.BCVP
        /// </summary>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query()
        {
            return await BaseDal.Query();
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// 作　　者:AZLinli.BCVP
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere)
        {
            return await BaseDal.Query(strWhere);
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// 作　　者:AZLinli.BCVP
        /// </summary>
        /// <param name="whereExpression">whereExpression</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await BaseDal.Query(whereExpression);
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
            return await BaseDal.Query(expression);
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
        public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression,string strOrderByFileds)
        {
            return await BaseDal.Query(expression, whereExpression, strOrderByFileds);
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// 作　　者:AZLinli.BCVP
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await BaseDal.Query(whereExpression, orderByExpression, isAsc);
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return await BaseDal.Query(whereExpression, strOrderByFileds);
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// 作　　者:AZLinli.BCVP
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
        {
            return await BaseDal.Query(strWhere, strOrderByFileds);
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="strSql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>泛型集合</returns>
        public async Task<List<TEntity>> QuerySql(string strSql, SugarParameter[] parameters = null)
        {
            return await BaseDal.QuerySql(strSql, parameters);

        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="strSql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public async Task<DataTable> QueryTable(string strSql, SugarParameter[] parameters = null)
        {
            return await BaseDal.QueryTable(strSql, parameters);

        }
        /// <summary>
        /// 功能描述:查询前N条数据
        /// 作　　者:AZLinli.BCVP
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds)
        {
            return await BaseDal.Query(whereExpression, intTop, strOrderByFileds);
        }

        /// <summary>
        /// 功能描述:查询前N条数据
        /// 作　　者:AZLinli.BCVP
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
            return await BaseDal.Query(strWhere, intTop, strOrderByFileds);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// 作　　者:AZLinli.BCVP
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
            return await BaseDal.Query(
              whereExpression,
              intPageIndex,
              intPageSize,
              strOrderByFileds);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// 作　　者:AZLinli.BCVP
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
            return await BaseDal.Query(
            strWhere,
            intPageIndex,
            intPageSize,
            strOrderByFileds);
        }

        public async Task<PageModel<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression,
        int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null)
        {
            return await BaseDal.QueryPage(whereExpression,
         intPageIndex, intPageSize, strOrderByFileds);
        }

        public async Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(Expression<Func<T, T2, T3, object[]>> joinExpression, Expression<Func<T, T2, T3, TResult>> selectExpression, Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new()
        {
            return await BaseDal.QueryMuch(joinExpression, selectExpression, whereLambda);
        }

        #region 扩展ExecuteCommand返回受影响行数，一般用于增删改SQL 

        /// <summary>
        /// 单条返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteCommand(string sql, object parameters)
        {
            return BaseDal.ExecuteCommand(sql, parameters);
        }

        /// <summary>
        /// 单条返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteCommand(string sql, SugarParameter[] parameters = null)
        {
            return BaseDal.ExecuteCommand(sql, parameters);
        }

        /// <summary>
        /// 批量返回受影响行数，增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteCommand(string sql, List<SugarParameter> parameters)
        {
            return BaseDal.ExecuteCommand(sql, parameters);
        }

        /// <summary>
        /// Task 单条返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteCommandAsync(string sql, object parameters)
        {
            return await BaseDal.ExecuteCommandAsync(sql, parameters);
        }

        /// <summary>
        /// Task 单条返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteCommandAsync(string sql, SugarParameter[] parameters = null)
        {
            return await BaseDal.ExecuteCommandAsync(sql, parameters);
        }

        /// <summary>
        /// Task 批量返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteCommandAsync(string sql, List<SugarParameter> parameters)
        {
            return await BaseDal.ExecuteCommandAsync(sql, parameters);
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
            return BaseDal.GetString(sql, parameters);
        }

        /// <summary>
        /// 获取首行首列string
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetString(string sql, SugarParameter[] parameters = null)
        {
            return BaseDal.GetString(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列string
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetString(string sql, List<SugarParameter> parameters)
        {
            return BaseDal.GetString(sql, parameters);
        }

        /// <summary>
        /// 获取首行首列Asyncstring
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(string sql, object parameters)
        {
            return await BaseDal.GetStringAsync(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列Asyncstring
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(string sql, SugarParameter[] parameters = null)
        {
            return await BaseDal.GetStringAsync(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列Asyncstring
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(string sql, List<SugarParameter> parameters)
        {
            return await BaseDal.GetStringAsync(sql, parameters);
        }

        /// <summary>
        /// 获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public int GetInt(string sql, object pars)
        {
            return BaseDal.GetInt(sql, pars);
        }
        /// <summary>
        /// 获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public int GetInt(string sql, SugarParameter[] parameters = null)
        {
            return BaseDal.GetInt(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public int GetInt(string sql, List<SugarParameter> parameters)
        {
            return BaseDal.GetInt(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public async Task<int> GetIntAsync(string sql, object parameters)
        {
            return await BaseDal.GetIntAsync(sql, parameters);
        }
        /// <summary>
        /// Async获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public async Task<int> GetIntAsync(string sql, SugarParameter[] parameters = null)
        {
            return await BaseDal.GetIntAsync(sql, parameters);
        }
        /// <summary>
        /// Async获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public async Task<int> GetIntAsync(string sql, List<SugarParameter> parameters)
        {
            return await BaseDal.GetIntAsync(sql, parameters);
        }

        /// <summary>
        /// 获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Double GetDouble(string sql, object parameters)
        {
            return BaseDal.GetDouble(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public Double GetDouble(string sql, SugarParameter[] parameters = null)
        {
            return BaseDal.GetDouble(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public Double GetDouble(string sql, List<SugarParameter> parameters)
        {
            return BaseDal.GetDouble(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<Double> GetDoubleAsync(string sql, object parameters)
        {
            return await BaseDal.GetDoubleAsync(sql, parameters);
        }
        /// <summary>
        /// Async获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public async Task<Double> GetDoubleAsync(string sql, SugarParameter[] parameters = null)
        {
            return await BaseDal.GetDoubleAsync(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public async Task<Double> GetDoubleAsync(string sql, List<SugarParameter> parameters)
        {
            return await BaseDal.GetDoubleAsync(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Decimal GetDecimal(string sql, object parameters)
        {
            return BaseDal.GetDecimal(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Decimal GetDecimal(string sql, SugarParameter[] parameters = null)
        {
            return BaseDal.GetDecimal(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Decimal GetDecimal(string sql, List<SugarParameter> parameters)
        {
            return BaseDal.GetDecimal(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<Decimal> GetDecimalAsync(string sql, object parameters)
        {
            return await BaseDal.GetDecimalAsync(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<Decimal> GetDecimalAsync(string sql, SugarParameter[] parameters = null)
        {
            return await BaseDal.GetDecimalAsync(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<Decimal> GetDecimalAsync(string sql, List<SugarParameter> parameters)
        {
            return await BaseDal.GetDecimalAsync(sql, parameters);
        }

        /// <summary>
        /// 获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string sql, object parameters)
        {
            return BaseDal.GetDateTime(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string sql, SugarParameter[] parameters = null)
        {
            return BaseDal.GetDateTime(sql, parameters);
        }
        /// <summary>
        /// 获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string sql, List<SugarParameter> parameters)
        {
            return BaseDal.GetDateTime(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DateTime> GetDateTimeAsync(string sql, object parameters)
        {
            return await BaseDal.GetDateTimeAsync(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DateTime> GetDateTimeAsync(string sql, SugarParameter[] parameters = null)
        {
            return await BaseDal.GetDateTimeAsync(sql, parameters);
        }

        /// <summary>
        /// Async获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DateTime> GetDateTimeAsync(string sql, List<SugarParameter> parameters)
        {
            return await BaseDal.GetDateTimeAsync(sql, parameters);
        }

        /// <summary>
        /// 查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<TEntity> SqlQuery(string sql, object parameters)
        {
            return BaseDal.SqlQuery(sql, parameters);
        }
        /// <summary>
        /// 查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<TEntity> SqlQuery(string sql, SugarParameter[] parameters = null)
        {
            return BaseDal.SqlQuery(sql, parameters);
        }

        /// <summary>
        /// 查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<TEntity> SqlQuery(string sql, List<SugarParameter> parameters)
        {
            return BaseDal.SqlQuery(sql, parameters);
        }

        /// <summary>
        /// Async查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> SqlQueryAsync(string sql, object parameters)
        {
            return await BaseDal.SqlQueryAsync(sql, parameters);
        }
        /// <summary>
        /// Async查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> SqlQueryAsync(string sql, SugarParameter[] parameters = null)
        {
            return await BaseDal.SqlQueryAsync(sql, parameters);
        }
        /// <summary>
        /// Async查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> SqlQueryAsync(string sql, List<SugarParameter> parameters)
        {
            return await BaseDal.SqlQueryAsync(sql, parameters);
        }

        /// <summary>
        /// 查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public TEntity SqlQuerySingle(string sql, object parameters)
        {
            return BaseDal.SqlQuerySingle(sql, parameters);
        }
        /// <summary>
        /// 查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public TEntity SqlQuerySingle(string sql, SugarParameter[] parameters = null)
        {
            return BaseDal.SqlQuerySingle(sql, parameters);
        }
        /// <summary>
        /// 查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public TEntity SqlQuerySingle(string sql, List<SugarParameter> parameters)
        {
            return BaseDal.SqlQuerySingle(sql, parameters);
        }
        /// <summary>
        /// Async查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TEntity> SqlQuerySingleAsync(string sql, object parameters)
        {
            return await BaseDal.SqlQuerySingleAsync(sql, parameters);
        }

        /// <summary>
        /// Async查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TEntity> SqlQuerySingleAsync(string sql, SugarParameter[] parameters = null)
        {
            return await BaseDal.SqlQuerySingleAsync(sql, parameters);
        }
        /// <summary>
        /// Async查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TEntity> SqlQuerySingleAsync(string sql, List<SugarParameter> parameters)
        {
            return await BaseDal.SqlQuerySingleAsync(sql, parameters);
        }

        public bool ExecuteCommandTransaction(Dictionary<string, SugarParameter[]> dictionary)
        {
            return BaseDal.ExecuteCommandTransaction(dictionary);
        }

        #region 存储过程
        /// <summary>
        /// 存储过程增删改
        /// </summary>
        /// <param name="sp_name">sp_sql</param>
        /// <param name="parameters">SugarParameter参数集合</param>
        /// <returns></returns>
        public int ExecuteProcedure(string sp_sql, SugarParameter[] parameters)
        {
            return BaseDal.ExecuteProcedure(sp_sql, parameters);
        }
        #endregion

        #endregion
    }

}
