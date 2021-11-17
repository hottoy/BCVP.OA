using BCVP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BCVP.IRepository.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// SqlsugarClient实体
        /// </summary>
        ISqlSugarClient Db { get;}
        /// <summary>
        /// 根据Id查询实体
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        Task<TEntity> QueryById(object objId);
        Task<TEntity> QueryById(object objId, bool blnUseCache = false);
        /// <summary>
        /// 根据id数组查询实体list
        /// </summary>
        /// <param name="lstIds"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryByIDs(object[] lstIds);
        
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> Add(TEntity model);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns></returns>
        Task<int> Add(List<TEntity> listEntity);

        /// <summary>
        /// 根据id 删除某一实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteById(object id);

        /// <summary>
        /// 根据对象，删除某一实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> Delete(TEntity model);

        /// <summary>
        /// 根据id数组，删除实体list
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIds(object[] ids);

        /// <summary>
        /// 更新model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> Update(TEntity model);

        /// <summary>
        /// 根据model，更新，带where条件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        Task<bool> Update(TEntity entity, string strWhere);
        Task<bool> Update(object operateAnonymousObjects);

        /// <summary>
        /// 根据model，更新，指定列
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lstColumns"></param>
        /// <param name="lstIgnoreColumns"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "");

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> Query();

        /// <summary>
        /// 带sql where查询
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        Task<List<TEntity>> Query(string strWhere);

        /// <summary>
        /// 根据表达式查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 根据表达式，指定返回对象模型，查询
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression);

        /// <summary>
        /// 根据表达式，指定返回对象模型，排序，查询
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <param name="whereExpression"></param>
        /// <param name="strOrderByFileds"></param>
        /// <returns></returns>
        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);
        Task<List<TEntity>> Query(string strWhere, string strOrderByFileds);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds);
        Task<List<TEntity>> Query(string strWhere, int intTop, string strOrderByFileds);
        Task<List<TEntity>> QuerySql(string strSql, SugarParameter[] parameters = null);
        Task<DataTable> QueryTable(string strSql, SugarParameter[] parameters = null);

        Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, string strOrderByFileds);
        Task<List<TEntity>> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds);

        /// <summary>
        /// 根据表达式，排序字段，分页查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="intPageIndex"></param>
        /// <param name="intPageSize"></param>
        /// <param name="strOrderByFileds"></param>
        /// <returns></returns>
        Task<PageModel<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null);

        /// <summary>
        /// 三表联查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="joinExpression"></param>
        /// <param name="selectExpression"></param>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new();

        /// <summary>
        /// 两表联查-分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="joinExpression"></param>
        /// <param name="selectExpression"></param>
        /// <param name="whereExpression"></param>
        /// <param name="intPageIndex"></param>
        /// <param name="intPageSize"></param>
        /// <param name="strOrderByFileds"></param>
        /// <returns></returns>
        Task<PageModel<TResult>> QueryTabsPage<T, T2, TResult>(
            Expression<Func<T, T2, object[]>> joinExpression,
            Expression<Func<T, T2, TResult>> selectExpression,
            Expression<Func<TResult, bool>> whereExpression,
            int intPageIndex = 1,
            int intPageSize = 20,
            string strOrderByFileds = null);

        /// <summary>
        /// 两表联合查询-分页-分组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="joinExpression"></param>
        /// <param name="selectExpression"></param>
        /// <param name="whereExpression"></param>
        /// <param name="groupExpression"></param>
        /// <param name="intPageIndex"></param>
        /// <param name="intPageSize"></param>
        /// <param name="strOrderByFileds"></param>
        /// <returns></returns>
        Task<PageModel<TResult>> QueryTabsPage<T, T2, TResult>(
            Expression<Func<T, T2, object[]>> joinExpression,
            Expression<Func<T, T2, TResult>> selectExpression,
            Expression<Func<TResult, bool>> whereExpression,
            Expression<Func<T, object>> groupExpression,
            int intPageIndex = 1,
            int intPageSize = 20,
            string strOrderByFileds = null);

        #region 扩展ExecuteCommand返回受影响行数，一般用于增删改SQ

        //DataTable QueryTable(string strSql, SugarParameter[] parameters = null);
        DataTable QueryTable(string strSql, Dictionary<string, object> dicParameters = null);
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
        /// <param name="pars"></param>
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
        /// Async获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<Double> GetDoubleAsync(string sql, object parameters);

        /// <summary>
        /// Async获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        Task<Double> GetDoubleAsync(string sql, SugarParameter[] parameters = null);

        /// <summary>
        /// Async获取首行首列Double
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        Task<Double> GetDoubleAsync(string sql, List<SugarParameter> parameters);


        /// <summary>
        /// 获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Decimal GetDecimal(string sql, object parameters);

        /// <summary>
        /// 获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Decimal GetDecimal(string sql, SugarParameter[] parameters = null);

        /// <summary>
        /// 获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Decimal GetDecimal(string sql, List<SugarParameter> parameters);

        /// <summary>
        /// Async获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<Decimal> GetDecimalAsync(string sql, object parameters);

        /// <summary>
        /// Async获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<Decimal> GetDecimalAsync(string sql, SugarParameter[] parameters = null);

        /// <summary>
        /// Async获取首行首列Decimal
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<Decimal> GetDecimalAsync(string sql, List<SugarParameter> parameters);

        /// <summary>
        /// 获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DateTime GetDateTime(string sql, object parameters);

        /// <summary>
        /// 获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DateTime GetDateTime(string sql, SugarParameter[] parameters = null);

        /// <summary>
        /// 获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DateTime GetDateTime(string sql, List<SugarParameter> parameters);

        /// <summary>
        /// Async获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<DateTime> GetDateTimeAsync(string sql, object parameters);

        /// <summary>
        /// Async获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<DateTime> GetDateTimeAsync(string sql, SugarParameter[] parameters = null);

        /// <summary>
        /// Async获取首行首列DateTime
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<DateTime> GetDateTimeAsync(string sql, List<SugarParameter> parameters);

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
        /// 存储过程获取一个集合
        /// </summary>
        /// <param name="sp_sql">存储过程名称</param>
        /// <param name="parameters">SugarParameter参数集合</param>
        /// <returns></returns>
        int ExecuteProcedure(string sp_sql, SugarParameter[] parameters);

        //List<TEntity> SqlQueryDynamic(string strSql);

        #endregion
        #endregion
    }
}
