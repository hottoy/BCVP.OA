using BCVP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BCVP.IServices.BASE
{
    public interface IBaseServices<TEntity> where TEntity : class
    {

        Task<TEntity> QueryById(object objId);
        Task<TEntity> QueryById(object objId, bool blnUseCache = false);
        Task<List<TEntity>> QueryByIDs(object[] lstIds);

        Task<int> Add(TEntity model);

        Task<int> Add(List<TEntity> listEntity);

        Task<bool> DeleteById(object id);

        Task<bool> Delete(TEntity model);

        Task<bool> DeleteByIds(object[] ids);

        Task<bool> Update(TEntity model);
        Task<bool> Update(TEntity entity, string strWhere);

        Task<bool> Update(object operateAnonymousObjects);

        Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "");

        Task<List<TEntity>> Query();
        Task<List<TEntity>> Query(string strWhere);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);
        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression);
        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression,string strOrderByFileds);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);
        Task<List<TEntity>> Query(string strWhere, string strOrderByFileds);
        Task<List<TEntity>> QuerySql(string strSql, SugarParameter[] parameters = null);
        Task<DataTable> QueryTable(string strSql, SugarParameter[] parameters = null);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds);
        Task<List<TEntity>> Query(string strWhere, int intTop, string strOrderByFileds);

        Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, string strOrderByFileds);
        Task<List<TEntity>> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds);


        Task<PageModel<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null);

        Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new();

        #region 扩展ExecuteCommand返回受影响行数，一般用于增删改SQL 

        /// <summary>
        /// 单条返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteCommand(string sql, object parameters);

        /// <summary>
        /// 单条返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteCommand(string sql, SugarParameter[] parameters = null);

        /// <summary>
        /// 批量返回受影响行数，增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteCommand(string sql, List<SugarParameter> parameters);

        /// <summary>
        /// Task 单条返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<int> ExecuteCommandAsync(string sql, object parameters);

        /// <summary>
        /// Task 单条返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<int> ExecuteCommandAsync(string sql, SugarParameter[] parameters = null);

        /// <summary>
        /// Task 批量返回受影响行数，用于增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<int> ExecuteCommandAsync(string sql, List<SugarParameter> parameters);

        #endregion

        #region SQL 查询

        /// <summary>
        /// 获取首行首列string
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string GetString(string sql, object parameters);

        /// <summary>
        /// 获取首行首列string
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string GetString(string sql, SugarParameter[] parameters = null);
        /// <summary>
        /// 获取首行首列string
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string GetString(string sql, List<SugarParameter> parameters);

        /// <summary>
        /// 获取首行首列Asyncstring
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<string> GetStringAsync(string sql, object parameters);

        /// <summary>
        /// 获取首行首列Asyncstring
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<string> GetStringAsync(string sql, SugarParameter[] parameters = null);
        /// <summary>
        /// 获取首行首列Asyncstring
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<string> GetStringAsync(string sql, List<SugarParameter> parameters);

        /// <summary>
        /// 获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int GetInt(string sql, object parameters);

        /// <summary>
        /// 获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int GetInt(string sql, SugarParameter[] parameters = null);

        /// <summary>
        /// 获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        int GetInt(string sql, List<SugarParameter> parameters);

        /// <summary>
        /// Async获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<int> GetIntAsync(string sql, object parameters);
        /// <summary>
        /// Async获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        Task<int> GetIntAsync(string sql, SugarParameter[] parameters = null);
        /// <summary>
        /// Async获取首行首列int
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        Task<int> GetIntAsync(string sql, List<SugarParameter> parameters);


        /// <summary>
        /// 获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Double GetDouble(string sql, object parameters);

        /// <summary>
        /// 获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        Double GetDouble(string sql, SugarParameter[] parameters = null);

        /// <summary>
        /// 获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        Double GetDouble(string sql, List<SugarParameter> parameters);

        /// <summary>
        /// 查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<TEntity> SqlQuery(string sql, object parameters);

        /// <summary>
        /// 查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<TEntity> SqlQuery(string sql, SugarParameter[] parameters = null);

        /// <summary>
        /// 查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<TEntity> SqlQuery(string sql, List<SugarParameter> parameters);

        /// <summary>
        /// Async查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<TEntity>> SqlQueryAsync(string sql, object parameters);

        /// <summary>
        /// Async查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<TEntity>> SqlQueryAsync(string sql, SugarParameter[] parameters = null);

        /// <summary>
        /// Async查询并返回List<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<TEntity>> SqlQueryAsync(string sql, List<SugarParameter> parameters);

        /// <summary>
        /// 查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        TEntity SqlQuerySingle(string sql, object parameters);

        /// <summary>
        /// 查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        TEntity SqlQuerySingle(string sql, SugarParameter[] parameters = null);

        /// <summary>
        /// 查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        TEntity SqlQuerySingle(string sql, List<SugarParameter> parameters);

        /// <summary>
        /// Async查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<TEntity> SqlQuerySingleAsync(string sql, object parameters);

        /// <summary>
        /// Async查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<TEntity> SqlQuerySingleAsync(string sql, SugarParameter[] parameters = null);

        /// <summary>
        /// Async查询返回单条记录<TEntity>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<TEntity> SqlQuerySingleAsync(string sql, List<SugarParameter> parameters);

        bool ExecuteCommandTransaction(Dictionary<string, SugarParameter[]> dictionary);
        //Task<bool> ExecuteCommandTransactionAsync(Dictionary<string, SugarParameter[]> dictionary);

        #region 存储过程
        /// <summary>
        /// 存储过程
        /// </summary>
        /// <param name="sp_sql">sp_sql</param>
        /// <param name="parameters">SugarParameter参数集合</param>
        /// <returns></returns>
        int ExecuteProcedure(string sp_sql, SugarParameter[] parameters);

        #endregion
        #endregion
    }

}
